using System;
using System.Linq;
using System.Windows.Forms;

using DiscordDetective.Database.Models;

using Microsoft.VisualBasic;

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
        DeleteBotToolStripMenuItem.Enabled = listViewBots.SelectedItems.Count == 1;
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
            var selectedItem = listViewBots.SelectedItems[0];
            var selectedToken = selectedItem.Tag as string;

            var bot = _databaseContext.Bots.FirstOrDefault(b => b.Token == selectedToken);

            if (bot != null)
            {
                _databaseContext.Bots.Remove(bot);
                _databaseContext.SaveChanges();

                listViewBots.Items.Remove(selectedItem);

                MessageBox.Show("Бот удален успешно.", "Удаление бота", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при удалении бота: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    #endregion
}