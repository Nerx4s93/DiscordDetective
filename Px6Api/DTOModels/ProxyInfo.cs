using System.Text.Json.Serialization;

namespace Px6Api.DTOModels;

public class ProxyInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("ip")]
    public string Ip { get; set; } = string.Empty;

    [JsonPropertyName("host")]
    public string Host { get; set; } = string.Empty;

    [JsonPropertyName("port")]
    public string Port { get; set; } = string.Empty;

    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("pass")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [JsonPropertyName("date_end")]
    public string DateEnd { get; set; } = string.Empty;

    [JsonPropertyName("unixtime")]
    public long UnixTime { get; set; }

    [JsonPropertyName("unixtime_end")]
    public long UnixTimeEnd { get; set; }

    [JsonPropertyName("descr")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public string Active { get; set; } = string.Empty;

    public bool IsActive => Active == "1";
}
