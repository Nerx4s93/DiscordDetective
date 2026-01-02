using System;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Goto : PipeCommand
{
    public override string Name => "goto";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("Usage: goto <label>");
        }    

        var labelName = args[0];

        return new JumpResult(labelName, false);
    }

    public override bool ValidateArgs(string[] args) => args.Length == 1;
}