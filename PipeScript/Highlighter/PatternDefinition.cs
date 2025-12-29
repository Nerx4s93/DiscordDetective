using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PipeScript.Highlighter;

internal class PatternDefinition
{
    public PatternDefinition(Regex regularExpression)
    {
        Regex = regularExpression ?? throw new ArgumentNullException(nameof(regularExpression));
    }

    public PatternDefinition(string regexPattern)
    {
        if (string.IsNullOrEmpty(regexPattern))
        {
            throw new ArgumentException("regex pattern must not be null or empty", nameof(regexPattern));
        }

        Regex = new Regex(regexPattern, RegexOptions.Compiled);
    }

    public PatternDefinition(params string[] tokens) : this(true, tokens) { }

    public PatternDefinition(IEnumerable<string> tokens) : this(true, tokens) { }

    internal PatternDefinition(bool caseSensitive, IEnumerable<string> tokens)
    {
        if (tokens == null)
        {
            throw new ArgumentNullException(nameof(tokens));
        }

        IsCaseSensitive = caseSensitive;

        var regexTokens = new List<string>();
        foreach (var token in tokens)
        {
            var escapedToken = Regex.Escape(token.Trim());

            if (escapedToken.Length > 0)
            {
                regexTokens.Add(char.IsLetterOrDigit(escapedToken[0]) ? $@"\b{escapedToken}\b" : escapedToken);
            }
        }

        var pattern = string.Join("|", regexTokens);
        var regexOptions = RegexOptions.Compiled;
        if (!caseSensitive)
        {
            regexOptions = regexOptions | RegexOptions.IgnoreCase;
        }

        Regex = new Regex(pattern, regexOptions);
    }

    internal ExpressionType ExpressionType { get; set; } = ExpressionType.Identifier;

    internal bool IsCaseSensitive { get; } = true;

    internal Regex Regex { get; }
}