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

        var existingPatternStyle = FindPatternStyle(name);

        if (existingPatternStyle != null)
        {
            throw new ArgumentException("A pattern style pair with the same name already exists");
        }

        _patternStyles.Add(new PatternStyleMap(name, patternDefinition, syntaxStyle));
    }

    protected SyntaxStyle GetDefaultStyle()
    {
        return new SyntaxStyle(_richTextBox.ForeColor, _richTextBox.Font.Bold, _richTextBox.Font.Italic);
    }

    private PatternStyleMap? FindPatternStyle(string name)
    {
        var patternStyle = _patternStyles
            .FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.Ordinal));
        return patternStyle;
    }

    public void ReHighlight()
    {
        if (!DisableHighlighting)
        {
            if (_isDuringHighlight)
            {
                return;
            }

            _richTextBox.DisableThenDoThenEnable(HighlighTextBase);
        }
    }

    private void RichTextBox_TextChanged(object sender, EventArgs e)
    {
        ReHighlight();
    }

    internal IEnumerable<Expression> Parse(string text)
    {
        text = text.NormalizeLineBreaks("\n");
        var parsedExpressions = new List<Expression> { new (text, ExpressionType.None, string.Empty) };

        foreach (var patternStyleMap in _patternStyles)
        {
            parsedExpressions = ParsePattern(patternStyleMap, parsedExpressions);
        }

        parsedExpressions = ProcessLineBreaks(parsedExpressions);
        return parsedExpressions;
    }

    private Regex GetLineBreakRegex()
    {
        if (_lineBreakRegex == null)
        {
            _lineBreakRegex = new Regex(Regex.Escape("\n"), RegexOptions.Compiled);
        }

        return _lineBreakRegex;
    }

    private List<Expression> ProcessLineBreaks(List<Expression> expressions)
    {
        var parsedExpressions = new List<Expression>();

        var regex = GetLineBreakRegex();

        foreach (var inputExpression in expressions)
        {
            var lastProcessedIndex = -1;

            foreach (var match in regex.Matches(inputExpression.Content).OrderBy(m => m.Index))
            {
                if (match.Success)
                {
                    if (match.Index > lastProcessedIndex + 1)
                    {
                        var nonMatchedContent = inputExpression.Content.Substring(lastProcessedIndex + 1,
                            match.Index - lastProcessedIndex - 1);
                        var nonMatchedExpression = new Expression(nonMatchedContent, inputExpression.Type, inputExpression.Group);
                        parsedExpressions.Add(nonMatchedExpression);
                    }

                    var matchedContent = inputExpression.Content.Substring(match.Index, match.Length);
                    var matchedExpression = new Expression(matchedContent, ExpressionType.Newline, "line-break");
                    parsedExpressions.Add(matchedExpression);
                    lastProcessedIndex = match.Index + match.Length - 1;
                }
            }

            if (lastProcessedIndex < inputExpression.Content.Length - 1)
            {
                var nonMatchedContent = inputExpression.Content.Substring(
                    lastProcessedIndex + 1,
                    inputExpression.Content.Length - lastProcessedIndex - 1);
                var nonMatchedExpression = new Expression(nonMatchedContent, inputExpression.Type, inputExpression.Group);
                parsedExpressions.Add(nonMatchedExpression);
            }
        }

        return parsedExpressions;
    }

    private List<Expression> ParsePattern(PatternStyleMap patternStyleMap, List<Expression> expressions)
    {
        var parsedExpressions = new List<Expression>();

        foreach (var inputExpression in expressions)
        {
            if (inputExpression.Type != ExpressionType.None)
            {
                parsedExpressions.Add(inputExpression);
            }
            else
            {
                var regex = patternStyleMap.PatternDefinition.Regex;

                var lastProcessedIndex = -1;

                foreach (var match in regex.Matches(inputExpression.Content).Cast<Match>().OrderBy(m => m.Index))
                {
                    if (match.Success)
                    {
                        if (match.Index > lastProcessedIndex + 1)
                        {
                            var nonMatchedContent = inputExpression.Content.Substring(lastProcessedIndex + 1, match.Index - lastProcessedIndex - 1);
                            var nonMatchedExpression = new Expression(nonMatchedContent, ExpressionType.None, string.Empty);
                            parsedExpressions.Add(nonMatchedExpression);
                        }

                        var matchedContent = inputExpression.Content.Substring(match.Index, match.Length);
                        var matchedExpression = new Expression(matchedContent, patternStyleMap.PatternDefinition.ExpressionType, patternStyleMap.Name);
                        parsedExpressions.Add(matchedExpression);
                        lastProcessedIndex = match.Index + match.Length - 1;
                    }
                }

                if (lastProcessedIndex < inputExpression.Content.Length - 1)
                {
                    var nonMatchedContent = inputExpression.Content.Substring(lastProcessedIndex + 1, inputExpression.Content.Length - lastProcessedIndex - 1);
                    var nonMatchedExpression = new Expression(nonMatchedContent, ExpressionType.None, string.Empty);
                    parsedExpressions.Add(nonMatchedExpression);
                }
            }
        }

        return parsedExpressions;
    }

    internal IEnumerable<StyleGroupPair> GetStyles()
    {
        yield return new StyleGroupPair(GetDefaultStyle(), string.Empty);

        foreach (var patternStyle in _patternStyles)
        {
            var style = patternStyle.SyntaxStyle;
            yield return new StyleGroupPair(new SyntaxStyle(style.Color, style.Bold, style.Italic), patternStyle.Name);
        }
    }

    internal virtual string GetGroupName(Expression expression)
    {
        return expression.Group;
    }

    private List<StyleGroupPair> GetStyleGroupPairs()
    {
        if (_styleGroupPairs == null)
        {
            _styleGroupPairs = GetStyles().ToList();

            for (var i = 0; i < _styleGroupPairs.Count; i++)
            {
                _styleGroupPairs[i].Index = i + 1;
            }
        }

        return _styleGroupPairs;
    }

    #region RTF

    private void HighlighTextBase()
    {
        _isDuringHighlight = true;

        try
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(RTFHeader());
            stringBuilder.AppendLine(RTFColorTable());
            stringBuilder.Append(@"\viewkind4\uc1\pard\f0\fs").Append(_fontSizeFactor).Append(' ');

            foreach (var exp in Parse(_richTextBox.Text))
            {
                if (exp.Type == ExpressionType.Whitespace)
                {
                    stringBuilder.Append(exp.Content);
                }
                else if (exp.Type == ExpressionType.Newline)
                {
                    stringBuilder.AppendLine(@"\par");
                }
                else
                {
                    var content = exp.Content.Replace("\\", "\\\\").Replace("{", @"\{").Replace("}", @"\}");

                    var styleGroups = GetStyleGroupPairs();

                    var groupName = GetGroupName(exp);

                    var style = styleGroups.FirstOrDefault(s => String.Equals(s.GroupName, groupName, StringComparison.Ordinal));
                    
                    if (style != null)
                    {
                        var opening = "";
                        var closing = "";

                        if (style.SyntaxStyle.Bold)
                        {
                            opening += @"\b";
                            closing += @"\b0";
                        }

                        if (style.SyntaxStyle.Italic)
                        {
                            opening += @"\i";
                            closing += @"\i0";
                        }

                        stringBuilder.AppendFormat(@"\cf{0}{2} {1}\cf0{3} ", style.Index, content, opening, closing);
                    }
                    else
                    {
                        stringBuilder.AppendFormat(@"\cf{0} {1}\cf0 ", 1, content);
                    }
                }
            }

            stringBuilder.Append(@"\par }");

            _richTextBox.Rtf = stringBuilder.ToString();
        }
        finally
        {
            _isDuringHighlight = false;
        }
    }

    private string RTFColorTable()
    {
        var styles = GetStyleGroupPairs();

        if (styles.Count <= 0)
        {
            styles.Add(new StyleGroupPair(GetDefaultStyle(), ""));
        }

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(@"{\colortbl ;");

        foreach (var style in styles)
        {
            stringBuilder.Append($"{style.SyntaxStyle.Color.ToRtfEntry()};");
        }

        stringBuilder.Append('}');
        return stringBuilder.ToString();
    }

    private string RTFHeader()
    {
        return string.Concat(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 ", _fontName, @";}}");
    }

    #endregion
}