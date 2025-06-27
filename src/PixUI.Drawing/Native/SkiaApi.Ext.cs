#if !__WEB__
using System;
using System.Runtime.InteropServices;

namespace PixUI;

partial class SkiaApi
{
    #region ====Canvas====

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe void sk_canvas_draw_glyph(IntPtr canvas, ushort glyphId,
        Point* pos, Point* origin, IntPtr font, IntPtr paint);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_canvas_draw_shadow(IntPtr canvas, IntPtr path, uint color,
        float elevation, bool transparentOccluder, float devicePixelRatio);

    #endregion

    #region ====FontCollection====

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sk_typeface_font_provider_new();

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_typeface_font_provider_delete(IntPtr provider);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_typeface_font_provider_register_typeface(IntPtr provider,
        IntPtr typeface);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sk_font_collection_new(IntPtr assetFontMgr, bool wasm);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_font_collection_delete(IntPtr fontCollection);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sk_font_collection_get_fallback_manager(IntPtr fontCollection);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sk_font_collection_find_typeface(IntPtr fontCollection, IntPtr familyNameUtf16,
        int len, bool bold, bool italic);

    #endregion

    #region ====TextStyle====

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sk_text_style_new();

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_text_style_delete(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_text_style_set_font_families(IntPtr style, IntPtr names, int len);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern Color sk_text_style_get_color(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_text_style_set_color(IntPtr style, Color color);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern float sk_text_style_get_font_size(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_text_style_set_font_size(IntPtr style, float size);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sk_text_style_get_font_style(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_text_style_set_font_style(IntPtr style, int value);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sk_text_style_get_font_weight(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_text_style_set_font_weight(IntPtr style, int value);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern float sk_text_style_get_height(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_text_style_set_height(IntPtr style, float size);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe void sk_text_style_add_shadow(IntPtr style, Shadow* shadow);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe void sk_text_style_reset_shadows(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sk_text_style_get_text_baseline(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_text_style_set_text_baseline(IntPtr style, int baseline);

    #endregion

    #region ====ParagraphStyle====

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sk_paragraph_style_new();

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_style_delete(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_style_set_text_style(IntPtr style, IntPtr textStyle);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern TextAlign sk_paragraph_style_get_text_align(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void
        sk_paragraph_style_set_text_align(IntPtr style, TextAlign align);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sk_paragraph_style_get_max_lines(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_style_set_max_lines(IntPtr style, int lines);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern float sk_paragraph_style_get_height(IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_style_set_height(IntPtr style, float height);

    #endregion

    #region ====ParagraphBuilder====

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sk_paragraph_builder_new(IntPtr style, IntPtr fontCollection);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_builder_delete(IntPtr builder);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_builder_push_style(IntPtr builder, IntPtr style);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_builder_pop(IntPtr builder);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sk_paragraph_builder_build(IntPtr builder);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe void sk_paragraph_builder_add_utf16_text(IntPtr builder,
        void* text, int size);

    #endregion

    #region ====Paragraph====

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_delete(IntPtr paragraph);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_layout(IntPtr paragraph, float width);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sk_paragraph_paint(IntPtr paragraph, IntPtr canvas, float x,
        float y);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern float sk_paragraph_get_max_width(IntPtr paragraph);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern float sk_paragraph_get_height(IntPtr paragraph);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern float sk_paragraph_get_longest_line(IntPtr paragraph);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern float sk_paragraph_get_max_intrinsic_width(IntPtr paragraph);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern ulong sk_paragraph_get_lines(IntPtr paragraph);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe void sk_paragraph_get_linemetrics_at(IntPtr paragraph, int lineNumber,
        LineMetrics* lineMetrics);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe int sk_paragraph_get_glyph_position(IntPtr paragraph, float x,
        float y, int* affinity);

    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe void sk_paragraph_get_rect_for_position(IntPtr paragraph,
        int pos, int rectHeightStyle, int rectWidthStyle, void* textbox);

    #endregion
}
#endif