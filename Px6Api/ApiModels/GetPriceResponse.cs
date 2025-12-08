using System.Text.Json.Serialization;

namespace DiscordDetective.Px6Api.ApiModels;

public class GetPriceResponse : ApiResponse
{
    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("price_single")]
    public double PriceSingle { get; set; }

    [JsonPropertyName("period")]
    public int Period { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}
