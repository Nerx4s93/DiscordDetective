using System;

namespace PipeScript;

public sealed class ScriptCode(string name, string source)
{
    public string Name { get; } = name;
    public string[] Lines { get; } = source.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
}