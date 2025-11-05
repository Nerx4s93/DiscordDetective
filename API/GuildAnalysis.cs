using System;
using System.Collections.Generic;

using DiscordDetective.API.Models;

namespace DiscordDetective.API;

public class GuildAnalysis
{
    public int TotalGuilds { get; set; }
    public int OwnedGuilds { get; set; }
    public List<Guild> Guilds { get; set; } = new();
    public DateTime AnalysisDate { get; set; }

    public int MemberGuilds => TotalGuilds - OwnedGuilds;

    public override string ToString()
    {
        return $"Серверов: {TotalGuilds} (Владелец: {OwnedGuilds}, Участник: {MemberGuilds})";
    }
}