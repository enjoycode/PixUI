using System;
using System.Collections.Generic;

namespace PixUI;

public sealed class TreeView<T> : Widget, IScrollable
{
    public TreeView(TreeController<T> controller, bool showCheckbox = false, float nodeHeight = 30)
    {
        _controller = controller;
        _controller.NodeHeight = nodeHeight;
        _controller.ShowCheckbox = showCheckbox;
    }

    private readonly TreeController<T> _controller;
    private State<Color>? _color;

    public required TreeNodeBuilder<T> NodeBuilder
    {
        init => _controller.NodeBuilder = value;
    }

    public required TreeChildrenGetter<T> ChildrenGetter
    {
        init => _controller.ChildrenGetter = value;
    }

    /// <summary>
    /// 背景色
    /// </summary>
    public State<Color>? Color
    {
        get => _color;
        set => _color = Bind(_color, value, RepaintOnStateChanged);
    }

    public Action<TreeNode<T>> OnCheckChanged
    {
        set => _controller.CheckChanged += value;
    }

    #region ====IScrollable====

    public float ScrollOffsetX => _controller.ScrollController.OffsetX;
    public float ScrollOffsetY => _controller.ScrollController.OffsetY;

    public Offset OnScroll(float dx, float dy)
    {
        if (_controller.Nodes.Count == 0) return Offset.Empty;

        var maxOffsetX = Math.Max(0, _controller.TotalWidth - W);
        var maxOffsetY = Math.Max(0, _controller.TotalHeight - H);
        var offset = _controller.ScrollController.OnScroll(dx, dy, maxOffsetX, maxOffsetY);
        if (!offset.IsEmpty)
            Repaint();

        return offset;
    }

    #endregion

    #region ====Overrides====

    public override bool IsOpaque => _color != null && _color.Value.Alpha == 0xFF;

    protected override void OnMounted() => _controller.InitNodes(this);

    public override void VisitChildren(Func<Widget, bool> action)
    {
        foreach (var node in _controller.Nodes)
        {
            if (action(node)) break;
        }
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);
        SetSize(width, height);

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
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy,
        AffectsByRelayout affects)
    {
        //修改子节点受影响的区域
        affects.OldW = W;
        affects.OldH = H - child.Y;

        //更新后续子节点的Y坐标
        UpdatePositionAfter(child, _controller.Nodes, dy);

        //更新TreeController总宽及总高
        if (dx > 0)
        {
            //宽度增加，总宽取现值及当前的大者
            _controller.TotalWidth = Math.Max(child.W, _controller.TotalWidth);
        }
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        else if (dx < 0 && child.W - dx == _controller.TotalWidth)
        {
            //宽度减小，且原本是最宽的那个, 重新计算最宽的子节点
            _controller.TotalWidth = CalcMaxChildWidth(_controller.Nodes);
        }

        _controller.TotalHeight += dy;
    }

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false, Rect? dirtyRect = null)
    {
        if (!onlyTransform)
        {
            canvas.Save();
            canvas.Translate(X, Y);
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);

            if (dirtyRect.HasValue)
                canvas.ClipRect(dirtyRect.Value, ClipOp.Intersect, false);

            canvas.Translate(-ScrollOffsetX, -ScrollOffsetY);
        }
        else
        {
            canvas.Translate(X - ScrollOffsetX, Y - ScrollOffsetY);
        }
    }

    protected internal override void AfterPaint(Canvas canvas)
    {
        canvas.Restore();
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_controller.IsLoading)
        {
            if (_color != null)
                canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PaintUtils.Shared(_color.Value));
            _controller.LoadingPainter!.PaintToWidget(this, canvas);
            return;
        }

        // draw background color if has
        if (_color != null)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PaintUtils.Shared(_color.Value));

        // draw nodes in visual region
        var dirtyRect = area?.GetRect() ?? Rect.FromLTWH(0, 0, W, H);
        foreach (var node in _controller.Nodes)
        {
            var vx = node.X - ScrollOffsetX;
            var vy = node.Y - ScrollOffsetY;
            if (vy >= dirtyRect.Bottom) break;
            var vb = vy + node.H;
            if (vb <= dirtyRect.Top) continue;

            canvas.Translate(node.X, node.Y);
            node.Paint(canvas, null /*area?.ToChild(vx, vy)*/);
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