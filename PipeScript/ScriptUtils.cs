namespace PipeScript;

internal static class ScriptUtils
{
    public static string ResolveArg(string arg, Variables vars)
    {
        arg = arg.Trim();
        if (arg.StartsWith('$'))
        {
            var varName = arg[1..];
            return vars.Get(varName).Value.ToString()!;
        }
        return arg;
    }
}