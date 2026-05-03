namespace PixUI;

public static class Image
{
    public static IImage? FromEncodedData(byte[] data) => Render.Provider.ImageFromEncodedData(data);

    public static IImage? FromEncodedData(Stream data) => Render.Provider.ImageFromEncodedData(data);
}