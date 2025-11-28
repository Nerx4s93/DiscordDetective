using DiscordDetective.Database.Models;
using DiscordDetective.GUI;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DiscordDetective;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormMain());
    }
}