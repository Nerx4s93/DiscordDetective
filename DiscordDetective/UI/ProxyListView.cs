using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using Px6Api;
using Px6Api.DTOModels;

namespace DiscordDetective.UI;

public partial class ProxyListView : UserControl
{
    private List<ProxyListViewItem> _items = [];
    private Px6Client? _px6Client;

    public List<ProxyInfo> SelectedProxies => SelectedItems
        .Select(i => i.Proxy!).ToList();

    public List<ProxyListViewItem> SelectedItems => _items
        .Where(i => i is { IsSelected: true, Proxy: not null }).ToList();

    public ProxyListView()
    {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Px6Client? Px6Client
    {
        get => _px6Client;
        set
        {
            _px6Client = value;
            _items.ForEach(item => item.Px6Client = value);
        }
    }

    public void AddProxy(ProxyInfo proxy)
    {
        var item = new ProxyListViewItem
        {
            Proxy = proxy,
            Px6Client = _px6Client
        };
        item.Selected += Item_Selected;
        item.Deleted += Item_Deleted;

        container.Controls.Add(item);
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
        var selected = _items.Where(i => i.IsSelected).ToList();
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

        container.Controls.Clear();
        _items.Clear();

        ResumeLayout();
    }

    public void SelectAll(bool select = true)
    {
        foreach (var item in _items)
        {
            item.IsSelected = select;
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

        container.Controls.Clear();
        container.Controls.AddRange(sortedItems.Cast<Control>().ToArray());
        _items = sortedItems;

        ResumeLayout();
    }

    private void RemoveItem(ProxyListViewItem item)
    {
        container.Controls.Remove(item);
        _items.Remove(item);
        item.Dispose();
    }

    #region События

    private void checkBoxItemSelected_CheckedChanged(object sender, EventArgs e)
    {
        var value = checkBoxItemSelected.Checked;
        _items.ForEach(item => item.IsSelected = value);
    }

    private void Item_Selected(object? sender, EventArgs e)
    {
        SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        checkBoxItemSelected.Checked = SelectedProxies.Count == _items.Count;
    }

    private void Item_Deleted(object? sender, EventArgs e)
    {
        var item = sender as ProxyListViewItem;
        RemoveItem(item!);
    }

    #endregion

    public event EventHandler? SelectedIndexChanged;
}