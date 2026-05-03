#if !__WEB__
namespace PixUI;

public unsafe class SKPathEffect : SKObject, ISKReferenceCounted, IPathEffect
{
    private SKPathEffect(IntPtr handle, bool owns) : base(handle, owns) { }

    internal static SKPathEffect? GetObject(IntPtr handle) =>
        GetOrAddObject(handle, (h, o) => new SKPathEffect(h, o));

    public static SKPathEffect? CreateDiscrete(float segLength, float deviation, uint seedAssist)
    {
        return GetObject(SkiaApi.sk_path_effect_create_discrete(segLength, deviation, seedAssist));
    }

    public static SKPathEffect? CreateCorner(float radius)
    {
        return GetObject(SkiaApi.sk_path_effect_create_corner(radius));
    }

    public static SKPathEffect? Create1DPath(SKPath path, float advance, float phase, Path1DPathEffectStyle style)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));
        return GetObject(SkiaApi.sk_path_effect_create_1d_path(path.Handle, advance, phase, style));
    }

    public static SKPathEffect? Create2DLine(float width, Matrix3 matrix)
    {
        return GetObject(SkiaApi.sk_path_effect_create_2d_line(width, &matrix));
    }

    public static SKPathEffect? Create2DPath(Matrix3 matrix, SKPath path)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));
        return GetObject(SkiaApi.sk_path_effect_create_2d_path(&matrix, path.Handle));
    }

    public static SKPathEffect? CreateDash(float[] intervals, float phase)
    {
        if (intervals == null)
            throw new ArgumentNullException(nameof(intervals));
        if (intervals.Length % 2 != 0)
            throw new ArgumentException("The intervals must have an even number of entries.", nameof(intervals));
        fixed (float* i = intervals)
        {
            return GetObject(SkiaApi.sk_path_effect_create_dash(i, intervals.Length, phase));
        }
    }

    #region ====CanvasKit不支持的====

    // public static PathEffect CreateCompose(PathEffect outer, PathEffect inner)
    // {
    //     if (outer == null)
    //         throw new ArgumentNullException(nameof(outer));
    //     if (inner == null)
    //         throw new ArgumentNullException(nameof(inner));
    //     return GetObject(SkiaApi.sk_path_effect_create_compose(outer.Handle, inner.Handle));
    // }

    // public static PathEffect CreateSum(PathEffect first, PathEffect second)
    // {
    //     if (first == null)
    //         throw new ArgumentNullException(nameof(first));
    //     if (second == null)
    //         throw new ArgumentNullException(nameof(second));
    //     return GetObject(SkiaApi.sk_path_effect_create_sum(first.Handle, second.Handle));
    // }

    // public static SKPathEffect CreateTrim(float start, float stop)
    // {
    //     return CreateTrim(start, stop, SKTrimPathEffectMode.Normal);
    // }

    // public static SKPathEffect CreateTrim(float start, float stop, SKTrimPathEffectMode mode)
    // {
    //     return GetObject(SkiaApi.sk_path_effect_create_trim(start, stop, mode));
    // }

    #endregion
}
#endif