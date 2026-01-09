using System.Text.Json.Serialization;

namespace DiscordApi.Models;

public sealed class GuildApiDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("banner")]
    public string? Banner { get; set; }

    [JsonPropertyName("roles")]
    public RoleApiDTO[] Roles { get; set; } = null!;

    [JsonIgnore]
    public string? IconUrl =>
    !string.IsNullOrEmpty(Icon)
        ? $"https://cdn.discordapp.com/icons/{Id}/{Icon}.png?size=48"
        : null;

    [JsonIgnore]
    public string? BannerUrl =>
    !string.IsNullOrEmpty(Banner)
        ? $"https://cdn.discordapp.com/banners/{Id}/{Banner}.png?size=48"
        : null;
}
