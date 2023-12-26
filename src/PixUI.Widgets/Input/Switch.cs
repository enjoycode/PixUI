using System;

namespace PixUI;

public sealed class Switch : Toggleable
{
    public Switch(State<bool> value)
    {
        InitState(RxComputed<bool?>.Make<bool, bool?>(value,
                v => v,
                v => value.Value = v ?? false),
            false);
    }

    #region ====Widget Overrides====

    private const float _kTrackWidth = 40;
    private const float _kTrackHeight = 24f;
    private const float _kTrackRadius = _kTrackHeight / 2.0f;
    private const float _kTrackInnerStart = _kTrackHeight / 2.0f;
    private const float _kTrackInnerEnd = _kTrackWidth - _kTrackInnerStart;
    private const float _kSwitchWidth = _kTrackWidth + 6;
    private const float _kSwitchHeight = _kTrackHeight + 6;

    // private const float _kTrackInnerLength = _kTrackInnerEnd - _kTrackInnerStart;

    private const float _kThumbExtension = 7f;
    private const float _kThumbRadius = _kTrackHeight / 2 - 2;
    private static readonly Color _kThumbBorderColor = new Color(0x0A000000);

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        SetSize(Math.Min(width, _kSwitchWidth), Math.Min(height, _kSwitchHeight));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        canvas.Save();
        canvas.Translate(0, (_kSwitchHeight - _kTrackHeight) / 2f);

        var currentValue = _positionController.Value;
        var currentReactionValue = 0f;
        var visualPosition = currentValue;

        var activeColor = Theme.AccentColor;
        var trackColor = new Color(0x52000000);
        var paint = PixUI.Paint.Shared(Color.Lerp(trackColor, activeColor, currentValue));
        paint.AntiAlias = true;

        // track
        var trackRect = Rect.FromLTWH(
            (W - _kTrackWidth) / 2f, (H - _kSwitchHeight) / 2f, _kTrackWidth, _kTrackHeight
        );
        var trackRRect = RRect.FromRectAndRadius(trackRect, _kTrackRadius, _kTrackRadius);
        canvas.DrawRRect(trackRRect, paint);

        // thumb
        var currentThumbExtension = _kThumbExtension * currentReactionValue;
        var thumbLeft = FloatUtils.Lerp(
            trackRect.Left + _kTrackInnerStart - _kThumbRadius,
            trackRect.Left + _kTrackInnerEnd - _kThumbRadius - currentThumbExtension,
            visualPosition
        );
        var thumbRight = FloatUtils.Lerp(
            trackRect.Left + _kTrackInnerStart + _kThumbRadius + currentThumbExtension,
            trackRect.Left + _kTrackInnerEnd + _kThumbRadius,
            visualPosition
        );
        var thumbCenterY = _kTrackHeight / 2.0f;
        var thumbBounds = new Rect(thumbLeft, thumbCenterY - _kThumbRadius, thumbRight,
            thumbCenterY + _kThumbRadius);

        var clipPath = new Path();
        clipPath.AddRRect(trackRRect);
        canvas.ClipPath(clipPath, ClipOp.Intersect, true);

        PaintThumb(canvas, thumbBounds);

        canvas.Restore();
    }

    private static void PaintThumb(Canvas canvas, Rect rect)
    {
        var shortestSide = Math.Min(rect.Width, rect.Height);
        var rrect = RRect.FromRectAndRadius(rect, shortestSide / 2f, shortestSide / 2f);

        var paint = PixUI.Paint.Shared(Color.Empty);
        paint.AntiAlias = true;

        // shadow
        rrect.Shift(0, 3);
        var shadowColor = new Color(0x26000000);
        var blurRadius = 8.0f;
        paint.Color = shadowColor;
        paint.MaskFilter = MaskFilter.CreateBlur(BlurStyle.Normal,
            MaskFilter.ConvertRadiusToSigma(blurRadius));
        canvas.DrawRRect(rrect, paint);

        shadowColor = new Color(0x0F000000);
        blurRadius = 1f;
        paint.Color = shadowColor;
        paint.MaskFilter = MaskFilter.CreateBlur(BlurStyle.Normal,
            MaskFilter.ConvertRadiusToSigma(blurRadius));
        canvas.DrawRRect(rrect, paint);
        rrect.Shift(0, -3);

        // border and fill
        rrect.Inflate(0.5f, 0.5f);
        paint.Color = _kThumbBorderColor;
        paint.MaskFilter = null;
        canvas.DrawRRect(rrect, paint);
        rrect.Deflate(0.5f, 0.5f);

        paint.Color = Colors.White;
        canvas.DrawRRect(rrect, paint);
    }

    #endregion
}