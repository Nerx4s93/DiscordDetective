using System;
using System.Collections.Generic;
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
        var scriptCode = new ScriptCode(scriptName, script);
        Context.LoadedScripts.Add(scriptCode);

        lock (_callStack)
        {
            _callStack.Push(new ScriptFrame(scriptCode));
            RaiseFrameChanged();

            while (_callStack.Count > 0)
            {
                token.ThrowIfCancellationRequested();

                var line = GetNextLine(out var frame);
                if (line == null || frame == null)
                {
                    if (_callStack.Count == 0)
                    {
                        return;
                    }

                    continue;
                }

                _observer?.BeforeExecute(frame);
                ExecuteLine(line, token);
                frame.LineIndex++;
                _observer?.AfterExecute(frame);
                RaiseFrameChanged();

                Thread.Sleep(1);
            }
        }
    }

    private void ExecuteLine(ScriptLine line, CancellationToken token)
    {
        Context.CurrentLineNumber = line.SourceLine + 1;

        var command = _commandRegistry.GetCommand(line.Command)
                      ?? throw new Exception($"Unknown command '{line.Command}' at line {Context.CurrentLineNumber}");

        if (!command.ValidateArgs(line.Args))
        {
            throw new Exception($"Invalid arguments for '{line.Command}' at line {Context.CurrentLineNumber}");
        }

        var result = command.Execute(line.Args, Context);
        ProcessCommandResult(result, token);
    }

    private void ProcessCommandResult(CommandResult result, CancellationToken token)
    {
        switch (result)
        {
            case ContinueResult:
                return;

            case IncludeResult includeResult:
                lock (_callStack)
                {
                    _callStack.Push(new ScriptFrame(includeResult.Code) { LineIndex = includeResult.StartIndex });
                    RaiseFrameChanged();
                }
                return;

            case JumpResult jumpResult:
                lock (_callStack)
                {
                    var frame = _callStack.Peek();

                    if (!frame.Code.Labels.TryGetValue(jumpResult.LabelName, out var targetIndex))
                    {
                        throw new Exception($"Label '{jumpResult.LabelName}' not found");
                    }

                    if (jumpResult.PushNewFrame)
                    {
                        var newFrame = new ScriptFrame(frame.Code)
                        {
                            LineIndex = targetIndex
                        };
                        _callStack.Push(newFrame);
                    }
                    else
                    {
                        frame.LineIndex = targetIndex - 1;
                    }

                    RaiseFrameChanged();
                }
                return;

            case ReturnResult:
                lock (_callStack)
                {
                    if (_callStack.Count > 0)
                    {
                        _callStack.Pop();
                        RaiseFrameChanged();
                    }
                }
                return;

            default:
                throw new Exception($"Unknown command result type: {result.GetType().Name}");
        }
    }

    private ScriptLine? GetNextLine(out ScriptFrame? frame)
    {
        lock (_callStack)
        {
            frame = _callStack.Count > 0 ? _callStack.Peek() : null;

            if (frame == null)
            {
                return null;
            }

            if (frame.LineIndex >= frame.Code.Compiled.Length)
            {
                _callStack.Pop();
                RaiseFrameChanged();
                return null;
            }

            var line = frame.Code.Compiled[frame.LineIndex];
            return line;
        }
    }

    private void RaiseFrameChanged()
    {
        var frame = _callStack.Count > 0 ? _callStack.Peek() : null;
        if (frame != null)
        {
            Context.CurrentScript = frame.Code.ScriptId;
            FrameChanged?.Invoke(frame);
        }
    }

    public event Action? Started;
    public event Action? Finished;
    public event Action? Stopped;
    public event Action<string>? Error;
    public event Action<ScriptFrame>? FrameChanged;
}   