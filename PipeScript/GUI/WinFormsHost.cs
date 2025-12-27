using PipeScript.API;

namespace PipeScript.GUI;

public sealed class WinFormsHost(ScriptForm form) : IScriptHost
{
    public void WriteLine(string text)
    {
        form.WriteLine(text);
    }

    public void Write(string text)
    {
        form.Write(text);
    }
}