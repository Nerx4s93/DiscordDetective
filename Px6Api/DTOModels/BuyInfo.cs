using System.Text.Json.Serialization;

namespace Px6Api.DTOModels;

public class BuyInfo
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("period")]
    public int Period { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;
}
