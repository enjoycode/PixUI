using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_bitmap_destructor(sk_bitmap_t* cbitmap)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_destructor(sk_bitmap_t cbitmap);

    // void sk_bitmap_erase(sk_bitmap_t* cbitmap, sk_color_t color)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_erase(sk_bitmap_t cbitmap, UInt32 color);

    // void sk_bitmap_erase_rect(sk_bitmap_t* cbitmap, sk_color_t color, sk_irect_t* rect)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_erase_rect(sk_bitmap_t cbitmap, UInt32 color, RectI* rect);

    // bool sk_bitmap_extract_alpha(sk_bitmap_t* cbitmap, sk_bitmap_t* dst, const sk_paint_t* paint, sk_ipoint_t* offset)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_extract_alpha(sk_bitmap_t cbitmap, sk_bitmap_t dst, sk_paint_t paint,
        PointI* offset);

    // bool sk_bitmap_extract_subset(sk_bitmap_t* cbitmap, sk_bitmap_t* dst, sk_irect_t* subset)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_extract_subset(sk_bitmap_t cbitmap, sk_bitmap_t dst, RectI* subset);

    // void* sk_bitmap_get_addr(sk_bitmap_t* cbitmap, int x, int y)
    [LibraryImport(SKIA)]
    internal static partial void* sk_bitmap_get_addr(sk_bitmap_t cbitmap, Int32 x, Int32 y);

    // uint16_t* sk_bitmap_get_addr_16(sk_bitmap_t* cbitmap, int x, int y)
    [LibraryImport(SKIA)]
    internal static partial UInt16* sk_bitmap_get_addr_16(sk_bitmap_t cbitmap, Int32 x, Int32 y);

    // uint32_t* sk_bitmap_get_addr_32(sk_bitmap_t* cbitmap, int x, int y)
    [LibraryImport(SKIA)]
    internal static partial UInt32* sk_bitmap_get_addr_32(sk_bitmap_t cbitmap, Int32 x, Int32 y);

    // uint8_t* sk_bitmap_get_addr_8(sk_bitmap_t* cbitmap, int x, int y)
    [LibraryImport(SKIA)]
    internal static partial Byte* sk_bitmap_get_addr_8(sk_bitmap_t cbitmap, Int32 x, Int32 y);

    // size_t sk_bitmap_get_byte_count(sk_bitmap_t* cbitmap)
    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_bitmap_get_byte_count(sk_bitmap_t cbitmap);

    // void sk_bitmap_get_info(sk_bitmap_t* cbitmap, sk_imageinfo_t* info)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_get_info(sk_bitmap_t cbitmap, SKImageInfoNative* info);

    // sk_color_t sk_bitmap_get_pixel_color(sk_bitmap_t* cbitmap, int x, int y)
    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_bitmap_get_pixel_color(sk_bitmap_t cbitmap, Int32 x, Int32 y);

    // void sk_bitmap_get_pixel_colors(sk_bitmap_t* cbitmap, sk_color_t* colors)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_get_pixel_colors(sk_bitmap_t cbitmap, UInt32* colors);

    // void* sk_bitmap_get_pixels(sk_bitmap_t* cbitmap, size_t* length)
    [LibraryImport(SKIA)]
    internal static partial void* sk_bitmap_get_pixels(sk_bitmap_t cbitmap, /* size_t */ IntPtr* length);

    // size_t sk_bitmap_get_row_bytes(sk_bitmap_t* cbitmap)
    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_bitmap_get_row_bytes(sk_bitmap_t cbitmap);

    // bool sk_bitmap_install_pixels(sk_bitmap_t* cbitmap, const sk_imageinfo_t* cinfo, void* pixels, size_t rowBytes, const sk_bitmap_release_proc releaseProc, void* context)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_install_pixels(sk_bitmap_t cbitmap, SKImageInfoNative* cinfo,
        void* pixels, /* size_t */ IntPtr rowBytes, SKBitmapReleaseProxyDelegate releaseProc, void* context);

    // bool sk_bitmap_install_pixels_with_pixmap(sk_bitmap_t* cbitmap, const sk_pixmap_t* cpixmap)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_install_pixels_with_pixmap(sk_bitmap_t cbitmap, sk_pixmap_t cpixmap);

    // bool sk_bitmap_is_immutable(sk_bitmap_t* cbitmap)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_is_immutable(sk_bitmap_t cbitmap);

    // bool sk_bitmap_is_null(sk_bitmap_t* cbitmap)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_is_null(sk_bitmap_t cbitmap);

    // sk_shader_t* sk_bitmap_make_shader(sk_bitmap_t* cbitmap, sk_shader_tilemode_t tmx, sk_shader_tilemode_t tmy, const sk_matrix_t* cmatrix)
    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_bitmap_make_shader(sk_bitmap_t cbitmap, TileMode tmx, TileMode tmy,
        Matrix3* cmatrix);

    // sk_bitmap_t* sk_bitmap_new()
    [LibraryImport(SKIA)]
    internal static partial sk_bitmap_t sk_bitmap_new();

    // void sk_bitmap_notify_pixels_changed(sk_bitmap_t* cbitmap)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_notify_pixels_changed(sk_bitmap_t cbitmap);

    // bool sk_bitmap_peek_pixels(sk_bitmap_t* cbitmap, sk_pixmap_t* cpixmap)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_peek_pixels(sk_bitmap_t cbitmap, sk_pixmap_t cpixmap);

    // bool sk_bitmap_ready_to_draw(sk_bitmap_t* cbitmap)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_ready_to_draw(sk_bitmap_t cbitmap);

    // void sk_bitmap_reset(sk_bitmap_t* cbitmap)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_reset(sk_bitmap_t cbitmap);

    // void sk_bitmap_set_immutable(sk_bitmap_t* cbitmap)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_set_immutable(sk_bitmap_t cbitmap);

    // void sk_bitmap_set_pixels(sk_bitmap_t* cbitmap, void* pixels)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_set_pixels(sk_bitmap_t cbitmap, void* pixels);

    // void sk_bitmap_swap(sk_bitmap_t* cbitmap, sk_bitmap_t* cother)
    [LibraryImport(SKIA)]
    internal static partial void sk_bitmap_swap(sk_bitmap_t cbitmap, sk_bitmap_t cother);

    // bool sk_bitmap_try_alloc_pixels(sk_bitmap_t* cbitmap, const sk_imageinfo_t* requestedInfo, size_t rowBytes)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_try_alloc_pixels(sk_bitmap_t cbitmap,
        SKImageInfoNative* requestedInfo, /* size_t */ IntPtr rowBytes);

    // bool sk_bitmap_try_alloc_pixels_with_flags(sk_bitmap_t* cbitmap, const sk_imageinfo_t* requestedInfo, uint32_t flags)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_bitmap_try_alloc_pixels_with_flags(sk_bitmap_t cbitmap,
        SKImageInfoNative* requestedInfo, UInt32 flags);
}