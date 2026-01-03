using System;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Carr : PipeCommand
{
    public override string Name => "carr";

    public override ContinueResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length < 3)
        {
            throw new ArgumentException("Usage: carr <name>, <type>, <size>");
        }

        var name = args[0].Trim();
        var typeExpr = args[1].Trim();
        var sizeExpr = args[2].Trim();

        var elementType = ctx.ScriptTypeRegistry.Resolve(typeExpr);

        var sizeRaw = ScriptUtils.ResolveArg(sizeExpr, ctx.Variables);
        var size = Convert.ToInt32(sizeRaw);
        if (size < 0)
        {
            throw new Exception("Array size cannot be negative");
        }

        var array = CreateArray(elementType, size); 
        ctx.Variables.Set(name, new Variable(elementType.MakeArrayType(), array));

        return ContinueResult.Instance;
    }

    private static Array CreateArray(Type elementType, int size)
    {
        return Array.CreateInstance(elementType, size);
    }

    public override bool ValidateArgs(string[] args) => args.Length >= 3;
}