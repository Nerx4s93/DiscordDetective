using System;
using System.Windows.Forms;

using DiscordDetective.Px6Api.DTOModels;
using DiscordDetective.UI.Tools;

namespace DiscordDetective.UI;

public partial class UserControl1 : UserControl
{
    private ProxyInfo? _proxy;

    public UserControl1()
    {
        InitializeComponent();

        if (DesignMode)
        {
            _proxy = new ProxyInfo()
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
    }

    private void UpdateUI()
    {
        if (_proxy == null)
        {
            return;
        }

        var svgCode = SvgFlagsLoader.GetFlagSvg(_proxy.Country);
        if (svgCode != null)
        {
            var svgImage = SvgRenderer.SvgToBitmap(svgCode, _flagPictureBox.Width, _flagPictureBox.Height);
            _flagPictureBox.Image = svgImage;
        }

        //_labelProxyData.Text = $"{_proxy.Ip}:{_proxy.Port}";
        //_labelLoginData.Text = _proxy.User;
        //_labelPasswordData.Text = _proxy.Password;
        //_labelTypeData.Text = _proxy.Type == "socks" ? "SOCKS5" : "HTTPS";
        //
        //var dateTimeStart = DateTime.Parse(_proxy.Date);
        //var dateTimeEnd = DateTime.Parse(_proxy.DateEnd);
        //var daysLeft = dateTimeEnd - dateTimeStart;
        //_labelDateData.Text = dateTimeEnd.ToString("dd:MM:yy, HH:mm");
        //_labelDaysData.Text = daysLeft.Days.ToString() + "д";
    }
}
