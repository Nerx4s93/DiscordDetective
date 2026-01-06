using System.Drawing;
using System.IO;
using System.Text;

using Svg;

namespace DiscordDetective.UI.Tools;

internal static class SvgRenderer
{
    public static Bitmap SvgToBitmap(string svgContent, int width, int height, Color? color = null)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(svgContent));
        var svgDocument = SvgDocument.Open<SvgDocument>(stream);

        svgDocument.Width = width;
        svgDocument.Height = height;

        if (color.HasValue)
        {
            ApplyColor(svgDocument, color.Value);
        }

        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(Color.Transparent);
        svgDocument.Draw(graphics);

        return bitmap;
    }

    private static void ApplyColor(SvgElement element, Color color)
    {
        if (element is SvgVisualElement visual)
        {
            visual.Fill = new SvgColourServer(color);
            visual.Stroke = new SvgColourServer(color);
        }

        foreach (var child in element.Children)
        {
            ApplyColor(child, color);
        }
    }
}