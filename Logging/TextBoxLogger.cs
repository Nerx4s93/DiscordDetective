using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordDetective.Logging;

internal class TextBoxLogger : ILoggerService
{
    private readonly TextBox _textBox;

    public TextBoxLogger(TextBox textBox)
    {
        _textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
    }

    public Task LogAsync(string message, LogLevel level = LogLevel.Info)
    {
        return LogAsync("General", message, level);
    }

    public Task LogAsync(string category, string message, LogLevel level = LogLevel.Info)
    {
        if (_textBox.InvokeRequired)
        {
            _textBox.BeginInvoke(new Action(() =>
                AppendLog(category, message, level)));
        }
        else
        {
            AppendLog(category, message, level);
        }

        return Task.CompletedTask;
    }

    private void AppendLog(string category, string message, LogLevel level)
    {
        var logLine = $"[{category}] [{level}] {message}";
        _textBox.AppendText(logLine + Environment.NewLine);
    }

    public Task ClearAsync()
    {
        if (_textBox.InvokeRequired)
        {
            _textBox.BeginInvoke(new Action(() => _textBox.Clear()));
        }
        else
        {
            _textBox.Clear();
        }

        return Task.CompletedTask;
    }
}
