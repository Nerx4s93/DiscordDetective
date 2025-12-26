using System;
using System.Collections.Generic;

namespace PipeScript;

public class Variables
{
    private readonly Dictionary<string, object> _variables = new();

    public void Set(string name, object value)
    {
        _variables[name] = value;
    }

    public T Get<T>(string name)
    {
        if (!_variables.TryGetValue(name, out var value))
        {
            throw new KeyNotFoundException($"Variable '{name}' not found");
        }

        if (value is T typedValue)
        {
            return typedValue;
        }
        throw new InvalidCastException($"Variable '{name}' is not of type {typeof(T)}");

    }

    public bool Exists(string name)
    {
        return _variables.ContainsKey(name);
    }

    public IEnumerable<KeyValuePair<string, object>> GetAllVariables()
    {
        return _variables;
    }
}
