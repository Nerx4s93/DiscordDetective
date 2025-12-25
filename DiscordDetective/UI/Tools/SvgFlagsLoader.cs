using System.IO;
using System.Reflection;

namespace DiscordDetective.UI.Tools;

internal static class SvgFlagsLoader
{
    public static string? GetFlagSvg(string countryCode)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"DiscordDetective.Resources.flags.{countryCode.ToLower()}.svg";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
