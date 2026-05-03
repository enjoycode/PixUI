namespace PixUI;

public readonly struct ImageInfo : IEquatable<ImageInfo>
{
    // public static readonly ImageInfo Empty;
    private static readonly ColorType PlatformColorType;
    // public static readonly int PlatformColorAlphaShift;
    // public static readonly int PlatformColorRedShift;
    // public static readonly int PlatformColorGreenShift;
    // public static readonly int PlatformColorBlueShift;

    static ImageInfo()
    {
        PlatformColorType = ColorType.Bgra8888;

        //fixed (int* a = &PlatformColorAlphaShift)
        //fixed (int* r = &PlatformColorRedShift)
        //fixed (int* g = &PlatformColorGreenShift)
        //fixed (int* b = &PlatformColorBlueShift)
        //{
        //    SkiaApi.sk_color_get_bit_shift(a, r, g, b);
        //}
    }

    public ImageInfo() { }

    public int Width { get; init; } = 0;

    public int Height { get; init; } = 0;

    public ColorType ColorType { get; init; } = PlatformColorType;

    public AlphaType AlphaType { get; init; } = AlphaType.Premul;

    public IColorSpace? ColorSpace { get; init; } = null;

    public int BytesPerPixel => ColorType.GetBytesPerPixel();

    // public readonly int BitsPerPixel => BytesPerPixel * 8;

    public int BytesSize => Width * Height * BytesPerPixel;

    // public readonly long BytesSize64 => (long)Width * (long)Height * (long)BytesPerPixel;
    //
    // public readonly int RowBytes => Width * BytesPerPixel;
    //
    // public readonly long RowBytes64 => (long)Width * (long)BytesPerPixel;
    //
    // public readonly bool IsEmpty => Width <= 0 || Height <= 0;
    //
    // public readonly bool IsOpaque => AlphaType == AlphaType.Opaque;

    // public readonly SKSizeI Size => new SKSizeI (Width, Height);
    //
    // public readonly SKRectI Rect => SKRectI.Create (Width, Height);

    public bool Equals(ImageInfo obj) =>
        ColorSpace == obj.ColorSpace &&
        Width == obj.Width &&
        Height == obj.Height &&
        ColorType == obj.ColorType &&
        AlphaType == obj.AlphaType;

    public override bool Equals(object? obj) => obj is ImageInfo f && Equals(f);

    public static bool operator ==(ImageInfo left, ImageInfo right) => left.Equals(right);

    public static bool operator !=(ImageInfo left, ImageInfo right) => !left.Equals(right);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(ColorSpace);
        hash.Add(Width);
        hash.Add(Height);
        hash.Add(ColorType);
        hash.Add(AlphaType);
        return hash.ToHashCode();
    }
}