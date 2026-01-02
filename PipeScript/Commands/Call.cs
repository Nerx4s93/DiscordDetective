using System;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Call : PipeCommand
{
    public override string Name => "call";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("Usage: call <label>");
        }

        var labelName = args[0];

        return new JumpResult(labelName, true);
    }

    public override bool ValidateArgs(string[] args) => args.Length == 1;
}