using System.Text.Json.Serialization;

namespace DiscordDetective.Px6Api.Models;

public class ApiResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;


    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = string.Empty;


    [JsonPropertyName("balance")]
    public decimal Balance { get; set; } = 0;


    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;


    [JsonPropertyName("error_id")]
    public int? ErrorId { get; set; }

    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;

    public bool IsSuccess => Status?.ToLower() == "yes";
}