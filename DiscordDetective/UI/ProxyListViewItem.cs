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
            UpdateUI();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Selected
    {
        get => checkBoxItemSelected.Checked;
        set => checkBoxItemSelected.Checked = value;
    }

    public ProxyListViewItem()
    {
        InitializeComponent();
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
