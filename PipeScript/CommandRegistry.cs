using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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

    public object ExecuteCommand(string name, string[] args, Variables variables)
    {
        var command = GetCommand(name);
        if (command == null)
        {
            throw new InvalidOperationException($"Command '{name}' not found");
        }

        if (!command.ValidateArgs(args))
        {
            throw new ArgumentException($"Invalid arguments for command '{name}'");
        }

        return command.Execute(args, variables);
    }

    public async Task<object> ExecuteCommandAsync(string name, string[] args, Variables variables)
    {
        var command = GetCommand(name);
        if (command == null)
        {
            throw new InvalidOperationException($"Command '{name}' not found");
        }

        if (!command.ValidateArgs(args))
        {
            throw new ArgumentException($"Invalid arguments for command '{name}'");
        }

        return await command.ExecuteAsync(args, variables);
    }
}
