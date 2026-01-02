using System;
using System.Reflection;

namespace PipeScript;

internal static class ScriptUtils
{
    public static object ResolveArg(string arg, Variables vars)
    {
        if (!arg.StartsWith('$'))
        {
            return arg;
        }

        var path = arg[1..].Split('.');

        var variable = vars.Get(path[0]);
        if (variable == null)
        {
            throw new Exception($"Variable '{path[0]}' not found");
        }

        object currentValue = variable.Value;
        for (int i = 1; i < path.Length; i++)
        {
            if (currentValue == null)
            {
                throw new Exception($"Cannot access '{path[i]}' of null in '{arg}'");
            }

            var type = currentValue.GetType();
            var prop = type.GetProperty(path[i], BindingFlags.Public | BindingFlags.Instance);
            if (prop == null)
            {
                throw new Exception($"Property '{path[i]}' not found in type '{type.Name}' for '{arg}'");
            }

            currentValue = prop.GetValue(currentValue)!;
        }

        return currentValue;
    }
}