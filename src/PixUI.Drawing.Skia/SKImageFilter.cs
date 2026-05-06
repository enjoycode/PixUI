namespace PixUI.Drawing.Skia;

public sealed class SKImageFilter : SKObject, ISKReferenceCounted, IImageFilter
{
    private SKImageFilter(IntPtr handle, bool owns) : base(handle, owns) { }

    internal static SKImageFilter? GetObject(IntPtr handle) =>
        GetOrAddObject(handle, (h, o) => new SKImageFilter(h, o))!;

    public static SKImageFilter? CreateBlendMode(BlendMode mode, SKImageFilter? background, SKImageFilter? foreground)
    {
        return GetObject(SkiaApi.sk_imagefilter_new_xfermode(mode,
            background?.Handle ?? IntPtr.Zero,
            foreground?.Handle ?? IntPtr.Zero, IntPtr.Zero));
    }

    public static SKImageFilter? CreateBlur(float sigmaX, float sigmaY, TileMode tileMode,
        SKImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_blur(sigmaX, sigmaY, tileMode,
            input?.Handle ?? IntPtr.Zero, IntPtr.Zero));

    public static SKImageFilter? CreateColorFilter(SKColorFilter cf, SKImageFilter? input)
    {
        if (cf == null)
            throw new ArgumentNullException(nameof(cf));
        return GetObject(
            SkiaApi.sk_imagefilter_new_color_filter(cf.Handle, input?.Handle ?? IntPtr.Zero, IntPtr.Zero));
    }

    public static SKImageFilter? CreateCompose(SKImageFilter? outer, SKImageFilter? inner) =>
        GetObject(SkiaApi.sk_imagefilter_new_compose(outer?.Handle ?? IntPtr.Zero, inner?.Handle ?? IntPtr.Zero));

    public static SKImageFilter? CreateDilate(float radiusX, float radiusY, SKImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_dilate(radiusX, radiusY, input?.Handle ?? IntPtr.Zero, IntPtr.Zero));

    public static SKImageFilter? CreateDisplacementMapEffect(ColorChannel xChannel,
        ColorChannel yChannel, float scale, SKImageFilter? displacement, SKImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_displacement_map_effect(xChannel, yChannel,
            scale, displacement?.Handle ?? IntPtr.Zero, input?.Handle ?? IntPtr.Zero,
            IntPtr.Zero));

    public static SKImageFilter? CreateDropShadow(float dx, float dy, float sigmaX, float sigmaY, Color color,
        SKImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_drop_shadow(dx, dy, sigmaX, sigmaY, (uint)color,
            input?.Handle ?? IntPtr.Zero, IntPtr.Zero));

    public static SKImageFilter? CreateDropShadowOnly(float dx, float dy, float sigmaX, float sigmaY, Color color,
        SKImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_drop_shadow_only(dx, dy, sigmaX, sigmaY, (uint)color,
            input?.Handle ?? IntPtr.Zero, IntPtr.Zero));

    public static SKImageFilter? CreateErode(float radiusX, float radiusY, SKImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_erode(radiusX, radiusY, input?.Handle ?? IntPtr.Zero,
            IntPtr.Zero));

    //TODO: others
}