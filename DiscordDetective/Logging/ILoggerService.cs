using System.Threading.Tasks;

namespace DiscordDetective.Logging;

public interface ILoggerService
{
    Task LogAsync(string message, LogLevel level = LogLevel.Info);
    Task LogAsync(string category, string message, LogLevel level = LogLevel.Info);
    Task LogEmptyLineAsync();
    Task ClearAsync();

    void BeginLog();
    void EndLog();
}