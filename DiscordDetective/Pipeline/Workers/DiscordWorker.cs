using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using DiscordApi;
using DiscordApi.Models;

using DiscordDetective.Database;

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
                PipelineTaskType.DownloadChannels => await DownloadGuildChannels(task),
                PipelineTaskType.FetchUsers => await DownloadUser(task),
                PipelineTaskType.FetchMessages => await DownloadChannelMessages(task),
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

    private async Task<PipelineTask[]> DownloadGuildChannels(PipelineTask task)
    {
        var guildId = task.Payload;

        var guildTask = client.GetGuildInfoAsync(guildId);
        var channelsTask = client.GetGuildChannelsAsync(guildId);
        await Task.WhenAll(guildTask, channelsTask);

        var guild = await guildTask;
        var channels = await channelsTask;

        await using var context = new DatabaseContext();
        DbHelper.Upsert([guild.ToDbDTO()], context, context.Guilds, g => g.Id);
        DbHelper.Upsert(channels.Select(c => c.ToDbDTO()), context, context.Channels, c => c.Id);
        await context.SaveChangesAsync();

        return channels.Select(c => new PipelineTask
        {
            Id = Guid.NewGuid(),
            GuildId = guildId,
            Type = PipelineTaskType.FetchMessages,
            Payload = c.Id
        }).ToArray();
    }

    private async Task<IEnumerable<PipelineTask>> DownloadUser(PipelineTask task)
    {
        var guildId = task.GuildId;
        var userId = task.Payload;

        var userApiDTO = await client.GetUser(guildId, userId);

        var user = userApiDTO.User.ToDbDTO();
        var member = userApiDTO.ToDbDTO(guildId);

        await using var context = new DatabaseContext();
        DbHelper.Upsert([user], context, context.Users, u => u.Id);
        DbHelper.Upsert([member], context, context.GuildMembers, m => m.UserId);
        await context.SaveChangesAsync();

        return [];
    }

    private async Task<PipelineTask[]> DownloadChannelMessages(PipelineTask task)
    {
        var channelId = task.Payload;
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

            if (messages.Count < 100)
            {
                break;
            }

            beforeMessageId = messages[^1].Id;

            await Task.Delay(1000);
        }

        var result = new List<PipelineTask>();

        #region FetchUsers

        var users = allMessages.Select(m => m.Author).ToHashSet();
        result.AddRange(users.Select(user =>
                new PipelineTask
                {
                    Id = Guid.NewGuid(),
                    GuildId = task.GuildId,
                    Payload = user.Id,
                    Type = PipelineTaskType.FetchUsers
                }).ToList());

        #endregion

        #region ProcessMessagesWithAi

        const int chunkSize = 50;
        var chunks = allMessages
            .Select((msg, index) => new { msg, index })
            .GroupBy(x => x.index / chunkSize)
            .Select(g => g.Select(x => x.msg).ToList())
            .ToList();

        foreach (var chunk in chunks)
        {
            var fileId = Guid.NewGuid().ToString();
            var filePath = Path.Combine("Pipeline", $"{fileId}.json");

            Directory.CreateDirectory("Pipeline");
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(chunk));

            result.Add(new PipelineTask
            {
                Id = Guid.NewGuid(),
                GuildId = task.GuildId,
                Payload = filePath,
                Type = PipelineTaskType.ProcessMessagesWithAi
            });
        }

        #endregion

        return result.ToArray();
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