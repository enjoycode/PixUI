using System;

namespace PixUI;

public abstract class SliderBase : Widget, IMouseRegion
{
    protected SliderBase(State<double> value)
    {
        _value = value;
        _value.AddListener(s => Repaint());

        MouseRegion = new MouseRegion();
        MouseRegion.PointerDown += OnPointerDown;
        MouseRegion.PointerMove += OnPointerMove;
    }

    private readonly State<double> _value;
    private double _minValue = 0;
    private double _maxValue = 100;
    protected const float SLIDER_HEIGHT = 16f;
    protected const float PADDING = SLIDER_HEIGHT / 2 + 1;

    public MouseRegion MouseRegion { get; }

    public double MinValue
    {
        get => _minValue;
        set
        {
            if (_minValue >= _maxValue)
                throw new ArgumentOutOfRangeException(nameof(MinValue));
            _minValue = value;
            Repaint();
        }
    }

    public double MaxValue
    {
        get => _maxValue;
        set
        {
            if (_maxValue <= _minValue)
                throw new ArgumentOutOfRangeException(nameof(MaxValue));
            _maxValue = value;
            Repaint();
        }
    }

    #region ====Event Handlers====

    private void OnPointerDown(PointerEvent e)
    {
        CalcAndSetValue(e.X);
    }

    private void OnPointerMove(PointerEvent e)
    {
        if (e.Buttons == PointerButtons.Left)
            CalcAndSetValue(e.X);
    }

    private void CalcAndSetValue(float posX)
    {
        double ratio = (posX - PADDING) / (W - PADDING * 2);
        ratio = Math.Clamp(ratio, 0, 1);
        _value.Value = ratio * (MaxValue - MinValue) + MinValue;
    }

    #endregion

    protected Rect GetSliderRect() => Rect.FromLTWH(PADDING, PADDING, W - PADDING * 2, H - PADDING * 2);

    protected float GetPositionForValue()
    {
        var w = W - PADDING * 2;
        var ratio = (_value.Value - MinValue) / (MaxValue - MinValue);
        return (float)(w * ratio);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        SetSize(maxSize.Width, SLIDER_HEIGHT + PADDING * 2);
    }

    protected virtual void DrawBackground(Canvas canvas)
    {
        var paint = PixUI.Paint.Shared(Colors.DarkGray);
        var sliderRect = GetSliderRect();
        using var rRect = RRect.FromRectAndRadius(sliderRect, SLIDER_HEIGHT / 2f, SLIDER_HEIGHT / 2f);
        canvas.DrawRRect(rRect, paint);
    }

    protected virtual void DrawThumb(Canvas canvas)
    {
        var cx = GetPositionForValue() + PADDING;
        var cy = H / 2;
        var radius = (SLIDER_HEIGHT / 2);

        var paint = PixUI.Paint.Shared(Colors.White);
        paint.AntiAlias = true;
        canvas.DrawCircle(cx, cy, radius, paint);

        paint = PixUI.Paint.Shared(Colors.Red, PaintStyle.Stroke, 1);
        paint.AntiAlias = true;
        canvas.DrawCircle(cx, cy, radius, paint);

        paint.Style = PaintStyle.Fill;
        canvas.DrawCircle(cx, cy, radius - 3, paint);
        paint.Reset();
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        DrawBackground(canvas);
        DrawThumb(canvas);
    }
}