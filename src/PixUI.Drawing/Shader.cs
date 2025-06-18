#if !__WEB__
using System;

namespace PixUI;

public sealed unsafe class Shader : SKObject, ISKReferenceCounted
{
    private Shader(IntPtr handle, bool owns) : base(handle, owns) { }

    internal static Shader? GetObject(IntPtr handle) =>
        GetOrAddObject(handle, (h, o) => new Shader(h, o));

    //TODO: others，另外注意参数需与CanvasKit一致

    public static Shader? CreateLinearGradient(Point start, Point end, Color[] colors, float[]? colorPos,
        TileMode mode)
    {
        if (colors == null)
            throw new ArgumentNullException(nameof(colors));
        if (colorPos != null && colors.Length != colorPos.Length)
            throw new ArgumentException("The number of colors must match the number of color positions.");

        var points = stackalloc Point[] { start, end };
        fixed (Color* c = colors)
        fixed (float* cp = colorPos)
        {
            return GetObject(
                SkiaApi.sk_shader_new_linear_gradient(points, (uint*)c, cp, colors.Length, mode, null));
        }
    }

    public static Shader? CreateRadialGradient(Point center, float radius, Color[] colors, float[]? colorPos,
        TileMode mode)
    {
        if (colors == null)
            throw new ArgumentNullException(nameof(colors));
        if (colorPos != null && colors.Length != colorPos.Length)
            throw new ArgumentException("The number of colors must match the number of color positions.");

        fixed (Color* c = colors)
        fixed (float* cp = colorPos)
        {
            return GetObject(SkiaApi.sk_shader_new_radial_gradient(&center, radius, (uint*)c, cp, colors.Length,
                mode, null));
        }
    }

    public static Shader? CreateImage(Image src) => CreateImage(src, TileMode.Clamp, TileMode.Clamp);

    public static Shader? CreateImage(Image src, TileMode tmx, TileMode tmy)
    {
        if (src == null)
            throw new ArgumentNullException(nameof(src));
        return src.ToShader(tmx, tmy);
    }

    public static Shader? CreateImage(Image src, TileMode tmx, TileMode tmy, Matrix3 localMatrix)
    {
        if (src == null)
            throw new ArgumentNullException(nameof(src));
        return src.ToShader(tmx, tmy, localMatrix);
    }
}
#endif