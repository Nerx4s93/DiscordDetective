using System.Text.Json.Serialization;

namespace DiscordApi.Models;

public class RoleApiDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("permissions")]
    public string Permissions { get; set; } = null!;
}
