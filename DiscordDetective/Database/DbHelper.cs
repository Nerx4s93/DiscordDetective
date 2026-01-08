using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.Database;

public static class DbHelper
{
    public static async Task UpsertAsync<T>(T value, DbSet<T> table) where T : class
    {
        var existing = await table.FindAsync(value);
        if (existing != null)
        {
            table.Entry(existing).CurrentValues.SetValues(value);
        }
        else
        {
            await table.AddAsync(value);
        }
    }

    public static async Task UpsertAsync<T>(IEnumerable<T> values, DbSet<T> table) where T : class
    {
        foreach (var value in values)
        {
            await UpsertAsync(value, table);
        }
    }
}