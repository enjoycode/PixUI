namespace PixUI;

public sealed class ColorHslPanel : Widget
{
    public ColorHslPanel(State<Color> noSlColor, State<Color> colorFromHsl)
    {
        _noSlColor = noSlColor;
        _colorFromHsl = colorFromHsl;
        
        _noSlColor.AddListener(_ => Repaint());
    }
    
    private readonly State<Color> _noSlColor;
    private readonly State<Color> _colorFromHsl;
    
    private static readonly Color[] Linear2Colors = [new(0x00FFFFFF), new(0xFF000000)];
    private static readonly float[] Positions = [0.0f, 1.0f];

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        using var linear1 = Shader.CreateLinearGradient(new Point(0, 0), new Point(W, 0),
            [Colors.White, _noSlColor.Value], Positions, TileMode.Clamp);

        var paint = PixUI.Paint.Shared();
        paint.Shader = linear1;
        canvas.DrawRect(0, 0, W, H, paint);

        using var linear2 = Shader.CreateLinearGradient(new Point(0, 0), new Point(0, H),
            Linear2Colors, Positions, TileMode.Clamp);
        paint.Shader = linear2;
        canvas.DrawRect(0, 0, W, H, paint);
        paint.Reset();
    }
}