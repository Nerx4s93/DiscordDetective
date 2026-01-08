using System;
using System.Threading.Tasks;

namespace Logger.Pages.Elements;

public class ProgressElement : IPageElement
{
    public string Text { get; set; } = string.Empty;
    public float Value { get; set; }
    public float MaxValue { get; set; } = 1f;
    public int Width { get; set; } = 20;
    public bool Inline { get; set; }

    public async Task Print(ILoggerService logger)
    {
        if (Inline)
        {
            await logger.LogAsync($"{Text}: [{GetProgressBar()}]");
        }
        else
        {
            await logger.LogAsync(Text);
            await logger.LogAsync($"[{GetProgressBar()}]");
        }
    }

    public string GetProgressBar()
    {
        if (MaxValue <= 0)
        {
            MaxValue = 1;
        }

        var clampedValue = Math.Max(0, Math.Min(Value, MaxValue));
        var filledLength = (int)Math.Round((clampedValue / MaxValue) * Width);

        var bar = new string('▓', filledLength) + new string('░', Width - filledLength);
        return bar;
    }
}