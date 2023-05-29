using System;
using System.Runtime.InteropServices;

#if !__WEB__
namespace PixUI
{
    public sealed class Paragraph : SKObject
    {
        internal Paragraph(IntPtr handle, bool owns) : base(handle, owns) { }

        protected override void DisposeNative() => SkiaApi.sk_paragraph_delete(Handle);

        public float Width => SkiaApi.sk_paragraph_get_max_width(Handle);

        public float LongestLine => SkiaApi.sk_paragraph_get_longest_line(Handle);

        public float MaxIntrinsicWidth => SkiaApi.sk_paragraph_get_max_intrinsic_width(Handle);

        public float Height => SkiaApi.sk_paragraph_get_height(Handle);

        public PositionWithAffinity GetGlyphPositionAtCoordinate(float x, float y)
        {
            int affinity = 0;
            int pos = 0;
            unsafe
            {
                pos = SkiaApi.sk_paragraph_get_glyph_position(Handle, x, y, &affinity);
            }

            return new PositionWithAffinity(pos, affinity);
        }

        public TextBox GetRectForPosition(int pos, /*TODO: maybe need endPos for multi code unit*/
            BoxHeightStyle heightStyle, BoxWidthStyle widthStyle)
        {
            TextBox textBox;
            unsafe
            {
                SkiaApi.sk_paragraph_get_rect_for_position(Handle, pos,
                    (int)heightStyle, (int)widthStyle, &textBox);
            }

            return textBox;
        }

        public void Layout(float width) => SkiaApi.sk_paragraph_layout(Handle, width);

        internal void Paint(Canvas canvas, float x, float y) =>
            SkiaApi.sk_paragraph_paint(Handle, canvas.Handle, x, y);
    }

    public struct PositionWithAffinity
    {
        public readonly int Position;
        public readonly Affinity Affinity;

        internal PositionWithAffinity(int pos, int affinity)
        {
            Position = pos;
            Affinity = (Affinity)affinity;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TextBox
    {
        public Rect Rect;
        public TextDirection Direction;
    }
}
#endif