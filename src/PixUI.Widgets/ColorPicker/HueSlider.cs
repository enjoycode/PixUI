namespace PixUI;

public sealed class HueSlider : SliderBase
{
    public HueSlider(State<double> value) : base(value)
    {
        MinValue = 0;
        MaxValue = 359.9;
    }

    private static readonly Color[] LinearColors =
        [Colors.Red, Colors.Yellow, Colors.Lime, Colors.Cyan, Colors.Blue, Colors.Magenta, Colors.Red];

    private static readonly float[] LinearPositions = [0.0f, 0.17f, 0.33f, 0.5f, 0.67f, 0.83f, 1.0f];

    protected override void DrawBackground(Canvas canvas)
    {
        using var linearGradient = Shader.CreateLinearGradient(new Point(0, 0), new Point(W, 0),
            LinearColors, LinearPositions, TileMode.Clamp);
        var paint = PixUI.Paint.Shared();
        paint.Shader = linearGradient;
        paint.AntiAlias = true;
        using var rRect = RRect.FromRectAndRadius(GetSliderRect(), SLIDER_HEIGHT / 2f, SLIDER_HEIGHT / 2f);
        canvas.DrawRRect(rRect, paint);
        paint.Reset();
    }
}