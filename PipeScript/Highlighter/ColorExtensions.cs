using System.Drawing;

namespace PipeScript.Highlighter;

internal static class ColorExtensions
{
    public static string ToRtfEntry(this Color color)
    {
        return $@"\red{color.R}\green{color.G}\blue{color.B}";
    }
}