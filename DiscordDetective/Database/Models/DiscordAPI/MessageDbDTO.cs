using System;
using System.ComponentModel.DataAnnotations;

namespace DiscordDetective.Database.Models.DiscordAPI;

public sealed class MessageDbDTO
{
    [Key]
    public string Id { get; init; } = null!;
    public string Content { get; init; } = null!;
    public string ChannelId { get; init; } = null!;
    public DateTime Timestamp { get; init; }
    public UserDbDTO Author { get; init; } = null!;
}