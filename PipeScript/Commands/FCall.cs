using System;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class FCall : PipeCommand
{
    public override string Name => "fcall";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("Usage: fcall <functionName>");
        }

        var functionName = args[0];

        if (!ctx.ForeignFunctions.TryGetValue(functionName, out var function))
        {
            throw new Exception($"Foreign function '{functionName}' not registered");
        }

        var script = ctx.LoadedScripts.Find(s => s.ScriptId == function.ScriptId);
        if (script == null)
        {
            throw new Exception($"Script with ID {function.ScriptId} not loaded");
        }

        return new IncludeResult(script, function.LineIndex);
    }

    public override bool ValidateArgs(string[] args) => args.Length == 1;
}