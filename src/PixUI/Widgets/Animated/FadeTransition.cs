namespace PixUI;

public sealed class FadeTransition : SingleChildWidget
{
    private readonly Animation<double> _opacity;

    public FadeTransition(Animation<double> opacity)
    {
        _opacity = opacity;
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_opacity.Value == 0 || Child == null)
            return;

        var alpha = (byte)(255 * _opacity.Value);
        var paint = PaintUtils.Shared(new Color(0, 0, 0, alpha));
        var rect = Rect.FromLTWH(Child.X, Child.Y, Child.W, Child.H);
        canvas.SaveLayer(paint, rect);

        PaintChildren(canvas, area);

        canvas.Restore();
    }
}