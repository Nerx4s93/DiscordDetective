using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PipeScript;

internal class CommandRegistry
{
    private readonly Dictionary<string, PipeCommand> _commands = new(StringComparer.OrdinalIgnoreCase);

    public int Count => _commands.Count;

    public IEnumerable<string> CommandNames => _commands.Keys;

    public IEnumerable<PipeCommand> Commands => _commands.Values;

    public CommandRegistry()
    {
        var commandType = typeof(PipeCommand);
        var commandTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => !t.IsAbstract && commandType.IsAssignableFrom(t));

        foreach (var type in commandTypes)
        {
            if (Activator.CreateInstance(type) is PipeCommand command)
            {
                _commands.Add(command.Name, command);
            }
        }
    }

    public PipeCommand? GetCommand(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        _commands.TryGetValue(name, out var command);
        return command;
    }
}
