using System;

namespace PipeScript;

public class ExecutionContext
{
    public Guid ExecutionId { get; } = Guid.NewGuid();
    public string ScriptName { get; internal set; } = "unnamed";

    public ScriptTypeRegistry ScriptTypeRegistry { get; } = new();
    public Variables Variables { get; } = new Variables();
}