using Newtonsoft.Json;

namespace DiscordDetective.API.Models;

public class Connection
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("verified")]
    public bool Verified { get; set; }
}