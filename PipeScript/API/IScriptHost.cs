namespace PipeScript.API;

public interface IScriptHost
{
    void WriteLine(string text);
    void Write(string text);
}