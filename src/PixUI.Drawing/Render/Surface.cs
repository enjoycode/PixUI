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
    public static ISurface Create(ImageInfo info) => Render.Backend.MakeSurface(info);
}