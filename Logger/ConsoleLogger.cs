using System;
using System.Threading.Tasks;

namespace Logger;

public class ConsoleLogger : ILoggerService
{
    public bool ShowCategory { get; set; } = true;

    public Task LogAsync(string message, LogLevel level = LogLevel.Text)
    {
        return LogAsync("General", message, level);
    }

    public Task LogAsync(string category, string message, LogLevel level = LogLevel.Text)
    {
        var color = GetColor(level);
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        var text = ShowCategory ? $"[{category}] [{level}] {message}" : message;
        Console.WriteLine(text);
        Console.ForegroundColor = originalColor;

        return Task.CompletedTask;
    }

    public Task LogEmptyLineAsync()
    {
        Console.WriteLine();
        return Task.CompletedTask; 
    }

    public Task ClearAsync()
    {
        Console.Clear();
        return Task.CompletedTask;
    }

    public void BeginLog() { }

    public void EndLog() { }

    private static ConsoleColor GetColor(LogLevel level)
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
