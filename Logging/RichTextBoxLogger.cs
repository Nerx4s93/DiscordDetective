using System;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiscordDetective.Logging;

internal class RichTextBoxLogger : ILoggerService
{
    private readonly RichTextBox _richTextBox;

    public RichTextBoxLogger(RichTextBox richTextBox)
    {
        _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));
    }

    public Task LogAsync(string message, LogLevel level = LogLevel.Info)
    {
        return LogAsync("General", message, level);
    }

    public Task LogAsync(string category, string message, LogLevel level = LogLevel.Info)
    {
        if (_richTextBox.InvokeRequired)
        {
            _richTextBox.BeginInvoke(new Action(() =>
                AppendLog(category, message, level)));
        }
        else
        {
            AppendLog(category, message, level);
        }

        return Task.CompletedTask;
    }

    public Task LogEmptyLineAsync()
    {
        if (_richTextBox.InvokeRequired)
        {
            _richTextBox.BeginInvoke(new Action(() => 
                _richTextBox.AppendText(Environment.NewLine)));
        }
        return Task.CompletedTask;
    }

    private void AppendLog(string category, string message, LogLevel level)
    {
        var logLine = $"[{category}] [{level}] {message}";
        _richTextBox.AppendText(logLine + Environment.NewLine);
    }

    public Task ClearAsync()
    {
        if (_richTextBox.InvokeRequired)
        {
            _richTextBox.BeginInvoke(new Action(() => _richTextBox.Clear()));
        }
        else
        {
            _richTextBox.Clear();
        }

        return Task.CompletedTask;
    }
}
