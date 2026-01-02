using System;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class If : PipeCommand
{
    public override string Name => "if";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length != 2)
        {
            throw new ArgumentException("Usage: if <value>, <label>");
        }

        var valueArg = args[0];
        var label = args[1];

        object? value;
        if (valueArg.StartsWith('$'))
        {
            value = ScriptUtils.ResolveArg(valueArg, ctx.Variables);
        }
        else
        {
            if (!bool.TryParse(valueArg, out var literal))
            {
                throw new Exception($"Invalid condition value '{valueArg}', expected bool");
            }

            value = literal;
        }

        if (value is not bool condition)
        {
            throw new Exception($"Condition value must be bool, got '{value?.GetType().Name}'");
        }

        if (condition)
        {
            return new JumpResult(label, false);
        }

        return ContinueResult.Instance;
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length == 2;
    }
}