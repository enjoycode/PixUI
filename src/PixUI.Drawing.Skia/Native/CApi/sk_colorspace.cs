using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_color4f_from_color(sk_color_t color, sk_color4f_t* color4f)

    [LibraryImport(SKIA)]
    internal static partial void sk_color4f_from_color(UInt32 color, SKColorF* color4f);


    // sk_color_t sk_color4f_to_color(const sk_color4f_t* color4f)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_color4f_to_color(SKColorF* color4f);


    // bool sk_colorspace_equals(const sk_colorspace_t* src, const sk_colorspace_t* dst)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_equals(sk_colorspace_t src, sk_colorspace_t dst);


    // bool sk_colorspace_gamma_close_to_srgb(const sk_colorspace_t* colorspace)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_gamma_close_to_srgb(sk_colorspace_t colorspace);


    // bool sk_colorspace_gamma_is_linear(const sk_colorspace_t* colorspace)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_gamma_is_linear(sk_colorspace_t colorspace);


    // void sk_colorspace_icc_profile_delete(sk_colorspace_icc_profile_t* profile)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_icc_profile_delete(sk_colorspace_icc_profile_t profile);


    // const uint8_t* sk_colorspace_icc_profile_get_buffer(const sk_colorspace_icc_profile_t* profile, uint32_t* size)

    [LibraryImport(SKIA)]
    internal static partial Byte*
        sk_colorspace_icc_profile_get_buffer(sk_colorspace_icc_profile_t profile, UInt32* size);


    // bool sk_colorspace_icc_profile_get_to_xyzd50(const sk_colorspace_icc_profile_t* profile, sk_colorspace_xyz_t* toXYZD50)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_icc_profile_get_to_xyzd50(sk_colorspace_icc_profile_t profile,
        SKColorSpaceXyz* toXYZD50);


    // sk_colorspace_icc_profile_t* sk_colorspace_icc_profile_new()

    [LibraryImport(SKIA)]
    internal static partial sk_colorspace_icc_profile_t sk_colorspace_icc_profile_new();


    // bool sk_colorspace_icc_profile_parse(const void* buffer, size_t length, sk_colorspace_icc_profile_t* profile)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_icc_profile_parse(void* buffer, /* size_t */ IntPtr length,
        sk_colorspace_icc_profile_t profile);


    // bool sk_colorspace_is_numerical_transfer_fn(const sk_colorspace_t* colorspace, sk_colorspace_transfer_fn_t* transferFn)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_is_numerical_transfer_fn(sk_colorspace_t colorspace,
        SKColorSpaceTransferFn* transferFn);


    // bool sk_colorspace_is_srgb(const sk_colorspace_t* colorspace)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_is_srgb(sk_colorspace_t colorspace);


    // sk_colorspace_t* sk_colorspace_make_linear_gamma(const sk_colorspace_t* colorspace)

    [LibraryImport(SKIA)]
    internal static partial sk_colorspace_t sk_colorspace_make_linear_gamma(sk_colorspace_t colorspace);


    // sk_colorspace_t* sk_colorspace_make_srgb_gamma(const sk_colorspace_t* colorspace)

    [LibraryImport(SKIA)]
    internal static partial sk_colorspace_t sk_colorspace_make_srgb_gamma(sk_colorspace_t colorspace);


    // sk_colorspace_t* sk_colorspace_new_icc(const sk_colorspace_icc_profile_t* profile)

    [LibraryImport(SKIA)]
    internal static partial sk_colorspace_t sk_colorspace_new_icc(sk_colorspace_icc_profile_t profile);


    // sk_colorspace_t* sk_colorspace_new_rgb(const sk_colorspace_transfer_fn_t* transferFn, const sk_colorspace_xyz_t* toXYZD50)

    [LibraryImport(SKIA)]
    internal static partial sk_colorspace_t sk_colorspace_new_rgb(SKColorSpaceTransferFn* transferFn,
        SKColorSpaceXyz* toXYZD50);


    // sk_colorspace_t* sk_colorspace_new_srgb()

    [LibraryImport(SKIA)]
    internal static partial sk_colorspace_t sk_colorspace_new_srgb();


    // sk_colorspace_t* sk_colorspace_new_srgb_linear()

    [LibraryImport(SKIA)]
    internal static partial sk_colorspace_t sk_colorspace_new_srgb_linear();


    // bool sk_colorspace_primaries_to_xyzd50(const sk_colorspace_primaries_t* primaries, sk_colorspace_xyz_t* toXYZD50)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_primaries_to_xyzd50(SKColorSpacePrimaries* primaries,
        SKColorSpaceXyz* toXYZD50);


    // void sk_colorspace_ref(sk_colorspace_t* colorspace)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_ref(sk_colorspace_t colorspace);


    // void sk_colorspace_to_profile(const sk_colorspace_t* colorspace, sk_colorspace_icc_profile_t* profile)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_to_profile(sk_colorspace_t colorspace,
        sk_colorspace_icc_profile_t profile);


    // bool sk_colorspace_to_xyzd50(const sk_colorspace_t* colorspace, sk_colorspace_xyz_t* toXYZD50)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_to_xyzd50(sk_colorspace_t colorspace, SKColorSpaceXyz* toXYZD50);


    // float sk_colorspace_transfer_fn_eval(const sk_colorspace_transfer_fn_t* transferFn, float x)

    [LibraryImport(SKIA)]
    internal static partial Single sk_colorspace_transfer_fn_eval(SKColorSpaceTransferFn* transferFn, Single x);


    // bool sk_colorspace_transfer_fn_invert(const sk_colorspace_transfer_fn_t* src, sk_colorspace_transfer_fn_t* dst)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_transfer_fn_invert(SKColorSpaceTransferFn* src,
        SKColorSpaceTransferFn* dst);


    // void sk_colorspace_transfer_fn_named_2dot2(sk_colorspace_transfer_fn_t* transferFn)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_transfer_fn_named_2dot2(SKColorSpaceTransferFn* transferFn);


    // void sk_colorspace_transfer_fn_named_hlg(sk_colorspace_transfer_fn_t* transferFn)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_transfer_fn_named_hlg(SKColorSpaceTransferFn* transferFn);


    // void sk_colorspace_transfer_fn_named_linear(sk_colorspace_transfer_fn_t* transferFn)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_transfer_fn_named_linear(SKColorSpaceTransferFn* transferFn);


    // void sk_colorspace_transfer_fn_named_pq(sk_colorspace_transfer_fn_t* transferFn)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_transfer_fn_named_pq(SKColorSpaceTransferFn* transferFn);


    // void sk_colorspace_transfer_fn_named_rec2020(sk_colorspace_transfer_fn_t* transferFn)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_transfer_fn_named_rec2020(SKColorSpaceTransferFn* transferFn);


    // void sk_colorspace_transfer_fn_named_srgb(sk_colorspace_transfer_fn_t* transferFn)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_transfer_fn_named_srgb(SKColorSpaceTransferFn* transferFn);


    // void sk_colorspace_unref(sk_colorspace_t* colorspace)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_unref(sk_colorspace_t colorspace);


    // void sk_colorspace_xyz_concat(const sk_colorspace_xyz_t* a, const sk_colorspace_xyz_t* b, sk_colorspace_xyz_t* result)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_xyz_concat(SKColorSpaceXyz* a, SKColorSpaceXyz* b,
        SKColorSpaceXyz* result);


    // bool sk_colorspace_xyz_invert(const sk_colorspace_xyz_t* src, sk_colorspace_xyz_t* dst)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_colorspace_xyz_invert(SKColorSpaceXyz* src, SKColorSpaceXyz* dst);


    // void sk_colorspace_xyz_named_adobe_rgb(sk_colorspace_xyz_t* xyz)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_xyz_named_adobe_rgb(SKColorSpaceXyz* xyz);


    // void sk_colorspace_xyz_named_display_p3(sk_colorspace_xyz_t* xyz)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_xyz_named_display_p3(SKColorSpaceXyz* xyz);


    // void sk_colorspace_xyz_named_rec2020(sk_colorspace_xyz_t* xyz)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_xyz_named_rec2020(SKColorSpaceXyz* xyz);


    // void sk_colorspace_xyz_named_srgb(sk_colorspace_xyz_t* xyz)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_xyz_named_srgb(SKColorSpaceXyz* xyz);


    // void sk_colorspace_xyz_named_xyz(sk_colorspace_xyz_t* xyz)

    [LibraryImport(SKIA)]
    internal static partial void sk_colorspace_xyz_named_xyz(SKColorSpaceXyz* xyz);
}