namespace PipeScript;

public interface IExecutionObserver
{
    void BeforeExecute(ScriptFrame frame);
    void AfterExecute(ScriptFrame frame);
}