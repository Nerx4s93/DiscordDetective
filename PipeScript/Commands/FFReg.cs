using System;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class FFReg : PipeCommand
{
    public override string Name => "ffreg";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("Usage: ffreg <functionName>");
        }

        var functionName = args[0];
        var currentScript = ctx.LoadedScripts.Find(s => s.ScriptId == ctx.CurrentScript)
                            ?? throw new Exception("Current script not found in LoadedScripts");

        if (!currentScript.Labels.TryGetValue(functionName, out var lineIndex))
        {
            throw new Exception($"Label '{functionName}' not found in script '{currentScript.Name}'");
        }

        ctx.ForeignFunctions[functionName] = (currentScript.ScriptId, lineIndex);

        return ContinueResult.Instance;
    }

    public override bool ValidateArgs(string[] args) => args.Length == 1;
}