using System.Linq;
using DiscordApi.Models;
using DiscordDetective.Database.Models;
using DiscordDetective.Database.Models.DiscordAPI;

namespace DiscordDetective;

public static class DiscordExtensions
{
    public static RoleDbDTO ToDbDTO(this RoleApiDTO role, GuildApiDTO guild)
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

    public static RoleDbDTO ToDbDTO(this RoleApiDTO role, string guildId)
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

    public static ChannelDbDTO ToDbDTO(this ChannelApiDTO channel)
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

    public static GuildDbDTO ToDbDTO(this GuildApiDTO guild)
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

    public static GuildApiDTO ToApiDTO(this GuildDbDTO guild)
    {
        var guildApi = new GuildApiDTO
        {
            Id = guild.Id,
            Name = guild.Name,
            Icon = guild.Icon,
            Banner = guild.Banner,
        };

        return guildApi;
    }

    public static PermissionOverwriteDbDTO ToDbDTO(this PermissionOverwriteApiDTO permission, string channelId)
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

    public static PermissionOverwriteApiDTO ToApiDTO(this PermissionOverwriteDbDTO permission)
    {
        var permissionDbDTO = new PermissionOverwriteApiDTO
        {
            Id = permission.Id,
            Type = permission.Type,
            Allow = permission.Allow,
            Deny = permission.Deny
        };

        return permissionDbDTO;
    }

    public static UserDbDTO ToDbDTO(this UserApiDTO user)
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

    public static UserApiDTO ToApiDTO(this UserDbDTO user)
    {
        var userDb = new UserApiDTO
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

    public static GuildMemberDTO ToDbDTO(this MemberApiDTO member, GuildApiDTO guild)
    {
        var memberDbDTO = new GuildMemberDTO
        {
            UserId = member.User.Id,
            GuildId = guild.Id,
            Nick = member.Nick,
            Avatar = member.Avatar,
            Banner = member.Banner,
            Roles = member.Roles,
            PremiumSince = member.PremiumSince,
            JoinedAt = member.JoinedAt
        };

        return memberDbDTO;
    }

    public static GuildMemberDTO ToDbDTO(this MemberApiDTO member, string guildId)
    {
        var memberDbDTO = new GuildMemberDTO
        {
            UserId = member.User.Id,
            GuildId = guildId,
            Nick = member.Nick,
            Avatar = member.Avatar,
            Banner = member.Banner,
            Roles = member.Roles,
            PremiumSince = member.PremiumSince,
            JoinedAt = member.JoinedAt
        };

        return memberDbDTO;
    }
}