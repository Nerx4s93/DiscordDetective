using System.Drawing;
using System.Windows.Forms;

using DiscordDetective.UI.Tools;
using System.ComponentModel;
using System.Net.Mime;

namespace DiscordDetective.UI;

public class SvgButton : Button
{
    private string _iconName;
    private int _padding;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string IconName
    {
        get => _iconName;
        set
        {
            _iconName = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new int Padding
    {
        get => _padding;
        set
        {
            _padding = value;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.Text = string.Empty;

        try
        {
            base.OnPaint(e);

            if (!string.IsNullOrEmpty(IconName))
            {
                DrawSvgImage(e.Graphics);
            }
        }
        catch { /* ignore */ }
    }

    private void DrawSvgImage(Graphics graphics)
    {
        var svgCode = SvgDataLoader.GetSvgData($"Icon.{IconName}");

        if (svgCode == null)
        {
            return;
        }

        var width = Width - _padding * 2;
        var height = Height - _padding * 2;
        using var svgImage = SvgRenderer.SvgToBitmap(svgCode, width, height);

        graphics.DrawImage(svgImage, _padding, _padding);
    }
}