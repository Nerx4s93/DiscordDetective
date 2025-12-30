using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PipeScript.Highlighter;

internal class SyntaxHighlighter
{
    private readonly RichTextBox _richTextBox;
    private readonly int _fontSizeFactor;
    private readonly string _fontName;

    private bool _isDuringHighlight;

    private List<StyleGroupPair>? _styleGroupPairs;
    private readonly List<PatternStyleMap> _patternStyles = [];

    private Regex? _lineBreakRegex;

    public SyntaxHighlighter(RichTextBox richTextBox)
    {
        _richTextBox = richTextBox ?? throw new ArgumentNullException(nameof(richTextBox));

        _fontSizeFactor = Convert.ToInt32(_richTextBox.Font.Size * 2);
        _fontName = _richTextBox.Font.Name;

        DisableHighlighting = false;

        _richTextBox.TextChanged += RichTextBox_TextChanged;
    }

    public bool DisableHighlighting { get; set; }

    public void AddPattern(PatternDefinition patternDefinition, SyntaxStyle syntaxStyle)
    {
        AddPattern((_patternStyles.Count + 1).ToString(CultureInfo.InvariantCulture), patternDefinition, syntaxStyle);
    }

    public void AddPattern(string name, PatternDefinition patternDefinition, SyntaxStyle syntaxStyle)
    {
        if (patternDefinition == null)
        {
            throw new ArgumentNullException(nameof(patternDefinition));
        }

        if (syntaxStyle == null)
        {
            throw new ArgumentNullException(nameof(syntaxStyle));
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("name must not be null or empty", nameof(name));
        }

        if (FindPatternStyle(name) != null)
        {
            throw new ArgumentException("A pattern style pair with the same name already exists");
        }

        _patternStyles.Add(new PatternStyleMap(name, patternDefinition, syntaxStyle));
    }

    protected SyntaxStyle GetDefaultStyle()
    {
        return new SyntaxStyle(
            _richTextBox.ForeColor,
            _richTextBox.Font.Bold,
            _richTextBox.Font.Italic
        );
    }

    private PatternStyleMap? FindPatternStyle(string name)
    {
        return _patternStyles.FirstOrDefault(p =>
            string.Equals(p.Name, name, StringComparison.Ordinal));
    }

    public void ReHighlight()
    {
        if (DisableHighlighting)
        {
            return;
        }

        if (_isDuringHighlight)
        {
            return;
        }

        _richTextBox.DisableThenDoThenEnable(HighlighTextBase);
    }

    private void RichTextBox_TextChanged(object sender, EventArgs e)
    {
        ReHighlight();
    }

    internal IEnumerable<Expression> Parse(string text)
    {
        text = text.NormalizeLineBreaks("\n");

        var parsed = new List<Expression>
        {
            new(text, ExpressionType.None, string.Empty)
        };

        foreach (var patternStyle in _patternStyles)
        {
            parsed = ParsePattern(patternStyle, parsed);
        }

        parsed = ProcessLineBreaks(parsed);
        return parsed;
    }

    private Regex GetLineBreakRegex()
    {
        _lineBreakRegex ??= new Regex(Regex.Escape("\n"), RegexOptions.Compiled);
        return _lineBreakRegex;
    }

    private List<Expression> ProcessLineBreaks(List<Expression> expressions)
    {
        var result = new List<Expression>();
        var regex = GetLineBreakRegex();

        foreach (var input in expressions)
        {
            var lastIndex = -1;

            foreach (Match match in regex.Matches(input.Content))
            {
                if (match.Index > lastIndex + 1)
                {
                    var text = input.Content.Substring(
                        lastIndex + 1,
                        match.Index - lastIndex - 1
                    );

                    result.Add(new Expression(text, input.Type, input.Group));
                }

                result.Add(new Expression(
                    match.Value,
                    ExpressionType.Newline,
                    "line-break"
                ));

                lastIndex = match.Index + match.Length - 1;
            }

            if (lastIndex < input.Content.Length - 1)
            {
                var text = input.Content.Substring(
                    lastIndex + 1,
                    input.Content.Length - lastIndex - 1
                );

                result.Add(new Expression(text, input.Type, input.Group));
            }
        }

        return result;
    }

    private List<Expression> ParsePattern(PatternStyleMap patternStyleMap, List<Expression> expressions)
    {
        var result = new List<Expression>();
        var regex = patternStyleMap.PatternDefinition.Regex;

        foreach (var input in expressions)
        {
            if (input.Type != ExpressionType.None)
            {
                result.Add(input);
                continue;
            }

            var lastIndex = -1;

            foreach (Match match in regex.Matches(input.Content))
            {
                if (match.Index > lastIndex + 1)
                {
                    var text = input.Content.Substring(
                        lastIndex + 1,
                        match.Index - lastIndex - 1
                    );

                    result.Add(new Expression(text, ExpressionType.None, string.Empty));
                }

                result.Add(new Expression(
                    match.Value,
                    patternStyleMap.PatternDefinition.ExpressionType,
                    patternStyleMap.Name
                ));

                lastIndex = match.Index + match.Length - 1;
            }

            if (lastIndex < input.Content.Length - 1)
            {
                var text = input.Content.Substring(
                    lastIndex + 1,
                    input.Content.Length - lastIndex - 1
                );

                result.Add(new Expression(text, ExpressionType.None, string.Empty));
            }
        }

        return result;
    }

    internal IEnumerable<StyleGroupPair> GetStyles()
    {
        yield return new StyleGroupPair(GetDefaultStyle(), string.Empty);

        foreach (var ps in _patternStyles)
        {
            var s = ps.SyntaxStyle;
            yield return new StyleGroupPair(
                new SyntaxStyle(s.Color, s.Bold, s.Italic),
                ps.Name
            );
        }
    }

    internal virtual string GetGroupName(Expression expression)
    {
        return expression.Group;
    }

    private List<StyleGroupPair> GetStyleGroupPairs()
    {
        if (_styleGroupPairs != null)
        {
            return _styleGroupPairs;
        }

        _styleGroupPairs = GetStyles().ToList();

        for (var i = 0; i < _styleGroupPairs.Count; i++)
        {
            _styleGroupPairs[i].Index = i + 1;
        }

        return _styleGroupPairs;
    }

    #region RTF

    private void HighlighTextBase()
    {
        _isDuringHighlight = true;

        try
        {
            var sb = new StringBuilder();

            sb.Append(RTFHeader());
            sb.Append(RTFColorTable());
            sb.Append(@"\viewkind4\uc1\pard\f0\fs")
              .Append(_fontSizeFactor)
              .Append(' ');

            foreach (var exp in Parse(_richTextBox.Text))
            {
                if (exp.Type == ExpressionType.Newline)
                {
                    sb.Append(@"\par ");
                    continue;
                }

                var content = EscapeRtf(exp.Content);

                var styles = GetStyleGroupPairs();
                var group = GetGroupName(exp);

                var style = styles.FirstOrDefault(s =>
                    string.Equals(s.GroupName, group, StringComparison.Ordinal));

                if (style == null)
                {
                    sb.Append(@"\cf1 ").Append(content).Append(@"\cf0 ");
                    continue;
                }

                var open = "";
                var close = "";

                if (style.SyntaxStyle.Bold)
                {
                    open += @"\b";
                    close += @"\b0";
                }

                if (style.SyntaxStyle.Italic)
                {
                    open += @"\i";
                    close += @"\i0";
                }

                sb.Append(@"\cf")
                  .Append(style.Index)
                  .Append(open)
                  .Append(' ')
                  .Append(content)
                  .Append(@"\cf0")
                  .Append(close)
                  .Append(' ');
            }

            sb.Append(@"\par }");

            _richTextBox.Rtf = sb.ToString();
        }
        finally
        {
            _isDuringHighlight = false;
        }
    }

    private string RTFColorTable()
    {
        var styles = GetStyleGroupPairs();

        var sb = new StringBuilder();
        sb.Append(@"{\colortbl ;");

        foreach (var style in styles)
        {
            sb.Append(style.SyntaxStyle.Color.ToRtfEntry()).Append(';');
        }

        sb.Append('}');
        return sb.ToString();
    }

    private string RTFHeader()
    {
        return string.Concat(
            @"{\rtf1\ansi\ansicpg65001\deff0\deflang1049",
            @"{\fonttbl{\f0\fnil\fcharset204 ",
            _fontName,
            @";}}"
        );
    }

    private static string EscapeRtf(string text)
    {
        var sb = new StringBuilder(text.Length);

        foreach (var ch in text)
        {
            switch (ch)
            {
                case '\\': sb.Append(@"\\"); break;
                case '{': sb.Append(@"\{"); break;
                case '}': sb.Append(@"\}"); break;
                case '\r': break;
                case '\n': sb.Append(@"\par "); break;

                default:
                    {
                        if (ch <= 0x7F)
                        {
                            sb.Append(ch);
                        }
                        else
                        {
                            sb.Append(@"\u")
                              .Append((short)ch)
                              .Append('?');
                        }
                        break;
                    }
            }
        }

        return sb.ToString();
    }

    #endregion
}
