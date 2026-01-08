namespace DiscordDetective.Pipeline;

public enum PipelineTaskType
{
    DownloadChannels,
    FetchUsers,
    FetchMessages,
    ProcessMessagesWithAi,
    PersistStructuredData
}