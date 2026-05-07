namespace PixUI.Drawing.Skia;

public sealed class SKImage : SKObject, ISKReferenceCounted, IImage
{
    private SKImage(IntPtr x, bool owns) : base(x, owns) { }

    public int Width => SkiaApi.sk_image_get_width(Handle);

    public int Height => SkiaApi.sk_image_get_height(Handle);

    public uint UniqueId => SkiaApi.sk_image_get_unique_id(Handle);

    public AlphaType AlphaType => SkiaApi.sk_image_get_alpha_type(Handle);

    public bool IsTextureBacked => SkiaApi.sk_image_is_texture_backed(Handle);
    public bool IsLazyGenerated => SkiaApi.sk_image_is_lazy_generated(Handle);

    internal static SKImage? GetObject(IntPtr handle) =>
        GetOrAddObject(handle, (h, o) => new SKImage(h, o));

    public static SKImage? FromEncodedData(SKData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var handle = SkiaApi.sk_image_new_from_encoded(data.Handle);
        return GetObject(handle);
    }

    public static SKImage? FromEncodedData(byte[] data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (data.Length == 0)
            throw new ArgumentException("The data buffer was empty.");

        using var skdata = SKData.CreateCopy(data);
        return FromEncodedData(skdata)!;
    }

    public static SKImage? FromEncodedData(Stream data)
    {
        using var skdata = SKData.Create(data);
        return skdata == null ? null : FromEncodedData(skdata);
    }

    public static unsafe SKImage FromPicture(SKPicture picture, SizeI dimensions) =>
        FromPicture(picture, dimensions, null, null);

    private static unsafe SKImage FromPicture(SKPicture picture, SizeI dimensions, Matrix3* matrix, SKPaint? paint)
    {
        if (picture == null)
            throw new ArgumentNullException(nameof(picture));

        var p = paint?.Handle ?? IntPtr.Zero;
        return GetObject(SkiaApi.sk_image_new_from_picture(picture.Handle, &dimensions, matrix, p))!;
    }

    public SKData Encode(EncodedImageFormat format, int quality)
    {
        //TODO: remove this or use `SkPngEncoder::Encode`, `SkJpegEncoder::Encode`, `SkWebpEncoder::Encode`
        throw new NotImplementedException();
    }

    #region ====ToShader====

    public IShader? ToShader() => ToShader(TileMode.Clamp, TileMode.Clamp);

    public unsafe IShader? ToShader(TileMode tileX, TileMode tileY) =>
        SKShader.GetObject(SkiaApi.sk_image_make_shader(Handle, tileX, tileY, null));

    public unsafe IShader? ToShader(TileMode tileX, TileMode tileY, Matrix3 localMatrix) =>
        SKShader.GetObject(SkiaApi.sk_image_make_shader(Handle, tileX, tileY, &localMatrix));

    #endregion
}