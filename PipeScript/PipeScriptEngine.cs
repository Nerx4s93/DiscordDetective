using System;

namespace PipeScript;

public sealed class PipeScriptEngine(string scriptName = "unnamed")
{
    private readonly CommandRegistry _commandRegistry = new();

    public ExecutionContext Context { get; } = new()
    {
        ScriptName = scriptName
    };

    public void Execute(string script)
    {
        var lines = script.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var l in lines)
        {
            var rawLine = l.Trim();

            if (rawLine.Length == 0)
            {
                continue;
            }

            var semicolonIndex = rawLine.IndexOf(';');
            var line = semicolonIndex >= 0 ? rawLine[..semicolonIndex].Trim() : rawLine;

            if (line.Length == 0)
            {
                continue;
            }

            ExecuteLine(line);
        }
    }

    private void ExecuteLine(string line)
    {
        var firstSpace = line.IndexOf(' ');
        if (firstSpace < 0)
        {
            throw new Exception($"Invalid syntax at line \"{line}\"");
        }

        var commandName = line[..firstSpace];
        var argString = line[(firstSpace + 1)..];

        var command = _commandRegistry.GetCommand(commandName);
        if (command == null)
        {
            throw new Exception($"Unknown command '{commandName}' at line \"{line}\"");
        }

        var args = ParseArgs(argString);
        if (!command.ValidateArgs(args))
        {
            throw new Exception($"Invalid arguments for '{commandName}' at line \"{line}\"");
        }

        command.Execute(args, Context);
    }

    private static string[] ParseArgs(string argString)
    {
        return argString.Split(',', StringSplitOptions.RemoveEmptyEntries); ;
    }
}
