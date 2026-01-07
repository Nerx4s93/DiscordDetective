using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using DiscordApi;
using DiscordApi.Models;

namespace DiscordDetective.Pipeline.Workers;

public sealed class DiscordWorker(DiscordClient client) : IWorker
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
                PipelineTaskType.DiscoverGuildChannels => await DiscoverChannels(task.Payload),
                PipelineTaskType.DownloadChannelMessages => await DownloadMessages(task.Payload),
                _ => null!
            };

            foreach (var newTask in result)
            {
                await queue.EnqueueAsync(newTask);
                await events.PublishEvent(newTask, PipelineTaskProgress.New);
            }
        }
        catch (Exception ex) when (IsForbidden(ex))
        {
            Console.WriteLine($"[403] Access denied, skipping task {task.Id}");
        }
        finally
        {
            await events.PublishEvent(task, PipelineTaskProgress.End);
            IsBusy = false;
        }
    }

    private async Task<PipelineTask[]> DiscoverChannels(string payloadJson)
    {
        var guildId = payloadJson;
        var channels = await client.GetGuildChannelsAsync(guildId);

        var result = new PipelineTask[channels.Count];
        for (var i = 0; i < channels.Count; i++)
        {
            result[i] = new PipelineTask
            {
                Id = Guid.NewGuid(),
                Type = PipelineTaskType.DownloadChannelMessages,
                Payload = channels[i].Id
            };
        }

        return result;
    }

    private async Task<PipelineTask[]> DownloadMessages(string payloadJson)
    {
        var channelId = payloadJson;
        var allMessages = new List<MessageApiDTO>();
        string? beforeMessageId = null;

        while (true)
        {
            var messages = await client.GetChannelMessagesAsync(channelId, 100, beforeMessageId);

            if (messages.Count == 0)
            {
                break;
            }

            allMessages.AddRange(messages);
            Console.WriteLine($"Fetched {allMessages.Count} messages");

            if (messages.Count < 100)
            {
                break;
            }

            allMessages.AddRange(messages);
            beforeMessageId = messages.Last().Id;

            await Task.Delay(1500);
        }

        return [];
    }

    private static bool IsForbidden(Exception ex)
    {
        while (true)
        {
            if (ex is HttpRequestException { StatusCode: System.Net.HttpStatusCode.Forbidden })
            {
                return true;
            }

            if (ex.InnerException != null)
            {
                ex = ex.InnerException;
                continue;
            }

            return false;
        }
    }
}