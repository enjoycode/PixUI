using System;

namespace PixUI;

public sealed class FormItem : Widget
{
    public FormItem() {}
    
    public FormItem(string label, Widget widget, int columnSpan = 1)
    {
        if (columnSpan < 1) throw new ArgumentException();

        Child = widget;
        Child.Parent = this;
        Label = label;
        ColumnSpan = columnSpan;
    }

    private HorizontalAlignment _labelHorizontalAlignment = HorizontalAlignment.Right;
    private VerticalAlignment _labelVerticalAlignment = VerticalAlignment.Middle;
    
    public Widget Child { get; init; }

    public string Label { get; init; }

    public HorizontalAlignment LabelHorizontalAlignment
    {
        get => _labelHorizontalAlignment;
        set
        {
            if (_labelHorizontalAlignment == value) return;
            _labelHorizontalAlignment = value;
            Repaint();
        }
    }

    public VerticalAlignment LabelVerticalAlignment
    {
        get => _labelVerticalAlignment;
        set
        {
            if (_labelVerticalAlignment == value) return;
            _labelVerticalAlignment = value;
            Repaint();
        }
    }

    internal readonly int ColumnSpan;
    //TODO: tooltip property to show some tips

    private Color? _textColor;
    private float? _fontSize;
    private Paragraph? _cachedLabelParagraph;

    public Color? TextColor
    {
        get => _textColor;
        set
        {
            _textColor = value;
            ClearCache();
            Repaint();
        }
    }

    public float? FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            ClearCache();
            Repaint();
        }
    }

    internal void ClearCache() => _cachedLabelParagraph = null;

    #region ====Widget Overrides====

    public override void VisitChildren(Func<Widget, bool> action) => action(Child);

    public override void Layout(float availableWidth, float availableHeight)
    {
        CachedAvailableWidth = availableWidth;
        CachedAvailableHeight = availableHeight;

        EnsureBuildLabelParagraph();

        var labelWidth = ((Form)Parent!).LabelWidth + 5;
        Child.Layout(availableWidth - labelWidth, availableHeight);
        Child.SetPosition(labelWidth, 0);

        SetSize(availableWidth, Math.Max(_cachedLabelParagraph!.Height, Child.H));
    }

    private void EnsureBuildLabelParagraph()
    {
        if (_cachedLabelParagraph != null) return;

        var form = Parent as Form;
        var textColor = (TextColor ?? form?.TextColor) ?? Colors.Black;
        var fontSize = (FontSize ?? form?.FontSize) ?? Theme.DefaultFontSize;
        _cachedLabelParagraph = TextPainter.BuildParagraph(Label, float.PositiveInfinity, fontSize, textColor);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //TODO: 考虑画边框

        EnsureBuildLabelParagraph();

        var parent = (Form)Parent!;
        var labelWidth = parent.LabelWidth;
        var hAlignment = _labelHorizontalAlignment;
        var vAlignment = _labelVerticalAlignment;
        //先画Label
        var x = 0f;
        if (hAlignment == HorizontalAlignment.Center)
            x = (labelWidth - _cachedLabelParagraph!.MaxIntrinsicWidth) / 2;
        else if (hAlignment == HorizontalAlignment.Right)
            x = labelWidth - _cachedLabelParagraph!.MaxIntrinsicWidth;
        var y = 0f;
        if (vAlignment == VerticalAlignment.Middle)
            y = (H - _cachedLabelParagraph!.Height) / 2f;
        else if (vAlignment == VerticalAlignment.Bottom)
            y = H - _cachedLabelParagraph!.Height;

        canvas.Save(); //TODO:优化不必要的Save and Clip
        canvas.ClipRect(Rect.FromLTWH(0, 0, labelWidth, H), ClipOp.Intersect, false);
        canvas.DrawParagraph(_cachedLabelParagraph!, x, y);
        canvas.Restore();

        //再画Widget
        PaintChildren(canvas, area);
    }

    public override string ToString() => $"{nameof(FormItem)}[\"{Label}\"]";

    #endregion
}