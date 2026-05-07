namespace PixUI;

public abstract class View : SingleChildWidget
{
    private State<Color>? _fillColor;

    public State<Color>? FillColor
    {
        get => _fillColor;
        set => Bind(ref _fillColor, value, RepaintOnStateChanged);
    }

    public override bool IsOpaque => _fillColor != null && _fillColor.Value.IsOpaque;

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        if (_fillColor != null)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(_fillColor.Value));

        PaintChildren(canvas, area);
    }
}