using System.Collections.Generic;
using System.Text.Json.Serialization;

using Px6Api.DTOModels;

namespace Px6Api.ApiModels;

public class BuyResponse : ApiResponse
{
    public BuyInfo BuyInfo { get; set; } = null!;

    [JsonPropertyName("list")]
    public Dictionary<string, ProxyInfo> ProxyList { get; set; } = new Dictionary<string, ProxyInfo>();
}
