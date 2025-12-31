namespace PipeScript.CommandResults;

public sealed class IncludeResult(string scriptName, string code) : CommandResult
{
    public string ScriptName { get; } = scriptName;
    public string Code { get; } = code;
}
