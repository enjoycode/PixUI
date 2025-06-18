#if !__WEB__
using System;
using System.Runtime.InteropServices;

namespace PixUI;

// typedef void (*)()* gr_gl_func_ptr
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void GRGlFuncPtr();

// typedef gr_gl_func_ptr (*)(void* ctx, const char* name)* gr_gl_get_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate IntPtr GRGlGetProcProxyDelegate(void* ctx, /* char */ void* name);

// typedef void (*)()* gr_vk_func_ptr
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void GRVkFuncPtr();

// typedef gr_vk_func_ptr (*)(void* ctx, const char* name, vk_instance_t* instance, vk_device_t* device)* gr_vk_get_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate IntPtr GRVkGetProcProxyDelegate(void* ctx, /* char */ void* name,
    IntPtr instance, IntPtr device);

// typedef void (*)(void* addr, void* context)* sk_bitmap_release_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKBitmapReleaseProxyDelegate(void* addr, void* context);

// typedef void (*)(const void* ptr, void* context)* sk_data_release_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKDataReleaseProxyDelegate(void* ptr, void* context);

// typedef void (*)(const sk_path_t* pathOrNull, const sk_matrix_t* matrix, void* context)* sk_glyph_path_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKGlyphPathProxyDelegate(IntPtr pathOrNull, Matrix3* matrix,
    void* context);

// typedef void (*)(const void* addr, void* context)* sk_image_raster_release_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKImageRasterReleaseProxyDelegate(void* addr, void* context);

// typedef void (*)(void* context)* sk_image_texture_release_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKImageTextureReleaseProxyDelegate(void* context);

// typedef void (*)(sk_manageddrawable_t* d, void* context)* sk_manageddrawable_destroy_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKManagedDrawableDestroyProxyDelegate(IntPtr d, void* context);

// typedef void (*)(sk_manageddrawable_t* d, void* context, sk_canvas_t* ccanvas)* sk_manageddrawable_draw_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKManagedDrawableDrawProxyDelegate(IntPtr d, void* context,
    IntPtr ccanvas);

// typedef void (*)(sk_manageddrawable_t* d, void* context, sk_rect_t* rect)* sk_manageddrawable_getBounds_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKManagedDrawableGetBoundsProxyDelegate(IntPtr d, void* context,
    Rect* rect);

// typedef sk_picture_t* (*)(sk_manageddrawable_t* d, void* context)* sk_manageddrawable_newPictureSnapshot_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate IntPtr SKManagedDrawableNewPictureSnapshotProxyDelegate(IntPtr d,
    void* context);

// typedef void (*)(sk_stream_managedstream_t* s, void* context)* sk_managedstream_destroy_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKManagedStreamDestroyProxyDelegate(IntPtr s, void* context);

// typedef sk_stream_managedstream_t* (*)(const sk_stream_managedstream_t* s, void* context)* sk_managedstream_duplicate_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate IntPtr SKManagedStreamDuplicateProxyDelegate(IntPtr s, void* context);

// typedef sk_stream_managedstream_t* (*)(const sk_stream_managedstream_t* s, void* context)* sk_managedstream_fork_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate IntPtr SKManagedStreamForkProxyDelegate(IntPtr s, void* context);

// typedef size_t (*)(const sk_stream_managedstream_t* s, void* context)* sk_managedstream_getLength_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate /* size_t */
    IntPtr SKManagedStreamGetLengthProxyDelegate(IntPtr s, void* context);

// typedef size_t (*)(const sk_stream_managedstream_t* s, void* context)* sk_managedstream_getPosition_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate /* size_t */
    IntPtr SKManagedStreamGetPositionProxyDelegate(IntPtr s, void* context);

// typedef bool (*)(const sk_stream_managedstream_t* s, void* context)* sk_managedstream_hasLength_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
internal unsafe delegate bool SKManagedStreamHasLengthProxyDelegate(IntPtr s, void* context);

// typedef bool (*)(const sk_stream_managedstream_t* s, void* context)* sk_managedstream_hasPosition_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
internal unsafe delegate bool SKManagedStreamHasPositionProxyDelegate(IntPtr s, void* context);

// typedef bool (*)(const sk_stream_managedstream_t* s, void* context)* sk_managedstream_isAtEnd_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
internal unsafe delegate bool SKManagedStreamIsAtEndProxyDelegate(IntPtr s, void* context);

// typedef bool (*)(sk_stream_managedstream_t* s, void* context, int offset)* sk_managedstream_move_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
internal unsafe delegate bool SKManagedStreamMoveProxyDelegate(IntPtr s, void* context,
    Int32 offset);

// typedef size_t (*)(const sk_stream_managedstream_t* s, void* context, void* buffer, size_t size)* sk_managedstream_peek_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate /* size_t */ IntPtr SKManagedStreamPeekProxyDelegate(IntPtr s,
    void* context, void* buffer, /* size_t */ IntPtr size);

// typedef size_t (*)(sk_stream_managedstream_t* s, void* context, void* buffer, size_t size)* sk_managedstream_read_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate /* size_t */ IntPtr SKManagedStreamReadProxyDelegate(IntPtr s,
    void* context, void* buffer, /* size_t */ IntPtr size);

// typedef bool (*)(sk_stream_managedstream_t* s, void* context)* sk_managedstream_rewind_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
internal unsafe delegate bool SKManagedStreamRewindProxyDelegate(IntPtr s, void* context);

// typedef bool (*)(sk_stream_managedstream_t* s, void* context, size_t position)* sk_managedstream_seek_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
internal unsafe delegate bool SKManagedStreamSeekProxyDelegate(IntPtr s,
    void* context, /* size_t */ IntPtr position);

// typedef void (*)(sk_managedtracememorydump_t* d, void* context, const char* dumpName, const char* valueName, const char* units, uint64_t value)* sk_managedtraceMemoryDump_dumpNumericValue_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKManagedTraceMemoryDumpDumpNumericValueProxyDelegate(IntPtr d,
    void* context, /* char */ void* dumpName, /* char */ void* valueName, /* char */
    void* units, UInt64 value);

// typedef void (*)(sk_managedtracememorydump_t* d, void* context, const char* dumpName, const char* valueName, const char* value)* sk_managedtraceMemoryDump_dumpStringValue_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKManagedTraceMemoryDumpDumpStringValueProxyDelegate(IntPtr d,
    void* context, /* char */ void* dumpName, /* char */ void* valueName, /* char */
    void* value);

// typedef size_t (*)(const sk_wstream_managedstream_t* s, void* context)* sk_managedwstream_bytesWritten_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate /* size_t */
    IntPtr SKManagedWStreamBytesWrittenProxyDelegate(IntPtr s, void* context);

// typedef void (*)(sk_wstream_managedstream_t* s, void* context)* sk_managedwstream_destroy_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKManagedWStreamDestroyProxyDelegate(IntPtr s, void* context);

// typedef void (*)(sk_wstream_managedstream_t* s, void* context)* sk_managedwstream_flush_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKManagedWStreamFlushProxyDelegate(IntPtr s, void* context);

// typedef bool (*)(sk_wstream_managedstream_t* s, void* context, const void* buffer, size_t size)* sk_managedwstream_write_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.I1)]
internal unsafe delegate bool SKManagedWStreamWriteProxyDelegate(IntPtr s, void* context,
    void* buffer, /* size_t */ IntPtr size);

// typedef void (*)(void* addr, void* context)* sk_surface_raster_release_proc
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
internal unsafe delegate void SKSurfaceRasterReleaseProxyDelegate(void* addr, void* context);
#endif