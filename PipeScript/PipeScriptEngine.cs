using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using PipeScript.CommandResults;

namespace PipeScript;

public sealed class PipeScriptEngine
{
    private readonly CommandRegistry _commandRegistry = new();
    private readonly Stack<ScriptFrame> _callStack = new();
    private readonly IExecutionObserver? _observer;

    private Thread? _scriptThread;
    private CancellationTokenSource? _cts;

    public PipeScriptEngine(string scriptName = "unnamed", IExecutionObserver? observer = null)
    {
        Context.ScriptName = scriptName;
        _observer = observer;
    }

    public ExecutionContext Context { get; } = new();

    public bool IsRunning => _scriptThread?.IsAlive == true;

    public Stack<ScriptFrame> CallStack
    {
        get
        {
            lock (_callStack)
            {
                return _callStack;
            }
        }
    }

    public void Start(string script)
    {
        if (IsRunning)
        {
            Stop();
            _scriptThread!.Join();
        }

        lock (_callStack)
        {
            _callStack.Clear();
        }
        Context.CurrentLineNumber = 0;
        Context.Variables.Clear();
        Context.ScriptTypeRegistry.Clear();

        _cts = new CancellationTokenSource();

        _scriptThread = new Thread(() =>
        {
            try
            {
                Started?.Invoke();
                Execute(script, _cts.Token);
                Finished?.Invoke();
            }
            catch (OperationCanceledException)
            {
                Stopped?.Invoke();
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex.ToString());
            }
            finally
            {
                _cts = null;
                _scriptThread = null;
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
    }

    private void Execute(string script, CancellationToken token)
    {
        Execute(Context.ScriptName, script, token);
    }

    private void Execute(string scriptName, string script, CancellationToken token)
    {
        lock (_callStack)
        {
            _callStack.Push(new ScriptFrame(new ScriptCode(scriptName, script)));
            RaiseFrameChanged();

            while (_callStack.Count > 0)
            {
                token.ThrowIfCancellationRequested();

                var line = GetCodeLine(out var frame);
                if (line == null || frame == null)
                {
                    continue;
                }

                _observer?.BeforeExecute(frame);
                ExecuteLine(line, token);
                _observer?.AfterExecute(frame);
                RaiseFrameChanged();
            }
        }
    }

    private void ExecuteLine(string line, CancellationToken token)
    {
        Context.Host.WriteLine(line);

        var firstSpace = line.IndexOf(' ');
        if (firstSpace < 0)
        {
            throw new Exception($"Invalid syntax at line {Context.CurrentLineNumber}");
        }

        var commandName = line[..firstSpace];
        var argString = line[(firstSpace + 1)..];

        var command = _commandRegistry.GetCommand(commandName)
            ?? throw new Exception($"Unknown command '{commandName}' at line {Context.CurrentLineNumber}");

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
                return;

            case IncludeResult include:
                lock (_callStack)
                {
                    _callStack.Push(new ScriptFrame(
                        new ScriptCode(include.ScriptName, include.Code)
                    ));
                    RaiseFrameChanged();
                }
                return;

            default:
                throw new Exception($"Unknown command result type: {result.GetType().Name}");
        }
    }

    private string? GetCodeLine(out ScriptFrame? frame)
    {
        lock (_callStack)
        {
            frame = _callStack.Count > 0 ? _callStack.Peek() : null;
            if (frame == null)
            {
                return null;
            }

            if (frame.LineIndex >= frame.Code.CleanLines.Length)
            {
                _callStack.Pop();
                RaiseFrameChanged();
                return null;
            }

            Context.CurrentLineNumber = frame.Code.SourceLineMap[frame.LineIndex] + 1;

            var line = frame.Code.CleanLines[frame.LineIndex];
            frame.LineIndex++;

            return line.Length == 0 ? null : line;
        }
    }

    private static string[] ParseArgs(string argString)
    {
        var result = new List<string>();
        var sb = new StringBuilder();

        var inQuotes = false;
        var skipSpaces = true;
        var escape = false;

        foreach (var c in argString)
        {
            if (escape)
            {
                sb.Append(c);
                escape = false;
                continue;
            }

            if (c == '\\')
            {
                escape = true;
                continue;
            }

            if (skipSpaces && char.IsWhiteSpace(c) && !inQuotes)
            {
                continue;
            }

            skipSpaces = false;

            switch (c)
            {
                case '"':
                    inQuotes = !inQuotes;
                    break;

                case ',' when !inQuotes:
                    result.Add(sb.ToString());
                    sb.Clear();
                    skipSpaces = true;
                    break;

                default:
                    sb.Append(c);
                    break;
            }
        }

        if (sb.Length > 0)
        {
            result.Add(sb.ToString());
        }

        return result.ToArray();
    }

    private void RaiseFrameChanged()
    {
        var frame = _callStack.Count > 0 ? _callStack.Peek() : null;
        if (frame != null)
        {
            FrameChanged?.Invoke(frame);
        }
    }

    public event Action? Started;
    public event Action? Finished;
    public event Action? Stopped;
    public event Action<string>? Error;
    public event Action<ScriptFrame>? FrameChanged;
}   