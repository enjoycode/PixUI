using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_matrix_concat(sk_matrix_t* result, sk_matrix_t* first, sk_matrix_t* second)

    [LibraryImport(SKIA)]
    internal static partial void sk_matrix_concat(Matrix3* result, Matrix3* first, Matrix3* second);


    // void sk_matrix_map_points(sk_matrix_t* matrix, sk_point_t* dst, sk_point_t* src, int count)

    [LibraryImport(SKIA)]
    internal static partial void sk_matrix_map_points(Matrix3* matrix, Point* dst, Point* src, Int32 count);


    // float sk_matrix_map_radius(sk_matrix_t* matrix, float radius)

    [LibraryImport(SKIA)]
    internal static partial Single sk_matrix_map_radius(Matrix3* matrix, Single radius);


    // void sk_matrix_map_rect(sk_matrix_t* matrix, sk_rect_t* dest, sk_rect_t* source)

    [LibraryImport(SKIA)]
    internal static partial void sk_matrix_map_rect(Matrix3* matrix, Rect* dest, Rect* source);


    // void sk_matrix_map_vector(sk_matrix_t* matrix, float x, float y, sk_point_t* result)

    [LibraryImport(SKIA)]
    internal static partial void sk_matrix_map_vector(Matrix3* matrix, Single x, Single y, Point* result);


    // void sk_matrix_map_vectors(sk_matrix_t* matrix, sk_point_t* dst, sk_point_t* src, int count)

    [LibraryImport(SKIA)]
    internal static partial void sk_matrix_map_vectors(Matrix3* matrix, Point* dst, Point* src, Int32 count);


    // void sk_matrix_map_xy(sk_matrix_t* matrix, float x, float y, sk_point_t* result)

    [LibraryImport(SKIA)]
    internal static partial void sk_matrix_map_xy(Matrix3* matrix, Single x, Single y, Point* result);


    // void sk_matrix_post_concat(sk_matrix_t* result, sk_matrix_t* matrix)

    [LibraryImport(SKIA)]
    internal static partial void sk_matrix_post_concat(Matrix3* result, Matrix3* matrix);


    // void sk_matrix_pre_concat(sk_matrix_t* result, sk_matrix_t* matrix)

    [LibraryImport(SKIA)]
    internal static partial void sk_matrix_pre_concat(Matrix3* result, Matrix3* matrix);


    // bool sk_matrix_try_invert(sk_matrix_t* matrix, sk_matrix_t* result)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_matrix_try_invert(Matrix3* matrix, Matrix3* result);
}