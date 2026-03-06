using System;

namespace PixUI;

public sealed class Row : MultiChildWidget<Widget>
{
    public Row() { }

    public Row(VerticalAlignment alignment = VerticalAlignment.Middle, float spacing = 0)
    {
        if (spacing < 0) throw new ArgumentOutOfRangeException(nameof(spacing));
        _alignment = alignment;
        _spacing = spacing;
    }

    private VerticalAlignment _alignment = VerticalAlignment.Middle;
    private float _spacing;

    public VerticalAlignment Alignment
    {
        get => _alignment;
        set
        {
            _alignment = value;
            if (IsMounted) Relayout();
        }
    }

    public float Spacing
    {
        get => _spacing;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException();
            _spacing = value;
            if (IsMounted) Relayout();
        }
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var availableSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        var remainWidth = availableSize.Width;
        float maxHeightOfChild = 0;

        //先计算非Expanded的子级
        bool hasExpanded = false;
        float totalFlex = 0;
        for (var i = 0; i < _children.Count; i++)
        {
            if (i != 0 && remainWidth >= _spacing)
                remainWidth = Math.Max(0, remainWidth - _spacing);

            var child = _children[i];
            if (child is Expanded expanded)
            {
                hasExpanded = true;
                totalFlex += expanded.Flex;
                continue;
            }

            if (remainWidth <= 0)
            {
                child.Layout(0, 0);
            }
            else
            {
                child.Layout(remainWidth, availableSize.Height);
                maxHeightOfChild = Math.Max(maxHeightOfChild, child.H);
                remainWidth -= child.W;
            }
        }

        //再计算Expanded子级
        if (hasExpanded)
        {
            foreach (var child in _children)
            {
                if (child is Expanded expanded)
                {
                    if (remainWidth <= 0)
                    {
                        child.Layout(0, 0);
                    }
                    else
                    {
                        child.Layout(remainWidth * (expanded.Flex / totalFlex), availableSize.Height);
                        maxHeightOfChild = Math.Max(maxHeightOfChild, child.H);
                    }
                }
            }
        }

        //最后计算位置
        var totalWidth = 0.0f;
        for (var i = 0; i < _children.Count; i++)
        {
            if (i != 0) totalWidth += _spacing;
            if (totalWidth >= availableSize.Width) break;

            var child = _children[i];
            var childY = _alignment switch
            {
                VerticalAlignment.Bottom => maxHeightOfChild - child.H,
                VerticalAlignment.Middle => (maxHeightOfChild - child.H) / 2,
                _ => 0
            };
            child.SetPosition(totalWidth, childY);

            totalWidth += child.W;
        }

        // 最高的子级 and 所有子级的宽度
        SetSize(Math.Min(availableSize.Width, totalWidth), maxHeightOfChild);
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        //TODO:暂全部重新布局并设脏区域为全部重绘，可优化
        var oldWidth = W;
        var oldHeight = H;
        Layout(CachedAvailableWidth, CachedAvailableHeight);
        affects.Widget = this;
        affects.OldX = 0;
        affects.OldY = 0;
        affects.OldW = W;
        affects.OldH = H;
        TryNotifyParentIfSizeChanged(oldWidth, oldHeight, affects);
    }
}