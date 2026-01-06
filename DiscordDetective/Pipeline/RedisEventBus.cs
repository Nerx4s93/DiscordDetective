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
        await _subscriber.PublishAsync("pipeline:events", json);
    }

    public void Subscribe(PipelineTaskType type, Action<PipelineEvent> handler)
    {
        _subscriber.Subscribe("pipeline:event", (redisChannel, value) =>
        {
            var task = JsonSerializer.Deserialize<PipelineEvent>((string)value, _jsonOptions);
            if (task != null)
            {
                handler(task);
            }
        });
    }
}