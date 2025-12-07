using System.Text.Json.Serialization;

namespace DiscordDetective.Px6Api.ApiModels;

public class ProxyCount : ApiResponse
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
}
