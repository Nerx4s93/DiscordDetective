using System;

using PipeScript.API;

namespace PipeScript;

public class ExecutionContext
{
    public IScriptHost Host { get; internal set; } = null!;

    public Guid ExecutionId { get; } = Guid.NewGuid();
    public string ScriptName { get; internal set; } = "unnamed";
    public int CurrentLineNumber { get; set; } = 0;

    public ScriptTypeRegistry ScriptTypeRegistry { get; } = new();
    public Variables Variables { get; } = new Variables();
}