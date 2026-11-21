namespace PixUI;

/// <summary>
/// 字体图标组件
/// </summary>
[StylableWidget<IconStyle>, BindableWidget]
public sealed partial class Icon : Widget
{
    public Icon() { }

    public Icon(BindableProperty<IconData> data)
    {
        Data = data.Value;
    }

    private IconGlyph _glyph;

    [BindableProperty(listener: "OnDataChanged")]
    public partial BindableProperty<IconData> Data { get; set; }

    private void OnDataChanged(State? state)
    {
        if (_glyph.Font == null)
            _glyph = new IconGlyph(Data.Value, GetStyle().Size.Value);
        else
            _glyph.GlyphId = _glyph.Font.GetGlyph(Data.Value.CodePoint);
        Repaint();
    }

    internal void OnSizeChanged()
    {
        if (_glyph.Font == null) return;
        _glyph.Font.Size = GetStyle().Size.Value;
        Relayout();
    }

    protected override void OnLayout(Size maxSize)
    {
        var size = GetStyle().Size.Value; //TODO: measure font size
        SetLayoutSize(Math.Max(0, Math.Min(maxSize.Width, size)),
            Math.Max(0, Math.Min(maxSize.Height, size)));
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        var color = GetStyle().Color.Value;
        _glyph.Paint(canvas, color);
    }

    protected override void DisposeInternal()
    {
        _glyph.Dispose();
        base.DisposeInternal();
    }
}

[StyleObject<Icon>]
public sealed partial class IconStyle : Style<Icon, IconStyle>
{
    [BindableProperty(defaultValue: "Theme.FontSize", listener: "OnSizeChanged")]
    public partial BindableProperty<float> Size { get; set; }

    [BindableProperty(defaultValue: "new PixUI.Color(0xFF5F6368)")]
    public partial BindableProperty<Color> Color { get; set; }

    private static void OnSizeChanged(Icon icon) => icon.OnSizeChanged();
}