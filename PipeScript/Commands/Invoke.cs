using System;
using System.Linq;
using System.Reflection;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Invoke : PipeCommand
{
    public override string Name => "invoke";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException(
                "Usage: invoke <target> <method> [arg1 arg2 ...]"
            );
        }

        var targetArg = args[0];
        object? targetInstance = null;
        Type targetType;

        if (targetArg.StartsWith('$'))
        {
            targetInstance = ScriptUtils.ResolveArg(targetArg, ctx.Variables);
            if (targetInstance == null)
            {
                throw new Exception($"Target '{targetArg}' is null");
            }

            targetType = targetInstance.GetType();
        }
        else
        {
            targetType = ctx.ScriptTypeRegistry.Resolve(targetArg);
        }

        var methodName = args[1];
        var methodArgs = args.Skip(2).Select(a => ScriptUtils.ResolveArg(a, ctx.Variables)).ToArray();
        var flags = BindingFlags.Public | (targetInstance != null ? BindingFlags.Instance : BindingFlags.Static);

        var method = GetMethod(targetType, methodName, methodArgs, flags);
        method.Invoke(targetInstance, methodArgs);

        return ContinueResult.Instance;
    }


    public override bool ValidateArgs(string[] args)
    {
        return args.Length >= 2;
    }

    private static MethodInfo GetMethod(Type targetType, string methodName, object?[] args, BindingFlags flags)
    {
        foreach (var method in targetType.GetMethods(flags))
        {
            if (method.Name != methodName)
            {
                continue;
            }

            var parameters = method.GetParameters();
            if (parameters.Length != args.Length)
            {
                continue;
            }

            var match = true;
            for (var i = 0; i < parameters.Length; i++)
            {
                var arg = args[i];
                if (arg == null)
                {
                    continue;
                }

                if (!parameters[i].ParameterType.IsInstanceOfType(arg))
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                return method;
            }
        }

        throw new Exception($"Method '{methodName}' with {args.Length} args not found on '{targetType.FullName}'");
    }
}
