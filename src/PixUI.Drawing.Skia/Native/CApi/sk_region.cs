using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_region_cliperator_delete(sk_region_cliperator_t* iter)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_cliperator_delete(sk_region_cliperator_t iter);


    // bool sk_region_cliperator_done(sk_region_cliperator_t* iter)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_cliperator_done(sk_region_cliperator_t iter);


    // sk_region_cliperator_t* sk_region_cliperator_new(const sk_region_t* region, const sk_irect_t* clip)

    [LibraryImport(SKIA)]
    internal static partial sk_region_cliperator_t sk_region_cliperator_new(sk_region_t region, RectI* clip);


    // void sk_region_cliperator_next(sk_region_cliperator_t* iter)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_cliperator_next(sk_region_cliperator_t iter);


    // void sk_region_cliperator_rect(const sk_region_cliperator_t* iter, sk_irect_t* rect)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_cliperator_rect(sk_region_cliperator_t iter, RectI* rect);


    // bool sk_region_contains(const sk_region_t* r, const sk_region_t* region)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_contains(sk_region_t r, sk_region_t region);


    // bool sk_region_contains_point(const sk_region_t* r, int x, int y)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_contains_point(sk_region_t r, Int32 x, Int32 y);


    // bool sk_region_contains_rect(const sk_region_t* r, const sk_irect_t* rect)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_contains_rect(sk_region_t r, RectI* rect);


    // void sk_region_delete(sk_region_t* r)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_delete(sk_region_t r);


    // bool sk_region_get_boundary_path(const sk_region_t* r, sk_path_t* path)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_get_boundary_path(sk_region_t r, sk_path_t path);


    // void sk_region_get_bounds(const sk_region_t* r, sk_irect_t* rect)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_get_bounds(sk_region_t r, RectI* rect);


    // bool sk_region_intersects(const sk_region_t* r, const sk_region_t* src)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_intersects(sk_region_t r, sk_region_t src);


    // bool sk_region_intersects_rect(const sk_region_t* r, const sk_irect_t* rect)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_intersects_rect(sk_region_t r, RectI* rect);


    // bool sk_region_is_complex(const sk_region_t* r)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_is_complex(sk_region_t r);


    // bool sk_region_is_empty(const sk_region_t* r)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_is_empty(sk_region_t r);


    // bool sk_region_is_rect(const sk_region_t* r)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_is_rect(sk_region_t r);


    // void sk_region_iterator_delete(sk_region_iterator_t* iter)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_iterator_delete(sk_region_iterator_t iter);


    // bool sk_region_iterator_done(const sk_region_iterator_t* iter)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_iterator_done(sk_region_iterator_t iter);


    // sk_region_iterator_t* sk_region_iterator_new(const sk_region_t* region)

    [LibraryImport(SKIA)]
    internal static partial sk_region_iterator_t sk_region_iterator_new(sk_region_t region);


    // void sk_region_iterator_next(sk_region_iterator_t* iter)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_iterator_next(sk_region_iterator_t iter);


    // void sk_region_iterator_rect(const sk_region_iterator_t* iter, sk_irect_t* rect)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_iterator_rect(sk_region_iterator_t iter, RectI* rect);


    // bool sk_region_iterator_rewind(sk_region_iterator_t* iter)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_iterator_rewind(sk_region_iterator_t iter);


    // sk_region_t* sk_region_new()

    [LibraryImport(SKIA)]
    internal static partial sk_region_t sk_region_new();


    // bool sk_region_op(sk_region_t* r, const sk_region_t* region, sk_region_op_t op)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_op(sk_region_t r, sk_region_t region, SKRegionOperation op);


    // bool sk_region_op_rect(sk_region_t* r, const sk_irect_t* rect, sk_region_op_t op)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_op_rect(sk_region_t r, RectI* rect, SKRegionOperation op);


    // bool sk_region_quick_contains(const sk_region_t* r, const sk_irect_t* rect)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_quick_contains(sk_region_t r, RectI* rect);


    // bool sk_region_quick_reject(const sk_region_t* r, const sk_region_t* region)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_quick_reject(sk_region_t r, sk_region_t region);


    // bool sk_region_quick_reject_rect(const sk_region_t* r, const sk_irect_t* rect)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_quick_reject_rect(sk_region_t r, RectI* rect);


    // bool sk_region_set_empty(sk_region_t* r)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_set_empty(sk_region_t r);


    // bool sk_region_set_path(sk_region_t* r, const sk_path_t* t, const sk_region_t* clip)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_set_path(sk_region_t r, sk_path_t t, sk_region_t clip);


    // bool sk_region_set_rect(sk_region_t* r, const sk_irect_t* rect)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_set_rect(sk_region_t r, RectI* rect);


    // bool sk_region_set_rects(sk_region_t* r, const sk_irect_t* rects, int count)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_set_rects(sk_region_t r, RectI* rects, Int32 count);


    // bool sk_region_set_region(sk_region_t* r, const sk_region_t* region)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_set_region(sk_region_t r, sk_region_t region);


    // void sk_region_spanerator_delete(sk_region_spanerator_t* iter)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_spanerator_delete(sk_region_spanerator_t iter);


    // sk_region_spanerator_t* sk_region_spanerator_new(const sk_region_t* region, int y, int left, int right)

    [LibraryImport(SKIA)]
    internal static partial sk_region_spanerator_t sk_region_spanerator_new(sk_region_t region, Int32 y, Int32 left,
        Int32 right);


    // bool sk_region_spanerator_next(sk_region_spanerator_t* iter, int* left, int* right)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_region_spanerator_next(sk_region_spanerator_t iter, Int32* left, Int32* right);


    // void sk_region_translate(sk_region_t* r, int x, int y)

    [LibraryImport(SKIA)]
    internal static partial void sk_region_translate(sk_region_t r, Int32 x, Int32 y);
}