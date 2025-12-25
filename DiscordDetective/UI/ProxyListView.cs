using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Px6Api.DTOModels;

namespace DiscordDetective.UI;

public class ProxyListView : Panel
{
    private List<ProxyListViewItem> _items = [];
    private FlowLayoutPanel _container = null!;

    // TODO
    public event EventHandler<ProxyInfo> SelectionChanged = null!;
    public event EventHandler<ProxyInfo> ItemClicked = null!;
    public event EventHandler<ProxyInfo> CopyClicked = null!;
    public event EventHandler<ProxyInfo> CheckClicked = null!;
    public event EventHandler<ProxyInfo> DeleteClicked = null!;
    public event EventHandler<ProxyInfo> CommentClicked = null!;

    public List<ProxyInfo> SelectedProxies => _items
        .Where(i => i is { Selected: true, Proxy: not null })
        .Select(i => i.Proxy!).ToList();

    public ProxyListView()
    {
        InitializeComponent();
        SetupContainer();
    }

    #region Загрузка

    private void InitializeComponent()
    {
        //Dock = DockStyle.Fill;
        BorderStyle = BorderStyle.FixedSingle;
        BackColor = Color.White;
        Padding = new Padding(1);
    }

    private void SetupContainer()
    {
        _container = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = false,
            AutoScroll = true,
            BackColor = Color.White
        };

        Controls.Add(_container);
    }

    #endregion

    public void AddProxy(ProxyInfo proxy)
    {
        var item = new ProxyListViewItem
        {
            Proxy = proxy
        };

        _container.Controls.Add(item);
        _items.Add(item);
    }

    public void AddRange(List<ProxyInfo> proxies)
    {
        SuspendLayout();

        foreach (var proxy in proxies)
        {
            AddProxy(proxy);
        }

        ResumeLayout();
    }

    public void AddRange(IEnumerable<ProxyInfo> proxies)
    {
        SuspendLayout();

        foreach (var proxy in proxies)
        {
            AddProxy(proxy);
        }

        ResumeLayout();
    }

    public void RemoveProxy(ProxyInfo proxy)
    {
        var item = _items.FirstOrDefault(i => i.Proxy?.Id == proxy.Id);
        if (item != null)
        {
            RemoveItem(item);
        }
    }

    public void RemoveSelected()
    {
        var selected = _items.Where(i => i.Selected).ToList();
        foreach (var item in selected)
        {
            RemoveItem(item);
        }
    }

    public void Clear()
    {
        SuspendLayout();

        foreach (var item in _items)
        {
            item.Dispose();
        }

        _container.Controls.Clear();
        _items.Clear();

        ResumeLayout();
    }

    public void SelectAll(bool select = true)
    {
        foreach (var item in _items)
        {
            item.Selected = select;
        }
    }

    // Фильтрация
    public void Filter(Func<ProxyInfo, bool> predicate)
    {
        SuspendLayout();

        foreach (var item in _items)
        {
            item.Visible = item.Proxy != null && predicate(item.Proxy);
        }

        ResumeLayout();
    }

    // Сортировка
    public void SortByDate(bool ascending = true)
    {
        var sorted = ascending
            ? _items.OrderBy(i => i.Proxy?.UnixTimeEnd ?? 0).ToList()
            : _items.OrderByDescending(i => i.Proxy?.UnixTimeEnd ?? 0).ToList();
        ReorderItems(sorted);
    }

    public void SortByCountry()
    {
        var sorted = _items.OrderBy(i => i.Proxy?.Country ?? "").ToList();
        ReorderItems(sorted);
    }

    private void ReorderItems(List<ProxyListViewItem> sortedItems)
    {
        SuspendLayout();

        _container.Controls.Clear();
        _container.Controls.AddRange(sortedItems.Cast<Control>().ToArray());
        _items = sortedItems;

        ResumeLayout();
    }

    private void RemoveItem(ProxyListViewItem item)
    {
        _container.Controls.Remove(item);
        _items.Remove(item);
        item.Dispose();
    }
}