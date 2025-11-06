using System.IO;

namespace DiscordDetective;

internal static class DataManager
{
    public const string Users = @"data\users";
    public const string UsersAvatar = @"data\users\avatar";

    public static void CheckDirectories()
    {
        CheckDirrectory("data");
        CheckDirrectory(Users);
        CheckDirrectory(UsersAvatar);
        CheckDirrectory(@"guilds\users");
    }

    private static void CheckDirrectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}