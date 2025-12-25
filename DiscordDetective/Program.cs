using System;
using System.Windows.Forms;

using DiscordDetective.GUI;

namespace DiscordDetective;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Console.Write($"\x1b[8;{12};{80}t");
        ApplicationConfiguration.Initialize();
        Application.Run(new FormMain());
    }
}