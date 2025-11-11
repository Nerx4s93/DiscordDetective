using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordDetective.API.Models;

public class User
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    [JsonProperty("global_name")]
    public string? GlobalName { get; set; }

    [JsonProperty("discriminator")]
    public string Discriminator { get; set; } = "0";

    [JsonProperty("avatar")]
    public string? Avatar { get; set; }

    [JsonProperty("banner")]
    public string? Banner { get; set; }

    [JsonProperty("accent_color")]
    public int? AccentColor { get; set; }

    [JsonProperty("bio")]
    public string? Bio { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("verified")]
    public bool? Verified { get; set; }

    public string FullUsername => Discriminator != "0"
        ? $"{Username}#{Discriminator}"
        : Username;

    public string DisplayName => GlobalName ?? Username;

    public string? AvatarUrl =>
        !string.IsNullOrEmpty(Avatar)
            ? $"https://cdn.discordapp.com/avatars/{Id}/{Avatar}.png"
            : null;

    public string? BannerUrl =>
        !string.IsNullOrEmpty(Banner)
            ? $"https://cdn.discordapp.com/banners/{Id}/{Banner}.png"
            : null;

    public string? AccentColorHex =>
        AccentColor.HasValue
            ? $"#{AccentColor.Value:X6}"
            : null;

    public async Task<Image?> GetAvatar(bool download = true)
    {
        if (string.IsNullOrEmpty(Avatar) || string.IsNullOrEmpty(Id))
        {
            return null;
        }

        var avatarFileName = $"{Avatar}.png";
        var fullPath = Path.Combine(DataManager.UsersAvatar, avatarFileName);

        if (File.Exists(fullPath))
        {
            return Image.FromFile(fullPath);
        }
        else if (download)
        {
            var proxy = new WebProxy
            {
                Address = new Uri("http://168.0.212.187:9772"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                userName: "khvpRd",
                password: "KTNp1s")
            };

            var httpClientHandler = new HttpClientHandler
            {
                Proxy = proxy,
                UseProxy = true
            };

            using var httpClient = new HttpClient(httpClientHandler);

            var avatarUrl = AvatarUrl;
            if (string.IsNullOrEmpty(avatarUrl))
            {
                return null;
            }

            var imageData = await httpClient.GetByteArrayAsync(avatarUrl);
            using var memoryStream = new MemoryStream(imageData);

            await File.WriteAllBytesAsync(fullPath, imageData);
            return Image.FromStream(memoryStream);
        }

        throw new Exception($"Аватара пользователя {Id} не загружен");
    }
}