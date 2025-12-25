using System;

namespace PixUI;

public abstract class TextBase : Widget
{
    private static readonly State<string> EmptyText = new RxProxy<string>(() => string.Empty);
    private State<string> _text = EmptyText;
    private State<float>? _fontSize;
    private State<FontWeight>? _fontWeight;
    private State<Color>? _textColor;
    private int _maxLines = 1;

    protected Paragraph? CachedParagraph { get; private set; }

    public State<string> Text
    {
        get => _text;
        set
        {
            Bind(ref _text!, value, OnTextChanged);
#if DEBUG
            DebugLabel = _text.Value;
#endif
        }
    }

    protected virtual bool ForceHeight { get; } = false;

    public State<float>? FontSize
    {
        get => _fontSize;
        set => Bind(ref _fontSize, value, RelayoutOnStateChanged);
    }

    public State<FontWeight>? FontWeight
    {
        get => _fontWeight;
        set => Bind(ref _fontWeight, value, RepaintOnStateChanged);
    }

    public State<Color>? TextColor
    {
        get => _textColor;
        set => Bind(ref _textColor, value, RepaintOnStateChanged);
    }

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

    protected virtual void OnTextChanged(State state)
    {
        if (this is EditableText)
            RepaintOnStateChanged(state);
        else
            RelayoutOnStateChanged(state);
    }

    public void ClearCachedParagraph()
    {
        //TODO: fast update font size or color use skia paragraph
        CachedParagraph?.Dispose();
        CachedParagraph = null;
    }

    protected override void RepaintOnStateChanged(State state)
    {
        ClearCachedParagraph();
        base.RepaintOnStateChanged(state);
    }

    protected override void RelayoutOnStateChanged(State state)
    {
        ClearCachedParagraph();
        base.RelayoutOnStateChanged(state);
    }

    protected void BuildParagraph(string text, float width)
    {
        //if (_cachedParagraph != null) return;
        CachedParagraph?.Dispose();

        var color = _textColor?.Value ?? Colors.Black;
        CachedParagraph = BuildParagraphInternal(text, width, color);
    }

    protected Paragraph BuildParagraphInternal(string text, float width, in Color color)
    {
        var fontSize = _fontSize?.Value ?? Theme.DefaultFontSize;
        FontStyle? fontStyle = _fontWeight == null
            ? null
            : new FontStyle(_fontWeight.Value, FontSlant.Upright);
        return TextPainter.BuildParagraph(text, width, fontSize, color, fontStyle, _maxLines, ForceHeight);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        if (string.IsNullOrEmpty(Text.Value) || Text.Value.Length == 0)
        {
            SetSize(0, 0);
            return;
        }

        BuildParagraph(Text.Value, maxSize.Width);

        //TODO:wait skia fix bug
        //https://groups.google.com/g/skia-discuss/c/WXUVWrcgiko?pli=1
        //https://bugs.chromium.org/p/skia/issues/list?q=Area=%22TextLayout%22

        //W = Math.Min(width, _cachedParagraph.LongestLine);
        SetSize(Math.Min(maxSize.Width, CachedParagraph!.MaxIntrinsicWidth),
            Math.Min(maxSize.Height, CachedParagraph.Height));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (string.IsNullOrEmpty(Text.Value) || Text.Value.Length == 0) return;

        if (CachedParagraph == null) //可能颜色改变后导致的缓存丢失，可以简单重建
        {
            var width = Width == null
                ? CachedAvailableWidth
                : Math.Min(Math.Max(0, Width.Value), CachedAvailableWidth);
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

    public override void Dispose()
    {
        ClearCachedParagraph();
        base.Dispose();
    }
}