using System;

namespace PipeScript;

public class ExecutionContext
{
    public Guid ExecutionId { get; } = Guid.NewGuid();
    public string ScriptName { get; } = "unnamed";
    public int CurrentLineNumber { get; set; } = 0;

    public Variables Variables { get; } = new Variables();
}