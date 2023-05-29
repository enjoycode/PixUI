namespace PixUI;

public sealed class Container : SingleChildWidget
{
    public Container()
    {
        IsLayoutTight = false; //暂默认布局时充满可用空间
    }

    private State<Color>? _bgColor;

    public override bool IsOpaque => _bgColor != null && _bgColor.Value.IsOpaque;

    public State<Color>? BgColor
    {
        get => _bgColor;
        set => _bgColor = Rebind(_bgColor, value, BindingOptions.AffectsVisual);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_bgColor != null)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PaintUtils.Shared(_bgColor.Value));

        PaintChildren(canvas, area);
    }
}