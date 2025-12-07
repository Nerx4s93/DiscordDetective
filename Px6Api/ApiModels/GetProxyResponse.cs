using System.Collections.Generic;
using System.Text.Json.Serialization;

using DiscordDetective.Px6Api.DTOModels;

namespace DiscordDetective.Px6Api.ApiModels;

public class GetProxyResponse : ApiResponse
{
    [JsonPropertyName("list_count")]
    public int ProxyCount { get; set; }

    [JsonPropertyName("list")]
    public Dictionary<string, ProxyInfo> ProxyList { get; set; } = new Dictionary<string, ProxyInfo>();
}