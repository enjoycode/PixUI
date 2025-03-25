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

    private const float KTrackWidth = 40;
    private const float KTrackHeight = 24f;
    private const float KTrackRadius = KTrackHeight / 2.0f;
    private const float KTrackInnerStart = KTrackHeight / 2.0f;
    private const float KTrackInnerEnd = KTrackWidth - KTrackInnerStart;
    private const float KSwitchWidth = KTrackWidth + 6;
    private const float KSwitchHeight = KTrackHeight + 6;

    // private const float _kTrackInnerLength = _kTrackInnerEnd - _kTrackInnerStart;

    private const float KThumbExtension = 7f;
    private const float KThumbRadius = KTrackHeight / 2 - 2;
    private static readonly Color KThumbBorderColor = new Color(0x0A000000);

    public override void Layout(float availableWidth, float availableHeight)
    {
        var max = CacheAndGetMaxSize(availableWidth, availableHeight);
        SetSize(Math.Min(max.Width, KSwitchWidth), Math.Min(max.Height, KSwitchHeight));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        canvas.Save();
        canvas.Translate(0, (KSwitchHeight - KTrackHeight) / 2f);

        var currentValue = _positionController.Value;
        var currentReactionValue = 0f;
        var visualPosition = currentValue;

        var activeColor = Theme.AccentColor;
        var trackColor = new Color(0x52000000);
        var paint = PixUI.Paint.Shared(Color.Lerp(trackColor, activeColor, currentValue));
        paint.AntiAlias = true;

        // track
        var trackRect = Rect.FromLTWH(
            (W - KTrackWidth) / 2f, (H - KSwitchHeight) / 2f, KTrackWidth, KTrackHeight
        );
        var trackRRect = RRect.FromRectAndRadius(trackRect, KTrackRadius, KTrackRadius);
        canvas.DrawRRect(trackRRect, paint);

        // thumb
        var currentThumbExtension = KThumbExtension * currentReactionValue;
        var thumbLeft = FloatUtils.Lerp(
            trackRect.Left + KTrackInnerStart - KThumbRadius,
            trackRect.Left + KTrackInnerEnd - KThumbRadius - currentThumbExtension,
            visualPosition
        );
        var thumbRight = FloatUtils.Lerp(
            trackRect.Left + KTrackInnerStart + KThumbRadius + currentThumbExtension,
            trackRect.Left + KTrackInnerEnd + KThumbRadius,
            visualPosition
        );
        var thumbCenterY = KTrackHeight / 2.0f;
        var thumbBounds = new Rect(thumbLeft, thumbCenterY - KThumbRadius, thumbRight,
            thumbCenterY + KThumbRadius);

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
        paint.Color = KThumbBorderColor;
        paint.MaskFilter = null;
        canvas.DrawRRect(rrect, paint);
        rrect.Deflate(0.5f, 0.5f);

        paint.Color = Colors.White;
        canvas.DrawRRect(rrect, paint);
    }

    #endregion
}