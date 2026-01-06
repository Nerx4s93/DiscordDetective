using System;
using System.Threading.Tasks;

using DiscordApi;

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
        await Task.Delay(1000);
        return [];
        //var channelId = ulong.Parse(payloadJson);
        //var channel = client.GetChannel(channelId) as IMessageChannel;
        // Скачиваем сообщения и заносим задачи ProcessMessagesWithAi
    }
}