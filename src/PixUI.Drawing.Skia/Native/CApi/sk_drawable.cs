using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_drawable_draw(sk_drawable_t*, sk_canvas_t*, const sk_matrix_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_drawable_draw(sk_drawable_t param0, sk_canvas_t param1, Matrix3* param2);


    // void sk_drawable_get_bounds(sk_drawable_t*, sk_rect_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_drawable_get_bounds(sk_drawable_t param0, Rect* param1);


    // uint32_t sk_drawable_get_generation_id(sk_drawable_t*)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_drawable_get_generation_id(sk_drawable_t param0);


    // sk_picture_t* sk_drawable_new_picture_snapshot(sk_drawable_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_picture_t sk_drawable_new_picture_snapshot(sk_drawable_t param0);


    // void sk_drawable_notify_drawing_changed(sk_drawable_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_drawable_notify_drawing_changed(sk_drawable_t param0);


    // void sk_drawable_unref(sk_drawable_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_drawable_unref(sk_drawable_t param0);
}