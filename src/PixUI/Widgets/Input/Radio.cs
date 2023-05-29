using System;

namespace PixUI;

public sealed class Radio : Toggleable
{
    public Radio(State<bool> value)
    {
        InitState(RxComputed<bool?>.Make<bool, bool?>(value,
                v => v,
                v => value.Value = v ?? false),
            false);
    }

    #region ====Widget Overrides====

    private const float _kRadioSize = 30;
    private const float _kOuterRadius = 8f;
    private const float _kInnerRadius = 4.5f;

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        SetSize(Math.Min(width, _kRadioSize), Math.Min(height, _kRadioSize));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var center = new Offset(W / 2, H / 2);

        var activeColor = Theme.AccentColor;
        var inactiveColor = new Color(0x52000000);
        var color = Color.Lerp(inactiveColor, activeColor, _positionController.Value);

        // outer circle
        var paint = PaintUtils.Shared(color, PaintStyle.Stroke, 2);
        paint.AntiAlias = true;
        canvas.DrawCircle(center.Dx, center.Dy, _kOuterRadius, paint);

        // inner circle
        if (!_positionController.IsDismissed)
        {
            paint.Style = PaintStyle.Fill;
            canvas.DrawCircle(center.Dx, center.Dy,
                (float)(_kInnerRadius * _positionController.Value), paint);
        }
    }

    #endregion
}