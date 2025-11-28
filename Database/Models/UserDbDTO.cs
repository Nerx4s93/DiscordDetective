namespace DiscordDetective.Database.Models;

public class UserDbDTO
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? GlobalName { get; set; }
    public string Discriminator { get; set; } = "0";
    public string? Avatar { get; set; }
    public string? Banner { get; set; }
    public int? AccentColor { get; set; }
    public string? Bio { get; set; }
    public string? Email { get; set; }
    public bool? Verified { get; set; }
}