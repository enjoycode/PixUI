using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // int sk_colortable_count(const sk_colortable_t* ctable)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_colortable_count(sk_colortable_t ctable);


    // sk_colortable_t* sk_colortable_new(const sk_pmcolor_t* colors, int count)

    [LibraryImport(SKIA)]
    internal static partial sk_colortable_t sk_colortable_new(UInt32* colors, Int32 count);


    // void sk_colortable_read_colors(const sk_colortable_t* ctable, sk_pmcolor_t** colors)

    [LibraryImport(SKIA)]
    internal static partial void sk_colortable_read_colors(sk_colortable_t ctable, UInt32** colors);


    // void sk_colortable_unref(sk_colortable_t* ctable)

    [LibraryImport(SKIA)]
    internal static partial void sk_colortable_unref(sk_colortable_t ctable);
}