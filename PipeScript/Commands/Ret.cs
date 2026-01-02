using System;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Ret : PipeCommand
{
    public override string Name => "ret";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length != 0)
        {
            throw new ArgumentException("Usage: ret");
        }

        return ReturnResult.Instance;
    }

    public override bool ValidateArgs(string[] args) => args.Length == 0;
}