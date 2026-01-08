using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.Pages;

namespace Logger;

public class PageLogger(ILoggerService logger)
{
    private readonly Dictionary<string, Page> _pages = new();

    public IReadOnlyDictionary<string, Page> Pages => _pages;
    public Page? ActivePage { get; private set; }

    public void CreatePage(Page page)
    {
        if (!_pages.TryAdd(page.Id, page))
        {
            throw new InvalidOperationException($"Page with Id '{page.Id}' already exists.");
        }
    }

    public void SelectPage(Page page)
    {
        SelectPage(page.Id);
    }

    public void SelectPage(string id)
    {
        if (!_pages.TryGetValue(id, out var page))
        {
            throw new KeyNotFoundException($"Page with Id '{id}' does not exist.");
        }

        ActivePage = page;
    }

    public async Task PrintPage()
    {
        if (ActivePage == null)
        {
            throw new InvalidOperationException("No active page selected.");
        }

        logger.BeginLog();
        await logger.ClearAsync();
        foreach (var element in ActivePage.Elements)
        {
            await element.Print(logger);
        }
        logger.EndLog();
    }
}