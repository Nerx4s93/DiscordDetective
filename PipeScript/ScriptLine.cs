namespace PipeScript;

public sealed class ScriptLine(int sourceLine, string command, string[] args)
{
    public int SourceLine { get; } = sourceLine;
    public string Command { get; } = command;
    public string[] Args { get; } = args;
}
