using System;

namespace PixUI;

internal sealed class MenuItemWidget : Widget, IMouseRegion
{
    internal MenuItemWidget(MenuItem menuItem, int depth, bool inPopup, MenuController controller)
    {
        Depth = depth;
        MenuItem = menuItem;
        _controller = controller;

        BuildChildren(inPopup);

        MouseRegion = new MouseRegion(() => Cursors.Hand);
        MouseRegion.HoverChanged += _OnHoverChanged;
        MouseRegion.PointerUp += _OnPointerUp;
    }

    internal readonly MenuItem MenuItem;
    internal readonly int Depth;
    private readonly MenuController _controller;

    private Icon? _icon;
    private Text? _label; //TODO:考虑实现并使用SimpleText
    private Icon? _expander;

    private bool _isHover;

    public MouseRegion MouseRegion { get; }

    private void BuildChildren(bool inPopup)
    {
        if (MenuItem.Type == MenuItemType.Divider)
            return;

        if (MenuItem.Icon != null)
        {
            _icon = new Icon(MenuItem.Icon) { Color = _controller.TextColor };
            _icon.Parent = this;
        }

        _label = new Text(MenuItem.Label!) { TextColor = _controller.TextColor };
        _label.Parent = this;

        if (MenuItem.Type == MenuItemType.SubMenu)
        {
            _expander = new Icon(inPopup ? MaterialIcons.ChevronRight : MaterialIcons.ExpandMore)
                { Color = _controller.TextColor };
            _expander.Parent = this;
        }
    }

    private void _OnPointerUp(PointerEvent e)
    {
        if (MenuItem.Type == MenuItemType.MenuItem && MenuItem.Action != null)
        {
            MenuItem.Action();
        }

        _controller.CloseAll();
    }

    private void _OnHoverChanged(bool hover)
    {
        if (MenuItem.Type == MenuItemType.Divider) return;

        _isHover = hover;
        Invalidate(InvalidAction.Repaint);

        _controller.OnMenuItemHoverChanged(this, hover);
    }

    /// <summary>
    /// 用于PopMenu计算完所有子节点宽度后重设
    /// </summary>
    internal void ResetWidth(float newWidth)
    {
        SetSize(newWidth, H);
        //右对齐快键指示orExpandIcon
        if (_expander != null)
        {
            var newX = W - _controller.ItemPadding.Right - _expander.W;
            _expander.SetPosition(newX, _expander.Y);
        }
    }

    public override void VisitChildren(Func<Widget, bool> action)
    {
        //need for lazy load icon font
        if (_icon != null)
            action(_icon);
        if (_expander != null)
            action(_expander);
    }

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        if (!ContainsPoint(x, y)) return false;
        result.Add(this);
        return true;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var offsetX = _controller.ItemPadding.Left;

        if (MenuItem.Type == MenuItemType.Divider)
        {
            SetSize(offsetX + 2, 6);
            return;
        }

        if (_icon != null)
        {
            _icon.Layout(availableWidth, availableHeight);
            _icon.SetPosition(offsetX, (availableHeight - _icon.H) / 2);
            offsetX += _icon.W + 5;
        }

        if (_label != null)
        {
            _label.Layout(availableWidth, availableHeight);
            _label.SetPosition(offsetX, (availableHeight - _label.H) / 2);
            offsetX += _label.W + 5;
        }

        if (_expander != null)
        {
            _expander.Layout(availableWidth, availableHeight);
            _expander.SetPosition(offsetX, (availableHeight - _expander.H) / 2);
            offsetX += _expander.W;
        }

        SetSize(offsetX + _controller.ItemPadding.Right, availableHeight);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (MenuItem.Type == MenuItemType.Divider)
        {
            var paint = PixUI.Paint.Shared(Colors.Gray, PaintStyle.Stroke, 2);
            var midY = H / 2;
            canvas.DrawLine(_controller.ItemPadding.Left, midY, W - _controller.ItemPadding.Horizontal, midY, paint);
            return;
        }

        if (_isHover)
        {
            var paint = PixUI.Paint.Shared(_controller.HoverColor, PaintStyle.Fill);
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), paint);
        }

        PaintChild(_icon, canvas, area);
        PaintChild(_label, canvas, area);
        PaintChild(_expander, canvas, area);
    }

    private static void PaintChild(Widget? child, Canvas canvas, IDirtyArea? area)
    {
        if (child == null) return;

        canvas.Translate(child.X, child.Y);
        child.Paint(canvas, area);
        canvas.Translate(-child.X, -child.Y);
    }

    public override string ToString()
    {
        var labelText = _label == null ? "" : _label.Text.Value;
        return $"MenuItemWidget[\"{labelText}\"]";
    }
}