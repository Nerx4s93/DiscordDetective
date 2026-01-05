using System.Text.Json.Serialization;

namespace DiscordApi.Models;

public class MessageReference
{
    [JsonPropertyName("type")]
    public int Type { get; init; }

    [JsonPropertyName("channel_id")]
    public string ChanelId { get; init; } = null!;

    [JsonPropertyName("message_id")]
    public string MessageId { get; init; } = null!;

    [JsonPropertyName("guild_id")]
    public string GuildId { get; init; } = null!;
}
