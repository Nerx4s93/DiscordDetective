using System.Collections.Generic;
using System.Threading;

namespace PipeScript;

public sealed class ScriptDebugger : IExecutionObserver
{
    private readonly ManualResetEventSlim _event = new(true);
    private Stack<ScriptFrame>? _callStack;

    private DebugMode _mode = DebugMode.None;
    private int _targetDepth;

    public bool IsPaused => !_event.IsSet;

    public void AttachCallStack(Stack<ScriptFrame> callStack)
    {
        _callStack = callStack;
    }

    public void Pause()
    {
        _mode = DebugMode.None;
        _event.Reset();
    }

    public void Resume()
    {
        _mode = DebugMode.None;
        _event.Set();
    }

    public void Step()
    {
        _mode = DebugMode.StepInto;
        _event.Set();
    }

    public void StepOver()
    {
        if (_callStack == null)
        {
            return;
        }

        _mode = DebugMode.StepOver;
        _targetDepth = _callStack.Count;
        _event.Set();
    }

    public void BeforeExecute(ScriptFrame frame)
    {
        if (_mode == DebugMode.StepOver && _callStack != null)
        {
            if (_callStack.Count <= _targetDepth)
            {
                _mode = DebugMode.None;
                _event.Reset();
            }
        }

        _event.Wait();
    }

    public void AfterExecute(ScriptFrame frame)
    {
        if (_mode == DebugMode.StepInto)
        {
            _mode = DebugMode.None;
            _event.Reset();
        }
    }
}