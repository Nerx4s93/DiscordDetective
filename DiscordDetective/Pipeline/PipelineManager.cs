using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DiscordDetective.Pipeline.Workers;

namespace DiscordDetective.Pipeline;

public sealed class PipelineManager(
    RedisTaskQueue queue,
    RedisEventBus events,
    List<DiscordWorker> discordWorkers,
    List<AiWorker> aiWorkers,
    List<DataWorker> dataWorkers)
{
    public async Task RunAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            foreach (var worker in discordWorkers.Where(worker => !worker.IsBusy))
            {
                var task = await queue.DequeueAsync(PipelineTaskType.DownloadChannels) ??
                           await queue.DequeueAsync(PipelineTaskType.FetchUsers) ??
                           await queue.DequeueAsync(PipelineTaskType.FetchMessages);
                if (task != null)
                {   
                    _ = worker.ExecuteTask(task, queue, events);
                }
            }

            /* TODO
             foreach (var worker in aiWorkers.Where(worker => !worker.IsBusy))
             {
                var task = await queue.DequeueAsync(PipelineTaskType.ProcessMessagesWithAi);
                if (task != null)
                {
                    _ = worker.ExecuteTask(task, queue, events);
                }
             }*/

            foreach (var worker in dataWorkers.Where(worker => !worker.IsBusy))
            {
                var task = await queue.DequeueAsync(PipelineTaskType.ProcessChatMessages) ??
                           await queue.DequeueAsync(PipelineTaskType.PersistStructuredData);
                if (task != null)
                {
                    _ = worker.ExecuteTask(task, queue, events);
                }
            }

            await Task.Delay(100, token);
        }
    }
}
