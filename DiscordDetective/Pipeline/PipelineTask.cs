namespace DiscordDetective.Pipeline;

public sealed class PipelineTask
{
    public PipelineTaskType Type { get; init; }

    public string PayloadJson { get; init; } = null!;
}
