using System;
using System.Collections.Generic;

namespace Px6Api;

internal class RequestParameter
{
    public required string Name { get; set; }
    public object? Value { get; set; }
    public object? DefaultValue { get; set; }

    public bool ShouldInclude => Value != null && !Value.Equals(DefaultValue);

    public string GetQueryString()
    {
        if (Value is Enum enumValue)
        {
            return $"{Name}={enumValue.ToString().ToLower()}";
        }

        if (Value is List<int> intList)
        {
            var ids = string.Join(",", intList);
            return $"{Name}={Uri.EscapeDataString(ids)}";
        }

        if (Value is List<string> stringList)
        {
            var ids = string.Join(",", stringList);
            return $"{Name}={Uri.EscapeDataString(ids)}";
        }

        return $"{Name}={Uri.EscapeDataString(Value?.ToString() ?? "")}";
    }
}