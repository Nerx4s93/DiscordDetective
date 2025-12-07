using System.Collections.Generic;

namespace DiscordDetective.Px6Api.DTOModels;

public class CountriesDTO
{
    public List<string> Iso2Code { get; set; } = new List<string>();
    public List<string> CountriesList { get; set; } = new List<string>();
}
