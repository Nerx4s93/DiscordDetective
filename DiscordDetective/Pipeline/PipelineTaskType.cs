namespace DiscordDetective.Pipeline;

public enum PipelineTaskType
{
    DownloadChannels,         // скачивание списка чатов сервера
    FetchUsers,               // скачивание юзеров
    FetchMessages,            // скачивание сообщений чатов
    ProcessChatMessages,      // обработка сообщений чатов
    ProcessMessagesWithAi,    // обработка кусков чатов в ИИ
    PersistStructuredData     // сохранение ответа ИИ
}