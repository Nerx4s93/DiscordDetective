using System;
using System.Linq;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Cvar : PipeCommand
{
    public override string Name => "cvar";

    public override ContinueResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException("Usage: cvar <name>, <type>, <value>[, ...]");
        }

        var name = args[0].Trim();
        var typeExpr = args[1].Trim();
        var valueArgs = args.Skip(2).ToArray();

        var type = ctx.ScriptTypeRegistry.Resolve(typeExpr);
        var value = CreateValue(type, valueArgs, ctx.Variables);

        ctx.Variables.Set(name, new Variable(type, value));
        return ContinueResult.Instance;
    }

    private static object? CreateValue(Type type, string[] args, Variables vars)
    {
        if (args.Length == 0 || args is ["null"])
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        if (type == typeof(string) || type.IsPrimitive)
        {
            var raw = ScriptUtils.ResolveArg(args[0], vars);
            return Convert.ChangeType(raw, type);
        }

        if (type.IsArray)
        {
            var elementType = type.GetElementType()!;
            var array = Array.CreateInstance(elementType, args.Length);

            for (var i = 0; i < args.Length; i++)
            {
                var val = ScriptUtils.ResolveArg(args[i], vars);
                array.SetValue(Convert.ChangeType(val, elementType), i);
            }

            return array;
        }

        var constructors = type.GetConstructors();
        foreach (var ctor in constructors)
        {
            var parameters = ctor.GetParameters();
            if (parameters.Length != args.Length) continue;

            try
            {
                var values = new object?[args.Length];
                for (var i = 0; i < args.Length; i++)
                {
                    var resolved = ScriptUtils.ResolveArg(args[i], vars);
                    values[i] = Convert.ChangeType(resolved, parameters[i].ParameterType);
                }
                return ctor.Invoke(values);
            }
            catch { /* игнорируем */ }
        }

        throw new Exception($"Cannot create instance of {type.Name} with {args.Length} argument(s)");
    }

    public override bool ValidateArgs(string[] args) => args.Length >= 2;
}
