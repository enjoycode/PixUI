namespace PixUI;

public interface IShader : IDisposable { }

public static class Shader
{
    public static IShader? CreateRadialGradient(Point center, float radius, Color[] colors, float[]? colorPos,
        TileMode mode) => Render.Backend.MakeShaderRadialGradient(center, radius, colors, colorPos, mode);

    public static IShader? CreateLinearGradient(Point start, Point end, Color[] colors, float[]? colorPos,
        TileMode mode) => Render.Backend.MakeShaderLinearGradient(start, end, colors, colorPos, mode);
}