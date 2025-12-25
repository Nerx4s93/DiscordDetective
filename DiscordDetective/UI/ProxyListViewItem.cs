using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using DiscordDetective.UI.Tools;

using Px6Api.DTOModels;

namespace DiscordDetective.UI;

public partial class ProxyListViewItem : UserControl
{
    private ProxyInfo? _proxy;

    private readonly Color BaseColor = Color.FromArgb(252, 252, 252);
    private readonly Color SelectedColor = Color.FromArgb(245, 245, 245);

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ProxyInfo? Proxy
    {
        get => _proxy;
        set
        {
            _proxy = value;
            Invalidate();
        }
    }

    public bool Selected => checkBoxItemSelected.Checked;

    public ProxyListViewItem()
    {
        InitializeComponent();

        _proxy = new ProxyInfo
        {
            Id = "35746304",
            Ip = "193.31.102.188",
            Host = "193.31.102.188",
            Port = "9481",
            User = "WqoQuf",
            Password = "PDQvDM",
            Type = "socks",
            Country = "us",
            Date = "2025-12-03 15:44:13",
            DateEnd = "2025-12-17 15:44:13",
            UnixTime = 1764765853,
            UnixTimeEnd = 1765975453,
            Description = "asdasd1",
            Active = "1"
        };
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_proxy == null)
        {
            return;
        }

        var svgCode = SvgDataLoader.GetSvgData($"Flags.{_proxy.Country}");
        if (svgCode != null)
        {
            var svgImage = SvgRenderer.SvgToBitmap(svgCode, _flagPictureBox.Width, _flagPictureBox.Height);
            _flagPictureBox.Image = svgImage;
        }

        _labelProxyData.Text = $"{_proxy.Ip}:{_proxy.Port}";
        _labelLoginData.Text = _proxy.User;
        _labelPasswordData.Text = _proxy.Password;
        _labelTypeData.Text = _proxy.Type == "socks" ? "SOCKS5" : "HTTPS";

        var dateTimeStart = DateTime.Parse(_proxy.Date);
        var dateTimeEnd = DateTime.Parse(_proxy.DateEnd);
        var daysLeft = dateTimeEnd - dateTimeStart;
        labelDateEnd.Text = dateTimeEnd.ToString("dd:MM:yy, HH:mm");
        labelDaysEnd.Text = daysLeft.Days.ToString() + "д";
    }

    private void checkBoxItemSelected_CheckedChanged(object sender, EventArgs e)
    {
        var selected = checkBoxItemSelected.Checked;
        BackColor = selected ? SelectedColor : BaseColor;
    }
}
