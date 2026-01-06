using System;

namespace DiscordDetective.Pipeline;

public sealed class PipelineTask
{
    public Guid Id { get; } = Guid.NewGuid();

    public PipelineTaskType Type { get; init; }

    public string Payload { get; init; } = null!;
}
