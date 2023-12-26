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
        set => Bind(ref _color, value, RepaintOnStateChanged);
    }

    public State<Color>? ShadowColor
    {
        get => _shadowColor;
        set => Bind(ref _shadowColor, value, RepaintOnStateChanged);
    }

    public State<float>? Elevation
    {
        get => _elevation;
        set => Bind(ref _elevation, value, RepaintOnStateChanged);
    }

    public State<EdgeInsets>? Margin
    {
        get => _margin;
        set => Bind(ref _margin, value, RelayoutOnStateChanged);
    }

    public State<ShapeBorder>? Shape
    {
        get => _shape;
        set => Bind(ref _shape, value, RelayoutOnStateChanged);
    }

    #endregion

    #region ====Overrides====

    //TODO:方形框无Margin且不透明背景
    //protected internal override bool IsOpaque => _color != null && _color.Value.Alpha == 0;

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        if (Child == null)
        {
            SetSize(maxSize.Width, maxSize.Height);
            return;
        }

        var margin = _margin?.Value ?? EdgeInsets.All(DefaultMargin);
        var padding = Padding?.Value ?? EdgeInsets.All(0);

        Child.Layout(maxSize.Width - margin.Horizontal - padding.Horizontal,
            maxSize.Height - margin.Vertical - padding.Vertical);
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
            canvas.Save();
            canvas.ClipPath(outerPath, ClipOp.Difference, true);
            canvas.DrawShadow(outerPath, shadowColor, elevation,
                shadowColor.Alpha != 0xFF, Root!.Window.ScaleFactor);
            canvas.Restore();
        }

        //Clip外形后填充背景及边框
        canvas.Save();
        canvas.ClipPath(outerPath, ClipOp.Intersect, true);
        var fill = PixUI.Paint.Shared(color);
        canvas.DrawRect(rect, fill);
        shape.Paint(canvas, rect);

        PaintChildren(canvas, area);

        canvas.Restore();
    }

    #endregion
}