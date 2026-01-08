using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DiscordDetective.Database;

public static class DbHelper
{
    public static void Upsert<TDto>(
        IEnumerable<TDto> newItems,
        DbContext context,
        DbSet<TDto> dbSet,
        Func<TDto, string> getId)
        where TDto : class
    {
        foreach (var newItem in newItems)
        {
            var id = getId(newItem);
            var existing = dbSet.FirstOrDefault(x => getId(x) == id);

            if (existing == null)
            {
                dbSet.Add(newItem);
            }
            else
            {
                ObjectHelper.CopyFields(existing, newItem);
            }
        }
    }
}