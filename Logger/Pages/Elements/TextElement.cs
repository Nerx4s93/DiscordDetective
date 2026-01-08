using System.Threading.Tasks;

namespace Logger.Pages.Elements;

public sealed class TextElement(string text, LogLevel level = LogLevel.Info) : IPageElement
{
    public string Text { get; set; } = text;
    public LogLevel Level { get; set; } = level;

    public async Task Print(ILoggerService logger)
    {
        await logger.LogAsync(Text, Level);
    }
}