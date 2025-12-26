using System;
using System.Collections.Generic;

namespace PipeScript;

public class ScriptTypeRegistry
{
    private readonly Dictionary<string, Type> _types = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string alias, string clrTypeName)
    {
        var type = Type.GetType(clrTypeName, throwOnError: false);
        _types[alias] = type ?? throw new Exception($"CLR type not found: {clrTypeName}");
    }

    public Type Resolve(string alias)
    {
        if (_types.TryGetValue(alias, out var type))
        {
            return type;
        }

        throw new Exception($"Type not registered: {alias}");
    }
}
