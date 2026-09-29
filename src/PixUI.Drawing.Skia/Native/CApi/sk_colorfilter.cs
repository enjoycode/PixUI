using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // sk_colorfilter_t* sk_colorfilter_new_color_matrix(const float[20] array = 20)

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_colorfilter_new_color_matrix(Single* array);


    // sk_colorfilter_t* sk_colorfilter_new_compose(sk_colorfilter_t* outer, sk_colorfilter_t* inner)

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_colorfilter_new_compose(sk_colorfilter_t outer, sk_colorfilter_t inner);


    // sk_colorfilter_t* sk_colorfilter_new_high_contrast(const sk_highcontrastconfig_t* config)

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_colorfilter_new_high_contrast(SKHighContrastConfig* config);


    // sk_colorfilter_t* sk_colorfilter_new_lighting(sk_color_t mul, sk_color_t add)

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_colorfilter_new_lighting(UInt32 mul, UInt32 add);


    // sk_colorfilter_t* sk_colorfilter_new_luma_color()

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_colorfilter_new_luma_color();


    // sk_colorfilter_t* sk_colorfilter_new_mode(sk_color_t c, sk_blendmode_t mode)

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_colorfilter_new_mode(UInt32 c, BlendMode mode);


    // sk_colorfilter_t* sk_colorfilter_new_table(const uint8_t[256] table = 256)

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_colorfilter_new_table(Byte* table);


    // sk_colorfilter_t* sk_colorfilter_new_table_argb(const uint8_t[256] tableA = 256, const uint8_t[256] tableR = 256, const uint8_t[256] tableG = 256, const uint8_t[256] tableB = 256)

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_colorfilter_new_table_argb(Byte* tableA, Byte* tableR, Byte* tableG,
        Byte* tableB);


    // void sk_colorfilter_unref(sk_colorfilter_t* filter)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorfilter_unref(sk_colorfilter_t filter);
}