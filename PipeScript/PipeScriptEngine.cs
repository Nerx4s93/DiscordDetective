using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using PipeScript.CommandResults;
using PipeScript.GUI;

namespace PipeScript;

public sealed class PipeScriptEngine(string scriptName = "unnamed")
{
    private readonly CommandRegistry _commandRegistry = new();
    private readonly Stack<ScriptFrame> _callStack = new();

    private Thread? _scriptThread;
    private CancellationTokenSource? _cts;
    private volatile bool _paused;

    public ExecutionContext Context { get; } = new()
    {
        ScriptName = scriptName
    };

    public bool IsRunning => _scriptThread?.IsAlive == true;

    public void Pause() => _paused = true;

    public void Resume() => _paused = false;

    public void Start(string script)
    {
        if (IsRunning)
        {
            throw new InvalidOperationException("Script already running");
        }

        _cts = new CancellationTokenSource();

        _scriptThread = new Thread(() =>
        {
            try
            {
                Execute(script, _cts.Token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Context.Host?.WriteLine("ERROR: " + ex.Message);
            }
        });

        _scriptThread.IsBackground = true;
        _scriptThread.Start();
    }

    public void Stop()
    {
        if (!IsRunning)
        {
            return;
        }

        _cts!.Cancel();
        _scriptThread!.Join();

        _cts = null;
        _scriptThread = null;
    }

    public void Restart(string script)
    {
        Stop();
        Start(script);
    }

    private void Execute(string script, CancellationToken token)
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
            token.ThrowIfCancellationRequested();

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

            ExecuteLine(line, token);
        }
    }

    private void ExecuteLine(string line, CancellationToken token)
    {
        WaitIfPaused();

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

        var result = command.Execute(args, Context);
        ProcessCommandResult(result, token);
    }

    private void ProcessCommandResult(CommandResult result, CancellationToken token)
    {
        switch (result)
        {
            case ContinueResult:
                {
                    break;
                }
            case IncludeResult include:
                {
                    Execute(include.Code, token);
                    break;
                }
            default:
                {
                    throw new Exception($"Unknown command result type: {result.GetType().Name}");
                }
        }
    }

    private static string[] ParseArgs(string argString)
    {
        return argString.Split(',', StringSplitOptions.RemoveEmptyEntries); ;
    }

    private void WaitIfPaused()
    {
        while (_paused)
        {
            Thread.Sleep(50);
        }
    }
}
