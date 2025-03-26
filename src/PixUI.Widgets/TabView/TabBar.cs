using System;
using System.Collections.Generic;

namespace PixUI;

internal interface ITabBar
{
    bool Scrollable { get; }
    Color? SelectedColor { get; }
    Color? HoverColor { get; }
}

public sealed class TabBar<T> : Widget, ITabBar
{
    public TabBar(TabController<T> controller, Action<T, Tab> tabBuilder, bool scrollable = false)
    {
        _controller = controller;
        _controller.BindTabBar(this);
        _tabBuilder = tabBuilder;
        Scrollable = scrollable;

        // build tabs
        if (_controller.DataSource.Count == _tabs.Count)
            return;

        foreach (var dataItem in _controller.DataSource)
        {
            _tabs.Add(BuildTab(dataItem));
        }

        _controller.SelectAt(0); //选中第一个Tab
    }

    private readonly TabController<T> _controller;
    private readonly Action<T, Tab> _tabBuilder;
    private readonly List<Tab> _tabs = new List<Tab>();
    internal IList<Tab> Tabs => _tabs;
    public bool Scrollable { get; }
    private float _scrollOffset;

    public Color? BgColor { get; set; }

    /// <summary>
    /// 选中的Tab的背景色
    /// </summary>
    public Color? SelectedColor { get; set; }

    /// <summary>
    /// Hover时的背景色
    /// </summary>
    public Color? HoverColor { get; set; }

    #region ====Event Handlers====

    private void OnTabSelected(Tab selected)
    {
        var selectedIndex = _tabs.IndexOf(selected);
        _controller.SelectAt(selectedIndex, true);
    }

    internal void OnAdd(T dataItem)
    {
        _tabs.Add(BuildTab(dataItem));
        Invalidate(InvalidAction.Relayout);
    }

    internal void OnRemoveAt(int index)
    {
        _tabs[index].Parent = null;
        _tabs.RemoveAt(index);
        Relayout();
    }

    #endregion

    #region ====Widget Overrides====

    private Tab BuildTab(T dataItem)
    {
        var tab = new Tab();
        _tabBuilder(dataItem, tab);
        tab.Parent = this;
        tab.OnTap = _ => OnTabSelected(tab);
        return tab;
    }

    public override bool IsOpaque => BgColor != null && BgColor.Value.IsOpaque;

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (_tabs.Count == 0) return;
        foreach (var tab in _tabs)
        {
            if (action(tab)) break;
        }
    }

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        if (!ContainsPoint(x, y)) return false;

        result.Add(this);
        if (_tabs.Count == 0) return true;

        foreach (var tab in _tabs)
        {
            var diffX = tab.X - _scrollOffset;
            if (tab.HitTest(x - diffX, y - tab.Y, result))
                return true;
        }

        return true;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var max = CacheAndGetMaxSize(availableWidth, availableHeight);
        if (_tabs.Count == 0)
        {
            SetSize(max.Width, max.Height);
            return;
        }

        if (Scrollable)
        {
            SetSize(max.Width, max.Height); //TODO:考虑累加宽度

            var offsetX = 0f;
            for (var i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].Layout(float.PositiveInfinity, max.Height);
                _tabs[i].SetPosition(offsetX, 0);
                offsetX += _tabs[i].W;
            }
        }
        else
        {
            SetSize(max.Width, max.Height);

            var tabWidth = max.Width / _tabs.Count;
            for (var i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].Layout(tabWidth, max.Height);
                _tabs[i].SetPosition(tabWidth * i, 0);
            }
        }
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (BgColor != null)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(BgColor.Value));

        if (area is RepaintChild repaintChild)
        {
            repaintChild.Repaint(canvas);
            return;
        }

        foreach (var tab in _tabs) //TODO: check visible
        {
            canvas.Translate(tab.X, tab.Y);
            tab.Paint(canvas, area?.ToChild(tab));
            canvas.Translate(-tab.X, -tab.Y);
        }

        //TODO: paint indicator(因为动画移动需要)
    }

    #endregion
}