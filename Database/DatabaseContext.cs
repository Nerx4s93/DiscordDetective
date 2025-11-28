using Microsoft.EntityFrameworkCore;

using DiscordDetective.Database.Models;

namespace DiscordDetective.Database;

public class DatabaseContext : DbContext
{
    public DbSet<UserDbDTO> Users { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }
}