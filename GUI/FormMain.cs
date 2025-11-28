using DiscordDetective.Database.Models;

using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DiscordDetective.GUI;

public partial class FormMain : Form
{
    private DatabaseContext _databaseContext;

    public FormMain()
    {
        InitializeComponent();
        _databaseContext = new();
        LoadBots();
    }

    #region Загрузка

    private void LoadBots()
    {
        try
        {
            var bots = _databaseContext.Bots.ToList();

            listViewBots.Items.Clear();

            foreach (var bot in bots)
            {
                var item = new ListViewItem
                {
                    Text = bot.UserId == null ? "Не загружено" : _databaseContext.Users.First(u => u.Id == bot.UserId).Username,
                    Tag = bot.Token
                };
                listViewBots.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке ботов: {ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Страница "Боты"

    private void listViewBots_SelectedIndexChanged(object sender, EventArgs e)
    {
        var isItemsSelected = listViewBots.SelectedItems.Count > 0;
        DeleteBotToolStripMenuItem.Enabled = isItemsSelected;
        UpdateBotToolStripMenuItem.Enabled = isItemsSelected;
    }

    private async void AddBotToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var result = Interaction.InputBox("Введите токен бота:", "Добавление бота", "");
        if (string.IsNullOrEmpty(result))
        {
            return;
        }

        try
        {
            var bot = new BotDTO() { Token = result };

            using var context = new DatabaseContext();
            context.Bots.Add(bot);
            await context.SaveChangesAsync();

            var item = new ListViewItem
            {
                Text = bot.UserId == null ? "Не загружено" : _databaseContext.Users.First(u => u.Id == bot.UserId).Username,
                Tag = bot.Token
            };
            listViewBots.Items.Add(item);

            MessageBox.Show("Бот добавлен успешно.", "Добавление бота", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при добавлении бота: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DeleteBotToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var itemsToRemove = new List<ListViewItem>();

            foreach (ListViewItem selectedItem in listViewBots.SelectedItems)
            {
                var selectedToken = selectedItem.Tag as string;
                var bot = _databaseContext.Bots.FirstOrDefault(b => b.Token == selectedToken);

                if (bot != null)
                {
                    _databaseContext.Bots.Remove(bot);
                    itemsToRemove.Add(selectedItem);
                }
            }

            _databaseContext.SaveChanges();
            foreach (var item in itemsToRemove)
            {
                listViewBots.Items.Remove(item);
            }

            MessageBox.Show($"Удалено ботов: {itemsToRemove.Count}", "Удаление бота", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при удалении бота: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion
}