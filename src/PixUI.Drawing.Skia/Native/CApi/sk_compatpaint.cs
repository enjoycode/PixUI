using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

partial class SkiaApi
{
    // sk_compatpaint_t* sk_compatpaint_clone(const sk_compatpaint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial sk_compatpaint_t sk_compatpaint_clone(sk_compatpaint_t paint);


    // void sk_compatpaint_delete(sk_compatpaint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_compatpaint_delete(sk_compatpaint_t paint);


    // sk_font_t* sk_compatpaint_get_font(sk_compatpaint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial sk_font_t sk_compatpaint_get_font(sk_compatpaint_t paint);


    // sk_text_align_t sk_compatpaint_get_text_align(const sk_compatpaint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial TextAlign sk_compatpaint_get_text_align(sk_compatpaint_t paint);


    // sk_text_encoding_t sk_compatpaint_get_text_encoding(const sk_compatpaint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial SKTextEncoding sk_compatpaint_get_text_encoding(sk_compatpaint_t paint);


    // sk_font_t* sk_compatpaint_make_font(sk_compatpaint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial sk_font_t sk_compatpaint_make_font(sk_compatpaint_t paint);


    // sk_compatpaint_t* sk_compatpaint_new()

    [LibraryImport(SKIA)]
    internal static partial sk_compatpaint_t sk_compatpaint_new();


    // sk_compatpaint_t* sk_compatpaint_new_with_font(const sk_font_t* font)

    [LibraryImport(SKIA)]
    internal static partial sk_compatpaint_t sk_compatpaint_new_with_font(sk_font_t font);


    // void sk_compatpaint_reset(sk_compatpaint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_compatpaint_reset(sk_compatpaint_t paint);


    // void sk_compatpaint_set_text_align(sk_compatpaint_t* paint, sk_text_align_t align)

    [LibraryImport(SKIA)]
    internal static partial void sk_compatpaint_set_text_align(sk_compatpaint_t paint, TextAlign align);


    // void sk_compatpaint_set_text_encoding(sk_compatpaint_t* paint, sk_text_encoding_t encoding)

    [LibraryImport(SKIA)]
    internal static partial void sk_compatpaint_set_text_encoding(sk_compatpaint_t paint, SKTextEncoding encoding);
}