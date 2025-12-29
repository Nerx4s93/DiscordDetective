using System;

namespace PipeScript.Highlighter;

internal class StyleGroupPair(SyntaxStyle syntaxStyle, string groupName)
{
    public int Index { get; set; }
    public SyntaxStyle SyntaxStyle { get; set; } = syntaxStyle ?? throw new ArgumentNullException(nameof(syntaxStyle));
    public string GroupName { get; set; } = groupName ?? throw new ArgumentNullException(nameof(groupName));
}