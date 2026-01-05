using StackExchange.Redis;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiscordDetective.Pipeline;

public sealed class RedisTaskQueue(IDatabase database)
{
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private string GetKey(PipelineTaskType type) => $"pipeline:tasks:{type}";

    public async Task EnqueueAsync(PipelineTask task)
    {
        var json = JsonSerializer.Serialize(task);
        await database.ListRightPushAsync(GetKey(task.Type), json);
    }

    public async Task<PipelineTask?> DequeueAsync(PipelineTaskType type)
    {
        var key = GetKey(type);
        var redisValue = await database.ListLeftPopAsync(key);
        if (redisValue.IsNullOrEmpty)
        {
            return null;
        }

        var json = (string)redisValue!;
        var task = JsonSerializer.Deserialize<PipelineTask>(json, _jsonOptions);
        return task;
    }

    public async Task<long> CountAsync(PipelineTaskType type) => await database.ListLengthAsync(GetKey(type));
}