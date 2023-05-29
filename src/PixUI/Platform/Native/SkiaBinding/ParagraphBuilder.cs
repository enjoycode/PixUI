using System;

#if !__WEB__
namespace PixUI
{
    public sealed class ParagraphBuilder : SKObject
    {
        private ParagraphBuilder(IntPtr handle, bool owns) : base(handle, owns) { }

        public ParagraphBuilder(ParagraphStyle style)
            : this(SkiaApi.sk_paragraph_builder_new(
                style.Handle, FontCollection.Instance.Handle), true) { }

        protected override void DisposeNative() => SkiaApi.sk_paragraph_builder_delete(Handle);

        public void PushStyle(TextStyle textStyle) =>
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

        public Paragraph Build()
        {
            var para = SkiaApi.sk_paragraph_builder_build(Handle);
            return new Paragraph(para, true);
        }
    }
}
#endif