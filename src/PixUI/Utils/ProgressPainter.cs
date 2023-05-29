using System;

namespace PixUI;

public sealed class CircularProgressPainter : IDisposable
{
    public CircularProgressPainter()
    {
        _controller = new AnimationController(_kIndeterminateCircularDuration);
    }

    public void Dispose() => _controller.Dispose();

    private const int _kIndeterminateCircularDuration = 1333 * 2222;
    private const int _pathCount = _kIndeterminateCircularDuration / 1333;
    private const int _rotationCount = _kIndeterminateCircularDuration / 2222;

    private const double _twoPi = Math.PI * 2.0;
    private const double _epsilon = 0.001;
    private const double _sweep = _twoPi - _epsilon;
    private const double _startAngle = -Math.PI / 2.0;

    private readonly AnimationController _controller;

    private readonly Animatable<double> _strokeHeadTween = new CurveTween(
            new Interval(0.0, 0.5, Curves.FastOutSlowIn))
        .Chain(new CurveTween(new SawTooth(_pathCount)));

    private readonly Animatable<double> _strokeTailTween = new CurveTween(
            new Interval(0.5, 1.0, Curves.FastOutSlowIn))
        .Chain(new CurveTween(new SawTooth(_pathCount)));

    private readonly Animatable<double> _offsetTween = new CurveTween(new SawTooth(_pathCount));

    private readonly Animatable<double> _rotationTween =
        new CurveTween(new SawTooth(_rotationCount));

    public void Start(Action valueChangedAction)
    {
        _controller.ValueChanged += valueChangedAction;
        _controller.Repeat();
    }

    public void Stop()
    {
        _controller.Stop();
    }

    /// <summary>
    /// 画至目标Widget的中心位置
    /// </summary>
    public void PaintToWidget(Widget target, Canvas canvas, float indicatorSize = 36f)
    {
        var dx = (target.W - indicatorSize) / 2.0f;
        var dy = (target.H - indicatorSize) / 2.0f;
        canvas.Translate(dx, dy);
        Paint(canvas, indicatorSize);
        canvas.Translate(-dx, -dy);
    }

    public void Paint(Canvas canvas, float indicatorSize)
    {
        var headValue = _strokeHeadTween.Evaluate(_controller);
        var tailValue = _strokeTailTween.Evaluate(_controller);
        var offsetValue = _offsetTween.Evaluate(_controller);
        var rotationValue = _rotationTween.Evaluate(_controller);

        var valueColor = Theme.FocusedColor; //default is Theme.Primary
        PaintInternal(canvas, indicatorSize, null, headValue, tailValue, offsetValue,
            rotationValue, 6, valueColor);
    }

    private static void PaintInternal(Canvas canvas, float size, float? value,
        double headValue, double tailValue, double offsetValue, double rotationValue,
        float strokeWidth, Color valueColor, Color? bgColor = null)
    {
        var rect = Rect.FromLTWH(0, 0, size, size);
        var arcStart = value != null
            ? _startAngle
            : _startAngle + tailValue * 3.0 / 2.0 * Math.PI + rotationValue * Math.PI * 2.0 +
              offsetValue * 0.5 * Math.PI;
        var arcSweep = value != null
            ? Math.Clamp(value.Value, 0.0, 1.0) * _sweep
            : Math.Max(headValue * 3.0 / 2.0 * Math.PI - tailValue * 3.0 / 2.0 * Math.PI,
                _epsilon);

        if (bgColor != null)
        {
            var bgPaint = PaintUtils.Shared(bgColor.Value, PaintStyle.Stroke, strokeWidth);
            bgPaint.AntiAlias = true;
            canvas.DrawArc(rect, 0, (float)_sweep, false, bgPaint);
        }

        var paint = PaintUtils.Shared(valueColor, PaintStyle.Stroke, strokeWidth);
        paint.AntiAlias = true;
        if (value == null) // Indeterminate
            paint.StrokeCap = StrokeCap.Square;

        canvas.DrawArc(rect, (float)arcStart, (float)arcSweep, false, paint);
    }
}