using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    #region ====Canvas====

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_glyph(IntPtr canvas, ushort glyphId,
        Point* pos, Point* origin, IntPtr font, IntPtr paint);

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_shadow(IntPtr canvas, IntPtr path, uint color,
        float elevation, [MarshalAs(UnmanagedType.I1)] bool transparentOccluder, float devicePixelRatio);

    #endregion

    #region ====FontCollection====

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_typeface_font_provider_new();

    [LibraryImport(SKIA)]
    internal static partial void sk_typeface_font_provider_delete(IntPtr provider);

    [LibraryImport(SKIA)]
    internal static partial void sk_typeface_font_provider_register_typeface(IntPtr provider,
        IntPtr typeface);

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_font_collection_new(IntPtr assetFontMgr, [MarshalAs(UnmanagedType.I1)] bool wasm);

    [LibraryImport(SKIA)]
    internal static partial void sk_font_collection_delete(IntPtr fontCollection);

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_font_collection_get_fallback_manager(IntPtr fontCollection);

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_font_collection_find_typeface(IntPtr fontCollection, IntPtr familyNameUtf16,
        int len, [MarshalAs(UnmanagedType.I1)] bool bold, [MarshalAs(UnmanagedType.I1)] bool italic);

    #endregion

    #region ====TextStyle====

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_text_style_new();

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_delete(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_set_font_families(IntPtr style, IntPtr names, int len);

    [LibraryImport(SKIA)]
    internal static partial uint sk_text_style_get_color(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_set_color(IntPtr style, uint color);

    [LibraryImport(SKIA)]
    internal static partial float sk_text_style_get_font_size(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_set_font_size(IntPtr style, float size);

    [LibraryImport(SKIA)]
    internal static partial int sk_text_style_get_font_style(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_set_font_style(IntPtr style, int value);

    [LibraryImport(SKIA)]
    internal static partial int sk_text_style_get_font_weight(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_set_font_weight(IntPtr style, int value);

    [LibraryImport(SKIA)]
    internal static partial float sk_text_style_get_height(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_set_height(IntPtr style, float size);

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_add_shadow(IntPtr style, Shadow* shadow);

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_reset_shadows(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial int sk_text_style_get_text_baseline(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_text_style_set_text_baseline(IntPtr style, int baseline);

    #endregion

    #region ====ParagraphStyle====

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_paragraph_style_new();

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_style_delete(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_style_set_text_style(IntPtr style, IntPtr textStyle);

    [LibraryImport(SKIA)]
    internal static partial TextAlign sk_paragraph_style_get_text_align(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_style_set_text_align(IntPtr style, TextAlign align);

    [LibraryImport(SKIA)]
    internal static partial int sk_paragraph_style_get_max_lines(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_style_set_max_lines(IntPtr style, int lines);

    [LibraryImport(SKIA)]
    internal static partial float sk_paragraph_style_get_height(IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_style_set_height(IntPtr style, float height);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_style_set_ellipsis(IntPtr style, IntPtr ellipsis);

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_paragraph_style_get_ellipsis(IntPtr style);

    #endregion

    #region ====ParagraphBuilder====

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_paragraph_builder_new(IntPtr style, IntPtr fontCollection);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_builder_delete(IntPtr builder);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_builder_push_style(IntPtr builder, IntPtr style);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_builder_pop(IntPtr builder);

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_paragraph_builder_build(IntPtr builder);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_builder_add_utf16_text(IntPtr builder, void* text, int size);

    #endregion

    #region ====Paragraph====

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_delete(IntPtr paragraph);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_layout(IntPtr paragraph, float width);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_paint(IntPtr paragraph, IntPtr canvas, float x,
        float y);

    [LibraryImport(SKIA)]
    internal static partial float sk_paragraph_get_max_width(IntPtr paragraph);

    [LibraryImport(SKIA)]
    internal static partial float sk_paragraph_get_height(IntPtr paragraph);

    [LibraryImport(SKIA)]
    internal static partial float sk_paragraph_get_longest_line(IntPtr paragraph);

    [LibraryImport(SKIA)]
    internal static partial float sk_paragraph_get_max_intrinsic_width(IntPtr paragraph);

    [LibraryImport(SKIA)]
    internal static partial ulong sk_paragraph_get_lines(IntPtr paragraph);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_get_linemetrics_at(IntPtr paragraph, int lineNumber,
        LineMetrics* lineMetrics);

    [LibraryImport(SKIA)]
    internal static partial int sk_paragraph_get_glyph_position(IntPtr paragraph, float x,
        float y, int* affinity);

    [LibraryImport(SKIA)]
    internal static partial void sk_paragraph_get_rect_for_position(IntPtr paragraph,
        int pos, int rectHeightStyle, int rectWidthStyle, void* textbox);

    [LibraryImport(SKIA)]
    internal static partial IntPtr sk_paragraph_get_line_first_textblob(IntPtr paragraph, int lineNum);

    #endregion
}