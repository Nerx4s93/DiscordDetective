using System.IO;
using DiscordDetective.Database.Models;
using DiscordDetective.Database.Models.DiscordAPI;
using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.Database;

public class DatabaseContext : DbContext
{
    public DbSet<ChannelDbDTO> Channels { get; set; }
    public DbSet<GuildDbDTO> Guilds { get; set; }
    public DbSet<PermissionOverwriteDbDTO> PermissionsOverwrite { get; set; }
    public DbSet<UserDbDTO> Users { get; set; }

    public DbSet<BotDTO> Bots { get; set; }
    public DbSet<GuildMemberDTO> GuildMembers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var password = File.ReadAllText("dbpassword.txt");
            var connectionString = $"Host=localhost;Port=5432;Database=DiscordDetective;Username=postgres;Password={password}";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}