using System;

using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.Database.Models;

[PrimaryKey(nameof(UserId), nameof(GuildId))]
public class GuildMemberDTO
{
    public string UserId { get; set; } = string.Empty;

    public string GuildId { get; set; } = string.Empty;

    public string? Nick { get; set; }

    public string? Avatar { get; set; }

    public string? Banner { get; set; }

    public string[] Roles { get; set; }

    public string? PremiumSince { get; set; }

    public string? JoinedAt { get; set; }
}
