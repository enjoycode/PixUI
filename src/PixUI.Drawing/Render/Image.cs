namespace PixUI;

public interface IImage : IDisposable
{
    AlphaType AlphaType { get; }

    int Width { get; }

    int Height { get; }
}

public static class Image
{
    public static IImage? FromEncodedData(byte[] data) => Render.Provider.ImageFromEncodedData(data);

    public static IImage? FromEncodedData(Stream data) => Render.Provider.ImageFromEncodedData(data);
}