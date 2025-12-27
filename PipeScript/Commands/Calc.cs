using System;
using System.Linq;

using DynamicExpresso;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Calc : PipeCommand
{
    public override string Name { get; } = "calc";

    public override ContinueResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException("Usage: calc <var>, <expression>");
        }

        var varName = args[0].Trim();
        var exprString = string.Join(",", args.Skip(1));

        foreach (var variable in ctx.Variables.GetAllVariables())
        {
            exprString = exprString.Replace($"${variable.Key}", variable.Key);
        }

        var interpreter = new Interpreter();
        foreach (var variable in ctx.Variables.GetAllVariables())
        {
            interpreter.SetVariable(variable.Key, variable.Value.Value, variable.Value.Type);
        }

        object? resultValue;
        try
        {
            resultValue = interpreter.Eval(exprString);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error evaluating expression: {ex.Message}");
        }

        var resultType = resultValue?.GetType() ?? typeof(object);
        var variableObj = new Variable(resultType, resultValue!);

        ctx.Variables.Set(varName, variableObj);

        return ContinueResult.Instance;
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length >= 2;
    }
}