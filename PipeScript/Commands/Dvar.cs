using System;
using System.Linq;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Dvar : PipeCommand
{
    public override string Name { get; } = "dvar";

    public override ContinueResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length < 3)
        {
            throw new ArgumentException("Usage: dvar <name>");
        }

        var name = args[0].Trim();
        ctx.Variables.Delete(name);

        return ContinueResult.Instance;
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length == 1;
    }
}
