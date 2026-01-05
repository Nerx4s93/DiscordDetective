namespace DiscordDetective.Pipeline;

public enum PipelineTaskType
{
    None,
    DiscoverGuildChannels,
    DownloadChannelMessages,
    ProcessMessagesWithAi,
    PersistStructuredData
}