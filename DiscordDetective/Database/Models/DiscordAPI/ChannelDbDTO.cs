using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscordDetective.Database.Models.DiscordAPI;

public sealed class ChannelDbDTO
{
    [Key]
    public string Id { get; set; } = null!;
    public string GuildId { get; set; } = null!;
    public int Type { get; set; }
    public string Name { get; set; } = null!;
    public string? ParentId { get; set; }
    public int Position { get; set; }
    public string? LastMessageId { get; set; }
    public int Flags { get; set; }
    public bool Nsfw { get; set; }
    public int? RateLimitPerUser { get; set; }
    public int? Bitrate { get; set; }
    public int? UserLimit { get; set; }
    public string? RtcRegion { get; set; }
    public List<PermissionOverwriteDbDTO> PermissionOverwrites { get; set; } = [];
}
