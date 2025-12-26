using DiscordDetective.Database.Models;
using DiscordApi.Models;

namespace DiscordDetective.DTOExtensions;

public static class GuildDTOExtensions
{
    public static GuildDbDTO ToDbDTO(this GuildApiDTO guild)
    {
        var guildDb = new GuildDbDTO()
        {
            Id = guild.Id,
            Name = guild.Name,
            Icon = guild.Icon,
            Banner = guild.Banner,
        };

        return guildDb;
    }

    public static GuildApiDTO ToApiDTO(this GuildDbDTO guild)
    {
        var guildApi = new GuildApiDTO()
        {
            Id = guild.Id,
            Name = guild.Name,
            Icon = guild.Icon,
            Banner = guild.Banner,
        };

        return guildApi;
    }
}
