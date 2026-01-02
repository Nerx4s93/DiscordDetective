namespace PipeScript.CommandResults;

public sealed class JumpResult(string labelName, bool pushNewFrame) : CommandResult
{
    public string LabelName { get; } = labelName;
    public bool PushNewFrame { get; } = pushNewFrame;
}