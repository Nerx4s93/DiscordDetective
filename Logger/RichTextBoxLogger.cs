using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logger;

public class RichTextBoxLogger(RichTextBox richTextBox) : ILoggerService
{
    private readonly RichTextBox _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));

    public bool ShowCategory { get; set; } = true;

    public Task LogAsync(string message, LogLevel level = LogLevel.Text)
    {
        return LogAsync("General", message, level);
    }

    public Task LogAsync(string category, string message, LogLevel level = LogLevel.Text)
    {
        if (_richTextBox.InvokeRequired)
        {
            _richTextBox.BeginInvoke(() => AppendLog(category, message, level));
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
            _richTextBox.BeginInvoke(() => _richTextBox.AppendText(Environment.NewLine));
        }
        else
        {
            _richTextBox.AppendText(Environment.NewLine);
        }

        return Task.CompletedTask;
    }

    private void AppendLog(string category, string message, LogLevel level)
    {
        var color = GetColor(level);

        _richTextBox.SuspendLayout();

        _richTextBox.SelectionStart = _richTextBox.TextLength;
        _richTextBox.SelectionLength = 0;
        _richTextBox.SelectionColor = color;

        var text = ShowCategory ? $"[{category}] [{level}] {message}" : message;
        _richTextBox.AppendText(text);
        _richTextBox.AppendText(Environment.NewLine);

        _richTextBox.SelectionColor = _richTextBox.ForeColor;

        _richTextBox.ResumeLayout();
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

    public void BeginLog()
    {
        if (_richTextBox.InvokeRequired)
        {
            _richTextBox.Invoke(() => _richTextBox.SuspendLayout());
        }
        else
        {

            _richTextBox.SuspendLayout();
        }
    }

    public void EndLog()
    {
        if (_richTextBox.InvokeRequired)
        {
            _richTextBox.Invoke(() => _richTextBox.ResumeLayout());
        }
        else
        {
            _richTextBox.ResumeLayout();
        }
    }

    private static Color GetColor(LogLevel level)
    {
        return level switch
        {
            LogLevel.Info => Color.DodgerBlue,
            LogLevel.Warning => Color.Goldenrod,
            LogLevel.Error => Color.IndianRed,
            LogLevel.Debug => Color.SeaGreen,
            _ => Color.Black
        };
    }
}