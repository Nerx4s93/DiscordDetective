using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using DiscordDetective.UI.Tools;

using Microsoft.VisualBasic;

using Px6Api;
using Px6Api.DTOModels;

namespace DiscordDetective.UI;

[ToolboxItem(false)]
public partial class ProxyListViewItem : UserControl
{
    private ProxyInfo? _proxy;

    private readonly Color BaseColor = Color.FromArgb(252, 252, 252);
    private readonly Color SelectedColor = Color.FromArgb(245, 245, 245);

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Px6Client? Px6Client { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    public bool IsSelected
    {
        get => checkBoxItemSelected.Checked;
        set => checkBoxItemSelected.Checked = value;
    }

    public ProxyListViewItem()
    {
        InitializeComponent();
    }

    public void UpdateUI()
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

        var dateTimeEnd = DateTime.Parse(_proxy.DateEnd);
        var daysLeft = dateTimeEnd - DateTime.Now;
        labelDateEnd.Text = dateTimeEnd.ToString("dd:MM:yy, HH:mm");
        labelDaysEnd.Text = daysLeft.Days + "д";

        textBoxDescription.Text = _proxy.Description;
    }

    private void checkBoxItemSelected_CheckedChanged(object sender, EventArgs e)
    {
        var selected = checkBoxItemSelected.Checked;
        BackColor = selected ? SelectedColor : BaseColor;

        Selected?.Invoke(this, EventArgs.Empty);
    }

    private async void svgButtonComment_Click(object sender, EventArgs e)
    {
        if (_proxy == null || Px6Client == null)
        {
            return;
        }

        var text = Interaction.InputBox(
            "Введите комментарий",
            "Новый комментарий",
            ""
        );

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        try
        {
            await Px6Client.SetProxyDescriptionAsync([_proxy.Id], text);
            textBoxDescription.Text = text;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void svgButtonCopy_Click(object sender, EventArgs e)
    {
        if (_proxy == null)
        {
            return;
        }

        Clipboard.SetText($"{_proxy.Ip}:{_proxy.Port}:{_proxy.User}:{_proxy.Password}");
    }

    private async void svgButtonCheck_Click(object sender, EventArgs e)
    {
        if (_proxy == null || Px6Client == null)
        {
            return;
        }

        svgButtonCheck.Enabled = false;
        svgButtonCheck.IconName = "Loading";
        svgButtonCheck.IconPadding = 5;
        svgButtonCheck.BackColor = Color.FromArgb(252, 252, 252);
        svgButtonCheck.IconColor = Color.FromArgb(51, 51, 51);

        try
        {
            var result = await Px6Client.CheckProxyAsync(_proxy.Id);

            svgButtonCheck.IconPadding = 8;
            svgButtonCheck.IconColor = Color.White;

            if (result.proxyStatus.Status)
            {
                svgButtonCheck.IconName = "Ok";
                svgButtonCheck.BackColor = Color.FromArgb(92, 184, 92);
            }
            else
            {
                svgButtonCheck.IconName = "NotOk";
                svgButtonCheck.BackColor = Color.FromArgb(184, 92, 92);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        svgButtonCheck.Enabled = true;
    }

    private async void svgButtonDelete_Click(object sender, EventArgs e)
    {
        if (_proxy == null || Px6Client == null)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Вы уверены в удалении прокси сервера {_proxy.Ip}:{_proxy.Port}",
            "Удаление прокси",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await Px6Client.DeleteProxyAsync([_proxy.Id]);

            MessageBox.Show(
                $"Прокси {_proxy.Ip}:{_proxy.Port} удалён",
                "Удаление прокси",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Deleted?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public event EventHandler? Selected;
    public event EventHandler? Deleted;
}