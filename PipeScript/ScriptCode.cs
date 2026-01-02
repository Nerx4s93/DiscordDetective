using System;
using System.Collections.Generic;
using System.Text;

namespace PipeScript;

public sealed class ScriptCode
{
    public Guid ScriptId { get; } = Guid.NewGuid();
    public string Name { get; }
    public string[] Lines { get; }
    public ScriptLine[] Compiled { get; }
    public IReadOnlyDictionary<string, int> Labels { get; }

    public ScriptCode(string name, string source)
    {
        Name = name;
        Lines = source.Split(["\r\n", "\n"], StringSplitOptions.None);

        var cleanedCode = CleanCode(Lines);
        Labels = CollectLabels(cleanedCode.cleanLines);
        Compiled = CompileCode(cleanedCode.cleanLines, cleanedCode.sourceLineMap);
    }

    private static (string[] cleanLines, int[] sourceLineMap) CleanCode(string[] lines)
    {
        var clean = new List<string>();
        var map = new List<int>();

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();

            if (line.Length == 0)
            {
                continue;
            }

            if (CheckForLabel(line))
            {
                clean.Add(line);
                map.Add(i);
                continue;
            }

            var semicolonIndex = -1;
            var inQuotes = false;
            for (var j = 0; j < line.Length; j++)
            {
                if (line[j] == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (line[j] == ';' && !inQuotes)
                {
                    semicolonIndex = j;
                    break;
                }
            }

            if (semicolonIndex >= 0)
            {
                line = line[..semicolonIndex].Trim();
            }

            if (line.Length == 0)
            {
                continue;
            }

            clean.Add(line);
            map.Add(i);
        }

        return (clean.ToArray(), map.ToArray());
    }

    private static bool CheckForLabel(string line)
    {
        if (!line.EndsWith(':'))
        {
            return false;
        }

        for (var i = 0; i < line.Length - 1; i++)
        {
            if (!char.IsLetterOrDigit(line[i]) && line[i] != '_')
            {
                return false;
            }
        }

        return true;
    }

    private static Dictionary<string, int> CollectLabels(string[] cleanLines)
    {
        var labels = new Dictionary<string, int>();
        var instructionIndex = 0;

        foreach (var line in cleanLines)
        {
            if (CheckForLabel(line))
            {
                var name = line[..^1];
                if (!labels.TryAdd(name, instructionIndex))
                {
                    throw new Exception($"Duplicate label '{name}'");
                }

                continue;
            }

            instructionIndex++;
        }

        return labels;
    }

    private static ScriptLine[] CompileCode(string[] cleanLines, int[] sourceLineMap)
    {
        var compiledCode = new List<ScriptLine>();

        for (var i = 0; i < cleanLines.Length; i++)
        {
            var line = cleanLines[i];

            if (CheckForLabel(line))
            {
                continue;
            }

            var commandName = GetCommandName(line);
            var argsString = GetArgs(line);
            var parsedArgs = argsString.Length == 0 ? [] : ParseArgs(argsString);

            compiledCode.Add(new ScriptLine(sourceLineMap[i], commandName, parsedArgs));
        }

        return compiledCode.ToArray();
    }

    private static string GetCommandName(string line)
    {
        var firstSpace = line.IndexOf(' ');
        return firstSpace < 0 ? line : line[..firstSpace];
    }

    private static string GetArgs(string line)
    {
        var firstSpace = line.IndexOf(' ');
        return firstSpace < 0 ? string.Empty : line[(firstSpace + 1)..];
    }

    private static string[] ParseArgs(string argString)
    {
        var result = new List<string>();
        var stringBuilder = new StringBuilder();

        var inQuotes = false;
        var skipSpaces = true;
        var escape = false;

        foreach (var c in argString)
        {
            if (escape)
            {
                stringBuilder.Append(c);
                escape = false;
                continue;
            }

            if (c == '\\')
            {
                escape = true;
                continue;
            }

            if (skipSpaces && char.IsWhiteSpace(c) && !inQuotes)
            {
                continue;
            }

            skipSpaces = false;

            switch (c)
            {
                case '"':
                    inQuotes = !inQuotes;
                    break;

                case ',' when !inQuotes:
                    result.Add(stringBuilder.ToString());
                    stringBuilder.Clear();
                    skipSpaces = true;
                    break;

                default:
                    stringBuilder.Append(c);
                    break;
            }
        }

        if (stringBuilder.Length > 0)
        {
            result.Add(stringBuilder.ToString());
        }

        return result.ToArray();
    }
}