using System.Text.Json.Serialization;

namespace DiscordApi.Models;

public sealed class UserApiDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("global_name")]
    public string? GlobalName { get; set; }

    [JsonPropertyName("discriminator")]
    public string Discriminator { get; set; } = "0";

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("banner")]
    public string? Banner { get; set; }

    [JsonPropertyName("accent_color")]
    public int? AccentColor { get; set; }

    [JsonPropertyName("bio")]
    public string? Bio { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("verified")]
    public bool? Verified { get; set; }

    public string FullUsername => Discriminator != "0"
        ? $"{Username}#{Discriminator}"
        : Username;

    [JsonIgnore]
    public string DisplayName => GlobalName ?? Username;

    [JsonIgnore]
    public string? AvatarUrl =>
        !string.IsNullOrEmpty(Avatar)
            ? $"https://cdn.discordapp.com/avatars/{Id}/{Avatar}.png"
            : null;

    [JsonIgnore]
    public string? BannerUrl =>
        !string.IsNullOrEmpty(Banner)
            ? $"https://cdn.discordapp.com/banners/{Id}/{Banner}.png"
            : null;

    [JsonIgnore]
    public string? AccentColorHex =>
        AccentColor.HasValue
            ? $"#{AccentColor.Value:X6}"
            : null;
}