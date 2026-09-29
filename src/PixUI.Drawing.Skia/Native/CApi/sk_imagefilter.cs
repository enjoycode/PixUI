using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_imagefilter_croprect_destructor(sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial void sk_imagefilter_croprect_destructor(sk_imagefilter_croprect_t cropRect);


    // uint32_t sk_imagefilter_croprect_get_flags(sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_imagefilter_croprect_get_flags(sk_imagefilter_croprect_t cropRect);


    // void sk_imagefilter_croprect_get_rect(sk_imagefilter_croprect_t* cropRect, sk_rect_t* rect)

    [LibraryImport(SKIA)]
    internal static partial void sk_imagefilter_croprect_get_rect(sk_imagefilter_croprect_t cropRect, Rect* rect);


    // sk_imagefilter_croprect_t* sk_imagefilter_croprect_new()

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_croprect_t sk_imagefilter_croprect_new();


    // sk_imagefilter_croprect_t* sk_imagefilter_croprect_new_with_rect(const sk_rect_t* rect, uint32_t flags)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_croprect_t sk_imagefilter_croprect_new_with_rect(Rect* rect, UInt32 flags);


    // sk_imagefilter_t* sk_imagefilter_new_arithmetic(float k1, float k2, float k3, float k4, bool enforcePMColor, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_arithmetic(Single k1, Single k2, Single k3, Single k4,
        [MarshalAs(UnmanagedType.I1)] bool enforcePMColor, sk_imagefilter_t background, sk_imagefilter_t foreground,
        sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_blur(float sigmaX, float sigmaY, sk_shader_tilemode_t tileMode, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_blur(Single sigmaX, Single sigmaY, TileMode tileMode,
        sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_color_filter(sk_colorfilter_t* cf, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_color_filter(sk_colorfilter_t cf,
        sk_imagefilter_t input,
        sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_compose(sk_imagefilter_t* outer, sk_imagefilter_t* inner)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_compose(sk_imagefilter_t outer, sk_imagefilter_t inner);


    // sk_imagefilter_t* sk_imagefilter_new_dilate(float radiusX, float radiusY, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_dilate(Single radiusX, Single radiusY,
        sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_displacement_map_effect(sk_color_channel_t xChannelSelector, sk_color_channel_t yChannelSelector, float scale, sk_imagefilter_t* displacement, sk_imagefilter_t* color, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_displacement_map_effect(ColorChannel xChannelSelector,
        ColorChannel yChannelSelector, Single scale, sk_imagefilter_t displacement, sk_imagefilter_t color,
        sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_distant_lit_diffuse(const sk_point3_t* direction, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_distant_lit_diffuse(Point3* direction,
        UInt32 lightColor,
        Single surfaceScale, Single kd, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_distant_lit_specular(const sk_point3_t* direction, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_distant_lit_specular(Point3* direction,
        UInt32 lightColor, Single surfaceScale, Single ks, Single shininess, sk_imagefilter_t input,
        sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_drop_shadow(float dx, float dy, float sigmaX, float sigmaY, sk_color_t color, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_drop_shadow(Single dx, Single dy, Single sigmaX,
        Single sigmaY, UInt32 color, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_drop_shadow_only(float dx, float dy, float sigmaX, float sigmaY, sk_color_t color, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_drop_shadow_only(Single dx, Single dy, Single sigmaX,
        Single sigmaY, UInt32 color, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_erode(float radiusX, float radiusY, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_erode(Single radiusX, Single radiusY,
        sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_image_source(sk_image_t* image, const sk_rect_t* srcRect, const sk_rect_t* dstRect, sk_filter_quality_t filterQuality)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_image_source(sk_image_t image, Rect* srcRect,
        Rect* dstRect, SKFilterQuality filterQuality);


    // sk_imagefilter_t* sk_imagefilter_new_image_source_default(sk_image_t* image)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_image_source_default(sk_image_t image);


    // sk_imagefilter_t* sk_imagefilter_new_magnifier(const sk_rect_t* src, float inset, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_magnifier(Rect* src, Single inset,
        sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_matrix(const sk_matrix_t* matrix, sk_filter_quality_t quality, sk_imagefilter_t* input)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_matrix(Matrix3* matrix, SKFilterQuality quality,
        sk_imagefilter_t input);


    // sk_imagefilter_t* sk_imagefilter_new_matrix_convolution(const sk_isize_t* kernelSize, const float[-1] kernel, float gain, float bias, const sk_ipoint_t* kernelOffset, sk_shader_tilemode_t tileMode, bool convolveAlpha, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_matrix_convolution(SizeI* kernelSize, Single* kernel,
        Single gain, Single bias, PointI* kernelOffset, TileMode tileMode,
        [MarshalAs(UnmanagedType.I1)] bool convolveAlpha, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_merge(sk_imagefilter_t*[-1] filters, int count, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_merge(sk_imagefilter_t* filters, Int32 count,
        sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_offset(float dx, float dy, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_offset(Single dx, Single dy, sk_imagefilter_t input,
        sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_paint(const sk_paint_t* paint, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_paint(sk_paint_t paint,
        sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_picture(sk_picture_t* picture)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_picture(sk_picture_t picture);


    // sk_imagefilter_t* sk_imagefilter_new_picture_with_croprect(sk_picture_t* picture, const sk_rect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_picture_with_croprect(sk_picture_t picture,
        Rect* cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_point_lit_diffuse(const sk_point3_t* location, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_point_lit_diffuse(Point3* location, UInt32 lightColor,
        Single surfaceScale, Single kd, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_point_lit_specular(const sk_point3_t* location, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_point_lit_specular(Point3* location, UInt32 lightColor,
        Single surfaceScale, Single ks, Single shininess, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_spot_lit_diffuse(const sk_point3_t* location, const sk_point3_t* target, float specularExponent, float cutoffAngle, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_spot_lit_diffuse(Point3* location, Point3* target,
        Single specularExponent, Single cutoffAngle, UInt32 lightColor, Single surfaceScale, Single kd,
        sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_spot_lit_specular(const sk_point3_t* location, const sk_point3_t* target, float specularExponent, float cutoffAngle, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_spot_lit_specular(Point3* location, Point3* target,
        Single specularExponent, Single cutoffAngle, UInt32 lightColor, Single surfaceScale, Single ks,
        Single shininess, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);


    // sk_imagefilter_t* sk_imagefilter_new_tile(const sk_rect_t* src, const sk_rect_t* dst, sk_imagefilter_t* input)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_tile(Rect* src, Rect* dst, sk_imagefilter_t input);


    // sk_imagefilter_t* sk_imagefilter_new_xfermode(sk_blendmode_t mode, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_imagefilter_croprect_t* cropRect)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_imagefilter_new_xfermode(BlendMode mode, sk_imagefilter_t background,
        sk_imagefilter_t foreground, sk_imagefilter_croprect_t cropRect);


    // void sk_imagefilter_unref(sk_imagefilter_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_imagefilter_unref(sk_imagefilter_t param0);
}