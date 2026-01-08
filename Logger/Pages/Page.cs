using System.Collections.Generic;

namespace Logger.Pages;

public sealed class Page(string id, string title)
{
    public string Id { get; } = id;
    public string Title { get; } = title;
    public List<IPageElement> Elements { get; } = [];
}
