using System;

namespace DiscordDetective.Pipeline;

public sealed class PipelineTask
{
    public Guid Id { get; init; } = Guid.Empty;

    public string GuildId { get; init; } = string.Empty;

    public PipelineTaskType Type { get; init; }

    public string Payload { get; init; } = null!;
}
