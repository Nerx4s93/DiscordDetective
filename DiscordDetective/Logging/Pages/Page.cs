using System.Collections.Generic;

namespace DiscordDetective.Logging.Pages;

public sealed class Page(string id, string title)
{
    public string Id { get; } = id;
    public string Title { get; } = title;
    public List<IPageElement> Elements { get; } = [];
}
