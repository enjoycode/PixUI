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
    private float _totalFlex;

    public HorizontalAlignment Alignment
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

    protected override void OnLayout(Size maxSize)
    {
        var remainHeight = maxSize.Height;
        float maxWidthOfChild = 0;

        //先计算非Expanded的子级
        _totalFlex = 0;
        for (var i = 0; i < _children.Count; i++)
        {
            if (i != 0 && remainHeight >= _spacing)
                remainHeight = Math.Max(0, remainHeight - _spacing);

            var child = _children[i];
            if (child is Expanded expanded)
            {
                _totalFlex += expanded.Flex;
                continue;
            }

            if (remainHeight <= 0)
            {
                child.PerformLayout(Size.Empty);
            }
            else
            {
                child.PerformLayout(new(maxSize.Width, remainHeight));
                maxWidthOfChild = Math.Max(maxWidthOfChild, child.W);
                remainHeight -= child.H;
            }
        }

        //再计算Expanded子级
        if (_totalFlex > 0)
        {
            foreach (var child in _children)
            {
                if (child is Expanded expanded)
                {
                    if (remainHeight <= 0)
                    {
                        child.PerformLayout(Size.Empty);
                    }
                    else
                    {
                        child.PerformLayout(new(maxSize.Width, remainHeight * (expanded.Flex / _totalFlex)));
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
            if (totalHeight >= maxSize.Height) break;

            var child = _children[i];
            var childX = _alignment switch
            {
                HorizontalAlignment.Right => maxWidthOfChild - child.W,
                HorizontalAlignment.Center => (maxWidthOfChild - child.W) / 2,
                _ => 0
            };
            child.SetLayoutLocation(childX, totalHeight);

            totalHeight += child.H;
        }

        //最宽的子级 and 所有子级的高度
        SetLayoutSize(maxWidthOfChild, Math.Min(maxSize.Height, totalHeight));
    }

    protected internal override void OnChildSizeChanged(Widget child, float dx, float dy, AffectsByRelayout affects)
    {
        Debug.Assert(AutoSize);

        var oldWidth = W;
        var oldHeight = H;

        var width = Width == null
            ? AvailableSize.Width
            : Math.Min(Width.Value, AvailableSize.Width);
        var height = Height == null
            ? AvailableSize.Height
            : Math.Min(Height.Value, AvailableSize.Height);

        //TODO:可优化变窄或变宽但不是原来最宽的

        if (dx != 0)
        {
            //重新计算最宽的
            var newWidth = 0f;
            foreach (var item in _children)
            {
                newWidth = Math.Min(Math.Max(item.W, newWidth), width);
            }

            SetLayoutSize(newWidth, oldHeight);

            //重设X坐标
            foreach (var item in _children)
            {
                var childX = _alignment switch
                {
                    HorizontalAlignment.Right => W - item.W,
                    HorizontalAlignment.Center => (W - item.W) / 2,
                    _ => 0
                };
                item.SetLayoutLocation(childX, item.Y);
            }
        }

        if (dy != 0)
        {
            if (_totalFlex > 0)
            {
                //重新计算所有Expanded布局，这里只有可能是非Expanded改变高度
                var remainHeight = H;
                for (var i = 0; i < _children.Count; i++)
                {
                    if (i != 0 && remainHeight >= _spacing)
                        remainHeight = Math.Max(0, remainHeight - _spacing);
                    if (remainHeight < _children[i].H) //eg: 子组件改变后高度超出原总高
                    {
                        _children[i].PerformLayout(new(width, remainHeight));
                        remainHeight -= _children[i].H;
                        break;
                    }

                    remainHeight -= _children[i].H; //TODO:***不要扣除Expanded的高度
                }

                var totalHeight = 0f;
                for (var i = 0; i < _children.Count; i++)
                {
                    if (i != 0) totalHeight += _spacing;

                    var c = _children[i];
                    if (totalHeight >= H)
                        c.PerformLayout(Size.Empty);
                    if (c is Expanded expanded)
                    {
                        c.PerformLayout(remainHeight <= 0
                            ? Size.Empty
                            : new(width, remainHeight * (expanded.Flex / _totalFlex)));
                    }

                    c.SetLayoutLocation(c.X, totalHeight);
                    totalHeight += c.H;
                }

                //不需要SetSize，不会超出原大小
            }
            else
            {
                var totalHeight = 0f;
                for (var i = 0; i < _children.Count; i++)
                {
                    if (i != 0) totalHeight += _spacing;
                    var remainHeight = height - totalHeight;
                    var c = _children[i];
                    if (remainHeight >= c.H)
                    {
                        c.SetLayoutLocation(c.X, totalHeight);
                        totalHeight += c.H;
                    }
                    else
                    {
                        c.PerformLayout(new(width, Math.Max(0, remainHeight)));
                        c.SetLayoutLocation(c.X, totalHeight);
                        totalHeight += c.H;
                    }
                }

                SetLayoutSize(W, Math.Min(height, H + dy));
            }
        }

        TryNotifyParentIfSizeChanged(oldWidth, oldHeight, affects);
    }
}