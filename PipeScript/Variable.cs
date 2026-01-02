using System;

namespace PipeScript;

public class Variable(Type type, object? value)
{
    public Type Type { get; } = type;
    public object? Value { get; set; } = value;

    public override string ToString()
    {
        return $"{Type.Name} = {Value}";
    }
}