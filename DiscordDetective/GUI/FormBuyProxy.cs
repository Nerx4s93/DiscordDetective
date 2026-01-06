using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using DiscordDetective.UI.Models;
using DiscordDetective.UI.Tools;

using Px6Api;
using Px6Api.DTOModels;

namespace DiscordDetective.GUI;

public partial class FormBuyProxy : Form
{
    private readonly Px6Client _px6Client;
    private int? _availableCount;

    public FormBuyProxy(Px6Client px6Client)
    {
        InitializeComponent();
        _px6Client = px6Client;
    }

    public List<ProxyInfo>? BuyResult;

    private async void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            comboBoxType.Enabled = false;
            comboBoxCountry.Enabled = false;
            comboBoxCountry.Items.Clear();
            labelBuyCount.Text = "Доступно к покупке: 0 шт";
            _availableCount = null;
            labelPrice.Text = "Цена: 0 ₽";
            buttonBuy.Enabled = false;

            var type = GetProxyType(comboBoxType.SelectedItem!.ToString());
            if (type == null)
            {
                return;
            }

            var answer = await _px6Client.GetCountriesAsync(type.Value);

            foreach (var country in answer.countries)
            {
                var countryName = GetCountryName(country);
                var svgCode = SvgDataLoader.GetSvgData($"Flags.{country}");
                var flagImage = SvgRenderer.SvgToBitmap(svgCode!, 20, 20);

                var item = new CountryItem(country, countryName, flagImage);
                comboBoxCountry.Items.Add(item);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            comboBoxType.Enabled = true;
            comboBoxCountry.Enabled = true;
        }
    }

    private async void comboBoxCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            comboBoxType.Enabled = false;
            comboBoxCountry.Enabled = false;
            labelBuyCount.Text = "Доступно к покупке: 0 шт";
            _availableCount = null;
            labelPrice.Text = "Цена: 0 ₽";
            buttonBuy.Enabled = false;

            if (comboBoxCountry.SelectedItem is not CountryItem selectedItem)
            {
                return;
            }

            var type = GetProxyType(comboBoxType.SelectedItem!.ToString())!.Value;
            var country = selectedItem.CountryCode;
            var answer = await _px6Client.GetProxyCountAsync(country, type);

            _availableCount = answer.proxiesCount;
            labelBuyCount.Text = $"Доступно к покупке: {answer.proxiesCount} шт";
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            comboBoxType.Enabled = true;
            comboBoxCountry.Enabled = true;
        }
    }

    private void comboBoxCountry_DrawItem(object sender, DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= comboBoxCountry.Items.Count)
        {
            return;
        }

        e.DrawBackground();

        if (comboBoxCountry.Items[e.Index] is not CountryItem item)
        {
            return;
        }

        e.Graphics.DrawImage(item.FlagImage, e.Bounds.Left + 2, e.Bounds.Top + 2);

        var textBounds = new Rectangle(
            e.Bounds.Left + 25, e.Bounds.Top,
            e.Bounds.Width - 25, e.Bounds.Height);

        using (var brush = new SolidBrush(e.ForeColor))
        {
            e.Graphics.DrawString(item.CountryName, e.Font, brush, textBounds);
        }

        e.DrawFocusRectangle();
    }

    private void numericUpDownCount_ValueChanged(object sender, EventArgs e)
    {
        labelPrice.Text = "Цена: 0 ₽";
        buttonBuy.Enabled = false;
        priceTimer.Stop();
        priceTimer.Start();
    }

    private void comboBoxChanging_SelectedIndexChanged(object sender, EventArgs e)
    {
        labelPrice.Text = "Цена: 0 ₽";
        buttonBuy.Enabled = false;
        priceTimer.Stop();
        priceTimer.Start();
    }

    private void priceTimer_Tick(object sender, EventArgs e)
    {
        priceTimer.Stop();
        GetPrice();
    }

    private async void buttonBuy_Click(object sender, EventArgs e)
    {
        try
        {
            comboBoxType.Enabled = false;
            comboBoxCountry.Enabled = false;
            numericUpDownCount.Enabled = false;
            comboBoxChanging.Enabled = false;
            buttonBuy.Enabled = false;


            var count = (int)numericUpDownCount.Value;
            var period = GetPeriod(comboBoxChanging.SelectedItem!.ToString()!)!.Value;
            var country = (comboBoxCountry.SelectedItem as CountryItem)!.CountryCode;
            var type = GetProxyType(comboBoxType.SelectedItem!.ToString())!.Value;

            var answer = await _px6Client.BuyProxy(count, period, country, 
                "", false, false, type);

            BuyResult = answer.proxyList.Select(p => p.Value).ToList();
            DialogResult = DialogResult.OK;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            comboBoxType.Enabled = true;
            comboBoxCountry.Enabled = true;
            numericUpDownCount.Enabled = true;
            comboBoxChanging.Enabled = true;
            buttonBuy.Enabled = true;
        }
    }

    private async void GetPrice()
    {
        try
        {
            if (comboBoxType.SelectedItem == null ||
                comboBoxCountry.SelectedItem == null ||
                comboBoxChanging.SelectedItem == null)
            {
                return;
            }

            var changingItem = comboBoxChanging.SelectedItem.ToString();
            var period = GetPeriod(changingItem!);
            if (period == null)
            {
                return;
            }

            if (numericUpDownCount.Value > _availableCount)
            {
                MessageBox.Show(
                    "Требуемое количество IP-адресов недоступно.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            comboBoxType.Enabled = false;
            comboBoxCountry.Enabled = false;
            numericUpDownCount.Enabled = false;
            comboBoxChanging.Enabled = false;
            buttonBuy.Enabled = false;

            if (comboBoxCountry.SelectedItem is not CountryItem selectedItem)
            {
                return;
            }

            var count = (int)numericUpDownCount.Value;
            var type = GetProxyType(comboBoxType.SelectedItem!.ToString())!.Value;
            var answer = await _px6Client.GetPriceAsync(count, period.Value, type);

            labelPrice.Text = $"Цена: {answer.price.Price} ₽";
            buttonBuy.Enabled = true;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            comboBoxType.Enabled = true;
            comboBoxCountry.Enabled = true;
            numericUpDownCount.Enabled = true;
            comboBoxChanging.Enabled = true;
        }
    }

    private static int? GetPeriod(string value)
    {
        return value switch
        {
            "1 неделя" => 7,
            "2 недели" => 14,
            "1 месяц" => 30,
            "2 месяца" => 60,
            "3 месяца" => 90,
            _ => null
        };
    }

    private static ProxyVersion? GetProxyType(string? version)
    {
        return version switch
        {
            "IPv6" => ProxyVersion.IPv6,
            "IPv4" => ProxyVersion.IPv4,
            "IPv4 Shared" => ProxyVersion.IPv4Shared,
            _ => null
        };
    }

    private static string GetCountryName(string iso2Code)
    {
        var region = new RegionInfo(iso2Code.ToUpper());
        return region.EnglishName;
    }
}