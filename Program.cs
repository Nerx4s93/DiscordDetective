using DiscordDetective.API;

namespace DiscordDetective
{
    internal static class Program
    {
        [STAThread]
        static async Task Main()
        {
            DataManager.CheckDirectories();
            var client = new DiscordClient("MTIxOTMxMzA4OTI2MzE3Nzc3MA.G1U6Xl.oAMaOjKQ65Sdz_ta6Y7Nj0mnpA8hTgmc66DD4Y");
            await client.UpdateAllDataAsync();
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
        }
    }
}