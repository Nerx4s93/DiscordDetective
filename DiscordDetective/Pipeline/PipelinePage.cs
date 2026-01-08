using System.Collections.Generic;
using Logger.Pages;
using Logger.Pages.Elements;

namespace DiscordDetective.Pipeline;

public class PipelinePage : Page
{
    private readonly Dictionary<PipelineTaskType, int> _taskCounts = new();

    private readonly TextElement _channelsDownload;
    private readonly TextElement _usersFetch;
    private readonly TextElement _messagesFetch;
    private readonly TextElement _messagesAi;
    private readonly TextElement _messagesSave;

    public PipelinePage(string id, string title) : base(id, title)
    {
        _channelsDownload = new TextElement(" ├─ Download: 0 tasks");
        _usersFetch = new TextElement(" └─ Fetch: 0 tasks");
        _messagesFetch = new TextElement(" ├─ Fetch: 0 tasks");
        _messagesAi = new TextElement(" ├─ AI processing: 0 tasks");
        _messagesSave = new TextElement(" └─ Save results: 0 tasks");

        Elements.Add(new TextElement($"[ Server: {id} ]"));
        Elements.Add(new TextElement(""));
        Elements.Add(new TextElement("Channels"));
        Elements.Add(_channelsDownload);
        Elements.Add(new TextElement(""));
        Elements.Add(new TextElement("Users"));
        Elements.Add(_usersFetch);
        Elements.Add(new TextElement(""));
        Elements.Add(new TextElement("Messages"));
        Elements.Add(_messagesFetch);
        Elements.Add(_messagesAi);
        Elements.Add(_messagesSave);
    }

    public void IncrementTaskCount(PipelineTaskType type, int value = 1)
    {
        _taskCounts.TryAdd(type, 0);
        _taskCounts[type] += value;

        switch (type)
        {
            case PipelineTaskType.DownloadChannels:
                _channelsDownload.Text = $" └─ Download: {_taskCounts[type]} tasks";
                break;
            case PipelineTaskType.FetchUsers:
                _usersFetch.Text = $" └─ Fetch: {_taskCounts[type]} tasks";
                break;
            case PipelineTaskType.FetchMessages:
                _messagesFetch.Text = $" ├─ Fetch: {_taskCounts[type]} tasks";
                break;
            case PipelineTaskType.ProcessMessagesWithAi:
                _messagesAi.Text = $" ├─ AI processing: {_taskCounts[type]} tasks";
                break;
            case PipelineTaskType.PersistStructuredData:
                _messagesSave.Text = $" └─ Save results: {_taskCounts[type]} tasks";
                break;
        }
    }
}