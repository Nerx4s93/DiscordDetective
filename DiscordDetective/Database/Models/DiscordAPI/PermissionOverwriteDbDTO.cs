using System.ComponentModel.DataAnnotations;

namespace DiscordDetective.Database.Models.DiscordAPI;

public sealed class PermissionOverwriteDbDTO
{
    [Key]
    public string Id { get; set; } = null!;
    public string ChannelId { get; set; } = null!;
    public int Type { get; set; }
    public string Allow { get; set; } = null!;
    public string Deny { get; set; } = null!;
}
