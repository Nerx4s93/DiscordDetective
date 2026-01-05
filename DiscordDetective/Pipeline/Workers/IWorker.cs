using System.Threading.Tasks;

namespace DiscordDetective.Pipeline.Workers;

public interface IWorker
{
    bool IsBusy { get; }
    Task ExecuteTask(PipelineTask task, RedisTaskQueue queue);
}
