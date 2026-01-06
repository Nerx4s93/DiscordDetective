using System.Drawing;

namespace DiscordDetective.UI.Models;

public class CountryItem(string countryCode, string countryName, Image flagImage)
{
    public string CountryCode { get; } = countryCode;
    public string CountryName { get; } = countryName;
    public Image FlagImage { get; } = flagImage;

    public override string ToString() => CountryName;
}