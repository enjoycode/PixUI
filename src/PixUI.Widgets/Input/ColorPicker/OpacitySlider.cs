namespace PixUI;

public sealed class OpacitySlider : SliderBase
{
    public OpacitySlider(State<double> opacity, State<Color> hsl) : base(opacity)
    {
        MinValue = 0;
        MaxValue = 255;

        _hsl = hsl;
        _hsl.AddListener(_ => Repaint());
    }

    private readonly State<Color> _hsl;

    protected override void DrawBackground(Canvas canvas)
    {
        var sliderRect = GetSliderRect();
        using var rRect = RRect.FromRectAndRadius(sliderRect, SLIDER_HEIGHT / 2f, SLIDER_HEIGHT / 2f);

        canvas.Save();
        canvas.ClipRRect(rRect, ClipOp.Intersect, true);

        //Draw chessboards
        DrawChessBoards(canvas, sliderRect);

        //Draw linear
        var paint = PixUI.Paint.Shared();
        var noAlphaColor = _hsl.Value;
        using var linear = Shader.CreateLinearGradient(new Point(sliderRect.X, 0), new Point(sliderRect.Right, 0),
            [Colors.Transparent, noAlphaColor], [0, 1], TileMode.Clamp);
        paint.Shader = linear;
        canvas.DrawRect(sliderRect, paint);

        paint.Reset();
        canvas.Restore();
    }

    /// <summary>
    /// 画棋盘格子
    /// </summary>
    internal static void DrawChessBoards(Canvas canvas, Rect rect)
    {
        var x = rect.X;
        var y = rect.Y;
        var checkSize = 4f;
        var checkColor = new Color(0xFFD0D0D0);
        var line = 0;
        var paint = PixUI.Paint.Shared(checkColor);
        while (y + checkSize <= rect.Bottom)
        {
            var check = line % 2 == 0;
            while (x + checkSize <= rect.Right)
            {
                paint.Color = check ? checkColor : Colors.White;
                canvas.DrawRect(Rect.FromLTWH(x, y, checkSize, checkSize), paint);
                x += checkSize;
                check = !check;
            }

            x = rect.X;
            y += checkSize;
            line++;
        }
    }
}