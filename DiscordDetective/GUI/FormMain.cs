using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordApi;
using DiscordDetective.Database.Models;
using DiscordDetective.DTOExtensions;
using DiscordDetective.Logging;
using DiscordDetective.Pipeline;
using DiscordDetective.Pipeline.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Px6Api;
using StackExchange.Redis;

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
    }

    #region Страница "Прокси"

    private bool _prolongMenuOpened;
    private bool _changeTypeMenuOpened;

    private async Task LoadProxyPageAsync()
    {
        try
        {
            await _loggerService.ClearAsync();

            var token = await File.ReadAllTextAsync("px6key.txt");
            _px6Client = new Px6Client(token);
            proxyListView.Px6Client = _px6Client;

            await LoadProxyAsync(_px6Client);
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync("Delete", $"Ошибка: {ex}", LogLevel.Error);
        }
    }

    private async Task LoadProxyAsync(Px6Client px6Client)
    {
        proxyListView.Clear();
        var proxies = (await px6Client.GetProxiesAsync()).proxies;
        foreach (var proxy in proxies)
        {
            proxyListView.AddProxy(proxy.Value);
        }
    }

    private void proxyListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selected = proxyListView.SelectedProxies.Count > 0;
        buttonProlong.Enabled = selected;
        buttonChangeType.Enabled = selected;
        buttonDelete.Enabled = selected;
    }

    private void buttonProlong_Click(object sender, EventArgs e)
    {
        if (_prolongMenuOpened)
        {
            contextMenuStripProlong.Close();
            _prolongMenuOpened = false;
            return;
        }

        _prolongMenuOpened = true;
        contextMenuStripProlong.Show(buttonProlong, 0, buttonProlong.Height);
    }

    private void buttonChangeType_Click(object sender, EventArgs e)
    {
        if (_changeTypeMenuOpened)
        {
            contextMenuStripChangeType.Close();
            _changeTypeMenuOpened = false;
            return;
        }

        _changeTypeMenuOpened = true;
        contextMenuStripChangeType.Show(buttonChangeType, 0, buttonChangeType.Height
        );
    }

    #region contextMenuStripProlong

    private void buttonProlong3Days_Click(object sender, EventArgs e) => ProlongProxy(3);

    private void buttonProlongWeek_Click(object sender, EventArgs e) => ProlongProxy(7);

    private void buttonProlong2Weeks_Click(object sender, EventArgs e) => ProlongProxy(14);

    private void buttonProlongMonth_Click(object sender, EventArgs e) => ProlongProxy(30);

    private void buttonProlong2Month_Click(object sender, EventArgs e) => ProlongProxy(60);

    private void buttonProlong3Month_Click(object sender, EventArgs e) => ProlongProxy(90);

    private async void ProlongProxy(int days)
    {
        try
        {
            buttonProlong.Enabled = false;

            var selectedItems = proxyListView.SelectedItems;
            var selectedProxies = proxyListView.SelectedProxies;

            var ids = selectedProxies.Select(p => p.Id).ToList();
            await _px6Client.ProlongProxyAsync(days, ids);

            foreach (var proxy in selectedProxies)
            {
                var dateTimeEnd = DateTime.Parse(proxy.DateEnd);
                dateTimeEnd = dateTimeEnd.AddDays(days);
                proxy.DateEnd = dateTimeEnd.ToString("yyyy-MM-dd HH:mm:ss");
            }

            foreach (var item in selectedItems)
            {
                item.UpdateUI();
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            buttonProlong.Enabled = true;
        }
    }

    #endregion

    #region contextMenuStripChangeType

    private void buttonTypeSocks5_Click(object sender, EventArgs e) => ChangeProxyType(ProxyProtocol.Socks);

    private void buttonTypeHttp_Click(object sender, EventArgs e) => ChangeProxyType(ProxyProtocol.Http);

    private async void ChangeProxyType(ProxyProtocol proxyProtocol)
    {
        try
        {
            buttonChangeType.Enabled = false;

            var selectedItems = proxyListView.SelectedItems;
            var selectedProxies = proxyListView.SelectedProxies;

            var ids = selectedProxies.Select(p => p.Id).ToList();
            await _px6Client.SetProxyTypeAsync(ids, proxyProtocol);

            foreach (var proxy in selectedProxies)
            {
                proxy.Type = proxyProtocol.ToString().ToLower();
            }

            foreach (var item in selectedItems)
            {
                item.UpdateUI();
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            buttonChangeType.Enabled = true;
        }
    }

    #endregion

    private void buttonUpdateProxyList_Click(object sender, EventArgs e) => _ = LoadProxyAsync(_px6Client);

    private async void buttonDelete_Click(object sender, EventArgs e)
    {
        var selectedProxies = proxyListView.SelectedProxies;

        var result = MessageBox.Show(
            $"Вы уверены в удалении {selectedProxies.Count} серверов",
            "Удаление прокси",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
        {
            return;
        }

        try
        {
            buttonDelete.Enabled = false;

            var ids = selectedProxies.Select(p => p.Id).ToList();
            await _px6Client.DeleteProxyAsync(ids);

            MessageBox.Show(
                $"Удалено {selectedProxies.Count} серверов",
                "Удаление прокси",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            proxyListView.RemoveSelected();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            buttonDelete.Enabled = true;
        }
    }

    private void buttonBuy_Click(object sender, EventArgs e)
    {
        var form = new FormBuyProxy(_px6Client);
        var result = form.ShowDialog();

        if (result == DialogResult.OK)
        {
            foreach (var proxy in form.BuyResult!)
            {
                proxyListView.AddProxy(proxy);
            }
        }
    }

    #endregion

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

    #region Страница "Выкачивание"

    private IConnectionMultiplexer _connection = null!;
    private IDatabase _database = null!;
    private RedisTaskQueue _redisTaskQueue = null!;
    private RedisEventBus _redisEventBus = null!;

    #region Загрузка

    private async Task LoadTasksAsync()
    {
        try
        {
            _connection = await RedisConnector.ConnectAsync();
            _database = _connection.GetDatabase();
            _redisTaskQueue = new RedisTaskQueue(_database);
            _redisEventBus = new RedisEventBus(_connection);
            _redisEventBus.Subscribe(RedisEventHandler);
            await LoadPipeLineTasks();
        }
        catch (Exception ex)
        {
            await _loggerService.LogAsync("Database", $"Ошибка при загрузке ботов: {ex}", LogLevel.Error);
        }
    }

    private async Task LoadPipeLineTasks()
    {
        await SetPipelineTasks(PipelineTaskType.DiscoverGuildChannels);
        await SetPipelineTasks(PipelineTaskType.DownloadChannelMessages);
        await SetPipelineTasks(PipelineTaskType.ProcessMessagesWithAi);
        await SetPipelineTasks(PipelineTaskType.PersistStructuredData);
    }

    private async Task SetPipelineTasks(PipelineTaskType pipelineEventType)
    {
        var tasks = await _redisTaskQueue.GetTasks(pipelineEventType);

        var listView = GetListView(pipelineEventType);

        foreach (var task in tasks)
        {
            var item = new ListViewItem(task.Id.ToString())
            {
                Tag = task.Id
            };

            _items[task.Id] = item; 
            listView.BeginInvoke(() =>
            {
                listView.Items.Add(item);
            });
        }
    }

    #endregion

    private async void buttonAddServerTask_Click(object sender, EventArgs e)
    {
        if (!_connection.IsConnected)
        {
            return;
        }

        var text = Interaction.InputBox(
            "Введите id сервера",
            "Новый задача",
            ""
        );

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        var task = new PipelineTask
        {
            Id = Guid.NewGuid(),
            Type = PipelineTaskType.DiscoverGuildChannels,
            Payload = text
        };

        await _redisTaskQueue.EnqueueAsync(task);
        await _redisEventBus.PublishEvent(task, PipelineTaskProgress.New);
    }

    private async void buttonStartPipeline_Click(object sender, EventArgs e)
    {
        await _loggerService.LogAsync("Pipeline", "Подготовка");

        var discordWorkers = await GetDiscordWorkers();

        if (discordWorkers.Count == 0)
        {
            return;
        }

        await _loggerService.LogAsync("Pipeline", $"Создан {discordWorkers.Count} DiscordWorker");
        var aiWorkers = new List<IWorker> { new AiWorker() };
        await _loggerService.LogAsync("Pipeline", $"Создан {aiWorkers.Count} AiWorker");
        var dataWorkers = new List<IWorker> { new DataPersistWorker() };
        await _loggerService.LogAsync("Pipeline", $"Создан {dataWorkers.Count} DataPersistWorker");

        var pipelineManager = new PipelineManager(_redisTaskQueue, _redisEventBus, discordWorkers, aiWorkers, dataWorkers);
        _ = pipelineManager.RunAsync(CancellationToken.None);
    }

    private async Task<List<IWorker>> GetDiscordWorkers()
    {
        var databaseContext = new DatabaseContext();

        var bots = await databaseContext.Bots.ToListAsync();
        var proxies = (await _px6Client.GetProxiesAsync()).proxies;

        var result = new List<IWorker>();
        foreach (var bot in bots)
        {
            var proxy = proxies.FirstOrDefault(p => p.Value.Description == bot.UserId).Value;

            if (proxy == null)
            {
                continue;
            }

            var proxyInfo = new ProxyInfo()
            {
                Host = proxy.Ip,
                Port = int.Parse(proxy.Port),
                Username = proxy.User,
                Password = proxy.Password
            };

            var client = new DiscordClient(bot.Token, proxyInfo);
            try
            {
                await client.GetMe();
                result.Add(new DiscordWorker(client));
            }
            catch (Exception exception)
            {
                await _loggerService.LogAsync("Pipeline", $"Ошибка при загрузке бота: {exception}", LogLevel.Error);
                throw;
            }
        }

        return result;
    }

    #region События Pipeline

    private readonly Dictionary<Guid, ListViewItem> _items = new();

    private void RedisEventHandler(PipelineEvent pipelineEvent)
    {
        switch (pipelineEvent.Progress)
        {
            case PipelineTaskProgress.New:
                AddPipelineTask(pipelineEvent);
                return;
            case PipelineTaskProgress.InProgress:
                MakeInProgressPipelineTask(pipelineEvent);
                return;
            case PipelineTaskProgress.End:
                DeletePipelineTask(pipelineEvent);
                return;
        }
    }

    private void AddPipelineTask(PipelineEvent pipelineEvent)
    {
        var listView = GetListView(pipelineEvent.Type);
        var item = new ListViewItem(pipelineEvent.TaskId.ToString())
        {
            Tag = pipelineEvent.TaskId
        };

        _items[pipelineEvent.TaskId] = item;
        listView.Invoke(() =>
        {
            listView.Items.Add(item);
        });
    }

    private void MakeInProgressPipelineTask(PipelineEvent pipelineEvent)
    {
        if (_items.TryGetValue(pipelineEvent.TaskId, out var item))
        {
            item.ForeColor = Color.DodgerBlue;
        }
    }

    private void DeletePipelineTask(PipelineEvent pipelineEvent)
    {
        var listView = GetListView(pipelineEvent.Type);
        if (_items.Remove(pipelineEvent.TaskId, out var item))
        {
            listView.Invoke(() =>
            {
                listView.Items.Remove(item);
            });
        }
    }

    private ListView GetListView(PipelineTaskType pipelineTaskType)
    {
        return pipelineTaskType switch
        {
            PipelineTaskType.DiscoverGuildChannels => listViewPipelineDiscoverGuildChannels,
            PipelineTaskType.DownloadChannelMessages => listViewPipelineDownloadChannelMessages,
            PipelineTaskType.ProcessMessagesWithAi => listViewPipelineProcessMessagesWithAi,
            PipelineTaskType.PersistStructuredData => listViewPipelinePersistStructuredData,
            _ => throw new ArgumentOutOfRangeException(nameof(pipelineTaskType), pipelineTaskType, null)
        };
    }

    #endregion

    #endregion

    private static string MaskToken(string token)
    {
        if (string.IsNullOrEmpty(token) || token.Length < 10)
        {
            return "***";
        }

        return $"{token[..6]}...{token[^6..]}";
    }

    private void FormMain_Shown(object sender, EventArgs e)
    {
        listViewBots.LargeImageList = _botsImageList;
        _ = LoadProxyPageAsync();
        _ = LoadBotsAsync();
        _ = LoadTasksAsync();
    }
}