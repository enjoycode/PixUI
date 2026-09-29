using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_surface_draw(sk_surface_t* surface, sk_canvas_t* canvas, float x, float y, const sk_paint_t* paint)

    [LibraryImport(SKIA)]
    internal static partial void sk_surface_draw(sk_surface_t surface, sk_canvas_t canvas, Single x, Single y,
        sk_paint_t paint);

    // sk_canvas_t* sk_surface_get_canvas(sk_surface_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_canvas_t sk_surface_get_canvas(sk_surface_t param0);


    // const sk_surfaceprops_t* sk_surface_get_props(sk_surface_t* surface)

    [LibraryImport(SKIA)]
    internal static partial sk_surfaceprops_t sk_surface_get_props(sk_surface_t surface);


    // gr_recording_context_t* sk_surface_get_recording_context(sk_surface_t* surface)

    [LibraryImport(SKIA)]
    internal static partial gr_recording_context_t sk_surface_get_recording_context(sk_surface_t surface);


    // sk_surface_t* sk_surface_new_backend_render_target(gr_recording_context_t* context, const gr_backendrendertarget_t* target, gr_surfaceorigin_t origin, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props)

    [LibraryImport(SKIA)]
    internal static partial sk_surface_t sk_surface_new_backend_render_target(gr_recording_context_t context,
        gr_backendrendertarget_t target, SurfaceOrigin origin, SKColorTypeNative colorType, sk_colorspace_t colorspace,
        sk_surfaceprops_t props);


    // sk_surface_t* sk_surface_new_backend_texture(gr_recording_context_t* context, const gr_backendtexture_t* texture, gr_surfaceorigin_t origin, int samples, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props)

    [LibraryImport(SKIA)]
    internal static partial sk_surface_t sk_surface_new_backend_texture(gr_recording_context_t context,
        gr_backendtexture_t texture, SurfaceOrigin origin, Int32 samples, SKColorTypeNative colorType,
        sk_colorspace_t colorspace, sk_surfaceprops_t props);


    // sk_image_t* sk_surface_new_image_snapshot(sk_surface_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_image_t sk_surface_new_image_snapshot(sk_surface_t param0);


    // sk_image_t* sk_surface_new_image_snapshot_with_crop(sk_surface_t* surface, const sk_irect_t* bounds)

    [LibraryImport(SKIA)]
    internal static partial sk_image_t sk_surface_new_image_snapshot_with_crop(sk_surface_t surface, RectI* bounds);


    // sk_surface_t* sk_surface_new_metal_layer(gr_recording_context_t* context, const void* layer, gr_surfaceorigin_t origin, int sampleCount, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props, const void** drawable)

    [LibraryImport(SKIA)]
    internal static partial sk_surface_t sk_surface_new_metal_layer(gr_recording_context_t context, void* layer,
        SurfaceOrigin origin, Int32 sampleCount, SKColorTypeNative colorType, sk_colorspace_t colorspace,
        sk_surfaceprops_t props, void** drawable);


    // sk_surface_t* sk_surface_new_metal_view(gr_recording_context_t* context, const void* mtkView, gr_surfaceorigin_t origin, int sampleCount, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props)

    [LibraryImport(SKIA)]
    internal static partial sk_surface_t sk_surface_new_metal_view(gr_recording_context_t context, void* mtkView,
        SurfaceOrigin origin, Int32 sampleCount, SKColorTypeNative colorType, sk_colorspace_t colorspace,
        sk_surfaceprops_t props);


    // sk_surface_t* sk_surface_new_raster(const sk_imageinfo_t*, size_t rowBytes, const sk_surfaceprops_t*)

    [LibraryImport(SKIA)]
    internal static partial sk_surface_t sk_surface_new_raster(SKImageInfoNative* param0, /* size_t */ IntPtr rowBytes,
        sk_surfaceprops_t param2);


    // sk_surface_t* sk_surface_new_raster_direct(const sk_imageinfo_t*, void* pixels, size_t rowBytes, const sk_surface_raster_release_proc releaseProc, void* context, const sk_surfaceprops_t* props)

    [LibraryImport(SKIA)]
    internal static partial sk_surface_t sk_surface_new_raster_direct(SKImageInfoNative* param0,
        void* pixels, /* size_t */ IntPtr rowBytes, SKSurfaceRasterReleaseProxyDelegate releaseProc, void* context,
        sk_surfaceprops_t props);


    // sk_surface_t* sk_surface_new_render_target(gr_recording_context_t* context, bool budgeted, const sk_imageinfo_t* cinfo, int sampleCount, gr_surfaceorigin_t origin, const sk_surfaceprops_t* props, bool shouldCreateWithMips)

    [LibraryImport(SKIA)]
    internal static partial sk_surface_t sk_surface_new_render_target(gr_recording_context_t context,
        [MarshalAs(UnmanagedType.I1)] bool budgeted, SKImageInfoNative* cinfo, Int32 sampleCount, SurfaceOrigin origin,
        sk_surfaceprops_t props, [MarshalAs(UnmanagedType.I1)] bool shouldCreateWithMips);


    // bool sk_surface_peek_pixels(sk_surface_t* surface, sk_pixmap_t* pixmap)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_surface_peek_pixels(sk_surface_t surface, sk_pixmap_t pixmap);


    // bool sk_surface_read_pixels(sk_surface_t* surface, sk_imageinfo_t* dstInfo, void* dstPixels, size_t dstRowBytes, int srcX, int srcY)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_surface_read_pixels(sk_surface_t surface, SKImageInfoNative* dstInfo,
        void* dstPixels, /* size_t */ IntPtr dstRowBytes, Int32 srcX, Int32 srcY);


    // void sk_surface_unref(sk_surface_t*)

    [LibraryImport(SKIA)]
    internal static partial void sk_surface_unref(sk_surface_t param0);


    // void sk_surfaceprops_delete(sk_surfaceprops_t* props)

    [LibraryImport(SKIA)]
    internal static partial void sk_surfaceprops_delete(sk_surfaceprops_t props);


    // uint32_t sk_surfaceprops_get_flags(sk_surfaceprops_t* props)

    [LibraryImport(SKIA)]
    internal static partial UInt32 sk_surfaceprops_get_flags(sk_surfaceprops_t props);


    // sk_pixelgeometry_t sk_surfaceprops_get_pixel_geometry(sk_surfaceprops_t* props)

    [LibraryImport(SKIA)]
    internal static partial SKPixelGeometry sk_surfaceprops_get_pixel_geometry(sk_surfaceprops_t props);


    // sk_surfaceprops_t* sk_surfaceprops_new(uint32_t flags, sk_pixelgeometry_t geometry)

    [LibraryImport(SKIA)]
    internal static partial sk_surfaceprops_t sk_surfaceprops_new(UInt32 flags, SKPixelGeometry geometry);
}