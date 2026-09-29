using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // sk_maskfilter_t* sk_maskfilter_new_blur(sk_blurstyle_t, float sigma)

    [LibraryImport(SKIA)]
    internal static partial sk_maskfilter_t sk_maskfilter_new_blur(BlurStyle param0, Single sigma);


    // sk_maskfilter_t* sk_maskfilter_new_blur_with_flags(sk_blurstyle_t, float sigma, bool respectCTM)

    [LibraryImport(SKIA)]
    internal static partial sk_maskfilter_t sk_maskfilter_new_blur_with_flags(BlurStyle param0, Single sigma,
        [MarshalAs(UnmanagedType.I1)] bool respectCTM);


    // sk_maskfilter_t* sk_maskfilter_new_clip(uint8_t min, uint8_t max)

    [LibraryImport(SKIA)]
    internal static partial sk_maskfilter_t sk_maskfilter_new_clip(Byte min, Byte max);


    // sk_maskfilter_t* sk_maskfilter_new_gamma(float gamma)

    [LibraryImport(SKIA)]
    internal static partial sk_maskfilter_t sk_maskfilter_new_gamma(Single gamma);


    // sk_maskfilter_t* sk_maskfilter_new_shader(sk_shader_t* cshader)

    [LibraryImport(SKIA)]
    internal static partial sk_maskfilter_t sk_maskfilter_new_shader(sk_shader_t cshader);


    // sk_maskfilter_t* sk_maskfilter_new_table(const uint8_t[256] table = 256)

    [LibraryImport(SKIA)]
    internal static partial sk_maskfilter_t sk_maskfilter_new_table(Byte* table);


    // void sk_maskfilter_ref(sk_maskfilter_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_maskfilter_ref(sk_maskfilter_t param0);


    // void sk_maskfilter_unref(sk_maskfilter_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_maskfilter_unref(sk_maskfilter_t param0);
}