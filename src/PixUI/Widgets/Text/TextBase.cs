using System;

namespace PixUI;

public abstract class TextBase : Widget
{
    protected TextBase(State<string> text)
    {
        Text = Bind(text,
            this is EditableText ? BindingOptions.AffectsVisual : BindingOptions.AffectsLayout);
    }

    public State<string> Text { get; }
    private State<float>? _fontSize;
    private State<FontWeight>? _fontWeight;
    private State<Color>? _textColor;
    private int _maxLines = 1;

    private Paragraph? _cachedParagraph;

    protected Paragraph? CachedParagraph => _cachedParagraph;

    protected virtual bool ForceHeight { get; } = false;

    public State<float>? FontSize
    {
        get => _fontSize;
        set => _fontSize = Rebind(_fontSize, value, BindingOptions.AffectsLayout);
    }

    public State<FontWeight>? FontWeight
    {
        get => _fontWeight;
        set => _fontWeight = Rebind(_fontWeight, value, BindingOptions.AffectsLayout);
    }

    public State<Color>? TextColor
    {
        get => _textColor;
        set => _textColor = Rebind(_textColor, value, BindingOptions.AffectsVisual);
    }

    public int MaxLines
    {
        set
        {
            if (value <= 0)
                throw new ArgumentException();
            if (_maxLines != value)
            {
                _maxLines = value;
                if (IsMounted)
                {
                    _cachedParagraph?.Dispose();
                    _cachedParagraph = null;
                    Invalidate(InvalidAction.Relayout);
                }
            }
        }
    }

    public override void OnStateChanged(StateBase state, BindingOptions options)
    {
        //TODO: fast update font size or color use skia paragraph
        _cachedParagraph?.Dispose();
        _cachedParagraph = null;

        base.OnStateChanged(state, options);
    }

    protected void BuildParagraph(string text, float width)
    {
        //if (_cachedParagraph != null) return;
        _cachedParagraph?.Dispose();

        var color = _textColor?.Value ?? Colors.Black;
        _cachedParagraph = BuildParagraphInternal(text, width, color);
    }

    protected Paragraph BuildParagraphInternal(string text, float width, in Color color)
    {
        var fontSize = _fontSize?.Value ?? Theme.DefaultFontSize;
        FontStyle? fontStyle = _fontWeight == null
            ? null
            : new FontStyle(_fontWeight.Value, FontSlant.Upright);
        return TextPainter.BuildParagraph(text, width, fontSize, color, fontStyle, _maxLines,
            ForceHeight);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        if (Text.Value == null || Text.Value.Length == 0)
        {
            SetSize(0, 0);
            return;
        }

        BuildParagraph(Text.Value, width);

        //TODO:wait skia fix bug
        //https://groups.google.com/g/skia-discuss/c/WXUVWrcgiko?pli=1
        //https://bugs.chromium.org/p/skia/issues/list?q=Area=%22TextLayout%22

        //W = Math.Min(width, _cachedParagraph.LongestLine);
        SetSize(Math.Min(width, _cachedParagraph!.MaxIntrinsicWidth),
            Math.Min(height, _cachedParagraph.Height));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (Text.Value == null || Text.Value.Length == 0) return;

        if (_cachedParagraph == null) //可能颜色改变后导致的缓存丢失，可以简单重建
        {
            var width = Width == null
                ? CachedAvailableWidth
                : Math.Min(Math.Max(0, Width.Value), CachedAvailableWidth);
            BuildParagraph(Text.Value, width);
        }

        canvas.DrawParagraph(_cachedParagraph!, 0, 0);
        //Console.WriteLine($"Paint Text Widget: {_value} at {Left},{Top},{Width},{Height}");
    }
}