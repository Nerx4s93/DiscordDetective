using System;
using System.Threading.Tasks;

namespace DiscordDetective.Logging;

internal class ConsoleLogger : ILoggerService
{
    public Task LogAsync(string message, LogLevel level = LogLevel.Info)
    {
        return LogAsync("General", message, level);
    }

    public Task LogAsync(string category, string message, LogLevel level = LogLevel.Info)
    {
        var color = GetConsoleColor(level);
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.WriteLine($"[{category}] [{level}] {message}");
        Console.ForegroundColor = originalColor;

        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        Console.Clear();
        return Task.CompletedTask;
    }

    private ConsoleColor GetConsoleColor(LogLevel level)
    {
        return level switch
        {
            LogLevel.Info => ConsoleColor.Blue,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Debug => ConsoleColor.Green,
            _ => ConsoleColor.White
        };
    }
}
