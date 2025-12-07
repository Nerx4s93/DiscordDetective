using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DiscordDetective.Px6Api.ApiModels;

public class Countries : ApiResponse
{
    [JsonPropertyName("list")]
    public List<string> CountriesList { get; set; } = new List<string>();
}
