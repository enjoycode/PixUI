using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

partial class SkiaApi
{
    // void sk_xmlstreamwriter_delete(sk_xmlstreamwriter_t* writer)

    [LibraryImport(SKIA)]
    internal static partial void sk_xmlstreamwriter_delete(sk_xmlstreamwriter_t writer);


    // sk_xmlstreamwriter_t* sk_xmlstreamwriter_new(sk_wstream_t* stream)

    [LibraryImport(SKIA)]
    internal static partial sk_xmlstreamwriter_t sk_xmlstreamwriter_new(sk_wstream_t stream);
}