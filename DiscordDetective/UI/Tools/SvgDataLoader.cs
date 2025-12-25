using System.IO;
using System.Reflection;

namespace DiscordDetective.UI.Tools;

internal static class SvgDataLoader
{
    public static string? GetSvgData(string svgPath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"DiscordDetective.Resources.{svgPath}.svg";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
