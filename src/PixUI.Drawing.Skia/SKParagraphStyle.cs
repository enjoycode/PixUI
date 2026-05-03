#if !__WEB__
namespace PixUI;

public sealed class SKParagraphStyle : SKObject
{
    private SKParagraphStyle(IntPtr handle, bool owns) : base(handle, owns) { }

    public SKParagraphStyle() : this(SkiaApi.sk_paragraph_style_new(), true) { }

    protected override void DisposeNative() => SkiaApi.sk_paragraph_style_delete(Handle);

    public uint MaxLines
    {
        get => (uint)SkiaApi.sk_paragraph_style_get_max_lines(Handle);
        set => SkiaApi.sk_paragraph_style_set_max_lines(Handle, (int)value);
    }

    public TextAlign TextAlign
    {
        get => SkiaApi.sk_paragraph_style_get_text_align(Handle);
        set => SkiaApi.sk_paragraph_style_set_text_align(Handle, value);
    }

    public float Height
    {
        get => (uint)SkiaApi.sk_paragraph_style_get_height(Handle);
        set => SkiaApi.sk_paragraph_style_set_height(Handle, value);
    }

    public SKTextStyle? TextStyle
    {
        get => throw new NotImplementedException();
        set => SkiaApi.sk_paragraph_style_set_text_style(Handle, value!.Handle);
    }
}
#endif