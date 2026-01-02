using System;
using System.Linq;
using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Cvar : PipeCommand
{
    public override string Name { get; } = "cvar";

    public override ContinueResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException("Usage: cvar <name>, <type>, <value>[, ...]");
        }

        var name = args[0].Trim();
        var typeAlias = args[1].Trim();
        var valueArgs = args.Skip(2).ToArray();

        var type = ctx.ScriptTypeRegistry.Resolve(typeAlias);
        var value = CreateValue(type, valueArgs, ctx.Variables);

        ctx.Variables.Set(name, new Variable(type, value));

        return ContinueResult.Instance;
    }

    private static object? CreateValue(Type type, string[] args, Variables vars)
    {
        if (args.Length == 0)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            var defaultCtor = type.GetConstructor(Type.EmptyTypes);
            if (defaultCtor != null)
            {
                return defaultCtor.Invoke([]);
            }

            return null;
        }

        if (args is ["null"])
        {
            return null;
        }

        if (type == typeof(string) || type.IsPrimitive)
        {
            if (args.Length != 1)
            {
                throw new Exception($"Type {type.Name} expects 1 argument, got {args.Length}");
            }

            var value = ScriptUtils.ResolveArg(args[0], vars);
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
            var value = ScriptUtils.ResolveArg(args[i], vars);
            ctorValues[i] = Convert.ChangeType(value, ctorParams[i].ParameterType);
        }

        return constructor.Invoke(ctorValues);
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length >= 2;
    }
}
