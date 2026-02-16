using System;
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
                PipelineTaskType.FetchMessages => await DownloadChannelMessages(task, queue, events),
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

    private async Task<PipelineTask[]> DownloadGuildChannels(PipelineTask task)
    {
        try
        {
            var guildId = task.Payload;

            var guildTask = client.GetGuildInfoAsync(guildId);
            var channelsTask = client.GetGuildChannelsAsync(guildId);
            await Task.WhenAll(guildTask, channelsTask);

            var guild = await guildTask;
            var channels = await channelsTask;

            await using var context = new DatabaseContext();
            await DbHelper.UpsertAsync(context, context.Guilds, guild.ToDbDTO());
            await context.SaveChangesAsync();

            var channelDTOs = channels.Where(c => c.Type != 15).Select(c =>
            {
                var dto = c.ToDbDTO();
                dto.GuildId = guildId;
                return dto;
            }).ToList();
            await DbHelper.UpsertAsync(context, context.Channels, channelDTOs);

            var permissions = channels.SelectMany(c =>
                    c.PermissionOverwrites.Select(p => p.ToDbDTO(c.Id))).ToList();
            await DbHelper.UpsertAsync(context, context.PermissionOverwrites, permissions);
            await DbHelper.UpsertAsync(context, context.Roles, guild.Roles.Select(r => r.ToDbDTO(guild)));

            await context.SaveChangesAsync();

            return channels.Select(c => new PipelineTask
            {
                Id = Guid.NewGuid(),
                GuildId = guildId,
                Type = PipelineTaskType.FetchMessages,
                Payload = c.Id
            }).ToArray();
        }
        catch (Exception ex) when (IsForbidden(ex)) { return []; }
    }

    private async Task<PipelineTask[]> DownloadUser(PipelineTask task)
    {
        var message = JsonSerializer.Deserialize<DiscrodMessage>(task.Payload);

        var guildId = task.GuildId;
        var userId = message!.Author.Id;

        try
        {
            var userApiDTO = await client.GetUser(guildId, userId);

            var user = userApiDTO.User.ToDbDTO();
            var member = userApiDTO.ToDbDTO(guildId);

            await using var context = new DatabaseContext();
            await DbHelper.UpsertAsync(context, context.Users, user);
            await context.SaveChangesAsync();
            await DbHelper.UpsertAsync(context, context.GuildMembers, member);
            await context.SaveChangesAsync();
        }
        catch (Exception ex) when (IsNotFound(ex))
        {
            var user = message.Author.ToDbDTO();

            await using var context = new DatabaseContext();
            await DbHelper.UpsertAsync(context, context.Users, user);
            await context.SaveChangesAsync();
        }

        return [];
    }

    private async Task<PipelineTask[]> DownloadChannelMessages(
        PipelineTask task, RedisTaskQueue queue, RedisEventBus events)
    {
        var channelId = task.Payload;
        string? beforeMessageId = null;

        while (true)
        {
            var messages = await client.GetChannelMessagesAsync(channelId, 100, beforeMessageId);

            if (messages.Count == 0)
            {
                break;
            }

            var guid = Guid.NewGuid();
            var path = Path.Combine("Pipeline", $"{guid}.json");

            await using (var writeStream = new FileStream(path,
                             FileMode.CreateNew, FileAccess.Write, FileShare.None,
                             bufferSize: 64 * 1024, useAsync: true))
            {
                await JsonSerializer.SerializeAsync(writeStream, messages);
            }

            var processingTask = new PipelineTask
            {
                Id = Guid.NewGuid(),
                GuildId = task.GuildId,
                Payload = guid.ToString(),
                Type = PipelineTaskType.ProcessChatMessages
            };

            await queue.EnqueueAsync(processingTask);
            await events.PublishEvent(processingTask, PipelineTaskProgress.New);

            beforeMessageId = messages[^1].Id;

            await Task.Delay(1000);
        }

        return [];
    }

    private static bool IsForbidden(Exception? ex)
    {
        while (ex != null)
        {
            if (ex is HttpRequestException { StatusCode: System.Net.HttpStatusCode.Forbidden })
            {
                return true;
            }
            ex = ex.InnerException;
        }
        return false;
    }

    private static bool IsNotFound(Exception? ex)
    {
        while (ex != null)
        {
            if (ex is HttpRequestException { StatusCode: System.Net.HttpStatusCode.NotFound })
            {
                return true;
            }
            ex = ex.InnerException;
        }
        return false;
    }
}