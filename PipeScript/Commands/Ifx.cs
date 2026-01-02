using System;
using System.Linq;

using DynamicExpresso;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Ifx : PipeCommand
{
    public override string Name => "ifx";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException("Usage: ifx <expression>, <label>");
        }

        var label = args[^1];
        var exprString = string.Join(",", args.Take(args.Length - 1));

        foreach (var variable in ctx.Variables.GetAllVariables())
        {
            exprString = exprString.Replace($"${variable.Key}",variable.Key);
        }

        var interpreter = new Interpreter();
        foreach (var variable in ctx.Variables.GetAllVariables())
        {
            interpreter.SetVariable(variable.Key,variable.Value.Value, variable.Value.Type);
        }

        object? result;
        try
        {
            result = interpreter.Eval(exprString);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error evaluating ifx expression: {ex.Message}");
        }

        if (result is not bool condition)
        {
            throw new Exception($"ifx expression must return bool, got '{result?.GetType().Name}'");
        }

        if (condition)
        {
            return new JumpResult(label, false);
        }

        return ContinueResult.Instance;
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length >= 2;
    }
}