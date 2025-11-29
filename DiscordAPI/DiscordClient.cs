using System;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using DiscordDetective.DiscordAPI.Models;

namespace DiscordDetective.DiscordAPI;

public class DiscordClient : IDisposable
{
    private readonly string _token;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    private const string BaseUrl = "https://discord.com/api/v9";

    private UserApiDTO? _userApiDTO;

    public DiscordClient(string token)
    {
        _token = token ?? throw new ArgumentNullException(nameof(token));
        _httpClient = new HttpClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };
    }

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

    public void Dispose()
    {
        _httpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
