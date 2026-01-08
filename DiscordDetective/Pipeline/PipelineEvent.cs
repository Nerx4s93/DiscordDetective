using System;

namespace DiscordDetective.Pipeline;

public sealed record PipelineEvent(
    PipelineTaskType Type,
    Guid TaskId,
    PipelineTaskProgress Progress,
    string GuildId);