using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.Database.Models.DiscordAPI;

[PrimaryKey(nameof(Id), nameof(GuildId))]
public class RoleDbDTO
{
    public string Id { get; set; } = null!;

    public string GuildId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Permissions { get; set; } = null!;
}