using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI;

public sealed class MainMenu : Widget
{
    public MainMenu(MenuItem[] items)
    {
        _children = new List<MenuItemWidget>(items.Length);
        _controller = new MenuController();
        BuildMenuItemWidgets(items);
    }

    private readonly IList<MenuItemWidget> _children;
    private readonly MenuController _controller;

    public Color BackgroudColor
    {
        set => _controller.BackgroundColor = value;
    }

    public Color Color
    {
        set => _controller.Color = value;
    }

    private void BuildMenuItemWidgets(MenuItem[] items)
    {
        foreach (var item in items)
        {
            var child = new MenuItemWidget(item, 0, false, _controller);
            child.Parent = this;
            _children.Add(child);
        }
    }

    public override void VisitChildren(Func<Widget, bool> action)
    {
        foreach (var child in _children)
        {
            if (action(child)) break;
        }
    }

    /// <summary>
    /// 布局充满可用空间
    /// </summary>
    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);
        SetSize(width, height);

        if (HasLayout) return; //只布局一次，除非强制重布
        HasLayout = true;

        var offsetX = 0f;
        foreach (var child in _children)
        {
            child.Layout(float.PositiveInfinity, height);
            child.SetPosition(offsetX, 0);
            offsetX += child.W;
        }
    }
}