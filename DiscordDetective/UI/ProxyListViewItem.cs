using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using DiscordDetective.Px6Api.DTOModels;
using DiscordDetective.UI.Tools;

namespace DiscordDetective.UI;

public class ProxyListViewItem : UserControl
{
    private List<Panel> _panels = new List<Panel>();

    private CheckBox _checkBoxItemSelected;

    private PictureBox _flagPictureBox;

    private Label _labelProxy;
    private Label _labelProxyFormat;
    private Label _labelLogin;
    private Label _labelPassword;
    private Label _labelType;
    private Label _labelProxyData;
    private Label _labelLoginData;
    private Label _labelPasswordData;
    private Label _labelTypeData;

    private Label _labelDate;
    private Label _labelDays;
    private Label _labelDateData;
    private Label _labelDaysData;

    private ProxyInfo? _proxy;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    public ProxyListViewItem()
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    {
        Font = new Font("Arial", 10);
        Size = new Size(1200, 150);
        InitializeComponent();

        AutoScaleMode = AutoScaleMode.None;
        AutoSize = false;
        MinimumSize = new Size(1200, 150);
        MaximumSize = new Size(1200, 150);

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


    public void SetProxyData(ProxyInfo proxy)
    {
        _proxy = proxy;
        UpdateUI();
    }

    public ProxyInfo? GetProxyData() => _proxy;

    #region Инициализация

    private void InitializeComponent()
    {
        Margin = new Padding(2);

        SetupPanels();

        #region panel[0]

        _checkBoxItemSelected = new CheckBox
        {
            Size = new Size(25, 25),
            AutoSize = false
        };
        _checkBoxItemSelected.Location = new Point((_panels[0].Width - _checkBoxItemSelected.Width) / 2 + 2, 15);

        #endregion

        #region panel[1]

        _flagPictureBox = new PictureBox
        {
            Size = new Size(36, 27)
        };
        _flagPictureBox.Location = new Point((_panels[1].Width - _flagPictureBox.Width) / 2 + 2, 15);

        #endregion

        #region panel[2]

        var offsetY = 15;
        var offsetText = Font.Height + 4;
        _labelProxy = new Label
        {
            Text = "Прокси",
            Location = new Point(5, offsetY),
            AutoSize = true
        };
        _labelProxyFormat = new Label
        {
            Text = "IP:PORT",
            ForeColor = Color.FromArgb(136, 136, 136),
            Location = new Point(80, offsetY),
            AutoSize = true
        };
        _labelLogin = new Label
        {
            Text = "Логин",
            Location = new Point(5, offsetY + offsetText),
            AutoSize = true
        };
        _labelPassword = new Label
        {
            Text = "Пароль",
            Location = new Point(5, offsetY + offsetText * 2),
            AutoSize = true
        };
        _labelType = new Label
        {
            Text = "Тип",
            Location = new Point(5, offsetY + offsetText * 3),
            AutoSize = true
        };

        var boldFont = new Font(Font, FontStyle.Bold);
        _labelProxyData = new Label
        {
            Location = new Point(0, offsetY),
            Font = boldFont,
            AutoSize = false,
            Size = new Size(_panels[2].Width - 5, Font.Height),
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 0, 5, 0)
        };
        _labelLoginData = new Label
        {
            Location = new Point(0, offsetY + offsetText),
            Font = boldFont,
            ForeColor = Color.FromArgb(182, 95, 43),
            AutoSize = false,
            Size = new Size(_panels[2].Width - 5, Font.Height),
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 0, 5, 0)
        };
        _labelPasswordData = new Label
        {
            Location = new Point(0, offsetY + offsetText * 2),
            Font = boldFont,
            AutoSize = false,
            Size = new Size(_panels[2].Width - 5, Font.Height),
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 0, 5, 0)
        };
        _labelTypeData = new Label
        {
            Location = new Point(0, offsetY + offsetText * 3),
            Font = boldFont,
            AutoSize = false,
            Size = new Size(_panels[2].Width - 5, Font.Height),
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 0, 5, 0)
        };

        #endregion

        #region panel[3]

        _labelDate = new Label
        {
            Text = "Дата",
            Location = new Point(5, offsetY),
            AutoSize = true
        };
        _labelDays = new Label
        {
            Text = "Дни",
            Location = new Point(5, offsetY + offsetText),
            AutoSize = true
        };

        _labelDateData = new Label
        {
            Location = new Point(0, offsetY),
            ForeColor = Color.Green,
            AutoSize = false,
            Size = new Size(_panels[3].Width - 5, Font.Height),
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 0, 5, 0)
        };
        _labelDaysData = new Label
        {
            Location = new Point(0, offsetY + offsetText),
            ForeColor = Color.FromArgb(182, 95, 43),
            AutoSize = false,
            Size = new Size(_panels[3].Width - 5, Font.Height),
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 0, 5, 0)
        };

        #endregion

        _panels[0].Controls.Add(_checkBoxItemSelected);
        _panels[1].Controls.Add(_flagPictureBox);
        _panels[2].Controls.Add(_labelProxy);
        _panels[2].Controls.Add(_labelProxyFormat);
        _panels[2].Controls.Add(_labelLogin);
        _panels[2].Controls.Add(_labelPassword);
        _panels[2].Controls.Add(_labelType);
        _panels[2].Controls.Add(_labelProxyData);
        _panels[2].Controls.Add(_labelLoginData);
        _panels[2].Controls.Add(_labelPasswordData);
        _panels[2].Controls.Add(_labelTypeData);
        _panels[3].Controls.Add(_labelDate);
        _panels[3].Controls.Add(_labelDays);
        _panels[3].Controls.Add(_labelDateData);
        _panels[3].Controls.Add(_labelDaysData);
        _panels.ForEach(p => Controls.Add(p));
    }

    private void SetupPanels()
    {
        int[] panelWidths = { 32, 88, 515, 230, 285, 50 };
        var currentX = 0;
        var height = Height - 2;

        foreach (var width in panelWidths)
        {
            var panel = CreatePanel(currentX, 0, width, height);
            _panels.Add(panel);
            currentX += width;
        }

        var backColor = Color.FromArgb(250, 250, 250);
        _panels[0].BackColor = backColor;
        _panels[1].BackColor = backColor;
        _panels[2].BackColor = backColor;
        _panels[3].BackColor = backColor;
        _panels[4].BackColor = Color.Coral;
        _panels[5].BackColor = Color.Black;
    }

    private Panel CreatePanel(int x, int y, int width, int height)
    {
        return new Panel
        {
            Location = new Point(x, y),
            Size = new Size(width, height),
            BorderStyle = BorderStyle.None,
            BackColor = Color.Transparent,
            Margin = Padding.Empty,
            Padding = new Padding(5, 2, 5, 2)
        };
    }

    #endregion

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

        _labelProxyData.Text = $"{_proxy.Ip}:{_proxy.Port}";
        _labelLoginData.Text = _proxy.User;
        _labelPasswordData.Text = _proxy.Password;
        _labelTypeData.Text = _proxy.Type == "socks" ? "SOCKS5" : "HTTPS";

        var dateTimeStart = DateTime.Parse(_proxy.Date);
        var dateTimeEnd = DateTime.Parse(_proxy.DateEnd);
        var daysLeft = dateTimeEnd - dateTimeStart;
        _labelDateData.Text = dateTimeEnd.ToString("dd:MM:yy, HH:mm");
        _labelDaysData.Text = daysLeft.Days.ToString() + "д";
    }
}