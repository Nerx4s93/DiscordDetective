using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace DiscordDetective.Pipeline;

public sealed class RedisTaskQueue(IDatabase database)
{
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private static string GetKey(PipelineTaskType type) => $"pipeline:tasks:{type}";

    public async Task<List<PipelineTask>> GetTasks(PipelineTaskType type)
    {
        var key = GetKey(type);
        var values = await database.ListRangeAsync(key);

        var result = new List<PipelineTask>(values.Length);

        foreach (var value in values)
        {
            if (!value.HasValue)
            {
                continue;
            }

            var task = JsonSerializer.Deserialize<PipelineTask>((string)value!);
            if (task != null)
            {
                result.Add(task);
            }
        }

        return result;
    }


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