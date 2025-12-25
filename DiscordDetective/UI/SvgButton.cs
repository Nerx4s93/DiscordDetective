using System.Drawing;
using System.Windows.Forms;

using DiscordDetective.UI.Tools;

namespace DiscordDetective.UI;

public class SvgButton : Button
{
    public SvgButton()
    {
        base.Text = string.Empty;
        TextImageRelation = TextImageRelation.Overlay;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var originalText = base.Text;
        base.Text = string.Empty;

        base.OnPaint(e);

        base.Text = originalText;
        DrawSvgImage(e.Graphics, 5, originalText);
    }

    private void DrawSvgImage(Graphics graphics, int padding, string svgPath)
    {
        var svgCode = SvgDataLoader.GetSvgData($"Icon.{svgPath}");

        if (svgCode == null)
        {
            return;
        }
        
        var width = Width - padding * 2;
        var height = Height - padding * 2;
        using var svgImage = SvgRenderer.SvgToBitmap(svgCode, width, height);

        graphics.DrawImage(svgImage, padding, padding);
    }
}