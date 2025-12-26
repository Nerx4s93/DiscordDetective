using DiscordDetective.Database.Models;
using DiscordDetective.DTOExtensions;
using DiscordDetective.Logging;
using DiscordApi;
using Px6Api;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordDetective.GUI;

public partial class FormMain : Form
{
    private Px6Client _px6Client = null!;

    private readonly ImageDatabase _imageDatabase = new("bots");
    private readonly ImageList _botsImageList = new() { ImageSize = new Size(48, 48) };

    private readonly ConsoleLogger _loggerService = new();

    public FormMain()
    {
        InitializeComponent();

        listViewBots.LargeImageList = _botsImageList;

        _ = LoadBotsAsync();
        _ = LoadProxyAsync();
    }

    #region Страница "Боты"

    #region Загрузка

    private async Task LoadBotsAsync()
    {
        try
        {
            var databaseContext = new DatabaseContext();
            var bots = await databaseContext.Bots.ToListAsync();

            if (InvokeRequired)
            {
                await InvokeAsync(() => UpdateBotsListView(bots));
            }
            else
            {
                UpdateBotsListView(bots);
            }
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync("Database", $"Ошибка при загрузке ботов: {ex}", LogLevel.Error);
        }
    }

    private void UpdateBotsListView(List<BotDTO> bots)
    {
        listViewBots.Items.Clear();
        _botsImageList.Images.Clear();

        var databaseContext = new DatabaseContext();
        foreach (var bot in bots)
        {
            var username = bot.UserId == null
                ? "Не загружено"
                : databaseContext.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Id == bot.UserId)?.Username ?? "Не найден";

            var imageKey = databaseContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == bot.UserId)?.Avatar;

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

    private void listViewBots_DoubleClick(object sender, EventArgs e)
    {
        if (listViewBots.SelectedItems.Count == 0)
        {
            return;
        }

        var selectedItem = listViewBots.SelectedItems[0];
        var token = selectedItem.Tag as string;
        new FormBot(_imageDatabase, token!).ShowAsync();
    }

    private void listViewBots_SelectedIndexChanged(object sender, EventArgs e)
    {
        var isItemsSelected = listViewBots.SelectedItems.Count > 0;
        DeleteBotToolStripMenuItem.Enabled = isItemsSelected;
        UpdateBotToolStripMenuItem.Enabled = isItemsSelected;
    }

    private async void AddBotToolStripMenuItem_Click(object sender, EventArgs e)
    {
        AddBotToolStripMenuItem.Enabled = false;

        try
        {
            await _loggerService.ClearAsync();

            var token = Interaction.InputBox("Введите токен бота:", "Добавление бота", "");
            if (string.IsNullOrEmpty(token))
            {
                await _loggerService.LogAsync("AddBot", "Отменено: пустой токен", LogLevel.Info);
                return;
            }

            var databaseContext = new DatabaseContext();
            var exists = await databaseContext.Bots.AnyAsync(b => b.Token == token);
            if (exists)
            {
                await _loggerService.LogAsync("AddBot", "Бот уже существует", LogLevel.Warning);
                return;
            }

            var bot = new BotDTO() { Token = token };
            databaseContext.Bots.Add(bot);
            await databaseContext.SaveChangesAsync();

            BeginInvoke(new Action(() =>
            {
                var item = new ListViewItem
                {
                    Text = bot.UserId == null ? "Не загружено" : "Загружено",
                    Tag = bot.Token
                };
                listViewBots.Items.Add(item);
            }));

            await _loggerService.LogAsync("AddBot", "Бот успешно добавлен", LogLevel.Info);
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync("AddBot", $"Ошибка: {ex}", LogLevel.Error);
        }
        finally
        {
            AddBotToolStripMenuItem.Enabled = true;
        }
    }

    private async void UpdateListToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdateListToolStripMenuItem.Enabled = false;

        try
        {
            await LoadBotsAsync();
        }
        finally
        {
            UpdateListToolStripMenuItem.Enabled = true;
        }
    }

    private async void UpdateBotToolStripMenuItem_Click(object sender, EventArgs e)
    {
        UpdateBotToolStripMenuItem.Enabled = false;

        await _loggerService.ClearAsync();

        try
        {
            var databaseContext = new DatabaseContext();
            var selectedItems = listViewBots.SelectedItems.Cast<ListViewItem>().ToList();

            await _loggerService.LogAsync("Update", $"Начато обновление {selectedItems.Count} ботов", LogLevel.Info);
            await _loggerService.LogEmptyLineAsync();

            for (var i = 0; i < selectedItems.Count; i++)
            {
                var selectedItem = selectedItems[i];
                var selectedToken = selectedItem.Tag as string;

                await _loggerService.LogAsync("Update", $"Бот #{i + 1}: начало обработки", LogLevel.Info);

                try
                {
                    var discordClient = new DiscordClient(selectedToken!);

                    await _loggerService.LogAsync("Update/Discord", "Получение данных бота...", LogLevel.Info);
                    var botData = await discordClient.GetMe();
                    await _loggerService.LogAsync("Update/Discord", $"Получены данные: {botData.Username}",
                        LogLevel.Info);

                    var userDb = botData.ToDbDTO();

                    var existingUser = await databaseContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == userDb.Id);

                    if (existingUser != null)
                    {
                        await _loggerService.LogAsync("Update/Database", $"Обновление пользователя {userDb.Username}",
                            LogLevel.Info);
                        databaseContext.Entry(existingUser).CurrentValues.SetValues(userDb);
                    }
                    else
                    {
                        await _loggerService.LogAsync("Update/Database",
                            $"Добавление нового пользователя {userDb.Username}", LogLevel.Info);
                        databaseContext.Users.Add(userDb);
                    }

                    await databaseContext.SaveChangesAsync();
                    await _loggerService.LogAsync("Update/Database", "Сохранение пользователя завершено",
                        LogLevel.Info);

                    var bot = await databaseContext.Bots.FirstAsync(b => b.Token == selectedToken);
                    bot.UserId = userDb.Id;

                    var avatar = _imageDatabase.Load(botData.Avatar!);
                    if (avatar == null)
                    {
                        await _loggerService.LogAsync("Update/Image", "Аватар бота не згружен, начало загрузки...",
                            LogLevel.Info);
                        var botAvatar = await discordClient.GetAvatar();
                        if (botAvatar != null)
                        {
                            _imageDatabase.Store(botAvatar, botData.Avatar!, new Size(48, 48));
                            await _loggerService.LogAsync("Update/Image", "Аватар сохранен", LogLevel.Info);
                        }
                        else
                        {
                            await _loggerService.LogAsync("Update/Image", "Не удалось загрузить аватар",
                                LogLevel.Warning);
                        }
                    }
                    else
                    {
                        await _loggerService.LogAsync("Update/Image", "Аватар бота уже загружен", LogLevel.Info);
                    }

                    await _loggerService.LogAsync("Update",
                        $"Бот {botData.Username} успешно обновлен ({i + 1}/{selectedItems.Count})",
                        LogLevel.Info);

                    await _loggerService.LogEmptyLineAsync();
                }
                catch (Exception botEx)
                {
                    await _loggerService.LogAsync("Update",
                        $"Ошибка при обновлении бота #{i + 1}: {botEx.Message}",
                        LogLevel.Error);
                }
            }

            await databaseContext.SaveChangesAsync();
            await _loggerService.LogAsync("Update/Database", "Все изменения сохранены", LogLevel.Info);

            await _loggerService.LogAsync("Update/UI", "Обновление интерфейса...", LogLevel.Info);
            await LoadBotsAsync();

            await _loggerService.LogAsync("Update",
                $"Успешно обновлено {selectedItems.Count} ботов",
                LogLevel.Info);
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync("Update",
                $"Критическая ошибка: {ex}",
                LogLevel.Error);
        }
        finally
        {
            UpdateBotToolStripMenuItem.Enabled = false;
        }
    }

    private async void DeleteBotToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DeleteBotToolStripMenuItem.Enabled = false;

        try
        {
            await _loggerService.ClearAsync();

            var databaseContext = new DatabaseContext();
            var selectedItems = listViewBots.SelectedItems.Cast<ListViewItem>().ToList();

            if (MessageBox.Show($"Удалить {selectedItems.Count} ботов?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                await _loggerService.LogAsync("Delete", "Отменено", LogLevel.Info);
                return;
            }

            await _loggerService.LogAsync("Delete", $"Удаление {selectedItems.Count} ботов", LogLevel.Info);

            foreach (var item in selectedItems)
            {
                var token = item.Tag as string;

                try
                {
                    var deleted = await databaseContext.Bots
                        .Where(b => b.Token == token)
                        .ExecuteDeleteAsync();

                    if (deleted > 0)
                    {
                        await _loggerService.LogAsync("Delete", $"Удален бот: {MaskToken(token!)}", LogLevel.Info);
                        listViewBots.Items.Remove(item);
                    }
                    else
                    {
                        await _loggerService.LogAsync("Delete", $"Бот не найден: {MaskToken(token!)}",
                            LogLevel.Warning);
                    }
                }
                catch (Exception ex)
                {
                    await _loggerService.LogAsync("Delete", $"Ошибка удаления: {ex}", LogLevel.Error);
                }
            }

            await _loggerService.LogAsync("Delete", "Удаление завершено", LogLevel.Info);
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync("Delete", $"Ошибка: {ex}", LogLevel.Error);
        }
        finally
        {
            DeleteBotToolStripMenuItem.Enabled = true;
        }
    }

    #endregion

    #region Страница "Прокси"

    private async Task LoadProxyAsync()
    {
        try
        {
            await _loggerService.ClearAsync();

            await _loggerService.LogAsync("Px6Api", $"Создание api клиента...", LogLevel.Info);
            var token = await File.ReadAllTextAsync("px6key.txt");
            _px6Client = new Px6Client(token);

            await _loggerService.LogAsync("Px6Api", $"Получение списка прокси...", LogLevel.Info);
            var proxies = (await _px6Client.GetProxiesAsync()).proxies;
            foreach (var proxy in proxies)
            {
                proxyListView.AddProxy(proxy.Value);
            }

            await _loggerService.LogAsync("Px6Api", $"Загрузка завершена", LogLevel.Info);
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync("Delete", $"Ошибка: {ex}", LogLevel.Error);
        }
    }

    #endregion

    private static string MaskToken(string token)
    {
        if (string.IsNullOrEmpty(token) || token.Length < 10)
        {
            return "***";
        }

        return $"{token[..6]}...{token[^6..]}";
    }
}