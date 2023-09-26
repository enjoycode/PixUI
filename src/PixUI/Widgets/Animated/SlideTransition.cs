namespace PixUI;

/// <summary>
/// Animates the position of a widget relative to its normal position.
/// </summary>
/// <remarks>
/// The translation is expressed as an Offset scaled to the child's size.
/// For example, an Offset with a dx of 0.25 will result in a horizontal
/// translation of one quarter the width of the child.
///
/// By default, the offsets are applied in the coordinate system of the canvas
/// (so positive x offsets move the child towards the right).
/// </remarks>
public sealed class SlideTransition : Transform
{
    private readonly Animation<Offset> _position;
    private float _offsetX = 0;
    private float _offsetY = 0;

    public SlideTransition(Animation<Offset> position)
        : base(Matrix4.CreateIdentity(), null, false)
    {
        _position = position;
        _position.ValueChanged += OnPositionChanged;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        base.Layout(availableWidth, availableHeight);
        //根据子组件大小计算并初始化偏移量
        CalcOffset();
        InitTransformAndOrigin(Matrix4.CreateTranslation(_offsetX, _offsetY, 0));
    }

    private void CalcOffset()
    {
        if (Child == null) return;

        _offsetX = _position.Value.Dx * Child.W;
        _offsetY = _position.Value.Dy * Child.H;
    }

    private void OnPositionChanged()
    {
        CalcOffset();
        SetTransform(Matrix4.CreateTranslation(_offsetX, _offsetY, 0));
    }

    public override void Dispose()
    {
        _position.ValueChanged -= OnPositionChanged;
        base.Dispose();
    }
}