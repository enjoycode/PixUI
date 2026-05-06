namespace PixUI.Drawing.Skia;

public sealed class SKParagraphBuilder : SKObject, IParagraphBuilder
{
    private SKParagraphBuilder(IntPtr handle, bool owns) : base(handle, owns) { }

    public SKParagraphBuilder(SKParagraphStyle style)
        : this(SkiaApi.sk_paragraph_builder_new(
            style.Handle, ((SKFontCollection)Render.Backend.FontCollection).Handle), true) { }

    protected override void DisposeNative() => SkiaApi.sk_paragraph_builder_delete(Handle);

    public void PushStyle(ITextStyle textStyle) =>
        SkiaApi.sk_paragraph_builder_push_style(Handle, ((SKTextStyle)textStyle).Handle);

    public void Pop() => SkiaApi.sk_paragraph_builder_pop(Handle);

    public unsafe void AddText(string text)
    {
        ReadOnlySpan<char> span = text.AsSpan();
        fixed (void* t = span)
        {
            SkiaApi.sk_paragraph_builder_add_utf16_text(Handle, t, span.Length);
        }
    }

    public IParagraph Build()
    {
        var para = SkiaApi.sk_paragraph_builder_build(Handle);
        return new SKParagraph(para, true);
    }
}