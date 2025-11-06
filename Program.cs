using System;
using System.Windows.Forms;

using DiscordDetective.GUI;

namespace DiscordDetective
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            DataManager.CheckDirectories();
            DataManager.LoadBots();

            ApplicationConfiguration.Initialize();
            Application.Run(new FormUsers());
        }
    }
}