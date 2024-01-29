using System;
using System.Collections.Generic;

namespace PixUI;

public sealed class TreeView<T> : Widget, IScrollable
{
    public TreeView(TreeController<T> controller,
        TreeNodeBuilder<T> nodeBuilder, TreeChildrenGetter<T> childrenGetter,
        bool showCheckbox = false, float nodeHeight = 30)
    {
        _controller = controller;
        _controller.NodeBuilder = nodeBuilder;
        _controller.ChildrenGetter = childrenGetter;
        _controller.NodeHeight = nodeHeight;
        _controller.ShowCheckbox = showCheckbox;
        _controller.AttachTreeView(this);
        _controller.TryBuildNodes();
    }

    private readonly TreeController<T> _controller;
    private readonly State<Color>? _fillColor;

    /// <summary>
    /// 背景色
    /// </summary>
    public State<Color>? FillColor
    {
        get => _fillColor;
        init => Bind(ref _fillColor, value, RepaintOnStateChanged);
    }

    public bool AllowDragDrop
    {
        get => _controller.AllowDragDrop;
        set => _controller.AllowDragDrop = value;
    }

    public Action<TreeNode<T>> OnCheckChanged
    {
        set => _controller.CheckChanged += value;
    }

    #region ====IScrollable====

    public float ScrollOffsetX => _controller.ScrollController.OffsetX;
    public float ScrollOffsetY => _controller.ScrollController.OffsetY;
    public ScrollDirection ScrollDirection => _controller.ScrollController.Direction;
    public ScrollBarVisibility ShowScrollBar => ScrollBarVisibility.Never;

    private float MaxScrollOffsetX => Math.Max(0, _controller.TotalWidth - W);
    private float MaxScrollOffsetY => Math.Max(0, _controller.TotalHeight - H);

    public Offset OnScroll(float dx, float dy)
    {
        if (_controller.Nodes.Count == 0) return Offset.Empty;

        var offset = _controller.ScrollController.OnScroll(dx, dy, MaxScrollOffsetX, MaxScrollOffsetY);
        if (!offset.IsEmpty)
            Repaint();

        return offset;
    }

    #endregion

    #region ====Overrides====

    public override bool IsOpaque => _fillColor != null && _fillColor.Value.Alpha == 0xFF;

    public override void VisitChildren(Func<Widget, bool> action)
    {
        foreach (var node in _controller.Nodes)
        {
            if (action(node)) break;
        }
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        SetSize(maxSize.Width, maxSize.Height);

        var totalWidth = 0f;
        var totalHeight = 0f;

        foreach (var node in _controller.Nodes)
        {
            node.Layout(float.PositiveInfinity, float.PositiveInfinity);
            node.SetPosition(0, totalHeight);
            totalWidth = Math.Max(totalWidth, node.W);
            totalHeight += node.H;
        }

        _controller.TotalWidth = totalWidth;
        _controller.TotalHeight = totalHeight;
        _controller.ScrollController.Adjust(MaxScrollOffsetX, MaxScrollOffsetY);
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        //修改子节点受影响的区域
        affects.OldW = W;
        affects.OldH = H - child.Y;

        //更新后续子节点的Y坐标
        UpdatePositionAfter(child, _controller.Nodes, dy);

        //更新TreeController总宽及总高
        _controller.TotalWidth = CalcMaxChildWidth(_controller.Nodes);
        _controller.TotalHeight += dy;
        _controller.ScrollController.Adjust(MaxScrollOffsetX, MaxScrollOffsetY);
    }

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false,
        IDirtyArea? dirtyArea = null)
    {
        if (!onlyTransform)
        {
            canvas.Save();
            canvas.Translate(X, Y);
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
            dirtyArea?.ApplyClip(canvas);
            canvas.Translate(-ScrollOffsetX, -ScrollOffsetY);
        }
        else
        {
            canvas.Translate(X - ScrollOffsetX, Y - ScrollOffsetY);
        }
    }

    protected internal override void AfterPaint(Canvas canvas) => canvas.Restore();

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        // draw background color if has
        if (_fillColor != null)
            canvas.DrawRect(Rect.FromLTWH(ScrollOffsetX, ScrollOffsetY, W, H), PixUI.Paint.Shared(_fillColor.Value));

        if (_controller.IsLoading)
        {
            _controller.LoadingPainter!.PaintToWidget(this, canvas);
            return;
        }

        if (area is RepaintChild repaintChild)
        {
            repaintChild.Repaint(canvas);
            return;
        }

        // draw nodes in visual region
        var dirtyRect = area?.GetRect() ?? Rect.FromLTWH(0, 0, W, H);
        foreach (var node in _controller.Nodes)
        {
            //var vx = node.X - ScrollOffsetX;
            var vy = node.Y - ScrollOffsetY;
            if (vy >= dirtyRect.Bottom) break;
            var vb = vy + node.H;
            if (vb <= dirtyRect.Top) continue;

            canvas.Translate(node.X, node.Y);
            node.Paint(canvas /*area?.ToChild(vx, vy)*/);
            canvas.Translate(-node.X, -node.Y);
        }
    }

    #endregion

    #region ====Static Utils====

    /// <summary>
    /// 计算所有子节点的最大宽度
    /// </summary>
    internal static float CalcMaxChildWidth<Tn>(IList<TreeNode<Tn>> nodes)
    {
        var maxChildWidth = 0f;
        foreach (var node in nodes)
        {
            maxChildWidth = Math.Max(maxChildWidth, node.W);
        }

        return maxChildWidth;
    }

    /// <summary>
    /// 更新指定子节点之后的子节点的Y坐标
    /// </summary>
    internal static void UpdatePositionAfter<Tn>(Widget child, IList<TreeNode<Tn>> nodes,
        float dy)
    {
        var indexOfChild = -1;
        for (var i = 0; i < nodes.Count; i++)
        {
            if (indexOfChild == -1)
            {
                if (ReferenceEquals(nodes[i], child))
                    indexOfChild = i;
            }
            else
            {
                var node = nodes[i];
                node.SetPosition(node.X, node.Y + dy);
            }
        }
    }

    #endregion
}