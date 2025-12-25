using System.ComponentModel.DataAnnotations;

namespace DiscordDetective.Database.Models;

public class GuildDbDTO
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Banner { get; set; }
}
