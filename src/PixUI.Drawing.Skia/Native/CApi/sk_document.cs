using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_document_abort(sk_document_t* document)

    [LibraryImport(SKIA)]
    internal static partial void sk_document_abort(sk_document_t document);


    // sk_canvas_t* sk_document_begin_page(sk_document_t* document, float width, float height, const sk_rect_t* content)

    [LibraryImport(SKIA)]
    internal static partial sk_canvas_t sk_document_begin_page(sk_document_t document, Single width, Single height,
        Rect* content);


    // void sk_document_close(sk_document_t* document)

    [LibraryImport(SKIA)]
    internal static partial void sk_document_close(sk_document_t document);


    // sk_document_t* sk_document_create_pdf_from_stream(sk_wstream_t* stream)

    [LibraryImport(SKIA)]
    internal static partial sk_document_t sk_document_create_pdf_from_stream(sk_wstream_t stream);


    // sk_document_t* sk_document_create_pdf_from_stream_with_metadata(sk_wstream_t* stream, const sk_document_pdf_metadata_t* metadata)

    [LibraryImport(SKIA)]
    internal static partial sk_document_t sk_document_create_pdf_from_stream_with_metadata(sk_wstream_t stream,
        SKDocumentPdfMetadataInternal* metadata);


    // sk_document_t* sk_document_create_xps_from_stream(sk_wstream_t* stream, float dpi)

    [LibraryImport(SKIA)]
    internal static partial sk_document_t sk_document_create_xps_from_stream(sk_wstream_t stream, Single dpi);


    // void sk_document_end_page(sk_document_t* document)

    [LibraryImport(SKIA)]
    internal static partial void sk_document_end_page(sk_document_t document);


    // void sk_document_unref(sk_document_t* document)

    [LibraryImport(SKIA)]
    internal static partial void sk_document_unref(sk_document_t document);
}