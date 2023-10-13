using System;

namespace PixUI;

public sealed class Icon : Widget
{
    public Icon(State<IconData> data)
    {
        _painter = new IconPainter(OnIconFontLoaded);
        _data = Bind(data, OnDataChanged);
    }

    private readonly State<IconData> _data;
    private State<float>? _size;
    private State<Color>? _color;

    private readonly IconPainter _painter;

    public State<float>? Size
    {
        get => _size;
        set => _size = Bind(_size, value, OnSizeChanged);
    }

    public State<Color>? Color
    {
        get => _color;
        set => _color = Bind(_color, value, RepaintOnStateChanged);
    }

    private void OnIconFontLoaded()
    {
        if (!IsMounted) //有时候会作为其他组件的不可访问的子组件
            Parent?.Invalidate(InvalidAction.Repaint,
                new RepaintArea(Rect.FromLTWH(X, Y, W, H)));
        else
            Invalidate(InvalidAction.Repaint);
    }

    private void OnDataChanged(State state)
    {
        _painter.Reset();
        Invalidate(InvalidAction.Repaint);
    }

    private void OnSizeChanged(State state)
    {
        _painter.Reset();
        Invalidate(InvalidAction.Relayout);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var size = _size?.Value ?? Theme.DefaultFontSize;
        SetSize(Math.Max(0, Math.Min(availableWidth, size)),
            Math.Max(0, Math.Min(availableHeight, size)));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var size = _size?.Value ?? Theme.DefaultFontSize;
        var color = _color?.Value ?? new Color(0xff5f6368);
        _painter.Paint(canvas, size, color, _data.Value);
    }

    public override void Dispose()
    {
        _painter.Dispose();
        base.Dispose();
    }
}