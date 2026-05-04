namespace PixUI;

public interface IImageFilter : IDisposable { }

public static class ImageFilter
{
    public static IImageFilter? CreateDropShadow(float dx, float dy, float sigmaX, float sigmaY,
        Color color, IImageFilter? input)
        => Render.Backend.MakeImageFilterDropShadow(dx, dy, sigmaX, sigmaY, color, input);

    public static IImageFilter? CreateBlur(float sigmaX, float sigmaY, TileMode tileMode, IImageFilter? input)
        => Render.Backend.MakeImageFilterBlur(sigmaX, sigmaY, tileMode, input);
}