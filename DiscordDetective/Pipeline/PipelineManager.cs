using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DiscordDetective.Pipeline.Workers;

namespace DiscordDetective.Pipeline;

public sealed class PipelineManager(
    RedisTaskQueue queue,
    List<IWorker> discordWorkers,
    List<IWorker> aiWorkers,
    List<IWorker> dataWorkers)
{
    public async Task RunAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            foreach (var worker in discordWorkers.Where(worker => !worker.IsBusy))
            {
                var task = await queue.DequeueAsync(PipelineTaskType.DiscoverGuildChannels) ??
                           await queue.DequeueAsync(PipelineTaskType.DownloadChannelMessages);
                if (task != null)
                {
                    _ = worker.ExecuteTask(task, queue);
                }
            }

            foreach (var worker in aiWorkers.Where(worker => !worker.IsBusy))
            {
                var task = await queue.DequeueAsync(PipelineTaskType.ProcessMessagesWithAi);
                if (task != null)
                    _ = worker.ExecuteTask(task, queue);
            }

            foreach (var worker in dataWorkers.Where(worker => !worker.IsBusy))
            {
                var task = await queue.DequeueAsync(PipelineTaskType.PersistStructuredData);
                if (task != null)
                    _ = worker.ExecuteTask(task, queue);
            }

            await Task.Delay(200, token);
        }
    }
}
