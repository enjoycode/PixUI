using System;
using System.Diagnostics;

namespace PixUI;

public sealed class Checkbox : Toggleable
{
    public Checkbox(State<bool> value)
    {
        _previousValue = value.Value;
        InitState(
            RxComputed<bool?>.Make<bool, bool?>(value, v => v, v => value.Value = v ?? false),
            false);
        _positionController.StatusChanged += OnPositionStatusChanged;
    }

    public static Checkbox Tristate(State<bool?> value)
    {
        var checkbox = new Checkbox(_notSetState);
        checkbox._previousValue = value.Value;
        checkbox.InitState(value, true); //replace to nullable state
        return checkbox;
    }

    private static readonly State<bool> _notSetState = false;

    private bool? _previousValue;

    private OutlinedBorder _shape =
        new RoundedRectangleBorder(null, BorderRadius.All(Radius.Circular(1.0f)));

    private void OnPositionStatusChanged(AnimationStatus status)
    {
        //暂在动画完成后更新缓存的旧值
        if (status == AnimationStatus.Completed || status == AnimationStatus.Dismissed)
            _previousValue = _value.Value;
    }

    #region ====Widget Overrides====

    private const float _kCheckboxSize = 30;
    private const float _kEdgeSize = 18;
    private const float _kStrokeWidth = 2.0f;

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        SetSize(Math.Min(width, _kCheckboxSize), Math.Min(height, _kCheckboxSize));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var origin = new Offset(W / 2f - _kEdgeSize / 2f, H / 2f - _kEdgeSize / 2f);
        var checkColor = Colors.White; //TODO:

        var status = _positionController.Status;
        var tNormalized =
            status == AnimationStatus.Forward || status == AnimationStatus.Completed
                ? _positionController.Value
                : 1.0 - _positionController.Value;

        // Four cases: false to null, false to true, null to false, true to false
        if (_previousValue == false || _value.Value == false)
        {
            var t = _value.Value == false ? 1.0f - tNormalized : tNormalized;
            var outer = OuterRectAt(origin, (float)t);
            var color = ColorAt(t);
            var paint = PaintUtils.Shared(color);

            if (t <= 0.5)
            {
                var border = /*_side ?? */ new BorderSide(color, 2);
                DrawBox(canvas, outer, paint, border, false); //only draw border
            }
            else
            {
                DrawBox(canvas, outer, paint, /*_side*/ null, true);
                var strokePaint =
                    PaintUtils.Shared(checkColor, PaintStyle.Stroke, _kStrokeWidth);
                var tShrink = (t - 0.5) * 2.0;
                if (_previousValue == null || _value.Value == null)
                    DrawDash(canvas, origin, tShrink, strokePaint);
                else
                    DrawCheck(canvas, origin, tShrink, strokePaint);
            }
        }
        // Two cases: null to true, true to null
        else
        {
            var outer = OuterRectAt(origin, 1.0f);
            var paint = PaintUtils.Shared(ColorAt(1.0));
            DrawBox(canvas, outer, paint, /*_side*/ null, true);

            var strokePaint = PaintUtils.Shared(checkColor, PaintStyle.Stroke, _kStrokeWidth);
            if (tNormalized <= 0.5)
            {
                var tShrink = 1.0 - tNormalized * 2.0;
                if (_previousValue.HasValue && _previousValue.Value)
                    DrawCheck(canvas, origin, tShrink, strokePaint);
                else
                    DrawDash(canvas, origin, tShrink, strokePaint);
            }
            else
            {
                var tExpand = (tNormalized - 0.5) * 2.0;
                if (_value.Value != null && _value.Value.Value)
                    DrawCheck(canvas, origin, tExpand, strokePaint);
                else
                    DrawDash(canvas, origin, tExpand, strokePaint);
            }
        }
    }

    private void DrawBox(Canvas canvas, in Rect outer, Paint paint, BorderSide? side, bool fill)
    {
        if (fill)
            canvas.DrawPath(_shape.GetOuterPath(outer), paint);
        if (side != null)
            _shape.CopyWith(side).Paint(canvas, outer);
    }

    private static void DrawCheck(Canvas canvas, Offset origin, double t, Paint paint)
    {
        Debug.Assert(t >= 0 && t <= 1.0);
        // As t goes from 0.0 to 1.0, animate the two check mark strokes from the
        // short side to the long side.
        var start = new Offset(_kEdgeSize * 0.15f, _kEdgeSize * 0.45f);
        var mid = new Offset(_kEdgeSize * 0.4f, _kEdgeSize * 0.7f);
        var end = new Offset(_kEdgeSize * 0.85f, _kEdgeSize * 0.25f);

            
        if (t < 0.5)
        {
            var strokeT = t * 2.0;
            var drawMid = Offset.Lerp(start, mid, strokeT)!.Value;
            canvas.DrawLine(origin.Dx + start.Dx, origin.Dy + start.Dy, 
                origin.Dx + drawMid.Dx, origin.Dy + drawMid.Dy, paint);
            //path.MoveTo(origin.Dx + start.Dx, origin.Dy + start.Dy);
            //path.LineTo(origin.Dx + drawMid.Dx, origin.Dy + drawMid.Dy);
        }
        else
        {
            var strokeT = (t - 0.5) * 2.0;
            var drawEnd = Offset.Lerp(mid, end, strokeT)!.Value;
            canvas.DrawLine(origin.Dx + start.Dx, origin.Dy + start.Dy, origin.Dx + mid.Dx + 0.8f, origin.Dy + mid.Dy + 0.8f, paint);
            canvas.DrawLine(origin.Dx + mid.Dx, origin.Dy + mid.Dy, origin.Dx + drawEnd.Dx, origin.Dy + drawEnd.Dy, paint);

            //using var path = new Path(); //TODO: swapbuffer error on dx12
            //path.MoveTo(origin.Dx + start.Dx, origin.Dy + start.Dy);
            //path.LineTo(origin.Dx + mid.Dx, origin.Dy + mid.Dy);
            //path.LineTo(origin.Dx + drawEnd.Dx, origin.Dy + drawEnd.Dy);
            //path.FillType = SKPathFillType.InverseEvenOdd;
            //canvas.DrawPath(path, paint);
        }
    }

    private static void DrawDash(Canvas canvas, Offset origin, double t, Paint paint)
    {
        Debug.Assert(t >= 0 && t <= 1.0);

        // As t goes from 0.0 to 1.0, animate the horizontal line from the
        // mid point outwards.
        var start = new Offset(_kEdgeSize * 0.2f, _kEdgeSize * 0.5f);
        var mid = new Offset(_kEdgeSize * 0.5f, _kEdgeSize * 0.5f);
        var end = new Offset(_kEdgeSize * 0.8f, _kEdgeSize * 0.5f);

        var drawStart = Offset.Lerp(start, mid, 1.0 - t)!.Value;
        var drawEnd = Offset.Lerp(mid, end, t)!.Value;
        canvas.DrawLine(origin.Dx + drawStart.Dx, origin.Dy + drawStart.Dy,
            origin.Dx + drawEnd.Dx, origin.Dy + drawEnd.Dy, paint);
    }

    /// <summary>
    /// The square outer bounds of the checkbox at t, with the specified origin.
    /// At t == 0.0, the outer rect's size is _kEdgeSize (Checkbox.width)
    /// At t == 0.5, .. is _kEdgeSize - _kStrokeWidth
    /// At t == 1.0, .. is _kEdgeSize
    /// </summary>
    private static Rect OuterRectAt(Offset origin, float t)
    {
        var inset = 1.0f - Math.Abs(t - 0.5f) * 2.0f;
        var size = _kEdgeSize - inset * _kStrokeWidth;
        return Rect.FromLTWH(origin.Dx + inset, origin.Dy + inset, size, size);
    }

    /// <summary>
    /// The checkbox's border color if value == false, or its fill color when
    /// value == true or null.
    /// </summary>
    private static Color ColorAt(double t)
    {
        //TODO: fix activeColor and inactiveColor
        var activeColor = Theme.AccentColor;
        var inactiveColor = new Color(0x52000000);
        // As t goes from 0.0 to 0.25, animate from the inactiveColor to activeColor.
        return t >= 0.25 ? activeColor : Color.Lerp(inactiveColor, activeColor, t * 4.0)!.Value;
    }

    #endregion
}