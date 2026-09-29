using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_color_get_bit_shift(int* a, int* r, int* g, int* b)

    [LibraryImport(SKIA)]
    internal static partial void sk_color_get_bit_shift(Int32* a, Int32* r, Int32* g, Int32* b);


    // sk_pmcolor_t sk_color_premultiply(const sk_color_t color)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_color_premultiply(UInt32 color);


    // void sk_color_premultiply_array(const sk_color_t* colors, int size, sk_pmcolor_t* pmcolors)

    [LibraryImport(SKIA)]
    internal static partial void sk_color_premultiply_array(UInt32* colors, Int32 size, UInt32* pmcolors);


    // sk_color_t sk_color_unpremultiply(const sk_pmcolor_t pmcolor)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_color_unpremultiply(UInt32 pmcolor);


    // void sk_color_unpremultiply_array(const sk_pmcolor_t* pmcolors, int size, sk_color_t* colors)

    [LibraryImport(SKIA)]
    internal static partial void sk_color_unpremultiply_array(UInt32* pmcolors, Int32 size, UInt32* colors);


    // bool sk_jpegencoder_encode(sk_wstream_t* dst, const sk_pixmap_t* src, const sk_jpegencoder_options_t* options)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool
        sk_jpegencoder_encode(sk_wstream_t dst, sk_pixmap_t src, SKJpegEncoderOptions* options);


    // void sk_pixmap_destructor(sk_pixmap_t* cpixmap)

    [LibraryImport(SKIA)]
    internal static partial void sk_pixmap_destructor(sk_pixmap_t cpixmap);


    // bool sk_pixmap_erase_color(const sk_pixmap_t* cpixmap, sk_color_t color, const sk_irect_t* subset)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pixmap_erase_color(sk_pixmap_t cpixmap, UInt32 color, RectI* subset);


    // bool sk_pixmap_erase_color4f(const sk_pixmap_t* cpixmap, const sk_color4f_t* color, sk_colorspace_t* colorspace, const sk_irect_t* subset)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pixmap_erase_color4f(sk_pixmap_t cpixmap, SKColorF* color,
        sk_colorspace_t colorspace, RectI* subset);


    // bool sk_pixmap_extract_subset(const sk_pixmap_t* cpixmap, sk_pixmap_t* result, const sk_irect_t* subset)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pixmap_extract_subset(sk_pixmap_t cpixmap, sk_pixmap_t result, RectI* subset);


    // void sk_pixmap_get_info(const sk_pixmap_t* cpixmap, sk_imageinfo_t* cinfo)

    [LibraryImport(SKIA)]
    internal static partial void sk_pixmap_get_info(sk_pixmap_t cpixmap, SKImageInfoNative* cinfo);


    // sk_color_t sk_pixmap_get_pixel_color(const sk_pixmap_t* cpixmap, int x, int y)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_pixmap_get_pixel_color(sk_pixmap_t cpixmap, Int32 x, Int32 y);


    // const void* sk_pixmap_get_pixels(const sk_pixmap_t* cpixmap)

    [LibraryImport(SKIA)]
    internal static partial void* sk_pixmap_get_pixels(sk_pixmap_t cpixmap);


    // const void* sk_pixmap_get_pixels_with_xy(const sk_pixmap_t* cpixmap, int x, int y)

    [LibraryImport(SKIA)]
    internal static partial void* sk_pixmap_get_pixels_with_xy(sk_pixmap_t cpixmap, Int32 x, Int32 y);


    // size_t sk_pixmap_get_row_bytes(const sk_pixmap_t* cpixmap)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_pixmap_get_row_bytes(sk_pixmap_t cpixmap);


    // void* sk_pixmap_get_writable_addr(const sk_pixmap_t* cpixmap)

    [LibraryImport(SKIA)]
    internal static partial void* sk_pixmap_get_writable_addr(sk_pixmap_t cpixmap);


    // sk_pixmap_t* sk_pixmap_new()

    [LibraryImport(SKIA)]
    internal static partial sk_pixmap_t sk_pixmap_new();


    // sk_pixmap_t* sk_pixmap_new_with_params(const sk_imageinfo_t* cinfo, const void* addr, size_t rowBytes)

    [LibraryImport(SKIA)]
    internal static partial sk_pixmap_t sk_pixmap_new_with_params(SKImageInfoNative* cinfo, void* addr, /* size_t */
        IntPtr rowBytes);


    // bool sk_pixmap_read_pixels(const sk_pixmap_t* cpixmap, const sk_imageinfo_t* dstInfo, void* dstPixels, size_t dstRowBytes, int srcX, int srcY)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pixmap_read_pixels(sk_pixmap_t cpixmap, SKImageInfoNative* dstInfo,
        void* dstPixels, /* size_t */ IntPtr dstRowBytes, Int32 srcX, Int32 srcY);


    // void sk_pixmap_reset(sk_pixmap_t* cpixmap)

    [LibraryImport(SKIA)]
    internal static partial void sk_pixmap_reset(sk_pixmap_t cpixmap);


    // void sk_pixmap_reset_with_params(sk_pixmap_t* cpixmap, const sk_imageinfo_t* cinfo, const void* addr, size_t rowBytes)

    [LibraryImport(SKIA)]
    internal static partial void sk_pixmap_reset_with_params(sk_pixmap_t cpixmap, SKImageInfoNative* cinfo,
        void* addr, /* size_t */ IntPtr rowBytes);


    // bool sk_pixmap_scale_pixels(const sk_pixmap_t* cpixmap, const sk_pixmap_t* dst, sk_filter_quality_t quality)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pixmap_scale_pixels(sk_pixmap_t cpixmap, sk_pixmap_t dst, SKFilterQuality quality);


    // bool sk_pngencoder_encode(sk_wstream_t* dst, const sk_pixmap_t* src, const sk_pngencoder_options_t* options)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pngencoder_encode(sk_wstream_t dst, sk_pixmap_t src, SKPngEncoderOptions* options);


    // void sk_swizzle_swap_rb(uint32_t* dest, const uint32_t* src, int count)

    [LibraryImport(SKIA)]
    internal static partial void sk_swizzle_swap_rb(UInt32* dest, UInt32* src, Int32 count);


    // bool sk_webpencoder_encode(sk_wstream_t* dst, const sk_pixmap_t* src, const sk_webpencoder_options_t* options)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool
        sk_webpencoder_encode(sk_wstream_t dst, sk_pixmap_t src, SKWebpEncoderOptions* options);
}