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
        Relayout();
    }

    internal void OnRemoveAt(int index)
    {
        _tabs[index].Parent = null;
        _tabs.RemoveAt(index);
        Relayout();
    }

    #endregion

    #region ====Widget Overrides====
    
    protected internal override Size LayoutSize
    {
        get
        {
            //重写因为可能TabView的布局空间不够TabBar指定的高度造成的overflow
            var tabView = Parent;
            //TODO: 还需要根据TabBar的方向判断处理
            return tabView == null
                ? base.LayoutSize
                : new(base.LayoutSize.Width, Math.Min(tabView.LayoutSize.Height, base.LayoutSize.Height));
        }
    }

    private Tab BuildTab(T dataItem)
    {
        var tab = new Tab();
        _tabBuilder(dataItem, tab);
        tab.Parent = this;
        tab.OnTap = _ => OnTabSelected(tab);
        return tab;
    }

    public override bool IsOpaque => BgColor is { IsOpaque: true };

    public override void VisitChildren<TVisitor>(ref TVisitor visitor)
    {
        if (_tabs.Count == 0) return;
        foreach (var tab in _tabs)
        {
            if (visitor.Visit(tab)) break;
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

    protected override void OnLayout(Size maxSize)
    {
        if (_tabs.Count == 0)
        {
            SetLayoutSize(maxSize);
            return;
        }

        if (Scrollable)
        {
            SetLayoutSize(maxSize); //TODO:考虑累加宽度

            var offsetX = 0f;
            for (var i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].PerformLayout(float.PositiveInfinity, maxSize.Height);
                _tabs[i].SetLayoutLocation(offsetX, 0);
                offsetX += _tabs[i].W;
            }
        }
        else
        {
            SetLayoutSize(maxSize);

            var tabWidth = maxSize.Width / _tabs.Count;
            for (var i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].PerformLayout(tabWidth, maxSize.Height);
                _tabs[i].SetLayoutLocation(tabWidth * i, 0);
            }
        }
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        if (BgColor != null)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), Paint.Shared(BgColor.Value));

        if (area is RepaintChild repaintChild)
        {
            repaintChild.Repaint(canvas);
            return;
        }

        foreach (var tab in _tabs) //TODO: check visible
        {
            PaintChild(tab, canvas, area);
        }

        //TODO: paint indicator(因为动画移动需要)
    }

    #endregion
}