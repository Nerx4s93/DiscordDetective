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

    private const string Channel = "pipeline:events";

    public async Task PublishEvent(PipelineTask task, PipelineTaskProgress progress)
    {
        var message = new PipelineEvent(task.Type, task.Id, progress, task.GuildId);
        var json = JsonSerializer.Serialize(message, _jsonOptions);

        await _subscriber.PublishAsync(Channel, json);
    }

    public void Subscribe(Action<PipelineEvent> handler)
    {
        _subscriber.Subscribe(Channel, (redisChannel, value) =>
        {
            try
            {
                var evt = JsonSerializer.Deserialize<PipelineEvent>((string)value, _jsonOptions);
                if (evt != null)
                {
                    handler(evt);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing event: {ex.Message}");
            }
        });
    }
}