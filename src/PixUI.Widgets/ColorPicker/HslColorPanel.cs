using System;

namespace PixUI;

public sealed class HslColorPanel : Widget, IMouseRegion
{
    public HslColorPanel(State<double> hue, State<Color> hsl)
    {
        _hsl = hsl;
        _hue = hue;
        _hue.AddListener(_ =>
        {
            _hsl.Value = Color.FromHsl(_hue.Value, _s, _l);
            Repaint();
        });

        _s = hsl.Value.GetSaturation();
        _l = hsl.Value.GetBrightness();

        MouseRegion = new MouseRegion();
        MouseRegion.PointerDown += OnPointerDown;
        MouseRegion.PointerMove += OnPointerMove;
    }

    private readonly State<double> _hue;
    private readonly State<Color> _hsl;
    private double _s;
    private double _l;

    private static readonly Color[] Linear2Colors = [new(0x00FFFFFF), new(0xFF000000)];
    private static readonly float[] Positions = [0.0f, 1.0f];
    private const float PADDING = DragThumb.THUMB_RADIUS + 0.5f;

    public MouseRegion MouseRegion { get; }

    #region ====Event Handlers====

    private void OnPointerDown(PointerEvent e)
    {
        CalcAndSetValue(e.X, e.Y);
    }

    private void OnPointerMove(PointerEvent e)
    {
        if (e.Buttons == PointerButtons.Left)
            CalcAndSetValue(e.X, e.Y);
    }

    private void CalcAndSetValue(float x, float y)
    {
        x = Math.Clamp(x - PADDING, 0, W - PADDING * 2);
        y = Math.Clamp(y - PADDING, 0, H - PADDING * 2);
        var saturation = x / (W - PADDING * 2);
        var topL = 1.0f - 0.5f * saturation;
        var lightness = topL * (1.0f - (y / (H - PADDING * 2)));

        _s = saturation;
        _l = lightness;
        _hsl.Value = Color.FromHsl(_hue.Value, _s, _l);
        Repaint();
    }

    #endregion

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        // Draw background
        var noSlColor = Color.FromHsl(_hue.Value);
        using var linear1 = Shader.CreateLinearGradient(new Point(0, 0), new Point(W - PADDING * 2, 0),
            [Colors.White, noSlColor], Positions, TileMode.Clamp);

        var paint = PixUI.Paint.Shared();
        paint.Shader = linear1;
        canvas.DrawRect(PADDING, PADDING, W - PADDING * 2, H - PADDING * 2, paint);

        using var linear2 = Shader.CreateLinearGradient(new Point(0, 0), new Point(0, H - PADDING * 2),
            Linear2Colors, Positions, TileMode.Clamp);
        paint.Shader = linear2;
        canvas.DrawRect(PADDING, PADDING, W - PADDING * 2, H - PADDING * 2, paint);
        paint.Reset();

        // Draw drag thumb
        DrawThumb(canvas);
    }

    private void DrawThumb(Canvas canvas)
    {
        var saturation = _s;
        var lightness = _l;
        var topL = 1.0f - 0.5f * saturation;
        var x = (W - PADDING * 2) * saturation + PADDING;
        var y = (1.0f - lightness / topL) * (H - PADDING * 2) + PADDING;

        DragThumb.Draw(canvas, (float)x, (float)y, Colors.Red);
    }
}