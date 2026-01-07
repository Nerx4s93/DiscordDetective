using System;
using System.Text.Json.Serialization;

namespace DiscordApi.Models;

public sealed class MessageApiDTO
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("content")]
    public string Content { get; init; } = null!;

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; init; } = null!;

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }

    [JsonPropertyName("author")]
    public UserApiDTO Author { get; init; } = null!;
}
