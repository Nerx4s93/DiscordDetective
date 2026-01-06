namespace DiscordDetective.Pipeline;

public enum PipelineTaskType
{
    DiscoverGuildChannels,
    DownloadChannelMessages,
    ProcessMessagesWithAi,
    PersistStructuredData
}