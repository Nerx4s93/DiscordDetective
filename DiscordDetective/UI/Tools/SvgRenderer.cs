using System.Drawing;
using System.IO;
using System.Text;

using Svg;

namespace DiscordDetective.UI.Tools;

internal class SvgRenderer
{
    public static Bitmap SvgToBitmap(string svgContent, int width, int height)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(svgContent));
        var svgDocument = SvgDocument.Open<SvgDocument>(stream);

        svgDocument.Width = width;
        svgDocument.Height = height;

        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.Clear(Color.Transparent);
        svgDocument.Draw(graphics);

        return bitmap;
    }
}