#if !__WEB__
using System;

namespace PixUI
{
    public sealed class ParagraphStyle : SKObject
    {
        private ParagraphStyle(IntPtr handle, bool owns) : base(handle, owns) { }

        public ParagraphStyle() : this(SkiaApi.sk_paragraph_style_new(), true) { }

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

        public TextStyle? TextStyle
        {
            get => throw new NotImplementedException();
            set => SkiaApi.sk_paragraph_style_set_text_style(Handle, value!.Handle);
        }
    }
}
#endif