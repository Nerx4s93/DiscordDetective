using Newtonsoft.Json;

namespace DiscordDetective.API.Models;

public class Guild
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("icon")]
    public string? Icon { get; set; }

    [JsonProperty("owner")]
    public bool Owner { get; set; }

    [JsonProperty("permissions")]
    public string Permissions { get; set; } = string.Empty;

    public string? IconUrl =>
        !string.IsNullOrEmpty(Icon)
            ? $"https://cdn.discordapp.com/icons/{Id}/{Icon}.png"
            : null;
}