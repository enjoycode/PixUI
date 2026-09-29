using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // sk_canvas_t* sk_svgcanvas_create_with_stream(const sk_rect_t* bounds, sk_wstream_t* stream)

    [LibraryImport(SKIA)]
    internal static partial sk_canvas_t sk_svgcanvas_create_with_stream(Rect* bounds, sk_wstream_t stream);


    // sk_canvas_t* sk_svgcanvas_create_with_writer(const sk_rect_t* bounds, sk_xmlwriter_t* writer)

    [LibraryImport(SKIA)]
    internal static partial sk_canvas_t sk_svgcanvas_create_with_writer(Rect* bounds, sk_xmlwriter_t writer);
}