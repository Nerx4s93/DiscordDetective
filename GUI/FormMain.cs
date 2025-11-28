using DiscordDetective.Database.Models;
using DiscordDetective.DiscordAPI;

using Microsoft.EntityFrameworkCore;
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
        ClearLog();

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

            Log("Ok", "Бот добавлен успешно");
        }
        catch (Exception ex)
        {
            Log("Error", $"Ошибка при добавлении бота: {ex.Message}");
        }
    }

    private async void UpdateBotToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ClearLog();

        try
        {
            for (var i = 0; i < listViewBots.SelectedItems.Count; i++)
            {
                var selectedItem = listViewBots.SelectedItems[i];
                var selectedToken = selectedItem.Tag as string;

                var discordClient = new DiscordClient(selectedToken!);
                var botData = await discordClient.GetMe();

                var userDb = botData.ToDbDTO();

                var existingUser = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Id == userDb.Id);

                if (existingUser != null)
                {
                    _databaseContext.Entry(existingUser).CurrentValues.SetValues(userDb);
                    Log("Progress", $"Обновлены данные бота {i + 1}/{listViewBots.SelectedItems.Count}");
                }
                else
                {
                    _databaseContext.Users.Add(userDb);
                    Log("Progress", $"Добавлен новый бот {i + 1}/{listViewBots.SelectedItems.Count}");
                }

                var bot = _databaseContext.Bots.First(b => b.Token == selectedToken);
                bot.UserId = userDb.Id;
            }

            await _databaseContext.SaveChangesAsync();
            Log("Ok", $"Обновлены данные у {listViewBots.SelectedItems.Count} ботов");
        }
        catch (Exception ex)
        {
            Log("Error", $"Ошибка при обновлении данных ботов: {ex.Message}");
        }
    }

    private async void DeleteBotToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ClearLog();

        try
        {
            var itemsToRemove = new List<ListViewItem>();

            for (var i = 0; i < listViewBots.SelectedItems.Count; i++)
            {
                var selectedItem = listViewBots.SelectedItems[i];
                var selectedToken = selectedItem.Tag as string;
                var bot = _databaseContext.Bots.FirstOrDefault(b => b.Token == selectedToken);

                if (bot != null)
                {
                    _databaseContext.Bots.Remove(bot);
                    itemsToRemove.Add(selectedItem);
                }

                Log("Progress", $"Удаление ботов: {i + 1}/{listViewBots.SelectedItems.Count}");
            }

            await _databaseContext.SaveChangesAsync();
            foreach (var item in itemsToRemove)
            {
                listViewBots.Items.Remove(item);
            }

            Log("Ok", $"Удалено ботов: {itemsToRemove.Count}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при удалении бота: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Log

    private void Log(string status, string message)
    {
        richTextBoxLogs.Text += status + ": " + message + "\n";
    }

    private void ClearLog()
    {
        richTextBoxLogs.Clear(); 
    }

    #endregion
}