using System;
using System.Runtime.Intrinsics.X86;

namespace PixUI;

public abstract class SliderBase : Widget, IMouseRegion
{
    protected SliderBase(State<double> value)
    {
        _value = value;
        _value.AddListener(_ => Repaint());

        MouseRegion = new MouseRegion();
        MouseRegion.PointerDown += OnPointerDown;
        MouseRegion.PointerMove += OnPointerMove;
    }

    private readonly State<double> _value;
    private double _minValue;
    private double _maxValue = 100;
    protected const float SLIDER_HEIGHT = DragThumb.THUMB_RADIUS * 2;
    protected const float V_PADDING = SLIDER_HEIGHT / 2 + 1;
    protected const float H_PADDING = 0.5f;

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
        double ratio = (posX - V_PADDING) / (W - V_PADDING * 2);
        ratio = Math.Clamp(ratio, 0, 1);
        _value.Value = ratio * (MaxValue - MinValue) + MinValue;
    }

    #endregion

    protected Rect GetSliderRect() => Rect.FromLTWH(V_PADDING, H_PADDING, W - V_PADDING * 2, H - H_PADDING * 2);

    protected float GetPositionForValue()
    {
        var w = W - V_PADDING * 2;
        var ratio = (_value.Value - MinValue) / (MaxValue - MinValue);
        return (float)(w * ratio);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        SetSize(maxSize.Width, SLIDER_HEIGHT + H_PADDING * 2);
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
        var cx = GetPositionForValue() + V_PADDING;
        var cy = H / 2;

        DragThumb.Draw(canvas, cx, cy, Colors.Red);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        DrawBackground(canvas);
        DrawThumb(canvas);
    }
}