using System;
using System.Linq;
using System.Reflection;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class RInvoke : PipeCommand
{
    public override string Name => "rinvoke";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length < 3)
        {
            throw new ArgumentException("Usage: rinvoke <resultVar> <target> <method> [arg1 arg2 ...]");
        }

        var resultVar = args[0];
        var targetArg = args[1];
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

        var methodName = args[2];
        var methodArgs = args.Skip(3).Select(a => ScriptUtils.ResolveArg(a, ctx.Variables)).ToArray();
        var flags = BindingFlags.Public |(targetInstance != null? BindingFlags.Instance: BindingFlags.Static);

        var method = GetMethod(targetType,methodName,methodArgs,flags);
        var result = method.Invoke(targetInstance, methodArgs);

        ctx.Variables.Set(resultVar, new Variable(result!.GetType(), result));

        return ContinueResult.Instance;
    }

    public override bool ValidateArgs(string[] args)
    {
        return args.Length >= 3;
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
