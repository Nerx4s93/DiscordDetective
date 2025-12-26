using System.Threading.Tasks;

namespace PipeScript;

public abstract class PipeCommand
{
    public abstract string Name { get; }

    public abstract object Execute(string[] args, Variables variables);

    public virtual Task<object> ExecuteAsync(string[] args, Variables variables)
    {
        return Task.FromResult(Execute(args, variables));
    }

    public virtual bool ValidateArgs(string[] args)
    {
        return true;
    }
}