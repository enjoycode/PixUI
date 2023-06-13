using System;

namespace PixUI;

public sealed class FormItem : Widget
{
    public FormItem(string label, Widget widget, int columnSpan = 1)
    {
        if (columnSpan < 1) throw new ArgumentException();

        _widget = widget;
        _widget.Parent = this;
        _label = label;
        ColumnSpan = columnSpan;
    }

    private readonly Widget _widget;
    private readonly string _label;

    internal readonly int ColumnSpan;
    //TODO: tooltip property to show some tips

    private Paragraph? _cachedLabelParagraph;

    #region ====Widget Overrides====

    public override void VisitChildren(Func<Widget, bool> action) => action(_widget);

    public override void Layout(float availableWidth, float availableHeight)
    {
        CachedAvailableWidth = availableWidth;
        CachedAvailableHeight = availableHeight;

        _cachedLabelParagraph ??= TextPainter.BuildParagraph(_label, float.PositiveInfinity,
            Theme.DefaultFontSize, Colors.Black, null, 1 /*TODO*/);

        var lableWidth = ((Form)Parent!).LabelWidth + 5;
        _widget.Layout(availableWidth - lableWidth, availableHeight);
        _widget.SetPosition(lableWidth, 0);

        SetSize(availableWidth, Math.Max(_cachedLabelParagraph.Height, _widget.H));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //TODO: 考虑画边框

        var parent = (Form)Parent!;
        var lableWidth = parent.LabelWidth;
        var alignment = parent.LabelAlignment;
        //先画Label
        var x = 0f;
        if (alignment == HorizontalAlignment.Center)
            x = (lableWidth - _cachedLabelParagraph!.MaxIntrinsicWidth) / 2;
        else if (alignment == HorizontalAlignment.Right)
            x = lableWidth - _cachedLabelParagraph!.MaxIntrinsicWidth;
        canvas.Save(); //TODO:优化不必要的Save and Clip
        canvas.ClipRect(Rect.FromLTWH(0, 0, lableWidth, H), ClipOp.Intersect, false);
        canvas.DrawParagraph(_cachedLabelParagraph!, x,
            (H - _cachedLabelParagraph!.Height) / 2f /*暂上下居中*/);
        canvas.Restore();

        //再画Widget
        PaintChildren(canvas, area);
    }

    #endregion
}