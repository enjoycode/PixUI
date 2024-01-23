using System;

namespace PixUI;

internal sealed class TreeNodeRow<T> : Widget, IMouseRegion
{
    internal TreeNodeRow()
    {
        MouseRegion = new MouseRegion(null, false);
        MouseRegion.HoverChanged += _OnHoverChanged;
        MouseRegion.PointerTap += _OnTap;
    }

    private ExpandIcon? _expander;
    private Checkbox? _checkbox;
    private Icon? _icon;
    private Text? _label;
    private bool _isHover;

    public MouseRegion MouseRegion { get; private set; }

    internal ExpandIcon ExpandIcon
    {
        set
        {
            _expander = value;
            _expander.Parent = this;
        }
    }

    internal Checkbox Checkbox
    {
        set
        {
            _checkbox = value;
            _checkbox.Parent = this;
        }
    }

    internal Icon Icon
    {
        get => _icon!;
        set
        {
            _icon = value;
            _icon.Parent = this;
        }
    }

    internal Text? Label
    {
        get => _label;
        set
        {
            _label = value;
            if (_label != null) _label.Parent = this;
        }
    }

    private TreeNode<T> TreeNode => (TreeNode<T>)Parent!;

    private TreeController<T> Controller => TreeNode.Controller;

    #region ====Event Handlers====

    private void _OnHoverChanged(bool hover)
    {
        _isHover = hover;
        var hoverRect = Rect.FromLTWH(Controller.TreeView!.ScrollOffsetX, 0,
            Controller.TreeView!.W, Controller.NodeHeight);
        Repaint(new RepaintArea(hoverRect));
    }

    private void _OnTap(PointerEvent e) => Controller.SelectNode(TreeNode);

    #endregion

    #region ====Overrides====

    public override bool IsOpaque => _isHover && Controller.HoverColor.IsOpaque;

    public override bool ContainsPoint(float x, float y) =>
        y >= 0 && y < H && x >= 0 && x < Controller.TreeView!.W;

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (_checkbox != null) action(_checkbox);
        if (_icon != null) action(_icon);
        if (_label != null) action(_label);
    }

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        if (y < 0 || y > H) return false;

        result.Add(this);

        //子级判断是否命中ExpandIcon
        if (_expander != null)
            HitTestChild(_expander, x, y, result);
        if (_checkbox != null)
            HitTestChild(_checkbox, x, y, result);

        return true;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var indentation = TreeNode.Depth * Controller.NodeIndent;

        // ExpandIcon
        if (_expander != null)
        {
            _expander?.Layout(Controller.NodeHeight, Controller.NodeHeight);
            _expander?.SetPosition(indentation, (Controller.NodeHeight - _expander.H) / 2);
        }

        indentation += Controller.NodeIndent; //always keep expand icon size

        // Icon or Checkbox
        if (Controller.ShowCheckbox)
        {
            _checkbox!.Layout(Controller.NodeHeight, Controller.NodeHeight);
            _checkbox.SetPosition(indentation, (Controller.NodeHeight - _checkbox.H) / 2);
            indentation += _checkbox.W;
        }
        else
        {
            if (_icon != null)
            {
                _icon.Layout(Controller.NodeHeight, Controller.NodeHeight);
                _icon.SetPosition(indentation, (Controller.NodeHeight - _icon.H) / 2);
            }

            indentation += Controller.NodeIndent; //always keep icon size
        }

        // Label
        if (_label != null)
        {
            _label.Layout(float.PositiveInfinity, Controller.NodeHeight);
            _label.SetPosition(indentation, (Controller.NodeHeight - _label.H) / 2);
            indentation += _label.W;
        }

        SetSize(indentation, Controller.NodeHeight);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //TODO: only paint expand icon when dirty area is not null
        //Console.WriteLine($"重绘TreeNodeRow[{_label?.Text.Value}]: isHover = {_isHover}");

        if (_isHover)
        {
            var paint = PixUI.Paint.Shared(Controller.HoverColor);
            var hoverRect = Rect.FromLTWH(Controller.TreeView!.ScrollOffsetX, 0,
                Controller.TreeView!.W, Controller.NodeHeight);
            canvas.DrawRect(hoverRect, paint);
        }

        PaintChild(_expander, canvas);
        if (Controller.ShowCheckbox)
            PaintChild(_checkbox, canvas);
        else
            PaintChild(_icon, canvas);
        PaintChild(_label, canvas);
    }

    private static void PaintChild(Widget? child, Canvas canvas)
    {
        if (child == null) return;

        canvas.Translate(child.X, child.Y);
        child.Paint(canvas);
        canvas.Translate(-child.X, -child.Y);

        PaintDebugger.PaintWidgetBorder(child, canvas);
    }

    public override string ToString()
    {
        var labelText = _label == null ? "" : _label.Text.Value;
        return $"TreeNodeRow[\"{labelText}\"]";
    }

    #endregion
}