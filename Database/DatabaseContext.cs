using System.IO;

using DiscordDetective.Database.Models;

using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public DbSet<UserDbDTO> Users { get; set; }

    public DatabaseContext()
    {
    }

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