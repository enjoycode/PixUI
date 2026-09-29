using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // sk_shader_t* sk_shader_new_blend(sk_blendmode_t mode, const sk_shader_t* dst, const sk_shader_t* src)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_blend(BlendMode mode, sk_shader_t dst, sk_shader_t src);


    // sk_shader_t* sk_shader_new_color(sk_color_t color)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_color(UInt32 color);


    // sk_shader_t* sk_shader_new_color4f(const sk_color4f_t* color, const sk_colorspace_t* colorspace)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_color4f(SKColorF* color, sk_colorspace_t colorspace);


    // sk_shader_t* sk_shader_new_empty()

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_empty();


    // sk_shader_t* sk_shader_new_lerp(float t, const sk_shader_t* dst, const sk_shader_t* src)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_lerp(Single t, sk_shader_t dst, sk_shader_t src);


    // sk_shader_t* sk_shader_new_linear_gradient(const sk_point_t[2] points = 2, const sk_color_t[-1] colors, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_linear_gradient(Point* points, UInt32* colors, Single* colorPos,
        Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);


    // sk_shader_t* sk_shader_new_linear_gradient_color4f(const sk_point_t[2] points = 2, const sk_color4f_t* colors, const sk_colorspace_t* colorspace, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_linear_gradient_color4f(Point* points, SKColorF* colors,
        sk_colorspace_t colorspace, Single* colorPos, Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);


    // sk_shader_t* sk_shader_new_perlin_noise_improved_noise(float baseFrequencyX, float baseFrequencyY, int numOctaves, float z)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_perlin_noise_improved_noise(Single baseFrequencyX,
        Single baseFrequencyY, Int32 numOctaves, Single z);


    // sk_shader_t* sk_shader_new_radial_gradient(const sk_point_t* center, float radius, const sk_color_t[-1] colors, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_radial_gradient(Point* center, Single radius, UInt32* colors,
        Single* colorPos, Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);


    // sk_shader_t* sk_shader_new_radial_gradient_color4f(const sk_point_t* center, float radius, const sk_color4f_t* colors, const sk_colorspace_t* colorspace, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_radial_gradient_color4f(Point* center, Single radius,
        SKColorF* colors, sk_colorspace_t colorspace, Single* colorPos, Int32 colorCount, TileMode tileMode,
        Matrix3* localMatrix);


    // sk_shader_t* sk_shader_new_sweep_gradient(const sk_point_t* center, const sk_color_t[-1] colors, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, float startAngle, float endAngle, const sk_matrix_t* localMatrix)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_sweep_gradient(Point* center, UInt32* colors, Single* colorPos,
        Int32 colorCount, TileMode tileMode, Single startAngle, Single endAngle, Matrix3* localMatrix);


    // sk_shader_t* sk_shader_new_sweep_gradient_color4f(const sk_point_t* center, const sk_color4f_t* colors, const sk_colorspace_t* colorspace, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, float startAngle, float endAngle, const sk_matrix_t* localMatrix)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_sweep_gradient_color4f(Point* center, SKColorF* colors,
        sk_colorspace_t colorspace, Single* colorPos, Int32 colorCount, TileMode tileMode, Single startAngle,
        Single endAngle, Matrix3* localMatrix);


    // sk_shader_t* sk_shader_new_two_point_conical_gradient(const sk_point_t* start, float startRadius, const sk_point_t* end, float endRadius, const sk_color_t[-1] colors, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_two_point_conical_gradient(Point* start, Single startRadius,
        Point* end, Single endRadius, UInt32* colors, Single* colorPos, Int32 colorCount, TileMode tileMode,
        Matrix3* localMatrix);


    // sk_shader_t* sk_shader_new_two_point_conical_gradient_color4f(const sk_point_t* start, float startRadius, const sk_point_t* end, float endRadius, const sk_color4f_t* colors, const sk_colorspace_t* colorspace, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_new_two_point_conical_gradient_color4f(Point* start,
        Single startRadius, Point* end, Single endRadius, SKColorF* colors, sk_colorspace_t colorspace,
        Single* colorPos, Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);


    // void sk_shader_ref(sk_shader_t* shader)

    [LibraryImport(SKIA)]
    internal static partial void sk_shader_ref(sk_shader_t shader);


    // void sk_shader_unref(sk_shader_t* shader)

    [LibraryImport(SKIA)]
    internal static partial void sk_shader_unref(sk_shader_t shader);


    // sk_shader_t* sk_shader_with_color_filter(const sk_shader_t* shader, const sk_colorfilter_t* filter)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_with_color_filter(sk_shader_t shader, sk_colorfilter_t filter);


    // sk_shader_t* sk_shader_with_local_matrix(const sk_shader_t* shader, const sk_matrix_t* localMatrix)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_shader_with_local_matrix(sk_shader_t shader, Matrix3* localMatrix);
}