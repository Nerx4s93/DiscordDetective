using System;

namespace DiscordApi.Models;

public sealed class MessageApiDTO
{
    public string Content { get; init; } = null!;
    public DateTime Timestamp { get; init; }
    public UserApiDTO Author { get; init; } = null!;
    public MessageReference MessageReference { get; init; } = null!;
}
