using System;

namespace PipeScript;

internal static class ScriptUtils
{
    public static object ResolveArg(string arg, Variables vars)
    {
        arg = arg.Trim();
        if (arg.StartsWith('$'))
        {
            var varName = arg[1..];
            var variable = vars.Get(varName);
            if (variable == null)
            {
                throw new Exception($"Variable '{varName}' not found");
            }
            return variable.Value;
        }
        return arg;
    }
}