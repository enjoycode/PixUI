using System;
using System.Collections.Generic;

namespace PixUI;

public sealed class ListViewController<T> : WidgetController<ListViewBase<T>>
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
            if (Widget != null!)
                Widget.OnDataSourceChanged();
        }
    }

    /// <summary>
    /// 滚动指定索引的子组件至可视区域
    /// </summary>
    public void ScrollTo(int index)
    {
        var toChild = Widget.GetChildAt(index);

        if (ScrollController.Direction == ScrollDirection.Vertical)
        {
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
        else
        {
            //判断是否可见
            var offsetX = ScrollController.OffsetX;
            if (toChild.X >= offsetX && toChild.X + toChild.W <= Widget.W + offsetX)
                return;

            var deltaX = toChild.X >= offsetX
                ? toChild.X + toChild.W - Widget.W - offsetX
                : toChild.X - offsetX;
            var offset = Widget.OnScroll(deltaX, 0);
            if (!offset.IsEmpty)
                Widget.Root!.Window.AfterScrollDone(Widget, offset);
        }
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

public abstract class ListViewBase<T> : MultiChildWidget<Widget>, IScrollable
{
    protected ListViewBase(Func<T, int, Widget> itemBuilder, IList<T>? dataSource = null,
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

        Relayout();
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var availableSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        if (Controller.ScrollController.Direction == ScrollDirection.Vertical)
        {
            float y = 0;
            foreach (var child in _children)
            {
                child.Layout(availableSize.Width, float.PositiveInfinity);
                child.SetPosition(0, y);
                y += child.H;
            }
        }
        else
        {
            float x = 0;
            foreach (var child in _children)
            {
                child.Layout(float.PositiveInfinity, availableSize.Height);
                child.SetPosition(x, 0);
                x += child.W;
            }
        }


        SetSize(availableSize.Width, availableSize.Height);
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

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false,
        IDirtyArea? dirtyArea = null)
    {
        canvas.Translate(X, Y);
        if (!onlyTransform)
        {
            canvas.Save();
            var selfBounds = Rect.FromLTWH(0, 0, W, H);
            var clipRect = dirtyArea != null ? Rect.Intersect(dirtyArea.GetRect(), selfBounds) : selfBounds;
            canvas.ClipRect(clipRect, ClipOp.Intersect, false);
        }

        canvas.Translate(0, -ScrollOffsetY);
    }

    protected internal override void AfterPaint(Canvas canvas)
    {
        canvas.Restore();
        canvas.Translate(-X, -Y);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (Controller.ScrollController.Direction == ScrollDirection.Vertical)
        {
            foreach (var child in _children)
            {
                if (child.Y + child.H <= ScrollOffsetY) continue; //小于上边界
                if (child.Y >= ScrollOffsetY + H) break; //大于下边界

                child.BeforePaint(canvas);
                child.Paint(canvas);
                child.AfterPaint(canvas);
            }
        }
        else
        {
            foreach (var child in _children)
            {
                if (child.X + child.W <= ScrollOffsetX) continue; //小于左边界
                if (child.X >= ScrollOffsetX + W) break; //大于右边界

                child.BeforePaint(canvas);
                child.Paint(canvas);
                child.AfterPaint(canvas);
            }
        }
    }

    #region ====IScrollable====

    public float ScrollOffsetX => Controller.ScrollController.OffsetX;
    public float ScrollOffsetY => Controller.ScrollController.OffsetY;
    public ScrollDirection ScrollDirection => Controller.ScrollController.Direction;
    public ScrollBarVisibility ShowScrollBar => ScrollBarVisibility.Never;

    public Offset OnScroll(float dx, float dy)
    {
        if (_children.Count == 0) return Offset.Empty;

        var lastChild = _children[_children.Count - 1];
        if (lastChild.Y + lastChild.H <= H) return Offset.Empty;

        var maxOffsetX = 0f;
        var maxOffsetY = lastChild.Y + lastChild.H - H;

        var offset = Controller.ScrollController.OnScroll(dx, dy, maxOffsetX, maxOffsetY);
        if (!offset.IsEmpty)
            Repaint();
        return offset;
    }

    #endregion
}

public sealed class ListView<T> : ListViewBase<T>
{
    public ListView(Func<T, int, Widget> itemBuilder, IList<T>? dataSource = null,
        ListViewController<T>? controller = null) : base(itemBuilder, dataSource, controller) { }
}

public sealed class ListView : ListViewBase<Widget>
{
    public ListView(Axis axis = Axis.Vertical) :
        base(static (w, _) => w, null, new ListViewController<Widget>(axis)) { }
}