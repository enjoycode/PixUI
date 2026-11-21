namespace PixUI;

public sealed class Icon : Widget
{
    public Icon(State<IconData> data)
    {
        _glyph = new IconGlyph(data.Value, Theme.FontSize);
        Bind(ref _data!, data, OnDataChanged);
    }

    private readonly State<IconData> _data;
    private IconGlyph _glyph;
    private State<float>? _size;
    private State<Color>? _color;

    public State<float>? Size
    {
        get => _size;
        set => Bind(ref _size, value, OnSizeChanged, true);
    }

    public State<Color>? Color
    {
        get => _color;
        set => Bind(ref _color, value, Repaint);
    }

    private void OnDataChanged(State state)
    {
        _glyph.GlyphId = _glyph.Font.GetGlyph(_data.Value.CodePoint);
        Repaint();
    }

    private void OnSizeChanged(State state)
    {
        _glyph.Font.Size = _size?.Value ?? Theme.FontSize;
        Relayout();
    }

    protected override void OnLayout(Size maxSize)
    {
        var size = _size?.Value ?? Theme.FontSize; //TODO: measure font size
        SetLayoutSize(Math.Max(0, Math.Min(maxSize.Width, size)),
            Math.Max(0, Math.Min(maxSize.Height, size)));
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        var color = _color?.Value ?? new Color(0xff5f6368);
        _glyph.Paint(canvas, color);
    }

    protected override void DisposeInternal()
    {
        _glyph.Dispose();
        base.DisposeInternal();
    }
}