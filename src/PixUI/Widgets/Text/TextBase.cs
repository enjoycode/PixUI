namespace PixUI;

[BindableWidget, StylableWidget<TextWidgetStyle>]
public abstract partial class TextBase : Widget
{
    private int _maxLines = 1;

    protected IParagraph? CachedParagraph { get; private set; }

    [BindableProperty(defaultValue: "string.Empty", listener: nameof(OnTextChanged))]
    public partial BindableProperty<string> Text { get; set; }

    protected virtual bool ForceHeight { get; } = false;

    public int MaxLines
    {
        set
        {
            if (value <= 0) throw new ArgumentException();
            if (_maxLines != value)
            {
                _maxLines = value;
                if (IsMounted)
                {
                    CachedParagraph?.Dispose();
                    CachedParagraph = null;
                    Relayout();
                }
            }
        }
    }

    protected virtual void OnTextChanged(State? state)
    {
        DebugLabel = Text.Value;
        if (this is EditableText)
            Repaint(state);
        else
            Relayout(state);
    }

    public void ClearCachedParagraph()
    {
        //TODO: fast update font size or color use skia paragraph
        CachedParagraph?.Dispose();
        CachedParagraph = null;
    }

    protected void BuildParagraph(string text, float width)
    {
        //if (_cachedParagraph != null) return;
        CachedParagraph?.Dispose();

        var color = GetStyle().TextColor.Value;
        CachedParagraph = BuildParagraphInternal(text, width, color);
    }

    protected IParagraph BuildParagraphInternal(string text, float width, in Color color)
    {
        var style = GetStyle();
        FontStyle? fontStyle = new FontStyle(style.FontWeight.Value, FontSlant.Upright);
        return TextPainter.BuildParagraph(text, width, style.FontSize.Value, color, fontStyle, _maxLines, ForceHeight);
    }

    protected override void OnLayout(Size maxSize)
    {
        if (string.IsNullOrEmpty(Text.Value) || Text.Value.Length == 0)
        {
            SetLayoutSize(Width == null ? 0 : maxSize.Width, Height == null ? 0 : maxSize.Height);
            return;
        }

        BuildParagraph(Text.Value, maxSize.Width);

        //TODO:wait skia fix bug
        //https://groups.google.com/g/skia-discuss/c/WXUVWrcgiko?pli=1
        //https://bugs.chromium.org/p/skia/issues/list?q=Area=%22TextLayout%22

        //W = Math.Min(width, _cachedParagraph.LongestLine);
        var textWidth = CachedParagraph!.MaxIntrinsicWidth;
        var textHeight = CachedParagraph.Height;
        SetLayoutSize(
            Width == null ? Math.Min(maxSize.Width, textWidth) : maxSize.Width,
            Height == null ? Math.Min(maxSize.Height, textHeight) : maxSize.Height
        );
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        if (string.IsNullOrEmpty(Text.Value) || Text.Value.Length == 0) return;

        if (CachedParagraph == null) //可能颜色改变后导致的缓存丢失，可以简单重建
        {
            var width = Width == null
                ? AvailableSize.Width
                : Math.Min(Math.Max(0, Width.Value), AvailableSize.Width);
            BuildParagraph(Text.Value, width);
        }

        //非EditableText超出范围(overflow)裁截绘制区域
        var paragraphWidth = CachedParagraph!.MaxIntrinsicWidth;
        var paragraphHeight = CachedParagraph.Height;
        var overflow = this is not EditableText && (paragraphWidth > W || paragraphHeight > H);
        if (overflow)
        {
            canvas.Save();
            canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
        }

        canvas.DrawParagraph(CachedParagraph!, 0, 0);

        if (overflow)
            canvas.Restore();
    }

    protected override void DisposeInternal()
    {
        ClearCachedParagraph();
        base.DisposeInternal();
    }
}

[StyleObject<TextBase>]
public sealed partial class TextWidgetStyle : Style<TextBase, TextWidgetStyle>
{
    [BindableProperty(defaultValue: "Theme.FontSize", listener: "RelayoutOnStyleChanged")]
    public partial BindableProperty<float> FontSize { get; set; }

    [BindableProperty(defaultValue: "PixUI.FontWeight.Normal", listener: "RepaintOnStyleChanged")]
    public partial BindableProperty<FontWeight> FontWeight { get; set; }

    [BindableProperty(defaultValue: "Theme.TextColor", listener: "RepaintOnStyleChanged")]
    public partial BindableProperty<Color> TextColor { get; set; }

    private static void RelayoutOnStyleChanged(TextBase widget)
    {
        widget.ClearCachedParagraph();
        widget.Relayout();
    }

    private static void RepaintOnStyleChanged(TextBase widget)
    {
        widget.ClearCachedParagraph();
        widget.Repaint();
    }
}