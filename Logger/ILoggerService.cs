using System.Threading.Tasks;

namespace Logger;

public interface ILoggerService
{
    bool ShowCategory { get; set; }

    Task LogAsync(string message, LogLevel level = LogLevel.Text);
    Task LogAsync(string category, string message, LogLevel level = LogLevel.Text);
    Task LogEmptyLineAsync();
    Task ClearAsync();

    void BeginLog();
    void EndLog();
}