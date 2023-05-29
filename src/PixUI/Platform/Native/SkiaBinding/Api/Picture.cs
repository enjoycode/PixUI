using System;
using System.IO;

#if !__WEB__
namespace PixUI;

public sealed unsafe class Picture : SKObject, ISKReferenceCounted
{
    internal Picture(IntPtr h, bool owns) : base(h, owns) { }

    protected override void Dispose(bool disposing) =>
        base.Dispose(disposing);

    public uint UniqueId => SkiaApi.sk_picture_get_unique_id(Handle);

    public Rect CullRect
    {
        get
        {
            Rect rect;
            SkiaApi.sk_picture_get_cull_rect(Handle, &rect);
            return rect;
        }
    }

    // Serialize

    public SKData Serialize() => SKData.GetObject(SkiaApi.sk_picture_serialize_to_data(Handle));

    public void Serialize(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        using var managed = new SKManagedWStream(stream);
        Serialize(managed);
    }

    public void Serialize(SKWStream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        SkiaApi.sk_picture_serialize_to_stream(Handle, stream.Handle);
    }

    // ToShader

    // public SKShader ToShader() =>
    //     ToShader(SKShaderTileMode.Clamp, SKShaderTileMode.Clamp);
    //
    // public SKShader ToShader(SKShaderTileMode tmx, SKShaderTileMode tmy) =>
    //     SKShader.GetObject(SkiaApi.sk_picture_make_shader(Handle, tmx, tmy, null, null));
    //
    // public SKShader ToShader(SKShaderTileMode tmx, SKShaderTileMode tmy, SKRect tile) =>
    //     SKShader.GetObject(SkiaApi.sk_picture_make_shader(Handle, tmx, tmy, null, &tile));
    //
    // public SKShader ToShader(SKShaderTileMode tmx, SKShaderTileMode tmy, SKMatrix localMatrix, SKRect tile) =>
    //     SKShader.GetObject(SkiaApi.sk_picture_make_shader(Handle, tmx, tmy, &localMatrix, &tile));

    // Deserialize

    public static Picture? Deserialize(IntPtr data, int length)
    {
        if (data == IntPtr.Zero)
            throw new ArgumentNullException(nameof(data));

        if (length == 0)
            return null;

        return GetObject(SkiaApi.sk_picture_deserialize_from_memory((void*)data, (IntPtr)length));
    }

    public static Picture? Deserialize(ReadOnlySpan<byte> data)
    {
        if (data.Length == 0)
            return null;

        fixed (void* ptr = data)
        {
            return GetObject(SkiaApi.sk_picture_deserialize_from_memory(ptr, (IntPtr)data.Length));
        }
    }

    public static Picture? Deserialize(SKData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        return GetObject(SkiaApi.sk_picture_deserialize_from_data(data.Handle));
    }

    public static Picture? Deserialize(Stream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        using var managed = new SKManagedStream(stream);
        return Deserialize(managed);
    }

    public static Picture? Deserialize(SKStream stream)
    {
        if (stream == null)
            throw new ArgumentNullException(nameof(stream));

        return GetObject(SkiaApi.sk_picture_deserialize_from_stream(stream.Handle));
    }

    //

    internal static Picture? GetObject(IntPtr handle, bool owns = true, bool unrefExisting = true) =>
        GetOrAddObject(handle, owns, unrefExisting, (h, o) => new Picture(h, o));
}
#endif