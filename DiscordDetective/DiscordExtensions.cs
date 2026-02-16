using System.Linq;
using DiscordApi.Models;
using DiscordDetective.Database.Models;
using DiscordDetective.Database.Models.DiscordAPI;

namespace DiscordDetective;

public static class DiscordExtensions
{
    public static RoleDbDTO ToDbDTO(this DiscordRole role, DiscordGuild guild)
    {
        var result = new RoleDbDTO
        {
            Id = role.Id,
            GuildId = guild.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = role.Permissions
        };

        return result;
    }

    public static RoleDbDTO ToDbDTO(this DiscordRole role, string guildId)
    {
        var result = new RoleDbDTO
        {
            Id = role.Id,
            GuildId = guildId,
            Name = role.Name,
            Description = role.Description,
            Permissions = role.Permissions
        };

        return result;
    }

    public static ChannelDbDTO ToDbDTO(this DiscordChannel channel)
    {
        var channelDbDTO = new ChannelDbDTO
        {
            Id = channel.Id,
            GuildId = channel.GuildId,
            Type = channel.Type,
            Name = channel.Name,
            ParentId = channel.ParentId,
            Position = channel.Position,
            LastMessageId = channel.LastMessageId,
            Flags = channel.Flags,
            Nsfw = channel.Nsfw,
            RateLimitPerUser = channel.RateLimitPerUser,
            Bitrate = channel.Bitrate,
            UserLimit = channel.UserLimit,
            RtcRegion = channel.RtcRegion,
        };

        return channelDbDTO;
    }

    public static GuildDbDTO ToDbDTO(this DiscordGuild guild)
    {
        var guildDb = new GuildDbDTO
        {
            Id = guild.Id,
            Name = guild.Name,
            Icon = guild.Icon,
            Banner = guild.Banner,
        };

        return guildDb;
    }

    public static DiscordGuild ToApiDTO(this GuildDbDTO guild)
    {
        var guildApi = new DiscordGuild
        {
            Id = guild.Id,
            Name = guild.Name,
            Icon = guild.Icon,
            Banner = guild.Banner,
        };

        return guildApi;
    }

    public static PermissionOverwriteDbDTO ToDbDTO(this DiscordPermissionOverwrite permission, string channelId)
    {
        var permissionDbDTO = new PermissionOverwriteDbDTO
        {
            Id = permission.Id,
            ChannelId = channelId,
            Type = permission.Type,
            Allow = permission.Allow,
            Deny = permission.Deny
        };

        return permissionDbDTO;
    }

    public static DiscordPermissionOverwrite ToApiDTO(this PermissionOverwriteDbDTO permission)
    {
        var permissionDbDTO = new DiscordPermissionOverwrite
        {
            Id = permission.Id,
            Type = permission.Type,
            Allow = permission.Allow,
            Deny = permission.Deny
        };

        return permissionDbDTO;
    }

    public static UserDbDTO ToDbDTO(this DiscordUser user)
    {
        var userDb = new UserDbDTO
        {
            Id = user.Id,
            Username = user.Username,
            GlobalName = user.GlobalName,
            Discriminator = user.Discriminator,
            Avatar = user.Avatar,
            Banner = user.Banner,
            AccentColor = user.AccentColor,
            Bio = user.Bio,
            Email = user.Email,
            Verified = user.Verified,
        };

        return userDb;
    }

    public static DiscordUser ToApiDTO(this UserDbDTO user)
    {
        var userDb = new DiscordUser
        {
            Id = user.Id,
            Username = user.Username,
            GlobalName = user.GlobalName,
            Discriminator = user.Discriminator,
            Avatar = user.Avatar,
            Banner = user.Banner,
            AccentColor = user.AccentColor,
            Bio = user.Bio,
            Email = user.Email,
            Verified = user.Verified,
        };

        return userDb;
    }

    public static GuildMemberDTO ToDbDTO(this DiscordMember member, DiscordGuild guild)
    {
        var memberDbDTO = new GuildMemberDTO
        {
            UserId = member.User.Id,
            GuildId = guild.Id,
            Nick = member.Nick,
            Avatar = member.Avatar,
            Banner = member.Banner,
            Roles = member.Roles,
            PremiumSince = member.PremiumSince.ToString(),
            JoinedAt = member.JoinedAt.ToString()
        };

        return memberDbDTO;
    }

    public static GuildMemberDTO ToDbDTO(this DiscordMember member, string guildId)
    {
        var memberDbDTO = new GuildMemberDTO
        {
            UserId = member.User.Id,
            GuildId = guildId,
            Nick = member.Nick,
            Avatar = member.Avatar,
            Banner = member.Banner,
            Roles = member.Roles,
            PremiumSince = member.PremiumSince.ToString(),
            JoinedAt = member.JoinedAt.ToString()
        };

        return memberDbDTO;
    }
}