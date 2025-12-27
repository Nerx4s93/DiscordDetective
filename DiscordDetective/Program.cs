using System;
using System.IO;
using System.Windows.Forms;

using DiscordDetective.GUI;

namespace DiscordDetective;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        CheckFile("dbpassword.txt", "¬ведите пароль от базы данных в файл \"dbpassword.txt\"");
        CheckFile("px6key.txt", "¬ведите api ключ от от сайта px6.me в файл \"px6key.txt\"");

        Console.Write($"\x1b[8;{12};{80}t");
        ApplicationConfiguration.Initialize();

        var code = File.ReadAllText(@"Resources\PipeScripts\Types\BasicTypes.psc");
        Application.Run(new PipeScript.GUI.ScriptForm("MyScript", code));
    }

    private static void CheckFile(string path, string message)
    {
        if (!File.Exists(path))
        {
            File.Create(path);
            throw new Exception(message);
        }

        if (string.IsNullOrEmpty(File.ReadAllText(path)))
        {
            throw new Exception(message);
        }
    }
}