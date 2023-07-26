using System;

namespace PixUI.Dynamic.Design;

public sealed class DesignPlaceholder : Widget
{
    private static Lazy<Paragraph> _hint = new Lazy<Paragraph>(() =>
        TextPainter.BuildParagraph("Drag Widget Here", float.PositiveInfinity, 16, Colors.Gray));

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        SetSize(width, height);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var paint = PaintUtils.Shared(Colors.Gray, PaintStyle.Stroke, 2f);
        paint.AntiAlias = true;
        canvas.DrawLine(0, 0, W, H, paint);
        canvas.DrawLine(W, 0, 0, H, paint);

        var paragraph = _hint.Value;
        canvas.DrawParagraph(paragraph, (W - paragraph.MaxIntrinsicWidth) / 2f, (H - paragraph.Height) / 2f);
    }
}