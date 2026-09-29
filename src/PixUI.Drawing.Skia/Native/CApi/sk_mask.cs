using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // uint8_t* sk_mask_alloc_image(size_t bytes)

    [LibraryImport(SKIA)]
    internal static partial Byte* sk_mask_alloc_image( /* size_t */ IntPtr bytes);


    // size_t sk_mask_compute_image_size(sk_mask_t* cmask)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_mask_compute_image_size(SKMask* cmask);


    // size_t sk_mask_compute_total_image_size(sk_mask_t* cmask)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_mask_compute_total_image_size(SKMask* cmask);


    // void sk_mask_free_image(void* image)

    [LibraryImport(SKIA)]
    internal static partial void sk_mask_free_image(void* image);


    // void* sk_mask_get_addr(sk_mask_t* cmask, int x, int y)

    [LibraryImport(SKIA)]
    internal static partial void* sk_mask_get_addr(SKMask* cmask, Int32 x, Int32 y);


    // uint8_t* sk_mask_get_addr_1(sk_mask_t* cmask, int x, int y)

    [LibraryImport(SKIA)]
    internal static partial Byte* sk_mask_get_addr_1(SKMask* cmask, Int32 x, Int32 y);


    // uint32_t* sk_mask_get_addr_32(sk_mask_t* cmask, int x, int y)

    [LibraryImport(SKIA)]
    internal static partial UInt32* sk_mask_get_addr_32(SKMask* cmask, Int32 x, Int32 y);


    // uint8_t* sk_mask_get_addr_8(sk_mask_t* cmask, int x, int y)

    [LibraryImport(SKIA)]
    internal static partial Byte* sk_mask_get_addr_8(SKMask* cmask, Int32 x, Int32 y);


    // uint16_t* sk_mask_get_addr_lcd_16(sk_mask_t* cmask, int x, int y)

    [LibraryImport(SKIA)]
    internal static partial UInt16* sk_mask_get_addr_lcd_16(SKMask* cmask, Int32 x, Int32 y);


    // bool sk_mask_is_empty(sk_mask_t* cmask)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_mask_is_empty(SKMask* cmask);
}