using System.Drawing;

namespace PipeScript.Highlighter;

internal class SyntaxStyle(Color color, bool bold, bool italic)
{
    public bool Bold { get; set; } = bold;
    public bool Italic { get; set; } = italic;
    public Color Color { get; set; } = color;

    public SyntaxStyle(Color color) : this(color, false, false) { }
}