using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace DiscordDetective.Pipeline;

public sealed class RedisEventBus(IConnectionMultiplexer multiplexer)
{
    private readonly ISubscriber _subscriber = multiplexer.GetSubscriber();
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task PublishEvent(PipelineTask task, PipelineTaskProgress progress)
    {
        var message = new PipelineEvent(task.Type, task.Id, progress);
        var json = JsonSerializer.Serialize(message, _jsonOptions);
        var channel = $"pipeline:events:{task.Type}";
        await _subscriber.PublishAsync(channel, json);
    }

    public void Subscribe(PipelineTaskType type, Action<PipelineEvent> handler)
    {
        var channel = $"pipeline:events:{type}";
        _subscriber.Subscribe(channel, (redisChannel, value) =>
        {
            var task = JsonSerializer.Deserialize<PipelineEvent>((string)value, _jsonOptions);
            if (task != null)
            {
                handler(task);
            }
        });
    }
}