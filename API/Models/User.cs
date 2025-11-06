using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

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

    public async Task<Image?> GetAvatar()
    {
        if (string.IsNullOrEmpty(Avatar) || string.IsNullOrEmpty(Id))
        {
            return null;
        }

        try
        {
            var avatarFileName = $"{Avatar}.png";
            var fullPath = Path.Combine(DataManager.UsersAvatar, avatarFileName);

            byte[] imageData;
            if (File.Exists(fullPath))
            {
                imageData = await File.ReadAllBytesAsync(fullPath);
            }
            else
            {
                using var httpClient = new HttpClient();

                var avatarUrl = AvatarUrl;
                if (string.IsNullOrEmpty(avatarUrl))
                {
                    return null;
                }

                imageData = await httpClient.GetByteArrayAsync(avatarUrl);

                await File.WriteAllBytesAsync(fullPath, imageData);
            }

            using var memoryStream = new MemoryStream(imageData);
            return Image.FromStream(memoryStream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении аватара для пользователя {Id}: {ex.Message}");
            return null;
        }
    }
}