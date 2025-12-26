using DiscordDetective.Database.Models;
using DiscordApi.Models;

namespace DiscordDetective.DTOExtensions;

public static class UserDTOExtensions
{
    public static UserDbDTO ToDbDTO(this UserApiDTO user)
    {
        var userDb = new UserDbDTO()
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
        var userDb = new UserApiDTO()
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
