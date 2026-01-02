using System;
using System.Collections;
using System.Collections.Generic;
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

        var (typeExpr, valueStart) = ParseType(args, 1);
        var valueArgs = args.Skip(valueStart).ToArray();

        var type = ctx.ScriptTypeRegistry.Resolve(typeExpr);
        var value = CreateValue(typeExpr, type, valueArgs, ctx.Variables);

        ctx.Variables.Set(name, new Variable(type, value));

        return ContinueResult.Instance;
    }

    private static (string TypeExpr, int NextIndex) ParseType(string[] args, int startIndex)
    {
        var parts = new List<string>();
        var genericDepth = 0;
        var arrayDepth = 0;
        var index = startIndex;

        for (; index < args.Length; index++)
        {
            var part = args[index].Trim();
            parts.Add(part);

            genericDepth += part.Count(c => c == '<');
            genericDepth -= part.Count(c => c == '>');

            arrayDepth += part.Count(c => c == '[');
            arrayDepth -= part.Count(c => c == ']');

            if (genericDepth == 0 && arrayDepth == 0)
            {
                index++;
                break;
            }
        }

        var typeExpr = string.Join(", ", parts);
        return (typeExpr, index);
    }

    private static object? CreateValue(string typeExpr, Type type, string[] args, Variables vars)
    {
        if (args is ["null"])
        {
            return null;
        }

        if (TryCreateSizedArray(typeExpr, type, out var array))
        {
            return array;
        }

        if (args.Length == 0)
        {
            if (type.IsValueType || IsGenericCollection(type))
            {
                return Activator.CreateInstance(type);
            }

            var ctor = type.GetConstructor(Type.EmptyTypes);
            if (ctor != null)
            {
                return ctor.Invoke([]);
            }

            return null;
        }

        if (type == typeof(string) || type.IsPrimitive)
        {
            if (args.Length != 1)
            {
                throw new Exception($"Type {type.Name} expects 1 argument, got {args.Length}");
            }

            var raw = ScriptUtils.ResolveArg(args[0], vars);
            return Convert.ChangeType(raw, type);
        }

        var constructors = type.GetConstructors();
        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length != args.Length)
            {
                continue;
            }

            try
            {
                var values = new object?[args.Length];

                for (var i = 0; i < args.Length; i++)
                {
                    var resolved = ScriptUtils.ResolveArg(args[i], vars);
                    values[i] = ConvertValue(resolved, parameters[i].ParameterType);
                }

                return constructor.Invoke(values);
            }
            catch { /* игнорируем */ }
        }

        throw new Exception($"Type {type.Name} has no suitable constructor for {args.Length} arguments");
    }

    private static bool TryCreateSizedArray(string typeExpr, Type arrayType, out object array)
    {
        array = null!;

        if (!arrayType.IsArray)
        {
            return false;
        }

        var start = typeExpr.IndexOf('[');
        var end = typeExpr.LastIndexOf(']');
        if (start < 0 || end < start)
        {
            return false;
        }

        var inside = typeExpr.Substring(start + 1, end - start - 1);
        var sizes = inside.Split(',').Select(s => int.Parse(s.Trim())).ToArray();

        var elementType = arrayType.GetElementType()!;
        array = Array.CreateInstance(elementType, sizes);
        return true;
    }

    private static bool IsGenericCollection(Type type)
    {
        if (!type.IsGenericType)
        {
            return false;
        }

        var def = type.GetGenericTypeDefinition();
        return def == typeof(List<>) || def == typeof(Dictionary<,>) || typeof(IEnumerable).IsAssignableFrom(type);
    }

    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value == null)
        {
            return null;
        }

        if (targetType.IsInstanceOfType(value))
        {
            return value;
        }

        return Convert.ChangeType(value, targetType);
    }

    public override bool ValidateArgs(string[] args) => args.Length >= 2;
}