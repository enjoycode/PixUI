using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_canvas_clear(sk_canvas_t*, sk_color_t)
    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_flush(sk_canvas_t param0);

    // void sk_canvas_clear(sk_canvas_t*, sk_color_t)
    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_clear(sk_canvas_t param0, UInt32 param1);

    // void sk_canvas_clear_color4f(sk_canvas_t*, sk_color4f_t)
    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_clear_color4f(sk_canvas_t param0, SKColorF param1);

    // void sk_canvas_clip_path_with_operation(sk_canvas_t* t, const sk_path_t* crect, sk_clipop_t op, bool doAA)
    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_clip_path_with_operation(sk_canvas_t t, sk_path_t crect, ClipOp op,
        [MarshalAs(UnmanagedType.I1)] bool doAA);

    // void sk_canvas_clip_rect_with_operation(sk_canvas_t* t, const sk_rect_t* crect, sk_clipop_t op, bool doAA)
    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_clip_rect_with_operation(sk_canvas_t t, Rect* crect, ClipOp op,
        [MarshalAs(UnmanagedType.I1)] bool doAA);

    // void sk_canvas_clip_region(sk_canvas_t* canvas, const sk_region_t* region, sk_clipop_t op)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_clip_region(sk_canvas_t canvas, sk_region_t region, ClipOp op);

    // void sk_canvas_clip_rrect_with_operation(sk_canvas_t* t, const sk_rrect_t* crect, sk_clipop_t op, bool doAA)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_clip_rrect_with_operation(sk_canvas_t t, sk_rrect_t crect, ClipOp op,
        [MarshalAs(UnmanagedType.I1)] bool doAA);

    // void sk_canvas_concat(sk_canvas_t*, const sk_matrix_t*)
    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_concat(sk_canvas_t param0, Matrix4* param1);

    // void sk_canvas_destroy(sk_canvas_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_destroy(sk_canvas_t param0);


    // void sk_canvas_discard(sk_canvas_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_discard(sk_canvas_t param0);


    // void sk_canvas_draw_annotation(sk_canvas_t* t, const sk_rect_t* rect, const char* key, sk_data_t* value)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_annotation(sk_canvas_t t, Rect* rect, /* char */ void* key,
        sk_data_t value);


    // void sk_canvas_draw_arc(sk_canvas_t* ccanvas, const sk_rect_t* oval, float startAngle, float sweepAngle, bool useCenter, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_arc(sk_canvas_t ccanvas, Rect* oval, Single startAngle,
        Single sweepAngle, [MarshalAs(UnmanagedType.I1)] bool useCenter, sk_paint_t paint);


    // void sk_canvas_draw_atlas(sk_canvas_t* ccanvas, const sk_image_t* atlas, const sk_rsxform_t* xform, const sk_rect_t* tex, const sk_color_t* colors, int count, sk_blendmode_t mode, const sk_rect_t* cullRect, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_atlas(sk_canvas_t ccanvas, sk_image_t atlas,
        SKRotationScaleMatrix* xform, Rect* tex, UInt32* colors, Int32 count, BlendMode mode, Rect* cullRect,
        sk_paint_t paint);


    // void sk_canvas_draw_circle(sk_canvas_t*, float cx, float cy, float rad, const sk_paint_t*)
    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_circle(sk_canvas_t param0, Single cx, Single cy, Single rad,
        sk_paint_t param4);

    // void sk_canvas_draw_color(sk_canvas_t* ccanvas, sk_color_t color, sk_blendmode_t mode)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_color(sk_canvas_t ccanvas, UInt32 color, BlendMode mode);


    // void sk_canvas_draw_color4f(sk_canvas_t* ccanvas, sk_color4f_t color, sk_blendmode_t mode)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_color4f(sk_canvas_t ccanvas, SKColorF color, BlendMode mode);


    // void sk_canvas_draw_drawable(sk_canvas_t*, sk_drawable_t*, const sk_matrix_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_drawable(sk_canvas_t param0, sk_drawable_t param1, Matrix3* param2);


    // void sk_canvas_draw_drrect(sk_canvas_t* ccanvas, const sk_rrect_t* outer, const sk_rrect_t* inner, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_drrect(sk_canvas_t ccanvas, sk_rrect_t outer, sk_rrect_t inner,
        sk_paint_t paint);


    // void sk_canvas_draw_image(sk_canvas_t*, const sk_image_t*, float x, float y, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_image(sk_canvas_t param0, sk_image_t param1, Single x, Single y,
        sk_paint_t param4);


    // void sk_canvas_draw_image_lattice(sk_canvas_t* t, const sk_image_t* image, const sk_lattice_t* lattice, const sk_rect_t* dst, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_image_lattice(sk_canvas_t t, sk_image_t image,
        SKLatticeInternal* lattice, Rect* dst, sk_paint_t paint);


    // void sk_canvas_draw_image_nine(sk_canvas_t* t, const sk_image_t* image, const sk_irect_t* center, const sk_rect_t* dst, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_image_nine(sk_canvas_t t, sk_image_t image, RectI* center, Rect* dst,
        sk_paint_t paint);


    // void sk_canvas_draw_image_rect(sk_canvas_t*, const sk_image_t*, const sk_rect_t* src, const sk_rect_t* dst, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_image_rect(sk_canvas_t param0, sk_image_t param1, Rect* src, Rect* dst,
        sk_paint_t param4);


    // void sk_canvas_draw_line(sk_canvas_t* ccanvas, float x0, float y0, float x1, float y1, sk_paint_t* cpaint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_line(sk_canvas_t ccanvas, Single x0, Single y0, Single x1, Single y1,
        sk_paint_t cpaint);


    // void sk_canvas_draw_link_destination_annotation(sk_canvas_t* t, const sk_rect_t* rect, sk_data_t* value)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_link_destination_annotation(sk_canvas_t t, Rect* rect, sk_data_t value);


    // void sk_canvas_draw_named_destination_annotation(sk_canvas_t* t, const sk_point_t* point, sk_data_t* value)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_named_destination_annotation(sk_canvas_t t, Point* point,
        sk_data_t value);


    // void sk_canvas_draw_oval(sk_canvas_t*, const sk_rect_t*, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_oval(sk_canvas_t param0, Rect* param1, sk_paint_t param2);


    // void sk_canvas_draw_paint(sk_canvas_t*, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_paint(sk_canvas_t param0, sk_paint_t param1);


    // void sk_canvas_draw_patch(sk_canvas_t* ccanvas, const sk_point_t* cubics, const sk_color_t* colors, const sk_point_t* texCoords, sk_blendmode_t mode, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_patch(sk_canvas_t ccanvas, Point* cubics, UInt32* colors,
        Point* texCoords, BlendMode mode, sk_paint_t paint);


    // void sk_canvas_draw_path(sk_canvas_t*, const sk_path_t*, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_path(sk_canvas_t param0, sk_path_t param1, sk_paint_t param2);


    // void sk_canvas_draw_picture(sk_canvas_t*, const sk_picture_t*, const sk_matrix_t*, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_picture(sk_canvas_t param0, sk_picture_t param1, Matrix3* param2,
        sk_paint_t param3);


    // void sk_canvas_draw_point(sk_canvas_t*, float, float, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_point(sk_canvas_t param0, Single param1, Single param2,
        sk_paint_t param3);


    // void sk_canvas_draw_points(sk_canvas_t*, sk_point_mode_t, size_t, const sk_point_t[-1], const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_points(sk_canvas_t param0, SKPointMode param1, /* size_t */
        IntPtr param2, Point* param3, sk_paint_t param4);


    // void sk_canvas_draw_rect(sk_canvas_t*, const sk_rect_t*, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_rect(sk_canvas_t param0, Rect* param1, sk_paint_t param2);


    // void sk_canvas_draw_region(sk_canvas_t*, const sk_region_t*, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_region(sk_canvas_t param0, sk_region_t param1, sk_paint_t param2);


    // void sk_canvas_draw_round_rect(sk_canvas_t*, const sk_rect_t*, float rx, float ry, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_round_rect(sk_canvas_t param0, Rect* param1, Single rx, Single ry,
        sk_paint_t param4);


    // void sk_canvas_draw_rrect(sk_canvas_t*, const sk_rrect_t*, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_rrect(sk_canvas_t param0, sk_rrect_t param1, sk_paint_t param2);


    // void sk_canvas_draw_simple_text(sk_canvas_t* ccanvas, const void* text, size_t byte_length, sk_text_encoding_t encoding, float x, float y, const sk_font_t* cfont, const sk_paint_t* cpaint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_simple_text(sk_canvas_t ccanvas, void* text, /* size_t */
        IntPtr byte_length, SKTextEncoding encoding, Single x, Single y, sk_font_t cfont, sk_paint_t cpaint);


    // void sk_canvas_draw_text_blob(sk_canvas_t*, sk_textblob_t* text, float x, float y, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_text_blob(sk_canvas_t param0, sk_textblob_t text, Single x, Single y,
        sk_paint_t paint);


    // void sk_canvas_draw_url_annotation(sk_canvas_t* t, const sk_rect_t* rect, sk_data_t* value)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_url_annotation(sk_canvas_t t, Rect* rect, sk_data_t value);


    // void sk_canvas_draw_vertices(sk_canvas_t* ccanvas, const sk_vertices_t* vertices, sk_blendmode_t mode, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_draw_vertices(sk_canvas_t ccanvas, sk_vertices_t vertices, BlendMode mode,
        sk_paint_t paint);

    // bool sk_canvas_get_device_clip_bounds(sk_canvas_t* t, sk_irect_t* cbounds)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_canvas_get_device_clip_bounds(sk_canvas_t t, RectI* cbounds);


    // bool sk_canvas_get_local_clip_bounds(sk_canvas_t* t, sk_rect_t* cbounds)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_canvas_get_local_clip_bounds(sk_canvas_t t, Rect* cbounds);


    // int sk_canvas_get_save_count(sk_canvas_t*)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_canvas_get_save_count(sk_canvas_t param0);


    // void sk_canvas_get_total_matrix(sk_canvas_t* ccanvas, sk_matrix_t* matrix)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_get_total_matrix(sk_canvas_t ccanvas, Matrix3* matrix);


    // bool sk_canvas_is_clip_empty(sk_canvas_t* ccanvas)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_canvas_is_clip_empty(sk_canvas_t ccanvas);


    // bool sk_canvas_is_clip_rect(sk_canvas_t* ccanvas)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_canvas_is_clip_rect(sk_canvas_t ccanvas);


    // sk_canvas_t* sk_canvas_new_from_bitmap(const sk_bitmap_t* bitmap)

    [LibraryImport(SKIA)]
    internal static partial sk_canvas_t sk_canvas_new_from_bitmap(sk_bitmap_t bitmap);


    // bool sk_canvas_quick_reject(sk_canvas_t*, const sk_rect_t*)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_canvas_quick_reject(sk_canvas_t param0, Rect* param1);


    // void sk_canvas_reset_matrix(sk_canvas_t* ccanvas)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_reset_matrix(sk_canvas_t ccanvas);


    // void sk_canvas_restore(sk_canvas_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_restore(sk_canvas_t param0);


    // void sk_canvas_restore_to_count(sk_canvas_t*, int saveCount)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_restore_to_count(sk_canvas_t param0, Int32 saveCount);


    // void sk_canvas_rotate_degrees(sk_canvas_t*, float degrees)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_rotate_degrees(sk_canvas_t param0, Single degrees);


    // void sk_canvas_rotate_radians(sk_canvas_t*, float radians)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_rotate_radians(sk_canvas_t param0, Single radians);


    // int sk_canvas_save(sk_canvas_t*)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_canvas_save(sk_canvas_t param0);


    // int sk_canvas_save_layer(sk_canvas_t*, const sk_rect_t*, const sk_paint_t*)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_canvas_save_layer(sk_canvas_t param0, Rect* param1, sk_paint_t param2);


    // void sk_canvas_scale(sk_canvas_t*, float sx, float sy)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_scale(sk_canvas_t param0, Single sx, Single sy);


    // void sk_canvas_set_matrix(sk_canvas_t* ccanvas, const sk_matrix_t* matrix)
    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_set_matrix(sk_canvas_t ccanvas, Matrix4* matrix);

    // void sk_canvas_skew(sk_canvas_t*, float sx, float sy)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_skew(sk_canvas_t param0, Single sx, Single sy);


    // void sk_canvas_translate(sk_canvas_t*, float dx, float dy)

    [LibraryImport(SKIA)]
    internal static partial void sk_canvas_translate(sk_canvas_t param0, Single dx, Single dy);


    // void sk_nodraw_canvas_destroy(sk_nodraw_canvas_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_nodraw_canvas_destroy(sk_nodraw_canvas_t param0);


    // sk_nodraw_canvas_t* sk_nodraw_canvas_new(int width, int height)

    [LibraryImport(SKIA)]
    internal static partial sk_nodraw_canvas_t sk_nodraw_canvas_new(Int32 width, Int32 height);


    // void sk_nway_canvas_add_canvas(sk_nway_canvas_t*, sk_canvas_t* canvas)

    [LibraryImport(SKIA)]
    internal static partial void sk_nway_canvas_add_canvas(sk_nway_canvas_t param0, sk_canvas_t canvas);


    // void sk_nway_canvas_destroy(sk_nway_canvas_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_nway_canvas_destroy(sk_nway_canvas_t param0);


    // sk_nway_canvas_t* sk_nway_canvas_new(int width, int height)

    [LibraryImport(SKIA)]
    internal static partial sk_nway_canvas_t sk_nway_canvas_new(Int32 width, Int32 height);


    // void sk_nway_canvas_remove_all(sk_nway_canvas_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_nway_canvas_remove_all(sk_nway_canvas_t param0);


    // void sk_nway_canvas_remove_canvas(sk_nway_canvas_t*, sk_canvas_t* canvas)

    [LibraryImport(SKIA)]
    internal static partial void sk_nway_canvas_remove_canvas(sk_nway_canvas_t param0, sk_canvas_t canvas);


    // void sk_overdraw_canvas_destroy(sk_overdraw_canvas_t* canvas)

    [LibraryImport(SKIA)]
    internal static partial void sk_overdraw_canvas_destroy(sk_overdraw_canvas_t canvas);


    // sk_overdraw_canvas_t* sk_overdraw_canvas_new(sk_canvas_t* canvas)

    [LibraryImport(SKIA)]
    internal static partial sk_overdraw_canvas_t sk_overdraw_canvas_new(sk_canvas_t canvas);
}