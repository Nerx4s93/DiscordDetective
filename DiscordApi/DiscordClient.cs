using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using DiscordApi.Models;

namespace DiscordApi;

public class DiscordClient(string token) : IDisposable
{
    private readonly string _token = token ?? throw new ArgumentNullException(nameof(token));
    private readonly HttpClient _httpClient = new();
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    private const string BaseUrl = "https://discord.com/api/v9";

    private UserApiDTO? _userApiDTO;

    #region Информация о боте

    public async Task<UserApiDTO> GetMe()
    {
        _userApiDTO = await MakeRequestAsync<UserApiDTO>("users/@me");
        return _userApiDTO;
    }

    public async Task<Image?> GetAvatar()
    {
        var user = _userApiDTO ?? await GetMe();

        if (string.IsNullOrEmpty(user.Avatar))
        {
            return null;
        }

        var response = await MakeRequestAsync(user.AvatarUrl!);
        await using var stream = await response.Content.ReadAsStreamAsync();
        return Image.FromStream(stream);
    }

    #endregion

    #region Сервера

    public async Task<IReadOnlyList<GuildApiDTO>> GetGuildsAsync()
    {
        return await MakeRequestAsync<IReadOnlyList<GuildApiDTO>>("users/@me/guilds");
    }

    public async Task<IReadOnlyList<ChannelApiDTO>> GetGuildChannelsAsync(string guildId)
    {
        if (string.IsNullOrWhiteSpace(guildId))
        {
            throw new ArgumentNullException(nameof(guildId));
        }

        return await MakeRequestAsync<IReadOnlyList<ChannelApiDTO>>($"guilds/{guildId}/channels");
    }

    #endregion

    #region Формирвание запроса

    private async Task<T> MakeRequestAsync<T>(string endpoint)
    {
        var response = await MakeRequestAsync($"{BaseUrl}/{endpoint}");

        await using var stream = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);

        return result ?? throw new InvalidOperationException("Failed to deserialize response");
    }

    private async Task<HttpResponseMessage> MakeRequestAsync(string url)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Authorization", $"{_token}");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return response;
    }

    #endregion

    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
