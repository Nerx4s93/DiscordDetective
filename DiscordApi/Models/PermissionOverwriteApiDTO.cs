using System.Text.Json.Serialization;

namespace DiscordApi.Models;

public sealed class PermissionOverwriteApiDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("allow")]
    public string Allow { get; set; } = null!;

    [JsonPropertyName("deny")]
    public string Deny { get; set; } = null!;
}