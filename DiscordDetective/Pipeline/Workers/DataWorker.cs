using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using DiscordAPI.Models;

namespace DiscordDetective.Pipeline.Workers;

public sealed class DataWorker : IWorker
{
    public bool IsBusy { get; private set; }

    public async Task ExecuteTask(PipelineTask task, RedisTaskQueue queue, RedisEventBus events)
    {
        IsBusy = true;
        await events.PublishEvent(task, PipelineTaskProgress.InProgress);

        try
        {
            var result = task.Type switch
            {
                PipelineTaskType.ProcessChatMessages => await ProcessMessages(task),
                // TODO: PipelineTaskType.PersistStructuredData => await DownloadUser(task),
                _ => null!
            };

            foreach (var newTask in result)
            {
                await queue.EnqueueAsync(newTask);
                await events.PublishEvent(newTask, PipelineTaskProgress.New);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            await events.PublishEvent(task, PipelineTaskProgress.End);
            IsBusy = false;
        }
    }

    private async Task<PipelineTask[]> ProcessMessages(PipelineTask task)
    {
        var guid = task.Payload;
        var inputPath = Path.Combine("Pipeline", $"{guid}.json");

        List<DiscrodMessage>? messages;
        await using (var readStream = new FileStream(inputPath,
                         FileMode.Open, FileAccess.Read, FileShare.Read,
                         bufferSize: 64 * 1024, useAsync: true))
        {
            messages = await JsonSerializer.DeserializeAsync<List<DiscrodMessage>>(readStream);
        }

        var result = new List<PipelineTask>();

        #region FetchUsers

        var userMessages = messages!
            .GroupBy(m => m.Author.Id)
            .ToDictionary(g => g.Key, g => g.First());

        result.AddRange(userMessages.Select(kvp =>
            new PipelineTask
            {
                Id = Guid.NewGuid(),
                GuildId = task.GuildId,
                Payload = JsonSerializer.Serialize(kvp.Value),
                Type = PipelineTaskType.FetchUsers
            }));

        #endregion

        #region ProcessMessagesWithAi

        var outputId = Guid.NewGuid().ToString();
        var outputPath = Path.Combine("Pipeline", $"{outputId}.json");

        await using (var writeStream = new FileStream(outputPath,
                         FileMode.CreateNew, FileAccess.Write, FileShare.None,
                         bufferSize: 64 * 1024, useAsync: true))
        {
            await JsonSerializer.SerializeAsync(writeStream, messages);
        }

        result.Add(new PipelineTask
        {
            Id = Guid.NewGuid(),
            GuildId = task.GuildId,
            Payload = outputId,
            Type = PipelineTaskType.ProcessMessagesWithAi
        });

        #endregion

        File.Delete(inputPath);

        return result.ToArray();
    }
}
