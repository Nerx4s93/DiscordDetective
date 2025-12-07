using System;

namespace DiscordDetective.Px6Api;

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

        return $"{Name}={Uri.EscapeDataString(Value?.ToString() ?? "")}";
    }
}