using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DiscordApi.Models;

public sealed class ChannelApiDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("guild_id")]
    public string GuildId { get; set; } = null!;

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("parent_id")]
    public string? ParentId { get; set; }

    [JsonPropertyName("position")]
    public int Position { get; set; }

    [JsonPropertyName("last_message_id")]
    public string? LastMessageId { get; set; }

    [JsonPropertyName("flags")]
    public int Flags { get; set; }

    [JsonPropertyName("nsfw")]
    public bool Nsfw { get; set; }

    [JsonPropertyName("rate_limit_per_user")]
    public int? RateLimitPerUser { get; set; }

    [JsonPropertyName("bitrate")]
    public int? Bitrate { get; set; }

    [JsonPropertyName("user_limit")]
    public int? UserLimit { get; set; }

    [JsonPropertyName("rtc_region")]
    public string? RtcRegion { get; set; }

    [JsonPropertyName("permission_overwrites")]
    public List<PermissionOverwriteApiDTO> PermissionOverwrites { get; set; } = [];
}