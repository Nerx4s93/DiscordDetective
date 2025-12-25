using System.Drawing;
using System.Windows.Forms;

using DiscordDetective.UI.Tools;
using System.ComponentModel;

namespace DiscordDetective.UI;

public class SvgButton : Button
{
    private string _iconName = string.Empty;
    private int _iconPadding = 0;
    private Point _iconOffset = Point.Empty;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string IconName
    {
        get => _iconName;
        set
        {
            _iconName = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new int IconPadding
    {
        get => _iconPadding;
        set
        {
            _iconPadding = value;
            Invalidate();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Point IconOffset
    {
        get => _iconOffset;
        set
        {
            _iconOffset = value;
            Invalidate();
        }
    }

    [DefaultValue(0)]
    public int OffsetX
    {
        get => _iconOffset.X;
        set
        {
            _iconOffset = new Point(value, _iconOffset.Y);
            Invalidate();
        }
    }

    [DefaultValue(0)]
    public int OffsetY
    {
        get => _iconOffset.Y;
        set
        {
            _iconOffset = new Point(_iconOffset.X, value);
            Invalidate();
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string Text
    {
        get => string.Empty;
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        set { }
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    }

    public SvgButton()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw, true);
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

        var width = Width - _iconPadding * 2;
        var height = Height - _iconPadding * 2;
        using var svgImage = SvgRenderer.SvgToBitmap(svgCode, width, height);

        var x = _iconPadding + OffsetX;
        var y = _iconPadding + OffsetY;
        graphics.DrawImage(svgImage, x, y);
    }
}