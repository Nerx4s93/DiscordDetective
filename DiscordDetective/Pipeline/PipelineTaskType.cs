namespace DiscordDetective.Pipeline;

public enum PipelineTaskType
{
    DownloadChannels,
    SaveChannels,
    FetchUsers,
    FetchMessages,
    ProcessMessagesWithAi,
    PersistStructuredData
}