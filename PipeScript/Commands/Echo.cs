using System;
using System.Linq;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Echo : PipeCommand
{
    public override string Name => "echo";

    public override ContinueResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length == 0)
        {
            throw new ArgumentException("Usage: echo <arg1> [arg2] [...]");
        }

        var text = string.Concat(
            args.Select(a => ScriptUtils.ResolveArg(a, ctx.Variables))
        );
        ctx.Host.WriteLine(text);

        return ContinueResult.Instance;
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length >= 1;
    }
}