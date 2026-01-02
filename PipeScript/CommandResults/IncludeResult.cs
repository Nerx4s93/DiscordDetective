namespace PipeScript.CommandResults;

public sealed class IncludeResult(ScriptCode code, int startIndex) : CommandResult
{
    public ScriptCode Code { get; } = code;
    public int StartIndex { get; } = startIndex;
}
