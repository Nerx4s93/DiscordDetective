using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Px6Api.ApiModels;

public class GetCountryResponse : ApiResponse
{
    [JsonPropertyName("list")]
    public List<string> CountriesList { get; set; } = new List<string>();
}
