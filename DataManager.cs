using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using DiscordDetective.API.Models;

namespace DiscordDetective;

internal static class DataManager
{
    public const string Users = @"data\users";
    public const string UsersAvatar = @"data\users\avatar";

    public static List<(string token, User user)> Bots = new();

    public static void CheckDirectories()
    {
        CheckDirrectory("data");
        CheckDirrectory(Users);
        CheckDirrectory(UsersAvatar);
        CheckDirrectory(@"guilds\users");
    }

    public static void LoadBots()
    {
        var files = Directory.GetFiles(Users);
        foreach (var file in files)
        {
            var token = file;
            var json = File.ReadAllText(file);
            var user = JsonConvert.DeserializeObject<User>(json);
            Bots.Add((token, user!));
        }
    }

    private static void CheckDirrectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}