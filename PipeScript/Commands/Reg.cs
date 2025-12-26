using System;

namespace PipeScript.Commands;

internal sealed class Reg : PipeCommand
{
    public override string Name => "reg";

    public override object Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length != 2)
        {
            throw new ArgumentException("Usage: reg <alias>, \"CLR.Type\"");
        }    

        var alias = args[0].Trim();
        var clrTypeName = args[1].Trim().Trim('"');

        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new Exception("Type alias is empty");
        }

        if (ctx.ScriptTypeRegistry.IsRegistered(alias))
        {
            throw new Exception($"Type '{alias}' already registered");
        }

        ctx.ScriptTypeRegistry.Register(alias, clrTypeName);
        return null;
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length == 2;
    }
}