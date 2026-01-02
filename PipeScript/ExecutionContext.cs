using System;
using System.Collections.Generic;

using PipeScript.API;

namespace PipeScript;

public class ExecutionContext
{
    public IScriptHost Host { get; internal set; } = null!;

    public Guid ExecutionId { get; } = Guid.NewGuid();
    public string ScriptName { get; internal set; } = "unnamed";
    public Guid CurrentScript;
    public int CurrentLineNumber { get; set; } = 0;

    public List<ScriptCode> LoadedScripts { get; } = new();

    public ScriptTypeRegistry ScriptTypeRegistry { get; } = new();
    public Variables Variables { get; } = new();
    public Dictionary<string, (Guid ScriptId, int LineIndex)> ForeignFunctions { get; } = new();
}