using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.Database;

public static class DbHelper
{
    public static async Task UpsertAsync<T>(DbContext context, DbSet<T> table, T value) where T : class
    {
        var keyProperties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Any()).ToArray();

        if (keyProperties.Length == 0)
        {
            var primaryKeyAttr = typeof(T).GetCustomAttribute<PrimaryKeyAttribute>();
            if (primaryKeyAttr != null)
            {
                keyProperties = primaryKeyAttr.PropertyNames
                    .Select(name => typeof(T).GetProperty(name)!).ToArray();
            }
        }

        if (keyProperties.Length == 0)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not have any key defined");
        }

        var keyValues = keyProperties.Select(p => p.GetValue(value)).ToArray();

        var tracked = context.ChangeTracker.Entries<T>()
            .FirstOrDefault(e => keyProperties
                .All(kp => Equals(kp.GetValue(value), e.Property(kp.Name).CurrentValue)));

        if (tracked != null)
        {
            tracked.CurrentValues.SetValues(value);
            return;
        }

        var existing = await table.FindAsync(keyValues);
        if (existing != null)
        {
            table.Entry(existing).CurrentValues.SetValues(value);
        }
        else
        {
            await table.AddAsync(value);
        }
    }

    public static async Task UpsertAsync<T>(DbContext context, DbSet<T> table, IEnumerable<T> values) where T : class
    {
        foreach (var value in values)
        {
            await UpsertAsync(context, table, value);
        }
    }
}