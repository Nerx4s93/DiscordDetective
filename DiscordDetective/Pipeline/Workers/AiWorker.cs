using System.Threading.Tasks;

namespace DiscordDetective.Pipeline.Workers;

public sealed class AiWorker : IWorker
{
    public bool IsBusy { get; private set; }

    public async Task ExecuteTask(PipelineTask task, RedisTaskQueue queue, RedisEventBus events)
    {
        IsBusy = true;
        try
        {
            if (task.Type == PipelineTaskType.ProcessMessagesWithAi)
            {

            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private Task<string> AiProcess(string messagesJson)
    {
        return Task.FromResult("{resultJson}");
    }
}