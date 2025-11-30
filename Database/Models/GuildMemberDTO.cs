using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.Database.Models;

[PrimaryKey(nameof(UserId), nameof(GuildId))]
public class GuildMemberDTO
{
    public string UserId { get; set; } = string.Empty;

    public string GuildId { get; set; } = string.Empty;
}
