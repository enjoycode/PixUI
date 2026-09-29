using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // sk_picture_t* sk_picture_deserialize_from_data(sk_data_t* data)

    [LibraryImport(SKIA)]
    internal static partial sk_picture_t sk_picture_deserialize_from_data(sk_data_t data);


    // sk_picture_t* sk_picture_deserialize_from_memory(void* buffer, size_t length)

    [LibraryImport(SKIA)]
    internal static partial sk_picture_t sk_picture_deserialize_from_memory(void* buffer, /* size_t */ IntPtr length);


    // sk_picture_t* sk_picture_deserialize_from_stream(sk_stream_t* stream)

    [LibraryImport(SKIA)]
    internal static partial sk_picture_t sk_picture_deserialize_from_stream(sk_stream_t stream);


    // void sk_picture_get_cull_rect(sk_picture_t*, sk_rect_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_picture_get_cull_rect(sk_picture_t param0, Rect* param1);


    // sk_canvas_t* sk_picture_get_recording_canvas(sk_picture_recorder_t* crec)

    [LibraryImport(SKIA)]
    internal static partial sk_canvas_t sk_picture_get_recording_canvas(sk_picture_recorder_t crec);


    // uint32_t sk_picture_get_unique_id(sk_picture_t*)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_picture_get_unique_id(sk_picture_t param0);


    // sk_shader_t* sk_picture_make_shader(sk_picture_t* src, sk_shader_tilemode_t tmx, sk_shader_tilemode_t tmy, const sk_matrix_t* localMatrix, const sk_rect_t* tile)

    [LibraryImport(SKIA)]
    internal static partial sk_shader_t sk_picture_make_shader(sk_picture_t src, TileMode tmx, TileMode tmy,
        Matrix3* localMatrix, Rect* tile);


    // sk_canvas_t* sk_picture_recorder_begin_recording(sk_picture_recorder_t*, const sk_rect_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_canvas_t sk_picture_recorder_begin_recording(sk_picture_recorder_t param0, Rect* param1);


    // void sk_picture_recorder_delete(sk_picture_recorder_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_picture_recorder_delete(sk_picture_recorder_t param0);


    // sk_picture_t* sk_picture_recorder_end_recording(sk_picture_recorder_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_picture_t sk_picture_recorder_end_recording(sk_picture_recorder_t param0);


    // sk_drawable_t* sk_picture_recorder_end_recording_as_drawable(sk_picture_recorder_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_drawable_t sk_picture_recorder_end_recording_as_drawable(sk_picture_recorder_t param0);


    // sk_picture_recorder_t* sk_picture_recorder_new()

    [LibraryImport(SKIA)]
    internal static partial sk_picture_recorder_t sk_picture_recorder_new();


    // void sk_picture_ref(sk_picture_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_picture_ref(sk_picture_t param0);


    // sk_data_t* sk_picture_serialize_to_data(const sk_picture_t* picture)

    [LibraryImport(SKIA)]
    internal static partial sk_data_t sk_picture_serialize_to_data(sk_picture_t picture);


    // void sk_picture_serialize_to_stream(const sk_picture_t* picture, sk_wstream_t* stream)

    [LibraryImport(SKIA)]
    internal static partial void sk_picture_serialize_to_stream(sk_picture_t picture, sk_wstream_t stream);


    // void sk_picture_unref(sk_picture_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_picture_unref(sk_picture_t param0);
}