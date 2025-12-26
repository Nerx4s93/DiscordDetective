using System;
using System.Collections.Generic;
using System.Linq;

namespace PipeScript;

public class ScriptTypeRegistry
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

        var type = Type.GetType(clrTypeName, throwOnError: false);
        if (type == null)
        {
            type = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType(clrTypeName))
                .FirstOrDefault(t => t != null);
        }

        if (type == null)
        {
            throw new Exception($"CLR type not found: {clrTypeName}");
        }

        _types[alias] = type;
    }

    public Type Resolve(string alias)
    {
        if (_types.TryGetValue(alias, out var type))
        {
            return type;
        }

        throw new Exception($"Type not registered: {alias}");
    }

    public bool IsRegistered(string alias) => _types.ContainsKey(alias);
}
