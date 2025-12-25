using System.Text.Json.Serialization;

namespace Px6Api.DTOModels;

public class ProxyProlong
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("date_end")]
    public string DateEnd { get; set; } = string.Empty;

    [JsonPropertyName("unixtime")]
    public long UnixTime { get; set; }
}
