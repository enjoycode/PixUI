namespace PixUI;

public sealed class SKParagraphBuilder : SKObject
{
    private SKParagraphBuilder(IntPtr handle, bool owns) : base(handle, owns) { }

    public SKParagraphBuilder(SKParagraphStyle style)
        : this(SkiaApi.sk_paragraph_builder_new(
            style.Handle, ((SKFontCollection)Render.Provider.FontCollection).Handle), true) { }

    protected override void DisposeNative() => SkiaApi.sk_paragraph_builder_delete(Handle);

    public void PushStyle(SKTextStyle textStyle) =>
        SkiaApi.sk_paragraph_builder_push_style(Handle, textStyle.Handle);

    public void Pop() => SkiaApi.sk_paragraph_builder_pop(Handle);

    public unsafe void AddText(string text)
    {
        ReadOnlySpan<char> span = text.AsSpan();
        fixed (void* t = span)
        {
            SkiaApi.sk_paragraph_builder_add_utf16_text(Handle, t, span.Length);
        }
    }

    public SKParagraph Build()
    {
        var para = SkiaApi.sk_paragraph_builder_build(Handle);
        return new SKParagraph(para, true);
    }
}