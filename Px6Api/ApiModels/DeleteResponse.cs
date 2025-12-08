using System.Text.Json.Serialization;

namespace DiscordDetective.Px6Api.ApiModels;

public class DeleteResponse : ApiResponse
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
}
