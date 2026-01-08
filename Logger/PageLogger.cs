using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Logger.Pages;

namespace Logger;

public class PageLogger(ILoggerService logger)
{
    private readonly Dictionary<string, Page> _pages = new();
    private Page? _activePage;

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

        _activePage = page;
    }

    public async Task PrintPage()
    {
        if (_activePage == null)
        {
            throw new InvalidOperationException("No active page selected.");
        }

        logger.BeginLog();
        await logger.ClearAsync();
        foreach (var element in _activePage.Elements)
        {
            await element.Print(logger);
        }
        logger.EndLog();
    }
}