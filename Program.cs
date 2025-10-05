namespace DiscordDetective
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            DataManager.CheckDirectories();
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}