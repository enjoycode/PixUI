namespace PixUI;

public sealed class Card : SingleChildWidget
{
    internal const float DefaultMargin = 4;

    private static readonly ShapeBorder DefaultShape =
        new RoundedRectangleBorder(null, BorderRadius.All(Radius.Circular(4)));

    private State<EdgeInsets>? _margin;
    private State<float>? _elevation;
    private State<Color>? _color;
    private State<Color>? _shadowColor;
    private State<ShapeBorder>? _shape;

    #region ====State Properties====

    public State<Color>? Color
    {
        get => _color;
        set => _color = Rebind(_color, value, BindingOptions.AffectsVisual);
    }

    public State<Color>? ShadowColor
    {
        get => _shadowColor;
        set => _shadowColor = Rebind(_shadowColor, value, BindingOptions.AffectsVisual);
    }

    public State<float>? Elevation
    {
        get => _elevation;
        set => _elevation = Rebind(_elevation, value, BindingOptions.AffectsVisual);
    }

    public State<EdgeInsets>? Margin
    {
        get => _margin;
        set => _margin = Rebind(_margin, value, BindingOptions.AffectsLayout);
    }

    public State<ShapeBorder>? Shape
    {
        get => _shape;
        set => _shape = Rebind(_shape, value, BindingOptions.AffectsLayout);
    }

    #endregion

    #region ====Overrides====

    //TODO:方形框无Margin且不透明背景
    //protected internal override bool IsOpaque => _color != null && _color.Value.Alpha == 0;

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        if (Child == null)
        {
            SetSize(width, height);
            return;
        }

        var margin = _margin?.Value ?? EdgeInsets.All(DefaultMargin);
        var padding = Padding?.Value ?? EdgeInsets.All(0);

        Child.Layout(width - margin.Horizontal - padding.Horizontal,
            height - margin.Vertical - padding.Vertical);
        Child.SetPosition(margin.Left + padding.Left, margin.Top + padding.Top);
        SetSize(Child.W + margin.Horizontal + padding.Horizontal,
            Child.H + margin.Vertical + padding.Vertical);
    }

    private Rect GetChildRect()
    {
        var margin = _margin?.Value ?? EdgeInsets.All(DefaultMargin);
        return Rect.FromLTWH(margin.Left, margin.Top, W - margin.Left - margin.Right,
            H - margin.Top - margin.Bottom);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var color = _color?.Value ?? Colors.White;
        var shadowColor = _shadowColor?.Value ?? Colors.Black;
        var elevation = _elevation?.Value ?? 2;
        var rect = GetChildRect();
        var shape = _shape?.Value ?? DefaultShape;
        var outerPath = shape.GetOuterPath(rect);

        //先画阴影
        if (elevation > 0)
        {
            canvas.DrawShadow(outerPath, shadowColor, elevation,
                shadowColor.Alpha != 0xFF, Root!.Window.ScaleFactor);
        }

        //Clip外形后填充背景及边框
        canvas.Save();
        canvas.ClipPath(outerPath, ClipOp.Intersect, true);
        canvas.Clear(color);
        shape.Paint(canvas, rect);

        PaintChildren(canvas, area);

        canvas.Restore();
    }

    #endregion
}