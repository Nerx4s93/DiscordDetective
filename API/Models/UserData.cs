using System.Collections.Generic;

using DiscordDetective.API.Models;

namespace DiscordDetective.API;

public class UserData
{
    public required User User { get; set; }
    public List<Guild> Guilds { get; set; } = new();
    public List<Connection> Connections { get; set; } = new();
}