using System;
using System.Collections.Generic;

namespace PixUI;

public sealed class ListViewController<T> : WidgetController<ListView<T>>
{
    internal readonly ScrollController ScrollController;
    private IList<T>? _dataSource;

    public ListViewController(Axis axis = Axis.Vertical)
    {
        ScrollController = new ScrollController(axis == Axis.Vertical
            ? ScrollDirection.Vertical
            : ScrollDirection.Horizontal);
    }

    public IList<T>? DataSource
    {
        get => _dataSource;
        set
        {
            _dataSource = value;
            Widget.OnDataSourceChanged();
        }
    }

    /// <summary>
    /// 滚动指定索引的子组件至可视区域
    /// </summary>
    public void ScrollTo(int index)
    {
        var toChild = Widget.GetChildAt(index);

        //判断是否可见
        var offsetY = ScrollController.OffsetY;
        if (toChild.Y >= offsetY && toChild.Y + toChild.H <= Widget.H + offsetY)
            return;

        var deltaY = toChild.Y >= offsetY
            ? toChild.Y + toChild.H - Widget.H - offsetY
            : toChild.Y - offsetY;
        var offset = Widget.OnScroll(0, deltaY);
        if (!offset.IsEmpty)
            Widget.Root!.Window.AfterScrollDone(Widget, offset);
    }
}

public sealed class ListView<T> : MultiChildWidget<Widget>, IScrollable
{
    public static ListView<Widget> From(IList<Widget> widgets,
        ListViewController<Widget>? controller = null)
        => new ListView<Widget>((w, i) => w, widgets, controller);

    public ListView(Func<T, int, Widget> itemBuilder, IList<T>? dataSource = null,
        ListViewController<T>? controller = null)
    {
        _itemBuilder = itemBuilder;
        _controller = controller ?? new ListViewController<T>();
        _controller.AttachWidget(this);
        if (dataSource != null)
            _controller.DataSource = dataSource;
    }

    private readonly ListViewController<T> _controller;
    private readonly Func<T, int, Widget> _itemBuilder;

    internal void OnDataSourceChanged()
    {
        _children.Clear();
        if (_controller.DataSource != null)
        {
            for (var i = 0; i < _controller.DataSource.Count; i++)
            {
                _children.Add(_itemBuilder(_controller.DataSource[i], i));
            }
        }

        if (IsMounted)
            Invalidate(InvalidAction.Relayout);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        float y = 0;
        foreach (var child in _children)
        {
            child.Layout(width, float.PositiveInfinity);
            // child.W = width;
            child.SetPosition(0, y);
            y += child.H;
        }

        SetSize(width, height);
    }

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false, Rect? dirtyRect = null)
    {
        base.BeforePaint(canvas, onlyTransform, null);
        if (!onlyTransform)
        {
            canvas.Save();
            var selfBounds = Rect.FromLTWH(0, 0, W, H);
            var clipRect = dirtyRect.HasValue ? Rect.Intersect(dirtyRect.Value, selfBounds) : selfBounds;
            canvas.ClipRect(clipRect, ClipOp.Intersect, false);
        }

        canvas.Translate(0, -ScrollOffsetY);
    }

    protected internal override void AfterPaint(Canvas canvas)
    {
        base.AfterPaint(canvas);
        canvas.Restore();
        canvas.Translate(0, ScrollOffsetY);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        foreach (var child in _children)
        {
            if (child.Y + child.H <= ScrollOffsetY) continue; //小于上边界
            if (child.Y >= ScrollOffsetY + H) break; //大于下边界

            child.BeforePaint(canvas);
            child.Paint(canvas, null);
            child.AfterPaint(canvas);
        }
    }

    public float ScrollOffsetX => _controller.ScrollController.OffsetX;
    public float ScrollOffsetY => _controller.ScrollController.OffsetY;

    public Offset OnScroll(float dx, float dy)
    {
        if (_children.Count == 0) return Offset.Empty;

        var lastChild = _children[_children.Count - 1];
        if (lastChild.Y + lastChild.H <= H) return Offset.Empty;

        var maxOffsetX = 0f;
        var maxOffsetY = lastChild.Y + lastChild.H - H;

        var offset = _controller.ScrollController.OnScroll(dx, dy, maxOffsetX, maxOffsetY);
        if (!offset.IsEmpty)
            Invalidate(InvalidAction.Repaint);
        return offset;
    }
}