namespace PixUI;

public interface IPathEffect : IDisposable { }

public static class PathEffect
{
    public static IPathEffect? CreateDash(float[] intervals, float phase)
        => Render.Backend.MakePathEffectDash(intervals, phase);

    public static IPathEffect? CreateCorner(float radius)
        => Render.Backend.MakePathEffectCorner(radius);
}