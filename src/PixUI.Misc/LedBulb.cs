namespace PixUI;

public sealed class LedBulb : Widget
{
    public LedBulb()
    {
        Width = Height = 30;
    }

    private State<Color> _color = new Color(153, 255, 54);
    private State<bool> _on = true;

    public State<Color> Color
    {
        get => _color;
        set => _color = Bind(_color, value, RepaintOnStateChanged)!;
    }

    private Color DarkColor => Colors.Dark(_color.Value);
    private Color DarkDarkColor => Colors.DarkDark(_color.Value);

    public State<bool> On
    {
        get => _on;
        set => _on = Bind(_on, value, RepaintOnStateChanged)!;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);
        SetSize(width, height);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        // Is the bulb on or off
        var lightColor = _on.Value ? Color.Value : DarkColor.WithAlpha(150);
        var darkColor = _on.Value ? DarkColor : DarkDarkColor;

        // Calculate the dimensions of the bulb
        var width = W /*- (this.Padding.Left + this.Padding.Right)*/;
        var height = H /*- (this.Padding.Top + this.Padding.Bottom)*/;
        // Diameter is the lesser of width and height
        var diameter = Math.Min(width, height);
        // Subtract 1 pixel so ellipse doesn't get cut off
        diameter = Math.Max(diameter - 1, 1);

        // Draw the background ellipse
        var rectangle = Rect.FromLTWH(0 /*this.Padding.Left*/, 0 /*this.Padding.Top*/, diameter, diameter);
        var paint = PaintUtils.Shared(darkColor);
        paint.AntiAlias = true;
        canvas.DrawOval(rectangle, paint);

        // Draw the glow gradient
        using var gradient1 = Shader.CreateRadialGradient(rectangle.Center, rectangle.Width / 2f,
            new[] { lightColor, lightColor.WithAlpha(0) }, null, TileMode.Clamp);
        paint = PaintUtils.Shared();
        paint.Shader = gradient1;
        paint.AntiAlias = true;
        canvas.DrawOval(rectangle, paint);

        // Draw the white reflection gradient
        var offset = Convert.ToInt32(diameter * .15F);
        var diameter1 = Convert.ToInt32(rectangle.Width * .8F);
        var whiteRect = Rect.FromLTWH(rectangle.X - offset, rectangle.Y - offset, diameter1, diameter1);
        var reflectionColor = new Color(255, 255, 255, 180);
        var surroundColor = new Color(255, 255, 255, 0);
        using var gradient2 = Shader.CreateRadialGradient(whiteRect.Center, whiteRect.Width / 2f,
            new[] { reflectionColor, surroundColor }, null, TileMode.Clamp);
        paint = PaintUtils.Shared();
        paint.Shader = gradient2;
        paint.AntiAlias = true;
        canvas.DrawOval(whiteRect, paint);

        // Draw the border
        if (_on.Value)
        {
            paint = PaintUtils.Shared(Colors.Black.WithAlpha(85), PaintStyle.Stroke);
            paint.AntiAlias = true;
            canvas.DrawOval(rectangle, paint);
        }
    }
}