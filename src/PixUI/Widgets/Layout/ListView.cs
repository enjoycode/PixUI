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

    /// <summary>
    /// 重置滚动位置
    /// </summary>
    public void ResetScroll()
    {
        if (ScrollController.OffsetX == 0 && ScrollController.OffsetY == 0)
            return;

        var offset = Widget.OnScroll(-ScrollController.OffsetX, -ScrollController.OffsetY);
        if (!offset.IsEmpty)
            Widget.Root!.Window.AfterScrollDone(Widget, offset);
    }
}

public sealed class ListView<T> : MultiChildWidget<Widget>, IScrollable
{
    public static ListView<Widget> From(IList<Widget> widgets, ListViewController<Widget>? controller = null) =>
        new((w, i) => w, widgets, controller);

    public ListView(Func<T, int, Widget> itemBuilder, IList<T>? dataSource = null,
        ListViewController<T>? controller = null)
    {
        _itemBuilder = itemBuilder;
        Controller = controller ?? new ListViewController<T>();
        Controller.AttachWidget(this);
        if (dataSource != null)
            Controller.DataSource = dataSource;
    }

    private readonly Func<T, int, Widget> _itemBuilder;
    public readonly ListViewController<T> Controller;

    internal void OnDataSourceChanged()
    {
        _children.Clear();
        if (Controller.DataSource != null)
        {
            for (var i = 0; i < Controller.DataSource.Count; i++)
            {
                _children.Add(_itemBuilder(Controller.DataSource[i], i));
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

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        //TODO:暂全部重新布局并设脏区域为全部重绘，可优化
        Layout(CachedAvailableWidth, CachedAvailableHeight);
        affects.Widget = this;
        affects.OldX = 0;
        affects.OldY = 0;
        affects.OldW = W;
        affects.OldH = H;
        //不需要再通知上级了
    }

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false, Rect? dirtyRect = null)
    {
        canvas.Translate(X, Y);
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
        canvas.Translate(-X, -Y);
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

    #region ====IScrollable====

    public float ScrollOffsetX => Controller.ScrollController.OffsetX;
    public float ScrollOffsetY => Controller.ScrollController.OffsetY;

    public Offset OnScroll(float dx, float dy)
    {
        if (_children.Count == 0) return Offset.Empty;

        var lastChild = _children[_children.Count - 1];
        if (lastChild.Y + lastChild.H <= H) return Offset.Empty;

        var maxOffsetX = 0f;
        var maxOffsetY = lastChild.Y + lastChild.H - H;

        var offset = Controller.ScrollController.OnScroll(dx, dy, maxOffsetX, maxOffsetY);
        if (!offset.IsEmpty)
            Invalidate(InvalidAction.Repaint);
        return offset;
    }

    #endregion
}