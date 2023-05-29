namespace PixUI;

public abstract class View : SingleChildWidget
{
    private State<Color>? _bgBgColor;

    public State<Color>? BgColor
    {
        get => _bgBgColor;
        set => _bgBgColor = Rebind(_bgBgColor, value, BindingOptions.AffectsVisual);
    }

    public override bool IsOpaque => _bgBgColor != null && _bgBgColor.Value.IsOpaque;

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_bgBgColor != null)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PaintUtils.Shared(_bgBgColor.Value));

        PaintChildren(canvas, area);
    }
}