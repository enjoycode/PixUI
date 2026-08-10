namespace PixUI;

public interface IColorFilter : IDisposable { }

public static class ColorFilter
{
    public static IColorFilter? CreateBlendMode(Color c, BlendMode mode) =>
        Render.Backend.MakeColorFilterBlendMode(c, mode);
}