using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

partial class SkiaApi
{
    // void sk_graphics_dump_memory_statistics(sk_tracememorydump_t* dump)

    [LibraryImport(SKIA)]
    internal static partial void sk_graphics_dump_memory_statistics(sk_tracememorydump_t dump);


    // int sk_graphics_get_font_cache_count_limit()

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_graphics_get_font_cache_count_limit();


    // int sk_graphics_get_font_cache_count_used()

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_graphics_get_font_cache_count_used();


    // size_t sk_graphics_get_font_cache_limit()

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_graphics_get_font_cache_limit();


    // int sk_graphics_get_font_cache_point_size_limit()

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_graphics_get_font_cache_point_size_limit();


    // size_t sk_graphics_get_font_cache_used()

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_graphics_get_font_cache_used();


    // size_t sk_graphics_get_resource_cache_single_allocation_byte_limit()

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_graphics_get_resource_cache_single_allocation_byte_limit();


    // size_t sk_graphics_get_resource_cache_total_byte_limit()

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_graphics_get_resource_cache_total_byte_limit();


    // size_t sk_graphics_get_resource_cache_total_bytes_used()

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_graphics_get_resource_cache_total_bytes_used();


    // void sk_graphics_init()

    [LibraryImport(SKIA)]
    internal static partial void sk_graphics_init();


    // void sk_graphics_purge_all_caches()

    [LibraryImport(SKIA)]
    internal static partial void sk_graphics_purge_all_caches();


    // void sk_graphics_purge_font_cache()

    [LibraryImport(SKIA)]
    internal static partial void sk_graphics_purge_font_cache();


    // void sk_graphics_purge_resource_cache()

    [LibraryImport(SKIA)]
    internal static partial void sk_graphics_purge_resource_cache();


    // int sk_graphics_set_font_cache_count_limit(int count)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_graphics_set_font_cache_count_limit(Int32 count);


    // size_t sk_graphics_set_font_cache_limit(size_t bytes)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_graphics_set_font_cache_limit( /* size_t */ IntPtr bytes);


    // int sk_graphics_set_font_cache_point_size_limit(int maxPointSize)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_graphics_set_font_cache_point_size_limit(Int32 maxPointSize);


    // size_t sk_graphics_set_resource_cache_single_allocation_byte_limit(size_t newLimit)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */
        IntPtr sk_graphics_set_resource_cache_single_allocation_byte_limit( /* size_t */ IntPtr newLimit);


    // size_t sk_graphics_set_resource_cache_total_byte_limit(size_t newLimit)

    [LibraryImport(SKIA)]
    internal static partial /* size_t */
        IntPtr sk_graphics_set_resource_cache_total_byte_limit( /* size_t */ IntPtr newLimit);
}