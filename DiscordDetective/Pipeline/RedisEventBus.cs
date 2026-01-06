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
        
        var channel = $"pipeline:events:{task.Type.ToString()}";
        await _subscriber.PublishAsync(channel, json);
        
        await _subscriber.PublishAsync("pipeline:events:all", json);
    }

    public void Subscribe(PipelineTaskType type, Action<PipelineEvent> handler)
    {
        var channel = $"pipeline:events:{type.ToString()}";

        _subscriber.Subscribe(channel, (redisChannel, value) =>
        {
            try
            {
                var task = JsonSerializer.Deserialize<PipelineEvent>((string)value, _jsonOptions);
                if (task != null)
                {
                    handler(task);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing event: {ex.Message}");
            }
        });
    }

    public void Subscribe(Action<PipelineEvent> handler)
    {
        _subscriber.Subscribe("pipeline:events:all", (redisChannel, value) =>
        {
            try
            {
                var task = JsonSerializer.Deserialize<PipelineEvent>((string)value, _jsonOptions);
                if (task != null)
                {
                    handler(task);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing event: {ex.Message}");
            }
        });
    }
}