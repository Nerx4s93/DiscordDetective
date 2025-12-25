using System.Text.Json.Serialization;

namespace Px6Api.DTOModels;

public class User
{
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = string.Empty;


    [JsonPropertyName("balance")]
    public decimal Balance { get; set; } = 0;


    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;
}
