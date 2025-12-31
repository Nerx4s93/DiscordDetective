using System;
using System.Collections.Generic;

namespace PipeScript;

public sealed class ScriptCode
{
    public string Name { get; }
    public string[] Lines { get; }
    public string[] CleanLines { get; }
    public int[] SourceLineMap { get; }

    public ScriptCode(string name, string source)
    {
        Name = name;
        Lines = source.Split(["\r\n", "\n"], StringSplitOptions.None);

        var clean = new List<string>();
        var map = new List<int>();

        for (var i = 0; i < Lines.Length; i++)
        {
            var line = Lines[i].Trim();

            if (line.Length == 0)
            {
                continue;
            }

            var semicolonIndex = -1;
            var inQuotes = false;
            for (var j = 0; j < line.Length; j++)
            {
                if (line[j] == '"') inQuotes = !inQuotes;
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

        CleanLines = clean.ToArray();
        SourceLineMap = map.ToArray();
    }
}