namespace PipeScript.CommandResults;

public sealed class GotoResult(string labelName) : CommandResult
{
    public string LabelName { get; } = labelName;
}