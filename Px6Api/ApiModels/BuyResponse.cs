using System.Collections.Generic;
using System.Text.Json.Serialization;

using Px6Api.DTOModels;

namespace Px6Api.ApiModels;

public class BuyResponse : ApiResponse
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("period")]
    public int Period { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("list")]
    public Dictionary<string, ProxyInfo> ProxyList { get; set; } = new Dictionary<string, ProxyInfo>();
}
