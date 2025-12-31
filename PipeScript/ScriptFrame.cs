namespace PipeScript;

public sealed class ScriptFrame(ScriptCode code)
{
    public ScriptCode Code { get; } = code;
    public int LineIndex { get; set; } = 0;
}