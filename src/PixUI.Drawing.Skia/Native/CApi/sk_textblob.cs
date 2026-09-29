using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_textblob_builder_alloc_run(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float x, float y, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_builder_alloc_run(sk_textblob_builder_t builder, sk_font_t font,
        Int32 count, Single x, Single y, Rect* bounds, SKRunBufferInternal* runbuffer);


    // void sk_textblob_builder_alloc_run_pos(sk_textblob_builder_t* builder, const sk_font_t* font, int count, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_builder_alloc_run_pos(sk_textblob_builder_t builder, sk_font_t font,
        Int32 count, Rect* bounds, SKRunBufferInternal* runbuffer);


    // void sk_textblob_builder_alloc_run_pos_h(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float y, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_builder_alloc_run_pos_h(sk_textblob_builder_t builder, sk_font_t font,
        Int32 count, Single y, Rect* bounds, SKRunBufferInternal* runbuffer);


    // void sk_textblob_builder_alloc_run_rsxform(sk_textblob_builder_t* builder, const sk_font_t* font, int count, sk_textblob_builder_runbuffer_t* runbuffer)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_builder_alloc_run_rsxform(sk_textblob_builder_t builder, sk_font_t font,
        Int32 count, SKRunBufferInternal* runbuffer);


    // void sk_textblob_builder_alloc_run_text(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float x, float y, int textByteCount, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_builder_alloc_run_text(sk_textblob_builder_t builder, sk_font_t font,
        Int32 count, Single x, Single y, Int32 textByteCount, Rect* bounds, SKRunBufferInternal* runbuffer);


    // void sk_textblob_builder_alloc_run_text_pos(sk_textblob_builder_t* builder, const sk_font_t* font, int count, int textByteCount, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_builder_alloc_run_text_pos(sk_textblob_builder_t builder, sk_font_t font,
        Int32 count, Int32 textByteCount, Rect* bounds, SKRunBufferInternal* runbuffer);


    // void sk_textblob_builder_alloc_run_text_pos_h(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float y, int textByteCount, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_builder_alloc_run_text_pos_h(sk_textblob_builder_t builder, sk_font_t font,
        Int32 count, Single y, Int32 textByteCount, Rect* bounds, SKRunBufferInternal* runbuffer);


    // void sk_textblob_builder_delete(sk_textblob_builder_t* builder)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_builder_delete(sk_textblob_builder_t builder);


    // sk_textblob_t* sk_textblob_builder_make(sk_textblob_builder_t* builder)

    [LibraryImport(SKIA)]
    internal static partial sk_textblob_t sk_textblob_builder_make(sk_textblob_builder_t builder);


    // sk_textblob_builder_t* sk_textblob_builder_new()

    [LibraryImport(SKIA)]
    internal static partial sk_textblob_builder_t sk_textblob_builder_new();


    // void sk_textblob_get_bounds(const sk_textblob_t* blob, sk_rect_t* bounds)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_get_bounds(sk_textblob_t blob, Rect* bounds);


    // int sk_textblob_get_intercepts(const sk_textblob_t* blob, const float[2] bounds = 2, float[-1] intervals, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_textblob_get_intercepts(sk_textblob_t blob, Single* bounds, Single* intervals,
        sk_paint_t paint);


    // uint32_t sk_textblob_get_unique_id(const sk_textblob_t* blob)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_textblob_get_unique_id(sk_textblob_t blob);


    // void sk_textblob_ref(const sk_textblob_t* blob)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_ref(sk_textblob_t blob);


    // void sk_textblob_unref(const sk_textblob_t* blob)

    [LibraryImport(SKIA)]
    internal static partial void sk_textblob_unref(sk_textblob_t blob);
}