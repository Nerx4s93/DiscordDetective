using System.ComponentModel.DataAnnotations;

namespace DiscordDetective.Database.Models;

public class BotDTO
{
    [Key]
    public string Token { get; set; } = string.Empty;
    public string? UserId { get; set; }
}
