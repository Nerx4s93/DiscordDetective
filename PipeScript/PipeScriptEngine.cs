using System;
using System.Collections.Generic;

namespace PipeScript;

public sealed class PipeScriptEngine
{
    private readonly CommandRegistry _commandRegistry = new();
    private readonly Stack<ScriptFrame> _callStack = new();

    public ExecutionContext Context { get; }

    public PipeScriptEngine(string scriptName = "unnamed")
    {
        Context = new ExecutionContext
        {
            Engine = this,
            ScriptName = scriptName
        };
    }

    public void Execute(string script)
    {
        var lines = script.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

        _callStack.Push(new ScriptFrame
        {
            ScriptName = Context.ScriptName,
            Lines = lines,
            LineIndex = 0
        });

        while (_callStack.Count > 0)
        {
            var frame = _callStack.Peek();

            if (frame.LineIndex >= frame.Lines.Length)
            {
                _callStack.Pop();
                continue;
            }

            Context.CurrentLineNumber = frame.LineIndex + 1;

            var rawLine = frame.Lines[frame.LineIndex].Trim();
            frame.LineIndex++;

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
