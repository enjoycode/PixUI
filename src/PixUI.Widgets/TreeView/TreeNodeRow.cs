using System;
using System.Diagnostics;

namespace PixUI;

internal sealed class TreeNodeRow<T> : Widget, IDraggable, IDroppable
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

    public MouseRegion MouseRegion { get; }

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

    #region ====DragDrop====

    public bool AllowDrag()
    {
        if (!Controller.AllowDrag) return false;
        return Controller.OnAllowDrag == null || Controller.OnAllowDrag(TreeNode);
    }

    public bool AllowDrop(DragEvent dragEvent) => Controller.AllowDrop;

    public void OnDragStart(DragEvent dragEvent)
    {
        dragEvent.TransferItem = TreeNode;
        dragEvent.DragHintImage = BuildDragHintImage();
    }

    public void OnDragEnd(DragEvent dragEvent) { }

    public void OnDragOver(DragEvent dragEvent, Point local)
    {
        dragEvent.DropEffect = DropEffect.Move;
        dragEvent.DropPosition = DropPosition.In;
        var posOffset = Controller.NodeHeight / 4;
        if (local.Y < posOffset)
            dragEvent.DropPosition = DropPosition.Upper;
        else if (local.Y > H - posOffset)
            dragEvent.DropPosition = DropPosition.Under;

        var allowDrop = true;
        if (Controller.OnAllowDrop != null)
            allowDrop = Controller.OnAllowDrop(TreeNode, dragEvent);
        if (!allowDrop)
        {
            dragEvent.DropEffect = DropEffect.None;
            dragEvent.DropHintImage = null;
        }
        else
        {
            //TODO:考虑缓存dropHintImage
            dragEvent.DropHintImage = BuildDropHintImage(dragEvent.DropPosition);
        }
    }

    public void OnDragLeave(DragEvent dragEvent) { }

    public void OnDrop(DragEvent dragEvent, Point local) => Controller.OnDrop?.Invoke(TreeNode, dragEvent);

    private Image BuildDragHintImage()
    {
        const float padding = 5f;
        var width = padding;
        if (_icon != null)
            width += _icon.W;
        if (_label != null)
            width += _label.W + padding * 2;

        var scale = Root!.Window.ScaleFactor;
        var rect = Rect.FromLTWH(0, 0, width * scale, H * scale);
        using var recorder = new PictureRecorder();
        using var canvas = recorder.BeginRecording(rect);
        canvas.Scale(scale, scale);
        var paint = PixUI.Paint.Shared(Colors.Gray.WithAlpha(128));
        canvas.DrawRect(rect, paint);
        var iconWidth = 0f;
        if (_icon != null)
        {
            var x = padding;
            var y = (H - _icon.H) / 2f;
            canvas.Translate(x, y);
            _icon.Paint(canvas);
            canvas.Translate(-x, -y);
            iconWidth = _icon.W + padding;
        }

        if (_label != null)
        {
            var x = padding + iconWidth;
            var y = (H - _label.H) / 2f;
            canvas.Translate(x, y);
            _label.Paint(canvas);
            canvas.Translate(-x, -y);
        }

        var image = recorder.EndRecording();
        return Image.FromPicture(image, new SizeI((int)(width * scale), (int)(H * scale)));
    }

    private Image BuildDropHintImage(DropPosition position)
    {
        var scrollX = Controller.TreeView!.ScrollOffsetX;
        var width = Controller.TreeView!.W + scrollX;
        var height = Controller.NodeHeight;

        var scale = Root!.Window.ScaleFactor;
        var rect = Rect.FromLTWH(0, 0, width * scale, height * scale);
        using var recorder = new PictureRecorder();
        using var canvas = recorder.BeginRecording(rect);
        canvas.Scale(scale, scale);
        var paint = PixUI.Paint.Shared(Colors.Gray, PaintStyle.Stroke, 2);
        canvas.DrawRect(Rect.FromLTWH(1 + scrollX, 1, width - scrollX - 2, height - 2), paint);

        const float indent = 4f;
        if (position == DropPosition.Upper)
        {
            paint = PixUI.Paint.Shared(Theme.FocusedColor);
            canvas.DrawRect(Rect.FromLTWH(1 + scrollX, 1, width - scrollX - 2, indent), paint);
        }
        else if (position == DropPosition.Under)
        {
            paint = PixUI.Paint.Shared(Theme.FocusedColor);
            canvas.DrawRect(Rect.FromLTWH(1 + scrollX, height - indent - 1, width - scrollX - 2, indent), paint);
        }

        var image = recorder.EndRecording();
        return Image.FromPicture(image, new SizeI((int)(width * scale), (int)(height * scale)));
    }

    #endregion

    #region ====Event Handlers====

    private void _OnHoverChanged(bool hover)
    {
        _isHover = hover;
        Repaint(); //因BeforePaint已clip整行，不再需要如下hoverRect
        // Repaint(new RepaintArea(GetRowRect()));
    }

    private void _OnTap(PointerEvent e) => Controller.SelectNode(TreeNode);

    private Rect GetRowRect() => Rect.FromLTWH(Controller.TreeView!.ScrollOffsetX, 0,
        Controller.TreeView!.W, Controller.NodeHeight);

    #endregion

    #region ====Overrides====

    public override bool IsOpaque => _isHover && Controller.HoverColor.IsOpaque;

    public override bool ContainsPoint(float x, float y) =>
        y >= 0 && y < H &&
        x >= Controller.TreeView!.ScrollOffsetX &&
        x < Controller.TreeView!.W + Controller.TreeView!.ScrollOffsetX;

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

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false,
        IDirtyArea? dirtyArea = null)
    {
        canvas.Translate(X, Y);
        if (dirtyArea is RepaintChild)
        {
            Debug.Assert(onlyTransform == false);
            //这里不需要保存画布状态,InvalidQueue会恢复
            dirtyArea.ApplyClip(canvas);
        }
        else
        {
            if (!onlyTransform)
            {
                //始终clip整行
                canvas.ClipRect(GetRowRect(), ClipOp.Intersect, false);
            }
        }
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_isHover)
        {
            var paint = PixUI.Paint.Shared(Controller.HoverColor);
            canvas.DrawRect(GetRowRect(), paint);
        }

        if (area is RepaintChild repaintChild)
        {
            repaintChild.Repaint(canvas);
            return;
        }

        //Log.Debug($"重绘TreeNodeRow[{_label?.Text.Value}]: isHover = {_isHover} clip={canvas.ClipBounds}");

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