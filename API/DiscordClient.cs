using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;
using DiscordDetective.API.Models;

namespace DiscordDetective.API;

public class DiscordClient
{
    private readonly string _token;
    private readonly HttpClient _httpClient;

    private const string StoragePath = @"data\users";
    private const string GuildsStoragePath = @"data\guilds";
    private const string BaseUrl = "https://discord.com/api/v10";

    public User? CurrentUser { get; private set; }

    public DiscordClient(string token)
    {
        _token = token ?? throw new ArgumentNullException(nameof(token));
        _httpClient = new HttpClient();
        LoadUserData().Wait();
    }

    #region file

    private async Task LoadUserData()
    {
        var filePath = GetUserFilePath();

        if (File.Exists(filePath))  
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                CurrentUser = JsonConvert.DeserializeObject<User>(json)!;
                Console.WriteLine($"Загружен пользователь из кэша: {CurrentUser.DisplayName}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки из кэша: {ex.Message}");
            }
        }

        await UpdateUserDataAsync();
    }

    private async Task SaveUserDataAsync()
    {
        if (CurrentUser == null)
        {
            return;
        }

        var filePath = GetUserFilePath();
        var json = JsonConvert.SerializeObject(CurrentUser, Formatting.Indented);
        await File.WriteAllTextAsync(filePath, json);
    }

    #endregion

    #region user information

    public async Task UpdateUserDataAsync()
    {
        try
        {
            var user = await MakeRequestAsync<User>("users/@me");
            CurrentUser = user;

            await SaveUserDataAsync();

            Console.WriteLine($"Обновлены данные пользователя: {user.DisplayName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка обновления данных: {ex.Message}");
            throw;
        }
    }

    public async Task<List<Guild>> GetUserGuildsAsync()
    {
        return await MakeRequestAsync<List<Guild>>("users/@me/guilds") ?? [];
    }

    public async Task<List<Connection>> GetUserConnectionsAsync()
    {
        return await MakeRequestAsync<List<Connection>>("users/@me/connections") ?? [];
    }

    public async Task<UserData> UpdateAllDataAsync()
    {
        await UpdateUserDataAsync();
        var guilds = await GetUserGuildsAsync();
        var connections = await GetUserConnectionsAsync();

        return new UserData
        {
            User = CurrentUser!,
            Guilds = guilds,
            Connections = connections
        };
    }

    #endregion

    #region guilds information

    public async Task<List<Guild>> GetUserGuildsCachedAsync(bool forceUpdate = false)
    {
        var cacheFilePath = GetGuildsCacheFilePath();

        if (!forceUpdate && File.Exists(cacheFilePath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(cacheFilePath);
                var cachedGuilds = JsonConvert.DeserializeObject<List<Guild>>(json);

                if (cachedGuilds != null && cachedGuilds.Count > 0)
                {
                    Console.WriteLine($"Загружено {cachedGuilds.Count} серверов из кэша");
                    return cachedGuilds;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки серверов из кэша: {ex.Message}");
            }
        }

        var guilds = await GetUserGuildsAsync();
        await SaveGuildsToCacheAsync(guilds);

        return guilds;
    }

    private async Task SaveGuildsToCacheAsync(List<Guild> guilds)
    {
        try
        {
            var cacheFilePath = GetGuildsCacheFilePath();
            var json = JsonConvert.SerializeObject(guilds, Formatting.Indented);
            await File.WriteAllTextAsync(cacheFilePath, json);
            Console.WriteLine($"Сохранено {guilds.Count} серверов в кэш");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения серверов в кэш: {ex.Message}");
        }
    }

    public async Task<Guild?> GetGuildAsync(string guildId)
    {
        return await MakeRequestAsync<Guild>($"guilds/{guildId}");
    }

    public void ClearGuildsCache()
    {
        var cacheFilePath = GetGuildsCacheFilePath();
        if (File.Exists(cacheFilePath))
        {
            File.Delete(cacheFilePath);
            Console.WriteLine("Кэш серверов очищен");
        }
    }

    #endregion

    #region parsing methods

    /// <summary>
    /// Получить все сервера и выполнить парсинг
    /// </summary>
    public async Task<GuildAnalysis> AnalyzeGuildsAsync()
    {
        var guilds = await GetUserGuildsCachedAsync();

        return new GuildAnalysis
        {
            TotalGuilds = guilds.Count,
            OwnedGuilds = guilds.Count(g => g.Owner),
            Guilds = guilds,
            AnalysisDate = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Найти сервера по имени
    /// </summary>
    public async Task<List<Guild>> FindGuildsByNameAsync(string name)
    {
        var guilds = await GetUserGuildsCachedAsync();
        return guilds
            .Where(g => g.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Получить сервера где пользователь владелец
    /// </summary>
    public async Task<List<Guild>> GetOwnedGuildsAsync()
    {
        var guilds = await GetUserGuildsCachedAsync();
        return guilds.Where(g => g.Owner).ToList();
    }

    #endregion

    private async Task<T> MakeRequestAsync<T>(string endpoint)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{endpoint}");
        request.Headers.Add("Authorization", $"{_token}");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }

    private string GetUserFilePath()
    {
        var safeFileName = _token
            .Replace(".", "")
            .Replace("-", "")
            .Replace("_", "")
            .Replace(" ", "")
            .Replace("\\", "")
            .Replace("/", "")
            .Replace(":", "")
            .Replace("*", "")
            .Replace("?", "")
            .Replace("\"", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("|", "");

        if (safeFileName.Length > 100)
        {
            safeFileName = safeFileName.Substring(0, 100);
        }

        return Path.Combine(StoragePath, $"{safeFileName}.json");
    }

    private string GetGuildsCacheFilePath()
    {
        var safeFileName = _token
            .Replace(".", "")
            .Replace("-", "")
            .Replace("_", "")
            .Replace(" ", "")
            .Replace("\\", "")
            .Replace("/", "")
            .Replace(":", "")
            .Replace("*", "")
            .Replace("?", "")
            .Replace("\"", "")
            .Replace("<", "")
            .Replace(">", "")
            .Replace("|", "");

        if (safeFileName.Length > 100)
        {
            safeFileName = safeFileName.Substring(0, 100);
        }

        return Path.Combine(GuildsStoragePath, $"{safeFileName}_guilds.json");
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}