namespace PixUI;

public interface IImageFilter : IDisposable { }

public static class ImageFilter
{
    public static IImageFilter? CreateDropShadow(float dx, float dy, float sigmaX, float sigmaY,
        Color color, IImageFilter? input)
        => Render.Provider.ImageFilterCreateDropShadow(dx, dy, sigmaX, sigmaY, color, input);

    public static IImageFilter? CreateBlur(float sigmaX, float sigmaY, TileMode tileMode, IImageFilter? input)
        => Render.Provider.ImageFilterCreateBlur(sigmaX, sigmaY, tileMode, input);
}