using System.IO;

namespace PipeScript.Commands;

internal sealed class Include : PipeCommand
{
    public override string Name => "include";

    public override object Execute(string[] args, ExecutionContext ctx)
    {
        var path = args[0];
        var script = File.ReadAllText(path);

        ctx.Engine.Execute(script);
        return null;
    }
}
