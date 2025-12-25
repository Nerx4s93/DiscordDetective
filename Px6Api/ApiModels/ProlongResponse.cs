using System.Collections.Generic;
using System.Text.Json.Serialization;

using Px6Api.DTOModels;

namespace Px6Api.ApiModels;

public class ProlongResponse : ApiResponse
{
    [JsonPropertyName("order_id")]
    public int OrderId { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("price_single")]
    public double PriceSingle { get; set; }

    [JsonPropertyName("proxy_id")]
    public int ProxyId { get; set; }

    [JsonPropertyName("period")]
    public int Period { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("list")]
    public Dictionary<string, ProxyProlong> ProxyList { get; set; } = new Dictionary<string, ProxyProlong>();
}
