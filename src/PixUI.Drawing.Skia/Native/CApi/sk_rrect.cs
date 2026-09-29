using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // bool sk_rrect_contains(const sk_rrect_t* rrect, const sk_rect_t* rect)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_rrect_contains(sk_rrect_t rrect, Rect* rect);


    // void sk_rrect_delete(const sk_rrect_t* rrect)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_delete(sk_rrect_t rrect);


    // float sk_rrect_get_height(const sk_rrect_t* rrect)

    [LibraryImport(SKIA)]
    internal static partial Single sk_rrect_get_height(sk_rrect_t rrect);


    // void sk_rrect_get_radii(const sk_rrect_t* rrect, sk_rrect_corner_t corner, sk_vector_t* radii)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_get_radii(sk_rrect_t rrect, SKRoundRectCorner corner, Point* radii);


    // void sk_rrect_get_rect(const sk_rrect_t* rrect, sk_rect_t* rect)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_get_rect(sk_rrect_t rrect, Rect* rect);


    // sk_rrect_type_t sk_rrect_get_type(const sk_rrect_t* rrect)

    [LibraryImport(SKIA)]
    internal static partial SKRoundRectType sk_rrect_get_type(sk_rrect_t rrect);


    // float sk_rrect_get_width(const sk_rrect_t* rrect)

    [LibraryImport(SKIA)]
    internal static partial Single sk_rrect_get_width(sk_rrect_t rrect);


    // void sk_rrect_inset(sk_rrect_t* rrect, float dx, float dy)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_inset(sk_rrect_t rrect, Single dx, Single dy);


    // bool sk_rrect_is_valid(const sk_rrect_t* rrect)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_rrect_is_valid(sk_rrect_t rrect);


    // sk_rrect_t* sk_rrect_new()

    [LibraryImport(SKIA)]
    internal static partial sk_rrect_t sk_rrect_new();


    // sk_rrect_t* sk_rrect_new_copy(const sk_rrect_t* rrect)

    [LibraryImport(SKIA)]
    internal static partial sk_rrect_t sk_rrect_new_copy(sk_rrect_t rrect);


    // void sk_rrect_offset(sk_rrect_t* rrect, float dx, float dy)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_offset(sk_rrect_t rrect, Single dx, Single dy);


    // void sk_rrect_outset(sk_rrect_t* rrect, float dx, float dy)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_outset(sk_rrect_t rrect, Single dx, Single dy);


    // void sk_rrect_set_empty(sk_rrect_t* rrect)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_set_empty(sk_rrect_t rrect);


    // void sk_rrect_set_nine_patch(sk_rrect_t* rrect, const sk_rect_t* rect, float leftRad, float topRad, float rightRad, float bottomRad)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_set_nine_patch(sk_rrect_t rrect, Rect* rect, Single leftRad, Single topRad,
        Single rightRad, Single bottomRad);


    // void sk_rrect_set_oval(sk_rrect_t* rrect, const sk_rect_t* rect)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_set_oval(sk_rrect_t rrect, Rect* rect);


    // void sk_rrect_set_rect(sk_rrect_t* rrect, const sk_rect_t* rect)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_set_rect(sk_rrect_t rrect, Rect* rect);


    // void sk_rrect_set_rect_radii(sk_rrect_t* rrect, const sk_rect_t* rect, const sk_vector_t* radii)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_set_rect_radii(sk_rrect_t rrect, Rect* rect, Point* radii);


    // void sk_rrect_set_rect_xy(sk_rrect_t* rrect, const sk_rect_t* rect, float xRad, float yRad)

    [LibraryImport(SKIA)]
    internal static partial void sk_rrect_set_rect_xy(sk_rrect_t rrect, Rect* rect, Single xRad, Single yRad);


    // bool sk_rrect_transform(sk_rrect_t* rrect, const sk_matrix_t* matrix, sk_rrect_t* dest)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_rrect_transform(sk_rrect_t rrect, Matrix3* matrix, sk_rrect_t dest);
}