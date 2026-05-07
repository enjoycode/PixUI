namespace PixUI;

public interface IImage : IDisposable
{
    AlphaType AlphaType { get; }

    int Width { get; }

    int Height { get; }

    IShader? ToShader(TileMode tileX, TileMode tileY);
}

public interface IPicture : IDisposable
{
    Rect CullRect { get; }
}

public static class Image
{
    public static IImage? FromEncodedData(byte[] data) => Render.Backend.MakeImageFromEncodedData(data);

    public static IImage? FromEncodedData(Stream data) => Render.Backend.MakeImageFromEncodedData(data);

    public static IImage FromPicture(IPicture picture, SizeI size) =>
        Render.Backend.MakeImageFromPicture(picture, size);
}