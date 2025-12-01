using DiscordDetective.Database.Models;
using DiscordDetective.DiscordAPI;
using DiscordDetective.DTOExtensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordDetective.GUI;

public partial class FormMain : Form
{
    private DatabaseContext _databaseContext;
    private ImageDatabase _imageDatabase;
    private ImageList _botsImageList;

    public FormMain()
    {
        InitializeComponent();

        _botsImageList = new() { ImageSize = new Size(48, 48) };
        listViewBots.LargeImageList = _botsImageList;

        _databaseContext = new();
        _imageDatabase = new("bots");

        _ = LoadBotsAsync();
    }

    #region Загрузка

    private async Task LoadBotsAsync()
    {
        try
        {
            var bots = await _databaseContext.Bots.ToListAsync();

            if (InvokeRequired)
            {
                Invoke(() => UpdateBotsListView(bots));
            }
            else
            {
                UpdateBotsListView(bots);
            }
        }
        catch (Exception ex)
        {
            Log("Error", $"Ошибка при загрузке ботов: {ex}");
        }
    }

    private void UpdateBotsListView(List<BotDTO> bots)
    {
        listViewBots.Items.Clear();
        _botsImageList.Images.Clear();

        foreach (var bot in bots)
        {
            var username = bot.UserId == null
                ? "Не загружено"
                : _databaseContext.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Id == bot.UserId)?.Username ?? "Не найден";

            var imageKey = _databaseContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == bot.UserId)?.Avatar;

            if (imageKey != null)
            {
                var image = _imageDatabase.Load(imageKey) ?? SystemIcons.Error.ToBitmap();
                _botsImageList.Images.Add(imageKey, image);
            }

            var item = new ListViewItem
            {
                ImageKey = imageKey ?? "",
                Text = username,
                Tag = bot.Token
            };
            listViewBots.Items.Add(item);
        }
    }

    #endregion

    #region Страница "Боты"

    private void listViewBots_DoubleClick(object sender, EventArgs e)
    {
        if (listViewBots.SelectedItems.Count == 0)
        {
            return;
        }

        var selectedItem = listViewBots.SelectedItems[0];
        var token = selectedItem.Tag as string;
        new FormBot(_databaseContext, token!).ShowAsync();
    }

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

            _databaseContext.Bots.Add(bot);
            await _databaseContext.SaveChangesAsync();

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
            Log("Error", $"Ошибка при добавлении бота: {ex}");
        }
    }

    private void UpdateListToolStripMenuItem_Click(object sender, EventArgs e)
    {
        _ = LoadBotsAsync();
    }

    private async void UpdateBotToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ClearLog();

        try
        {
            var selectedItems = listViewBots.SelectedItems.Cast<ListViewItem>().ToList();

            for (var i = 0; i < selectedItems.Count; i++)
            {
                var selectedItem = selectedItems[i];
                var selectedToken = selectedItem.Tag as string;

                var discordClient = new DiscordClient(selectedToken!);

                var botData = await discordClient.GetMe();
                var userDb = botData.ToDbDTO();

                var existingUser = await _databaseContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userDb.Id);
                if (existingUser != null)
                {
                    _databaseContext.Entry(existingUser).CurrentValues.SetValues(userDb);
                }
                else
                {
                    _databaseContext.Users.Add(userDb);
                }
                await _databaseContext.SaveChangesAsync();

                var bot = _databaseContext.Bots.First(b => b.Token == selectedToken);
                bot.UserId = userDb.Id;

                var avatar = _imageDatabase.Load(botData.Avatar!);
                if (avatar == null)
                {
                    var botAvatar = await discordClient.GetAvatar();
                    _imageDatabase.Store(botAvatar!, botData.Avatar!, new Size(48, 48));
                }

                Log("Progress", $"Обновлены данные бота {i + 1}/{selectedItems.Count}");
            }

            await _databaseContext.SaveChangesAsync();
            await LoadBotsAsync();
            Log("Ok", $"Обновлены данные у {selectedItems.Count} ботов");
        }
        catch (Exception ex)
        {
            Log("Error", $"Ошибка при обновлении данных ботов: {ex}");
        }
    }

    private async void DeleteBotToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ClearLog();

        try
        {
            var selectedItems = listViewBots.SelectedItems.Cast<ListViewItem>().ToList();

            for (var i = 0; i < selectedItems.Count; i++)
            {
                var selectedItem = selectedItems[i];
                var selectedToken = selectedItem.Tag as string;
                var rowsDeleted = await _databaseContext.Bots
                    .Where(b => b.Token == selectedToken)
                    .ExecuteDeleteAsync();

                Log("Progress", $"Удаление ботов: {i + 1}/{selectedItems.Count}");
            }

            await _databaseContext.SaveChangesAsync();
            foreach (var item in selectedItems)
            {
                listViewBots.Items.Remove(item);
            }

            Log("Ok", $"Удалено ботов: {selectedItems.Count}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при удалении бота: {ex}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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