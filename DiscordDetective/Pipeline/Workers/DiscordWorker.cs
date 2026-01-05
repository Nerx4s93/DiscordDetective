using System.Threading.Tasks;

using DiscordApi;

namespace DiscordDetective.Pipeline.Workers;

public sealed class DiscordWorker(DiscordClient client) : IWorker
{
    public bool IsBusy { get; private set; }

    public async Task ExecuteTask(PipelineTask task, RedisTaskQueue queue, RedisEventBus events)
    {
        IsBusy = true;
        try
        {
            PipelineTask[] result = null!;

            switch (task.Type)
            {
                case PipelineTaskType.DiscoverGuildChannels:
                    result = await DiscoverChannels(task.PayloadJson);
                    break;
                case PipelineTaskType.DownloadChannelMessages:
                    result = await DownloadMessages(task.PayloadJson);
                    break;
            }

            foreach (var newTask in result)
            {
                await queue.EnqueueAsync(newTask);
            }
        }
        finally
        {
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
            result[i] = new PipelineTask()
            {
                Type = PipelineTaskType.DownloadChannelMessages,
                PayloadJson = channels[i].Id
            };
        }

        return result;
    }

    private async Task<PipelineTask[]> DownloadMessages(string payloadJson)
    {
        return [];
        //var channelId = ulong.Parse(payloadJson);
        //var channel = client.GetChannel(channelId) as IMessageChannel;
        // Скачиваем сообщения и заносим задачи ProcessMessagesWithAi
    }
}