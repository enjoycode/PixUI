namespace PixUI;

public sealed class OpacitySlider : SliderBase
{
    public OpacitySlider(State<double> value, State<Color> noAlphaColor) : base(value)
    {
        MinValue = 0;
        MaxValue = 255;
        _noAlphaColor = noAlphaColor;
        _noAlphaColor.AddListener(_ => Repaint());
    }

    private readonly State<Color> _noAlphaColor;

    protected override void DrawBackground(Canvas canvas)
    {
        var sliderRect = GetSliderRect();
        using var rRect = RRect.FromRectAndRadius(sliderRect, SLIDER_HEIGHT / 2f, SLIDER_HEIGHT / 2f);

        //Draw chessboards
        canvas.Save();
        canvas.ClipRRect(rRect);

        var x = sliderRect.X;
        var y = sliderRect.Y;
        var checkSize = 4f;
        var checkColor = new Color(0xFFD0D0D0);
        var line = 0;
        var paint = PixUI.Paint.Shared(checkColor);
        while (y + checkSize <= sliderRect.Bottom)
        {
            var check = line % 2 == 0;
            while (x + checkSize <= sliderRect.Right)
            {
                paint.Color = check ? checkColor : Colors.White;
                canvas.DrawRect(Rect.FromLTWH(x, y, checkSize, checkSize), paint);
                x += checkSize;
                check = !check;
            }

            x = sliderRect.X;
            y += checkSize;
            line++;
        }

        //Draw linear
        using var linear = Shader.CreateLinearGradient(new Point(sliderRect.X, 0), new Point(sliderRect.Right, 0),
            [Colors.Transparent, _noAlphaColor.Value], [0, 1], TileMode.Clamp);
        paint.Shader = linear;
        canvas.DrawRect(sliderRect, paint);

        paint.Reset();
        canvas.Restore();
    }
}