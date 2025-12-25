using System.Text.Json.Serialization;

namespace Px6Api.ApiModels;

public class CheckResponse : ApiResponse
{
    [JsonPropertyName("proxy_id")]
    public int ProxyId { get; set; }

    [JsonPropertyName("proxy_status")]
    public bool ProxyStatus { get; set; }
}
