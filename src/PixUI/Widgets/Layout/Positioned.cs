using System;

namespace PixUI;

public sealed class Positioned : Widget
{
    private Widget? _child;
    private State<float>? _left;
    private State<float>? _top;
    private State<float>? _right;
    private State<float>? _bottom;

    public Widget? Child
    {
        get => _child;
        set
        {
            if (_child != null)
                _child.Parent = null;

            _child = value;

            if (_child != null)
                _child.Parent = this;
        }
    }

    public State<float>? Left
    {
        get => _left;
        set
        {
            _left = Bind(_left, value, RelayoutOnStateChanged);
            Parent?.Invalidate(InvalidAction.Repaint); //TODO: remove when impl OnChildBoundsChanged
        }
    }

    public State<float>? Right
    {
        get => _right;
        set
        {
            _right = Bind(_right, value, RelayoutOnStateChanged);
            Parent?.Invalidate(InvalidAction.Repaint); //TODO:
        }
    }

    public State<float>? Top
    {
        get => _top;
        set
        {
            _top = Bind(_top, value, RelayoutOnStateChanged);
            Parent?.Invalidate(InvalidAction.Repaint); //TODO:
        }
    }

    public State<float>? Bottom
    {
        get => _bottom;
        set
        {
            _bottom = Bind(_bottom, value, RelayoutOnStateChanged);
            Parent?.Invalidate(InvalidAction.Repaint); //TODO:
        }
    }

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (_child != null) action(_child);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        CachedAvailableWidth = availableWidth;
        CachedAvailableHeight = availableHeight;

        if (_child == null)
        {
            SetSize(0, 0);
            return;
        }

        var x = 0f;
        var y = 0f;
        var w = Width?.Value;
        var h = Height?.Value;
        var calcX_afterLayoutChild = false;
        var calcY_afterLayoutChild = false;

        if (_left != null && _right != null)
        {
            x = _left.Value;
            w = availableWidth - x - _right.Value;
        }
        else
        {
            if (_left != null)
                x = _left.Value;
            else if (_right != null)
            {
                if (w.HasValue)
                    x = availableWidth - w.Value - _right.Value;
                else
                    calcX_afterLayoutChild = true;
            }
        }

        if (_top != null && _bottom != null)
        {
            y = _top.Value;
            h = availableHeight - y - _bottom.Value;
        }
        else
        {
            if (_top != null)
                y = _top.Value;
            else if (_bottom != null)
            {
                if (h.HasValue)
                    y = availableHeight - h.Value - _bottom.Value;
                else
                    calcY_afterLayoutChild = true;
            }
        }

        if (w is <= 0 || h is <= 0) //eg: availableWidth - left - right <= 0
        {
            SetSize(0, 0);
            return;
        }

        //注意优先顺序 (_child.Width, _child.Height) > (w,h) > (availableWidth, availableHeight)
        _child.Layout(_child.Width?.Value ?? (w ?? availableWidth),
            _child.Height?.Value ?? (h ?? availableHeight));
        _child.SetPosition(0, 0);

        w ??= _child.W;
        h ??= _child.H;

        if (calcX_afterLayoutChild)
            x = availableWidth - w.Value - _right!.Value;
        if (calcY_afterLayoutChild)
            y = availableHeight - h.Value - _bottom!.Value;

        SetPosition(x, y);
        SetSize(w.Value, h.Value);
    }
}