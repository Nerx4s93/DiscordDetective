using System.Linq;
using DiscordApi.Models;
using DiscordDetective.Database.Models.DiscordAPI;

namespace DiscordDetective;

public static class DiscordExtensions
{
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
            PermissionOverwrites = channel.PermissionOverwrites.Select(p => p.ToDbDTO(channel.Id)).ToList()
        };

        return channelDbDTO;
    }

    public static ChannelApiDTO ToApiDTO(this ChannelDbDTO channel)
    {
        var channelDbDTO = new ChannelApiDTO
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
            PermissionOverwrites = channel.PermissionOverwrites.Select(p => p.ToApiDTO()).ToList()
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

    public static MessageDbDTO ToDbDTO(this MessageApiDTO message)
    {
        var messageDbDTO = new MessageDbDTO()
        {
            Id = message.Id,
            Content = message.Content,
            ChannelId = message.ChannelId,
            Timestamp = message.Timestamp,
            Author = message.Author.ToDbDTO()
        };

        return messageDbDTO;
    }

    public static MessageApiDTO ToApiDTO(this MessageDbDTO message)
    {
        var messageApiDTO = new MessageApiDTO()
        {
            Id = message.Id,
            Content = message.Content,
            ChannelId = message.ChannelId,
            Timestamp = message.Timestamp,
            Author = message.Author.ToApiDTO()
        };

        return messageApiDTO;
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
}