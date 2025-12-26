namespace PipeScript.CommandResults;

public sealed class IncludeResult(string code) : CommandResult
{
    public string Code { get; } = code;
}
