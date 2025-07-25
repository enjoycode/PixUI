namespace PixUI;

public sealed unsafe class SKDocument : SKObject, ISKReferenceCounted, ISKSkipObjectRegistration
{
    public const float DefaultRasterDpi = 72.0f;

    internal SKDocument(IntPtr handle, bool owns)
        : base(handle, owns) { }

    protected override void Dispose(bool disposing) =>
        base.Dispose(disposing);

    public void Abort() =>
        SkiaApi.sk_document_abort(Handle);

    public Canvas BeginPage(float width, float height) =>
        OwnedBy(Canvas.GetObject(SkiaApi.sk_document_begin_page(Handle, width, height, null), false), this);

    public Canvas BeginPage(float width, float height, Rect content) =>
        OwnedBy(Canvas.GetObject(SkiaApi.sk_document_begin_page(Handle, width, height, &content), false), this);

    public void EndPage() =>
        SkiaApi.sk_document_end_page(Handle);

    public void Close() =>
        SkiaApi.sk_document_close(Handle);

    // CreateXps

    public static SKDocument CreateXps(string path) =>
        CreateXps(path, DefaultRasterDpi);

    public static SKDocument CreateXps(Stream stream) =>
        CreateXps(stream, DefaultRasterDpi);

    public static SKDocument CreateXps(SKWStream stream) =>
        CreateXps(stream, DefaultRasterDpi);

    public static SKDocument CreateXps(string path, float dpi)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        var stream = SKFileWStream.OpenStream(path);
        return Owned(CreateXps(stream, dpi), stream);
    }

    public static SKDocument CreateXps(Stream stream, float dpi)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        var managed = new SKManagedWStream(stream);
        return Owned(CreateXps(managed, dpi), managed);
    }

    public static SKDocument CreateXps(SKWStream stream, float dpi)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        return Referenced(GetObject(SkiaApi.sk_document_create_xps_from_stream(stream.Handle, dpi)), stream);
    }

    // CreatePdf

    public static SKDocument CreatePdf(string path)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        var stream = SKFileWStream.OpenStream(path);
        return Owned(CreatePdf(stream), stream)!;
    }

    public static SKDocument CreatePdf(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        var managed = new SKManagedWStream(stream);
        return Owned(CreatePdf(managed), managed)!;
    }

    public static SKDocument CreatePdf(SKWStream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        return Referenced(GetObject(SkiaApi.sk_document_create_pdf_from_stream(stream.Handle)), stream)!;
    }

    public static SKDocument CreatePdf(string path, float dpi) =>
        CreatePdf(path, new SKDocumentPdfMetadata(dpi));

    public static SKDocument CreatePdf(Stream stream, float dpi) =>
        CreatePdf(stream, new SKDocumentPdfMetadata(dpi));

    public static SKDocument CreatePdf(SKWStream stream, float dpi) =>
        CreatePdf(stream, new SKDocumentPdfMetadata(dpi));

    public static SKDocument CreatePdf(string path, SKDocumentPdfMetadata metadata)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));

        var stream = SKFileWStream.OpenStream(path);
        return Owned(CreatePdf(stream, metadata), stream)!;
    }

    public static SKDocument CreatePdf(Stream stream, SKDocumentPdfMetadata metadata)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        var managed = new SKManagedWStream(stream);
        return Owned(CreatePdf(managed, metadata), managed)!;
    }

    public static SKDocument CreatePdf(SKWStream stream, SKDocumentPdfMetadata metadata)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        using var title = new SKString(metadata.Title ?? string.Empty);
        using var author = new SKString(metadata.Author ?? string.Empty);
        using var subject = new SKString(metadata.Subject ?? string.Empty);
        using var keywords = new SKString(metadata.Keywords ?? string.Empty);
        using var creator = new SKString(metadata.Creator ?? string.Empty);
        using var producer = new SKString(metadata.Producer ?? string.Empty);

        var cmetadata = new SKDocumentPdfMetadataInternal
        {
            fTitle = title?.Handle ?? IntPtr.Zero,
            fAuthor = author?.Handle ?? IntPtr.Zero,
            fSubject = subject?.Handle ?? IntPtr.Zero,
            fKeywords = keywords?.Handle ?? IntPtr.Zero,
            fCreator = creator?.Handle ?? IntPtr.Zero,
            fProducer = producer?.Handle ?? IntPtr.Zero,
            fRasterDPI = metadata.RasterDpi,
            fPDFA = metadata.PdfA ? (byte)1 : (byte)0,
            fEncodingQuality = metadata.EncodingQuality,
        };

        // SKTimeDateTimeInternal creation;
        // if (metadata.Creation != null)
        // {
        //     creation = SKTimeDateTimeInternal.Create(metadata.Creation.Value);
        //     cmetadata.fCreation = &creation;
        // }
        //
        // SKTimeDateTimeInternal modified;
        // if (metadata.Modified != null)
        // {
        //     modified = SKTimeDateTimeInternal.Create(metadata.Modified.Value);
        //     cmetadata.fModified = &modified;
        // }

        return Referenced(
            GetObject(SkiaApi.sk_document_create_pdf_from_stream_with_metadata(stream.Handle, &cmetadata)), stream)!;
    }

    private static SKDocument? GetObject(IntPtr handle) =>
        handle == IntPtr.Zero ? null : new SKDocument(handle, true);
}