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

        Context.CurrentLineNumber = 0;

        while (Context.CurrentLineNumber < lines.Length)
        {
            var rawLine = lines[Context.CurrentLineNumber].Trim();

            if (rawLine.Length == 0 || rawLine.StartsWith(';'))
            {
                Context.CurrentLineNumber++;
                continue;
            }

            var semicolonIndex = rawLine.IndexOf(';');
            var line = semicolonIndex >= 0 ? rawLine[..semicolonIndex].Trim() : rawLine;

            if (line.Length == 0)
            {
                Context.CurrentLineNumber++;
                continue;
            }

            ExecuteLine(line);

            Context.CurrentLineNumber++;
        }
    }


    private void ExecuteLine(string line)
    {
        var firstSpace = line.IndexOf(' ');
        if (firstSpace < 0)
        {
            throw new Exception($"Invalid syntax at line {Context.CurrentLineNumber}");
        }

        var commandName = line[..firstSpace];
        var argString = line[(firstSpace + 1)..];

        var command = _commandRegistry.GetCommand(commandName);
        if (command == null)
        {
            throw new Exception($"Unknown command '{commandName}' at line {Context.CurrentLineNumber}");
        }

        var args = ParseArgs(argString);
        if (!command.ValidateArgs(args))
        {
            throw new Exception($"Invalid arguments for '{commandName}' at line {Context.CurrentLineNumber}");
        }

        command.Execute(args, Context);
    }

    private static string[] ParseArgs(string argString)
    {
        return argString.Split(',', StringSplitOptions.RemoveEmptyEntries); ;
    }
}
