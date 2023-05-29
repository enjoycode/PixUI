using System;

namespace PixUI;

public sealed class Row : MultiChildWidget<Widget>
{
    private readonly VerticalAlignment _alignment;
    private readonly float _spacing;

    public Row(VerticalAlignment alignment = VerticalAlignment.Middle, float spacing = 0)
    {
        if (spacing < 0) throw new ArgumentOutOfRangeException(nameof(spacing));
        _alignment = alignment;
        _spacing = spacing;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        var remaingWidth = width;
        float maxHeightOfChild = 0;

        //先计算非Expanded的子级
        bool hasExpanded = false;
        float totalFlex = 0;
        for (var i = 0; i < _children.Count; i++)
        {
            if (i != 0 && remaingWidth >= _spacing)
                remaingWidth = Math.Max(0, remaingWidth - _spacing);

            var child = _children[i];
            if (child is Expanded expanded)
            {
                hasExpanded = true;
                totalFlex += expanded.Flex;
                continue;
            }

            if (remaingWidth <= 0)
            {
                child.Layout(0, 0);
            }
            else
            {
                child.Layout(remaingWidth, height);
                maxHeightOfChild = Math.Max(maxHeightOfChild, child.H);
                remaingWidth -= child.W;
            }
        }

        //再计算Expanded子级
        if (hasExpanded)
        {
            foreach (var child in _children)
            {
                if (child is Expanded expanded)
                {
                    if (remaingWidth <= 0)
                    {
                        child.Layout(0, 0);
                    }
                    else
                    {
                        child.Layout(remaingWidth * (expanded.Flex / totalFlex), height);
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
            if (totalWidth >= width) break;

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
        SetSize(Math.Min(width, totalWidth), maxHeightOfChild);
    }
}