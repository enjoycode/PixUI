namespace PixUI;

public abstract class View : SingleChildWidget
{
    private State<Color>? _fillFillColor;

    public State<Color>? FillColor
    {
        get => _fillFillColor;
        set => Bind(ref _fillFillColor, value, RepaintOnStateChanged);
    }

    public override bool IsOpaque => _fillFillColor != null && _fillFillColor.Value.IsOpaque;

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_fillFillColor != null)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PaintUtils.Shared(_fillFillColor.Value));

        PaintChildren(canvas, area);
    }
}