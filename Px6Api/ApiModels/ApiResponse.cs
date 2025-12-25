using System.Text.Json.Serialization;

using Px6Api.DTOModels;

namespace Px6Api.ApiModels;

public class ApiResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    public UserDTO User { get; set; } = new();

    [JsonPropertyName("error_id")]
    public int? ErrorId { get; set; }

    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;

    public bool IsSuccess => Status?.ToLower() == "yes";
}