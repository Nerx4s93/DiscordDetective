using System.Threading.Tasks;

namespace PipeScript;

public abstract class PipeCommand
{
    public abstract string Name { get; }

    public abstract object Execute(string[] args, ExecutionContext ctx);

    public virtual Task<object> ExecuteAsync(string[] args, ExecutionContext ctx)
    {
        return Task.FromResult(Execute(args, ctx));
    }

    public virtual bool ValidateArgs(string[] args) => true;
}