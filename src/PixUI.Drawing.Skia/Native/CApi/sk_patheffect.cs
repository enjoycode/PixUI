using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // sk_path_effect_t* sk_path_effect_create_1d_path(const sk_path_t* path, float advance, float phase, sk_path_effect_1d_style_t style)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_path_effect_create_1d_path(sk_path_t path, Single advance, Single phase,
        Path1DPathEffectStyle style);


    // sk_path_effect_t* sk_path_effect_create_2d_line(float width, const sk_matrix_t* matrix)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_path_effect_create_2d_line(Single width, Matrix3* matrix);


    // sk_path_effect_t* sk_path_effect_create_2d_path(const sk_matrix_t* matrix, const sk_path_t* path)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_path_effect_create_2d_path(Matrix3* matrix, sk_path_t path);


    // sk_path_effect_t* sk_path_effect_create_compose(sk_path_effect_t* outer, sk_path_effect_t* inner)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_path_effect_create_compose(sk_path_effect_t outer,
        sk_path_effect_t inner);


    // sk_path_effect_t* sk_path_effect_create_corner(float radius)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_path_effect_create_corner(Single radius);


    // sk_path_effect_t* sk_path_effect_create_dash(const float[-1] intervals, int count, float phase)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_path_effect_create_dash(Single* intervals, Int32 count, Single phase);


    // sk_path_effect_t* sk_path_effect_create_discrete(float segLength, float deviation, uint32_t seedAssist)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_path_effect_create_discrete(Single segLength, Single deviation,
        UInt32 seedAssist);


    // sk_path_effect_t* sk_path_effect_create_sum(sk_path_effect_t* first, sk_path_effect_t* second)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_path_effect_create_sum(sk_path_effect_t first, sk_path_effect_t second);


    // sk_path_effect_t* sk_path_effect_create_trim(float start, float stop, sk_path_effect_trim_mode_t mode)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_path_effect_create_trim(Single start, Single stop,
        SKTrimPathEffectMode mode);


    // void sk_path_effect_unref(sk_path_effect_t* t)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_effect_unref(sk_path_effect_t t);
}