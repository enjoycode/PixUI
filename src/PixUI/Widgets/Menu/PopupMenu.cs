using System;
using System.Collections.Generic;

namespace PixUI;

/// <summary>
/// 弹出的子菜单，由MenuController创建并加入至其PopupMenuStack内
/// </summary>
internal sealed class PopupMenu : Widget
{
    internal PopupMenu(MenuItemWidget? owner, MenuItem[]? items, int depth, MenuController controller)
    {
        if (owner == null && items == null)
            throw new ArgumentNullException();

        Owner = owner;
        _controller = controller;
        if (owner != null)
        {
            _children = new List<MenuItemWidget>(owner.MenuItem.Children!.Count);
            BuildMenuItemWidgets(owner.MenuItem.Children!, depth);
        }
        else
        {
            _children = new List<MenuItemWidget>(items!.Length);
            BuildMenuItemWidgets(items, depth);
        }
    }

    internal readonly MenuItemWidget? Owner; //maybe null on ContextMenu
    private readonly IList<MenuItemWidget> _children;
    private readonly MenuController _controller;

    private void BuildMenuItemWidgets(IEnumerable<MenuItem> items, int depth)
    {
        foreach (var item in items)
        {
            var child = new MenuItemWidget(item, depth, true, _controller);
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

    public override void Layout(float availableWidth, float availableHeight)
    {
        if (HasLayout) return;
        HasLayout = true;

        var maxChildWidth = 0f;
        MenuItemWidget? maxWidthChild = null;
        var offsetY = 0f;
        foreach (var child in _children)
        {
            child.Layout(float.PositiveInfinity, _controller.PopupItemHeight);
            child.SetPosition(0, offsetY);
            if (child.W >= maxChildWidth)
            {
                maxChildWidth = child.W;
                if (maxWidthChild == null || child.MenuItem.Type != MenuItemType.SubMenu)
                    maxWidthChild = child;
            }

            offsetY += child.H;
        }

        if (maxWidthChild!.MenuItem.Type != MenuItemType.SubMenu)
        {
            maxChildWidth += Theme.DefaultFontSize;
        }

        //重设所有子项等宽
        foreach (var child in _children)
        {
            child.ResetWidth(maxChildWidth);
        }

        SetSize(maxChildWidth, offsetY);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //画背景及阴影
        using var rrect = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, W, H), 4f, 4f);
        using var path = new Path();
        path.AddRRect(rrect);
        canvas.DrawShadow(path, Colors.Black, 5, false, Root!.Window.ScaleFactor);
        var paint = PaintUtils.Shared(_controller.BackgroundColor);
        canvas.DrawRRect(rrect, paint);

        canvas.Save();
        canvas.ClipPath(path, ClipOp.Intersect, false);

        PaintChildren(canvas, area);

        canvas.Restore();
    }
}