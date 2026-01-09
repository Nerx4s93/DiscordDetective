using System;
using System.Text.Json.Serialization;

namespace DiscordApi.Models;

public sealed class MemberApiDTO
{
    [JsonPropertyName("nick")]
    public string? Nick { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("banner")]
    public string? Banner { get; set; }

    [JsonPropertyName("roles")]
    public string[] Roles { get; set; } = [];


    [JsonPropertyName("user")]
    public UserApiDTO User { get; set; } = null!;

    [JsonPropertyName("premium_since")]
    public DateTime? PremiumSince { get; set; }

    [JsonPropertyName("joined_at")]
    public DateTime? JoinedAt { get; set; }
}
