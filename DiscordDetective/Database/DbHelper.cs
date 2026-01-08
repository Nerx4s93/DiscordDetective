using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.Database;

public static class DbHelper
{
    public static void Upsert<T>(T newItem, DbContext context, DbSet<T> dbSet) where T : class
    {
        var keyProperty = context.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties.First();
        var keyValue = keyProperty.PropertyInfo!.GetValue(newItem);

        var existing = dbSet.FirstOrDefault(x => keyProperty.PropertyInfo!.GetValue(x)!.Equals(keyValue));

        if (existing == null)
        {
            dbSet.Add(newItem);
        }
        else
        {
            ObjectHelper.CopyFields(existing, newItem);
        }
    }

    public static void Upsert<T>(IEnumerable<T> newItems, DbContext context, DbSet<T> dbSet) where T : class
    {
        foreach (var item in newItems)
        {
            Upsert(item, context, dbSet);
        }
    }
}