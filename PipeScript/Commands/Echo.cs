using System;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Echo : PipeCommand
{
    public override string Name { get; } = "echo";

    public override ContinueResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("Usage: echo <text>");
        }

        var arg = args[0].Trim();
        var text = ScriptUtils.ResolveArg(arg, ctx.Variables).ToString();
        ctx.Host.WriteLine(text!);

        return ContinueResult.Instance;
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length == 1;
    }
}