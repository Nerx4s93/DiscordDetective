using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace PipeScript.UI;

internal class ScriptCodeView : RichTextBoxNoSmoothScroll
{
    private List<PipeCommand>? _commands;
    private readonly Regex _variableRegex = new Regex(@"\$\w+", RegexOptions.Compiled);

    private string[]? _lines;
    private int _currentLine = -1;

    public ScriptCodeView()
    {
        DetectUrls = false;
        WordWrap = false;
    }

    public void SendCommands(List<PipeCommand> commands)
    {
        _commands = commands;
    }

    public void SetScript(string code)
    {
        _lines = code.Split('\n');
        Text = code;
        _currentLine = -1;
    }

    public void SetScript(string[] lines)
    {
        _lines = lines;
        Text = string.Join(Environment.NewLine, lines);
        _currentLine = -1;
    }

    public void HighlightVisibleSyntax()
    {
        if (_lines == null || _lines.Length == 0)
        {
            return;
        }

        var firstCharIndex = GetCharIndexFromPosition(new Point(0, 0));
        var lastCharIndex = GetCharIndexFromPosition(new Point(ClientSize.Width, ClientSize.Height));

        var firstLine = GetLineFromCharIndex(firstCharIndex);
        var lastLine = GetLineFromCharIndex(lastCharIndex);

        if (firstLine < 0)
        {
            firstLine = 0;
        }

        if (lastLine >= _lines.Length)
        {
            lastLine = _lines.Length - 1;
        }

        var selStart = SelectionStart;
        var selLength = SelectionLength;

        SuspendLayout();

        for (var i = firstLine; i <= lastLine; i++)
        {
            var line = _lines[i];
            var lineStart = GetFirstCharIndexFromLine(i);

            // сброс цвета всей строки
            SelectionStart = lineStart;
            SelectionLength = line.Length;
            SelectionColor = ForeColor;

            // подсветка комментария
            var commentIndex = line.IndexOf(';');
            if (commentIndex >= 0)
            {
                SelectionStart = lineStart + commentIndex;
                SelectionLength = line.Length - commentIndex;
                SelectionColor = Color.Green;
            }

            // подсветка команды (до комментария)
            var firstWord = line.TrimStart().Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            if (!string.IsNullOrEmpty(firstWord) && _commands?.Exists(x => x.Name == firstWord) == true)
            {
                var cmdStart = lineStart + line.IndexOf(firstWord);
                var cmdLength = firstWord.Length;
                if (commentIndex >= 0 && cmdStart + cmdLength > lineStart + commentIndex)
                {
                    cmdLength = Math.Max(0, lineStart + commentIndex - cmdStart);
                }

                if (cmdLength > 0)
                {
                    SelectionStart = cmdStart;
                    SelectionLength = cmdLength;
                    SelectionColor = Color.Blue;
                }
            }

            foreach (Match m in _variableRegex.Matches(line))
            {
                var varStart = lineStart + m.Index;
                var varLength = m.Length;

                if (commentIndex >= 0 && varStart >= lineStart + commentIndex)
                {
                    continue;
                }

                SelectionStart = varStart;
                SelectionLength = varLength;
                SelectionColor = Color.DarkRed;
            }
        }

        SelectionStart = selStart;
        SelectionLength = selLength;

        ResumeLayout();
    }

}