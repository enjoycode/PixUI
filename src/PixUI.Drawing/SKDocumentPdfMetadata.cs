namespace PixUI;

public struct SKDocumentPdfMetadata : IEquatable<SKDocumentPdfMetadata>
{
    public const float DefaultRasterDpi = SKDocument.DefaultRasterDpi;
    public const int DefaultEncodingQuality = 101;

    public static readonly SKDocumentPdfMetadata Default;

    static SKDocumentPdfMetadata()
    {
        Default = new SKDocumentPdfMetadata()
        {
            RasterDpi = DefaultRasterDpi,
            PdfA = false,
            EncodingQuality = 101,
        };
    }

    public SKDocumentPdfMetadata(float rasterDpi)
    {
        Title = null;
        Author = null;
        Subject = null;
        Keywords = null;
        Creator = null;
        Producer = null;
        Creation = null;
        Modified = null;
        RasterDpi = rasterDpi;
        PdfA = false;
        EncodingQuality = DefaultEncodingQuality;
    }

    public SKDocumentPdfMetadata(int encodingQuality)
    {
        Title = null;
        Author = null;
        Subject = null;
        Keywords = null;
        Creator = null;
        Producer = null;
        Creation = null;
        Modified = null;
        RasterDpi = DefaultRasterDpi;
        PdfA = false;
        EncodingQuality = encodingQuality;
    }

    public SKDocumentPdfMetadata(float rasterDpi, int encodingQuality)
    {
        Title = null;
        Author = null;
        Subject = null;
        Keywords = null;
        Creator = null;
        Producer = null;
        Creation = null;
        Modified = null;
        RasterDpi = rasterDpi;
        PdfA = false;
        EncodingQuality = encodingQuality;
    }

    public string? Title { readonly get; set; }
    public string? Author { readonly get; set; }
    public string? Subject { readonly get; set; }
    public string? Keywords { readonly get; set; }
    public string? Creator { readonly get; set; }
    public string? Producer { readonly get; set; }
    public DateTime? Creation { readonly get; set; }
    public DateTime? Modified { readonly get; set; }
    public float RasterDpi { readonly get; set; }
    public bool PdfA { readonly get; set; }
    public int EncodingQuality { readonly get; set; }

    public readonly bool Equals(SKDocumentPdfMetadata obj) =>
        Title == obj.Title &&
        Author == obj.Author &&
        Subject == obj.Subject &&
        Keywords == obj.Keywords &&
        Creator == obj.Creator &&
        Producer == obj.Producer &&
        Creation == obj.Creation &&
        Modified == obj.Modified &&
        RasterDpi == obj.RasterDpi &&
        PdfA == obj.PdfA &&
        EncodingQuality == obj.EncodingQuality;

    public readonly override bool Equals(object obj) =>
        obj is SKDocumentPdfMetadata f && Equals(f);

    public static bool operator ==(SKDocumentPdfMetadata left, SKDocumentPdfMetadata right) =>
        left.Equals(right);

    public static bool operator !=(SKDocumentPdfMetadata left, SKDocumentPdfMetadata right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Title);
        hash.Add(Author);
        hash.Add(Subject);
        hash.Add(Keywords);
        hash.Add(Creator);
        hash.Add(Producer);
        hash.Add(Creation);
        hash.Add(Modified);
        hash.Add(RasterDpi);
        hash.Add(PdfA);
        hash.Add(EncodingQuality);
        return hash.ToHashCode();
    }
}