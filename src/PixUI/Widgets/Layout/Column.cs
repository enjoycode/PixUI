using System;
using System.Diagnostics;

namespace PixUI;

public sealed class Column : MultiChildWidget<Widget>
{
    public Column() { }

    public Column(HorizontalAlignment alignment = HorizontalAlignment.Center, float spacing = 0,
        string? debugLabel = null)
    {
        if (spacing < 0) throw new ArgumentOutOfRangeException(nameof(spacing));
        _alignment = alignment;
        _spacing = spacing;
        DebugLabel = debugLabel;
    }

    private HorizontalAlignment _alignment = HorizontalAlignment.Center;
    private float _spacing;
    private float _totalFlex = 0;

    public HorizontalAlignment Alignment
    {
        get => _alignment;
        set
        {
            _alignment = value;
            if (IsMounted)
                Invalidate(InvalidAction.Relayout);
        }
    }

    public float Spacing
    {
        get => _spacing;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException();
            _spacing = value;
            if (IsMounted)
                Invalidate(InvalidAction.Relayout);
        }
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        var remaingHeight = height;
        float maxWidthOfChild = 0;

        //先计算非Expanded的子级
        _totalFlex = 0;
        for (var i = 0; i < _children.Count; i++)
        {
            if (i != 0 && remaingHeight >= _spacing)
                remaingHeight = Math.Max(0, remaingHeight - _spacing);

            var child = _children[i];
            if (child is Expanded expanded)
            {
                _totalFlex += expanded.Flex;
                continue;
            }

            if (remaingHeight <= 0)
            {
                child.Layout(0, 0);
            }
            else
            {
                child.Layout(width, remaingHeight);
                maxWidthOfChild = Math.Max(maxWidthOfChild, child.W);
                remaingHeight -= child.H;
            }
        }

        //再计算Expanded子级
        if (_totalFlex > 0)
        {
            foreach (var child in _children)
            {
                if (child is Expanded expanded)
                {
                    if (remaingHeight <= 0)
                    {
                        child.Layout(0, 0);
                    }
                    else
                    {
                        child.Layout(width, remaingHeight * (expanded.Flex / _totalFlex));
                        maxWidthOfChild = Math.Max(maxWidthOfChild, child.W);
                    }
                }
            }
        }

        //最后计算位置
        var totalHeight = 0.0f;
        for (var i = 0; i < _children.Count; i++)
        {
            if (i != 0) totalHeight += _spacing;
            if (totalHeight >= height) break;

            var child = _children[i];
            var childX = _alignment switch
            {
                HorizontalAlignment.Right => maxWidthOfChild - child.W,
                HorizontalAlignment.Center => (maxWidthOfChild - child.W) / 2,
                _ => 0
            };
            child.SetPosition(childX, totalHeight);

            totalHeight += child.H;
        }

        //最宽的子级 and 所有子级的高度
        SetSize(maxWidthOfChild, Math.Min(height, totalHeight));
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        Debug.Assert(AutoSize);

        var oldWidth = W;
        var oldHeight = H;

        var width = Width == null
            ? CachedAvailableWidth
            : Math.Min(Width.Value, CachedAvailableWidth);
        var height = Height == null
            ? CachedAvailableHeight
            : Math.Min(Height.Value, CachedAvailableHeight);

        //TODO:可优化变窄或变宽但不是原来最宽的

        if (dx != 0)
        {
            //重新计算最宽的
            var newWidth = 0f;
            foreach (var item in _children)
            {
                newWidth = Math.Min(Math.Max(item.W, newWidth), width);
            }

            SetSize(newWidth, oldHeight);

            //重设X坐标
            foreach (var item in _children)
            {
                var childX = _alignment switch
                {
                    HorizontalAlignment.Right => W - item.W,
                    HorizontalAlignment.Center => (W - item.W) / 2,
                    _ => 0
                };
                item.SetPosition(childX, item.Y);
            }
        }

        if (dy != 0)
        {
            if (_totalFlex > 0)
            {
                //TODO: recalc expanded and layout 
                throw new NotImplementedException();
            }
            else
            {
                var indexOfChild = _children.IndexOf(child);
                for (var i = indexOfChild + 1; i < _children.Count; i++)
                {
                    _children[i].SetPosition(_children[i].X, _children[i].Y + dy);
                }

                SetSize(W, H + dy);
            }
        }

        TryNotifyParentIfSizeChanged(oldWidth, oldHeight, affects);
    }
}