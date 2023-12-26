using System;

namespace PixUI;

public sealed class Container : SingleChildWidget
{
    public Container()
    {
        IsLayoutTight = false; //暂默认布局时充满可用空间
    }

    private State<Color>? _fillColor;

    public override bool IsOpaque => _fillColor != null && _fillColor.Value.IsOpaque;

    public State<Color>? FillColor
    {
        get => _fillColor;
        set => Bind(ref _fillColor, value, RepaintOnStateChanged);
    }

    [Obsolete("Use FillColor")]
    public State<Color>? BgColor
    {
        get => FillColor;
        set => FillColor = value;
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_fillColor != null)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(_fillColor.Value));

        PaintChildren(canvas, area);
    }
}