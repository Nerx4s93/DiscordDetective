using DiscordDetective.GUI;
using System;
using System.Windows.Forms;

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