namespace PixUI;

public interface ISurfaceProperties : IDisposable { }

public interface ISurface : IDisposable
{
    ICanvas Canvas { get; }

    IImage Snapshot();

    IImage Snapshot(RectI bounds);

    void Draw(ICanvas canvas, float x, float y, IPaint? paint);
}

public static class Surface
{
    /// <summary>
    /// Create a Raster surface
    /// </summary>
    public static ISurface Create(ImageInfo info) => Render.Backend.MakeSurface(info);

    /// <summary>
    /// Create a Raster surface
    /// </summary>
    public static ISurface Create(ImageInfo info, IntPtr pixels, int rowBytes) =>
        Render.Backend.MakeSurface(info, pixels, rowBytes);

    /// <summary>
    /// Create a new gpu surface (like a Texture)
    /// </summary>
    public static ISurface? Create(IGRContext context, bool budgeted, ImageInfo info,
        int sampleCount = 0, SurfaceOrigin origin = SurfaceOrigin.TopLeft,
        ISurfaceProperties? props = null, bool shouldCreateWithMips = false)
        => Render.Backend.MakeSurface(context, budgeted, info, sampleCount, origin, props, shouldCreateWithMips);

    /// <summary>
    /// Create a WebGL onscreen surface
    /// </summary>
    public static ISurface? CreateForWebGL(IGRContext context, int width, int height) =>
        Render.Backend.MakeSurfaceForWebGL(context, width, height);
}