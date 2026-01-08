using System;
using System.Reflection;

namespace DiscordDetective;

public static class ObjectHelper
{
    public static void CopyFields<T>(T target, T source)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var value = field.GetValue(source);
            field.SetValue(target, value);
        }
    }
}
