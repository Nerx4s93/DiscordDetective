using System.Collections.Generic;

namespace PipeScript;

public class Variables
{
    private readonly Dictionary<string, Variable> _variables = new();

    public void Set(string name, Variable value)
    {
        _variables[name] = value;
    }

    public Variable Get(string name)
    {
        if (!_variables.TryGetValue(name, out var value))
        {
            throw new KeyNotFoundException($"Variable '{name}' not found");
        }

        return value;
    }

    public bool Exists(string name)
    {
        return _variables.ContainsKey(name);
    }

    public void Clear() => _variables.Clear();

    public IEnumerable<KeyValuePair<string, Variable>> GetAllVariables()
    {
        return _variables;
    }
}
