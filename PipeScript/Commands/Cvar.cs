using System;
using System.Linq;

namespace PipeScript.Commands;

internal class Cvar : PipeCommand
{
    public override string Name { get; } = "cvar";

    public override object Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length < 3)
        {
            throw new ArgumentException("Usage: cvar <name>, <type>, <value>[, ...]");
        }

        var name = args[0].Trim();
        var typeAlias = args[1].Trim();
        var valueArgs = args.Skip(2).ToArray();

        var type = ctx.ScriptTypeRegistry.Resolve(typeAlias);
        var value = CreateValue(type, valueArgs, ctx.Variables);
        ctx.Variables.Set(name, new Variable(type, value));

        return null;
    }

    private static object CreateValue(Type type, string[] args, Variables vars)
    {
        if (type == typeof(string) || type.IsPrimitive)
        {
            if (args.Length != 1)
            {
                throw new Exception($"Type {type.Name} expects 1 argument, got {args.Length}");
            }

            var value = args[0];
            if (value.StartsWith('$'))
            {
                var varName = value[1..];
                value = vars.Get(varName).Value.ToString();
            }

            return Convert.ChangeType(value, type)!;
        }

        var constructors = type.GetConstructors();

        var constructor = constructors.FirstOrDefault(c => c.GetParameters().Length == args.Length);
        if (constructor == null)
        {
            throw new Exception($"Type {type.Name} has no public constructor with {args.Length} arguments");
        }

        var ctorParams = constructor.GetParameters();
        var ctorValues = new object[args.Length];

        for (var i = 0; i < args.Length; i++)
        {
            var value = args[i];

            if (value.StartsWith('$'))
            {
                var varName = value[1..];
                value = vars.Get(varName).Value.ToString();
            }

            ctorValues[i] = Convert.ChangeType(value, ctorParams[i].ParameterType)!;
        }

        return constructor.Invoke(ctorValues);
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length >= 3;
    }
}
