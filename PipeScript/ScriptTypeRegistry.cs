using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeScript;

public sealed class ScriptTypeRegistry
{
    private readonly Dictionary<string, Type> _types = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string alias, string clrTypeName)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException("Alias cannot be empty", nameof(alias));
        }

        if (_types.ContainsKey(alias))
        {
            throw new Exception($"Type '{alias}' already registered");
        }

        var type = FindType(clrTypeName);
        _types[alias] = type ?? throw new Exception($"CLR type not found: {clrTypeName}");
    }

    public void Clear() => _types.Clear();

    public bool IsRegistered(string alias) => _types.ContainsKey(alias);

    public Type Resolve(string aliasOrFullName)
    {
        aliasOrFullName = aliasOrFullName.Trim();
        
        // Массив
        if (TryResolveSizedArray(aliasOrFullName, out var arrayType, out _))
        {
            return arrayType;
        }

        // Generic тип
        if (aliasOrFullName.Contains('<'))
        {
            return ResolveGeneric(aliasOrFullName);
        }

        // Получение типа из словаря
        if (_types.TryGetValue(aliasOrFullName, out var aliasType))
        {
            return aliasType;
        }

        // Поиск типа
        var clrType = FindType(aliasOrFullName);
        if (clrType != null)
        {
            return clrType;
        }

        throw new Exception($"Type not registered or not found: {aliasOrFullName}");
    }

    // Массив
    private bool TryResolveSizedArray(string expr, out Type type, out int[] sizes)
    {
        type = null!;
        sizes = [];

        var bracketStart = expr.IndexOf('[');
        var bracketEnd = expr.LastIndexOf(']');
        if (bracketStart < 0 || bracketEnd < bracketStart)
        {
            return false;
        }

        var inside = expr.Substring(bracketStart + 1, bracketEnd - bracketStart - 1);
        var elementExpr = expr.Substring(0, bracketStart).Trim();

        if (string.IsNullOrWhiteSpace(inside))
        {
            return false;
        }

        var sizeParts = inside.Split(',', StringSplitOptions.RemoveEmptyEntries);
        sizes = sizeParts.Select(p => int.Parse(p.Trim())).ToArray();

        var elementType = Resolve(elementExpr);
        type = elementType.MakeArrayType(sizes.Length);
        return true;
    }

    // Generic тип
    private Type ResolveGeneric(string expr)
    {
        var nameEnd = expr.IndexOf('<');
        var typeName = expr[..nameEnd].Trim();
        var argsPart = expr[(nameEnd + 1)..^1];

        var genericArgs = SplitGenericArguments(argsPart).Select(Resolve).ToArray();
        var baseType = Resolve(typeName);

        if (baseType is { IsGenericTypeDefinition: false, IsGenericType: true })
        {
            baseType = baseType.GetGenericTypeDefinition();
        }

        if (!baseType.IsGenericTypeDefinition)
        {
            throw new Exception($"Type '{typeName}' is not generic");
        }

        if (baseType.GetGenericArguments().Length != genericArgs.Length)
        {
            throw new Exception(
                $"Generic argument count mismatch for '{typeName}': expected {baseType.GetGenericArguments().Length}, got {genericArgs.Length}"
            );
        }

        return baseType.MakeGenericType(genericArgs);
    }
    
    private static List<string> SplitGenericArguments(string input)
    {
        var result = new List<string>();
        var depth = 0;
        var start = 0;

        for (var i = 0; i < input.Length; i++)
        {
            switch (input[i])
            {
                case '<':
                    depth++;
                    break;
                case '>':
                    depth--;
                    break;
                case ',' when depth == 0:
                    result.Add(input[start..i].Trim());
                    start = i + 1;
                    break;
            }
        }

        result.Add(input[start..].Trim());
        return result;
    }

    private static Type? FindType(string clrName)
    {
        var type = Type.GetType(clrName, false);
        if (type != null)
        {
            return type;
        }

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = asm.GetType(clrName, false);
            if (type != null)
            {
                return type;
            }
        }

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            var types = asm.GetTypes();
            foreach (var t in types)
            {
                if (!t.IsGenericTypeDefinition)
                {
                    continue;
                }

                if (t.FullName == null)
                {
                    continue;
                }

                var shortName = t.FullName.Split('`')[0];
                if (shortName == clrName)
                {
                    return t;
                }
            }
        }

        return null;
    }
}