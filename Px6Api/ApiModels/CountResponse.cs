using System.Text.Json.Serialization;

namespace Px6Api.ApiModels;

public class CountResponse : ApiResponse
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
}
