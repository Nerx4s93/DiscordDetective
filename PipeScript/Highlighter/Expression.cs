using System;

namespace PipeScript.Highlighter;

internal class Expression(string content, ExpressionType type, string group)
{
    public ExpressionType Type { get; private set; } = type;
    public string Content { get; private set; } = content ?? throw new ArgumentNullException(nameof(content));
    public string Group { get; private set; } = group ?? throw new ArgumentNullException(nameof(group));

    public Expression(string content, ExpressionType type) : this(content, type, string.Empty) { }

    public override string ToString()
    {
        if (Type == ExpressionType.Newline)
        {
            return $"({Type})";
        }

        return $"({Content} --> {Type}{(Group.Length > 0 ? " --> " + Group : string.Empty)})";
    }
}
