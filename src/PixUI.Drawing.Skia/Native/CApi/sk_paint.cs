using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // sk_paint_t* sk_paint_clone(sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_paint_t sk_paint_clone(sk_paint_t param0);


    // void sk_paint_delete(sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_delete(sk_paint_t param0);


    // sk_blendmode_t sk_paint_get_blendmode(sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial BlendMode sk_paint_get_blendmode(sk_paint_t param0);


    // sk_color_t sk_paint_get_color(const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_paint_get_color(sk_paint_t param0);


    // void sk_paint_get_color4f(const sk_paint_t* paint, sk_color4f_t* color)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_get_color4f(sk_paint_t paint, SKColorF* color);


    // sk_colorfilter_t* sk_paint_get_colorfilter(sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_colorfilter_t sk_paint_get_colorfilter(sk_paint_t param0);


    // bool sk_paint_get_fill_path(const sk_path_t* src, const sk_paint_t* cpaint, sk_pathbuilder_t* dst)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_paint_get_fill_path(sk_path_t src, sk_paint_t param0,  sk_pathbuilder_t pathBuilder);


    // sk_filter_quality_t sk_paint_get_filter_quality(sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial SKFilterQuality sk_paint_get_filter_quality(sk_paint_t param0);


    // sk_imagefilter_t* sk_paint_get_imagefilter(sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_imagefilter_t sk_paint_get_imagefilter(sk_paint_t param0);


    // sk_maskfilter_t* sk_paint_get_maskfilter(sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_maskfilter_t sk_paint_get_maskfilter(sk_paint_t param0);


    // sk_path_effect_t* sk_paint_get_path_effect(sk_paint_t* cpaint)

    [LibraryImport(SKIA)]
    internal static partial sk_path_effect_t sk_paint_get_path_effect(sk_paint_t cpaint);


    // sk_shader_t* sk_paint_get_shader(sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_paint_get_shader(sk_paint_t param0);


    // sk_stroke_cap_t sk_paint_get_stroke_cap(const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial StrokeCap sk_paint_get_stroke_cap(sk_paint_t param0);


    // sk_stroke_join_t sk_paint_get_stroke_join(const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial StrokeJoin sk_paint_get_stroke_join(sk_paint_t param0);


    // float sk_paint_get_stroke_miter(const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial Single sk_paint_get_stroke_miter(sk_paint_t param0);


    // float sk_paint_get_stroke_width(const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial Single sk_paint_get_stroke_width(sk_paint_t param0);


    // sk_paint_style_t sk_paint_get_style(const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial PaintStyle sk_paint_get_style(sk_paint_t param0);


    // bool sk_paint_is_antialias(const sk_paint_t*)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_paint_is_antialias(sk_paint_t param0);


    // bool sk_paint_is_dither(const sk_paint_t*)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_paint_is_dither(sk_paint_t param0);


    // sk_paint_t* sk_paint_new()

    [LibraryImport(SKIA)]
    internal static partial sk_paint_t sk_paint_new();


    // void sk_paint_reset(sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_reset(sk_paint_t param0);


    // void sk_paint_set_antialias(sk_paint_t*, bool)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_antialias(sk_paint_t param0, [MarshalAs(UnmanagedType.I1)] bool param1);


    // void sk_paint_set_blendmode(sk_paint_t*, sk_blendmode_t)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_blendmode(sk_paint_t param0, BlendMode param1);


    // void sk_paint_set_color(sk_paint_t*, sk_color_t)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_color(sk_paint_t param0, UInt32 param1);


    // void sk_paint_set_color4f(sk_paint_t* paint, sk_color4f_t* color, sk_colorspace_t* colorspace)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_color4f(sk_paint_t paint, SKColorF* color, sk_colorspace_t colorspace);


    // void sk_paint_set_colorfilter(sk_paint_t*, sk_colorfilter_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_colorfilter(sk_paint_t param0, sk_colorfilter_t param1);


    // void sk_paint_set_dither(sk_paint_t*, bool)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_dither(sk_paint_t param0, [MarshalAs(UnmanagedType.I1)] bool param1);


    // void sk_paint_set_filter_quality(sk_paint_t*, sk_filter_quality_t)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_filter_quality(sk_paint_t param0, SKFilterQuality param1);


    // void sk_paint_set_imagefilter(sk_paint_t*, sk_imagefilter_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_imagefilter(sk_paint_t param0, sk_imagefilter_t param1);


    // void sk_paint_set_maskfilter(sk_paint_t*, sk_maskfilter_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_maskfilter(sk_paint_t param0, sk_maskfilter_t param1);


    // void sk_paint_set_path_effect(sk_paint_t* cpaint, sk_path_effect_t* effect)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_path_effect(sk_paint_t cpaint, sk_path_effect_t effect);


    // void sk_paint_set_shader(sk_paint_t*, sk_shader_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_shader(sk_paint_t param0, sk_shader_t param1);


    // void sk_paint_set_stroke_cap(sk_paint_t*, sk_stroke_cap_t)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_stroke_cap(sk_paint_t param0, StrokeCap param1);


    // void sk_paint_set_stroke_join(sk_paint_t*, sk_stroke_join_t)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_stroke_join(sk_paint_t param0, StrokeJoin param1);


    // void sk_paint_set_stroke_miter(sk_paint_t*, float miter)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_stroke_miter(sk_paint_t param0, Single miter);


    // void sk_paint_set_stroke_width(sk_paint_t*, float width)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_stroke_width(sk_paint_t param0, Single width);


    // void sk_paint_set_style(sk_paint_t*, sk_paint_style_t)

    [LibraryImport(SKIA)]
    internal static partial void sk_paint_set_style(sk_paint_t param0, PaintStyle param1);
}