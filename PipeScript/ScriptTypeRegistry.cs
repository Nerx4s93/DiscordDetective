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

        var type = Type.GetType(clrTypeName, throwOnError: false) ?? AppDomain.CurrentDomain.GetAssemblies()
            .Select(a => a.GetType(clrTypeName))
            .FirstOrDefault(t => t != null);

        _types[alias] = type ?? throw new Exception($"CLR type not found: {clrTypeName}");
    }

    public Type Resolve(string aliasOrFullName)
    {
        if (_types.TryGetValue(aliasOrFullName, out var type))
        {
            return type;
        }

        type = Type.GetType(aliasOrFullName);
        if (type != null)
        {
            return type;
        }

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = asm.GetType(aliasOrFullName);
            if (type != null)
            {
                return type;
            }
        }

        throw new Exception($"Type not registered or not found: {aliasOrFullName}");
    }

    public void Clear() => _types.Clear();

    public bool IsRegistered(string alias) => _types.ContainsKey(alias);
}
