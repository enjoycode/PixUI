namespace PixUI.Drawing.Skia;

public sealed unsafe class SKColorFilter : SKObject, ISKReferenceCounted
{
    private SKColorFilter(IntPtr handle, bool owns) : base (handle, owns) { }
        
    internal static SKColorFilter? GetObject (IntPtr handle) =>
        GetOrAddObject (handle, (h, o) => new SKColorFilter (h, o))!;
        
    //TODO: others to fit CanvasKit
        
    public static SKColorFilter? CreateBlendMode(Color c, BlendMode mode) 
        => GetObject (SkiaApi.sk_colorfilter_new_mode((uint)c, mode));
        
    public static SKColorFilter? CreateCompose(SKColorFilter outer, SKColorFilter inner)
    {
        if (outer == null)
            throw new ArgumentNullException(nameof(outer));
        if (inner == null)
            throw new ArgumentNullException(nameof(inner));
        return GetObject (SkiaApi.sk_colorfilter_new_compose(outer.Handle, inner.Handle));
    }
        
    public static SKColorFilter? CreateLumaColor() => GetObject (SkiaApi.sk_colorfilter_new_luma_color());
        
    public static SKColorFilter? CreateColorMatrix(float[] matrix)
    {
        if (matrix == null)
            throw new ArgumentNullException(nameof(matrix));
        if (matrix.Length != 20)
            throw new ArgumentException("Matrix must have a length of 20.", nameof(matrix));
        fixed (float* m = matrix) {
            return GetObject (SkiaApi.sk_colorfilter_new_color_matrix (m));
        }
    }
}