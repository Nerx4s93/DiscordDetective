namespace DiscordDetective;

internal static class DataManager
{
    public static void CheckDirectories()
    {
        CheckDirrectory("data");
        CheckDirrectory(@"data\users");
    }

    private static void CheckDirrectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}