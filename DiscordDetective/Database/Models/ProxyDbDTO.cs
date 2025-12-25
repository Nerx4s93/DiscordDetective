using System.ComponentModel.DataAnnotations;

namespace DiscordDetective.Database.Models;

public class ProxyDbDTO
{
    [Key]
    public string IP { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public string? Login { get; set; }
    public string? Password { get; set; }
}
