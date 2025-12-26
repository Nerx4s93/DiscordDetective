using System;
using System.IO;

using PipeScript.CommandResults;

namespace PipeScript.Commands;

internal sealed class Include : PipeCommand
{
    public override string Name => "include";

    public override CommandResult Execute(string[] args, ExecutionContext ctx)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("Usage: include <path>");
        }

        var path = args[0];
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: {path}", path);
        }

        var code = File.ReadAllText(path);

        return new IncludeResult(code);
    }

    public override bool ValidateArgs(string[] args) => args.Length == 1;
}
