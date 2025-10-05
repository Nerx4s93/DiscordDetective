namespace DiscordDetective
{
    internal static class Program
    {
        [STAThread]
        static async Task Main()
        {
            DataManager.CheckDirectories();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}