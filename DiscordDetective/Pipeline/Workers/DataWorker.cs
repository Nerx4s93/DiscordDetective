using System.Threading.Tasks;

namespace DiscordDetective.Pipeline.Workers;

public sealed class DataWorker : IWorker
{
    public bool IsBusy { get; private set; }

    public async Task ExecuteTask(PipelineTask task, RedisTaskQueue queue, RedisEventBus events)
    {
        IsBusy = true;
        try
        {
            if (task.Type == PipelineTaskType.PersistStructuredData)
            {
                // Сохраняем результат в базу
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
