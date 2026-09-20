using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_opbuilder_add(sk_opbuilder_t* builder, const sk_path_t* path, sk_pathop_t op)

    [LibraryImport(SKIA)]
    internal static partial void sk_opbuilder_add(sk_opbuilder_t builder, sk_path_t path, PathOp op);


    // void sk_opbuilder_destroy(sk_opbuilder_t* builder)

    [LibraryImport(SKIA)]
    internal static partial void sk_opbuilder_destroy(sk_opbuilder_t builder);


    // sk_opbuilder_t* sk_opbuilder_new()

    [LibraryImport(SKIA)]
    internal static partial sk_opbuilder_t sk_opbuilder_new();


    // bool sk_opbuilder_resolve(sk_opbuilder_t* builder, sk_path_t* result)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_opbuilder_resolve(sk_opbuilder_t builder, sk_path_t result);


    // sk_path_t* sk_path_clone(const sk_path_t* cpath)

    [LibraryImport(SKIA)]
    internal static partial sk_path_t sk_path_clone(sk_path_t cpath);


    // void sk_path_compute_tight_bounds(const sk_path_t*, sk_rect_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_compute_tight_bounds(sk_path_t param0, Rect* param1);


    // bool sk_path_contains(const sk_path_t* cpath, float x, float y)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_path_contains(sk_path_t cpath, Single x, Single y);


    // int sk_path_convert_conic_to_quads(const sk_point_t* p0, const sk_point_t* p1, const sk_point_t* p2, float w, sk_point_t* pts, int pow2)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_path_convert_conic_to_quads(Point* p0, Point* p1, Point* p2, Single w, Point* pts,
        Int32 pow2);


    // int sk_path_count_points(const sk_path_t* cpath)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_path_count_points(sk_path_t cpath);


    // int sk_path_count_verbs(const sk_path_t* cpath)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_path_count_verbs(sk_path_t cpath);


    // sk_path_iterator_t* sk_path_create_iter(sk_path_t* cpath, int forceClose)

    [LibraryImport(SKIA)]
    internal static partial sk_path_iterator_t sk_path_create_iter(sk_path_t cpath, Int32 forceClose);


    // sk_path_rawiterator_t* sk_path_create_rawiter(sk_path_t* cpath)

    [LibraryImport(SKIA)]
    internal static partial sk_path_rawiterator_t sk_path_create_rawiter(sk_path_t cpath);


    // void sk_path_delete(sk_path_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_delete(sk_path_t param0);


    // void sk_path_get_bounds(const sk_path_t*, sk_rect_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_get_bounds(sk_path_t param0, Rect* param1);


    // sk_path_filltype_t sk_path_get_filltype(sk_path_t*)

    [LibraryImport(SKIA)]
    internal static partial PathFillType sk_path_get_filltype(sk_path_t param0);


    // bool sk_path_get_last_point(const sk_path_t* cpath, sk_point_t* point)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_path_get_last_point(sk_path_t cpath, Point* point);


    // void sk_path_get_point(const sk_path_t* cpath, int index, sk_point_t* point)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_get_point(sk_path_t cpath, Int32 index, Point* point);


    // int sk_path_get_points(const sk_path_t* cpath, sk_point_t* points, int max)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_path_get_points(sk_path_t cpath, Point* points, Int32 max);


    // uint32_t sk_path_get_segment_masks(sk_path_t* cpath)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_path_get_segment_masks(sk_path_t cpath);


    // bool sk_path_is_convex(const sk_path_t* cpath)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_path_is_convex(sk_path_t cpath);

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_path_is_last_contour_closed(sk_path_t cpath);

    // bool sk_path_is_line(sk_path_t* cpath, sk_point_t[2] line = 2)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_path_is_line(sk_path_t cpath, Point* line);


    // bool sk_path_is_oval(sk_path_t* cpath, sk_rect_t* bounds)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_path_is_oval(sk_path_t cpath, Rect* bounds);


    // bool sk_path_is_rect(sk_path_t* cpath, sk_rect_t* rect, bool* isClosed, sk_path_direction_t* direction)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_path_is_rect(sk_path_t cpath, Rect* rect, Byte* isClosed, PathDirection* direction);


    // bool sk_path_is_rrect(sk_path_t* cpath, sk_rrect_t* bounds)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_path_is_rrect(sk_path_t cpath, sk_rrect_t bounds);


    // float sk_path_iter_conic_weight(sk_path_iterator_t* iterator)

    [LibraryImport(SKIA)]
    internal static partial Single sk_path_iter_conic_weight(sk_path_iterator_t iterator);


    // void sk_path_iter_destroy(sk_path_iterator_t* iterator)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_iter_destroy(sk_path_iterator_t iterator);


    // int sk_path_iter_is_close_line(sk_path_iterator_t* iterator)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_path_iter_is_close_line(sk_path_iterator_t iterator);


    // int sk_path_iter_is_closed_contour(sk_path_iterator_t* iterator)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_path_iter_is_closed_contour(sk_path_iterator_t iterator);


    // sk_path_verb_t sk_path_iter_next(sk_path_iterator_t* iterator, sk_point_t[4] points = 4)

    [LibraryImport(SKIA)]
    internal static partial SKPathVerb sk_path_iter_next(sk_path_iterator_t iterator, Point* points);


    // sk_path_t* sk_path_new()

    [LibraryImport(SKIA)]
    internal static partial sk_path_t sk_path_new();


    // bool sk_path_parse_svg_string(sk_path_t* cpath, const char* str)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_path_parse_svg_string(sk_path_t cpath, [MarshalAs(UnmanagedType.LPStr)] String str);


    // float sk_path_rawiter_conic_weight(sk_path_rawiterator_t* iterator)

    [LibraryImport(SKIA)]
    internal static partial Single sk_path_rawiter_conic_weight(sk_path_rawiterator_t iterator);


    // void sk_path_rawiter_destroy(sk_path_rawiterator_t* iterator)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_rawiter_destroy(sk_path_rawiterator_t iterator);


    // sk_path_verb_t sk_path_rawiter_next(sk_path_rawiterator_t* iterator, sk_point_t[4] points = 4)

    [LibraryImport(SKIA)]
    internal static partial SKPathVerb sk_path_rawiter_next(sk_path_rawiterator_t iterator, Point* points);


    // sk_path_verb_t sk_path_rawiter_peek(sk_path_rawiterator_t* iterator)

    [LibraryImport(SKIA)]
    internal static partial SKPathVerb sk_path_rawiter_peek(sk_path_rawiterator_t iterator);


    // void sk_path_reset(sk_path_t* cpath)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_reset(sk_path_t cpath);


    // void sk_path_rewind(sk_path_t* cpath)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_rewind(sk_path_t cpath);


    // void sk_path_set_filltype(sk_path_t*, sk_path_filltype_t)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_set_filltype(sk_path_t param0, PathFillType param1);


    // void sk_path_to_svg_string(const sk_path_t* cpath, sk_string_t* str)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_to_svg_string(sk_path_t cpath, sk_string_t str);


    // void sk_path_transform(sk_path_t* cpath, const sk_matrix_t* cmatrix)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_transform(sk_path_t cpath, Matrix3* cmatrix);


    // void sk_path_transform_to_dest(const sk_path_t* cpath, const sk_matrix_t* cmatrix, sk_path_t* destination)

    [LibraryImport(SKIA)]
    internal static partial void sk_path_transform_to_dest(sk_path_t cpath, Matrix3* cmatrix, sk_path_t destination);


    // void sk_pathmeasure_destroy(sk_pathmeasure_t* pathMeasure)

    [LibraryImport(SKIA)]
    internal static partial void sk_pathmeasure_destroy(sk_pathmeasure_t pathMeasure);


    // float sk_pathmeasure_get_length(sk_pathmeasure_t* pathMeasure)

    [LibraryImport(SKIA)]
    internal static partial Single sk_pathmeasure_get_length(sk_pathmeasure_t pathMeasure);


    // bool sk_pathmeasure_get_matrix(sk_pathmeasure_t* pathMeasure, float distance, sk_matrix_t* matrix, sk_pathmeasure_matrixflags_t flags)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pathmeasure_get_matrix(sk_pathmeasure_t pathMeasure, Single distance,
        Matrix3* matrix, SKPathMeasureMatrixFlags flags);


    // bool sk_pathmeasure_get_pos_tan(sk_pathmeasure_t* pathMeasure, float distance, sk_point_t* position, sk_vector_t* tangent)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pathmeasure_get_pos_tan(sk_pathmeasure_t pathMeasure, Single distance,
        Point* position, Point* tangent);


    // bool sk_pathmeasure_get_segment(sk_pathmeasure_t* pathMeasure, float start, float stop, sk_path_t* dst, bool startWithMoveTo)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pathmeasure_get_segment(sk_pathmeasure_t pathMeasure, Single start, Single stop,
        sk_path_t dst, [MarshalAs(UnmanagedType.I1)] bool startWithMoveTo);


    // bool sk_pathmeasure_is_closed(sk_pathmeasure_t* pathMeasure)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pathmeasure_is_closed(sk_pathmeasure_t pathMeasure);


    // sk_pathmeasure_t* sk_pathmeasure_new()

    [LibraryImport(SKIA)]
    internal static partial sk_pathmeasure_t sk_pathmeasure_new();


    // sk_pathmeasure_t* sk_pathmeasure_new_with_path(const sk_path_t* path, bool forceClosed, float resScale)

    [LibraryImport(SKIA)]
    internal static partial sk_pathmeasure_t sk_pathmeasure_new_with_path(sk_path_t path,
        [MarshalAs(UnmanagedType.I1)] bool forceClosed, Single resScale);


    // bool sk_pathmeasure_next_contour(sk_pathmeasure_t* pathMeasure)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pathmeasure_next_contour(sk_pathmeasure_t pathMeasure);


    // void sk_pathmeasure_set_path(sk_pathmeasure_t* pathMeasure, const sk_path_t* path, bool forceClosed)

    [LibraryImport(SKIA)]
    internal static partial void sk_pathmeasure_set_path(sk_pathmeasure_t pathMeasure, sk_path_t path,
        [MarshalAs(UnmanagedType.I1)] bool forceClosed);


    // bool sk_pathop_as_winding(const sk_path_t* path, sk_path_t* result)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pathop_as_winding(sk_path_t path, sk_path_t result);


    // bool sk_pathop_op(const sk_path_t* one, const sk_path_t* two, sk_pathop_t op, sk_path_t* result)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pathop_op(sk_path_t one, sk_path_t two, PathOp op, sk_path_t result);


    // bool sk_pathop_simplify(const sk_path_t* path, sk_path_t* result)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pathop_simplify(sk_path_t path, sk_path_t result);


    // bool sk_pathop_tight_bounds(const sk_path_t* path, sk_rect_t* result)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_pathop_tight_bounds(sk_path_t path, Rect* result);
}