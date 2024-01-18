#if !__WEB__
using System;

namespace PixUI;

public sealed class ImageFilter : SKObject, ISKReferenceCounted
{
    private ImageFilter(IntPtr handle, bool owns) : base(handle, owns) { }

    internal static ImageFilter? GetObject(IntPtr handle) =>
        GetOrAddObject(handle, (h, o) => new ImageFilter(h, o))!;

    public static ImageFilter? CreateBlendMode(BlendMode mode, ImageFilter? background, ImageFilter? foreground)
    {
        return GetObject(SkiaApi.sk_imagefilter_new_xfermode(mode,
            background?.Handle ?? IntPtr.Zero,
            foreground?.Handle ?? IntPtr.Zero, IntPtr.Zero));
    }

    public static ImageFilter? CreateBlur(float sigmaX, float sigmaY, TileMode tileMode,
        ImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_blur(sigmaX, sigmaY, tileMode,
            input?.Handle ?? IntPtr.Zero, IntPtr.Zero));

    public static ImageFilter? CreateColorFilter(ColorFilter cf, ImageFilter? input)
    {
        if (cf == null)
            throw new ArgumentNullException(nameof(cf));
        return GetObject(
            SkiaApi.sk_imagefilter_new_color_filter(cf.Handle, input?.Handle ?? IntPtr.Zero, IntPtr.Zero));
    }

    public static ImageFilter? CreateCompose(ImageFilter? outer, ImageFilter? inner) =>
        GetObject(SkiaApi.sk_imagefilter_new_compose(outer?.Handle ?? IntPtr.Zero, inner?.Handle ?? IntPtr.Zero));

    public static ImageFilter? CreateDilate(float radiusX, float radiusY, ImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_dilate(radiusX, radiusY, input?.Handle ?? IntPtr.Zero, IntPtr.Zero));

    public static ImageFilter? CreateDisplacementMapEffect(ColorChannel xChannel,
        ColorChannel yChannel, float scale, ImageFilter? displacement, ImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_displacement_map_effect(xChannel, yChannel,
            scale, displacement?.Handle ?? IntPtr.Zero, input?.Handle ?? IntPtr.Zero,
            IntPtr.Zero));

    public static ImageFilter? CreateDropShadow(float dx, float dy, float sigmaX, float sigmaY, Color color,
        ImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_drop_shadow(dx, dy, sigmaX, sigmaY, (uint)color,
            input?.Handle ?? IntPtr.Zero, IntPtr.Zero));

    public static ImageFilter? CreateDropShadowOnly(float dx, float dy, float sigmaX, float sigmaY, Color color,
        ImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_drop_shadow_only(dx, dy, sigmaX, sigmaY, (uint)color,
            input?.Handle ?? IntPtr.Zero, IntPtr.Zero));

    public static ImageFilter? CreateErode(float radiusX, float radiusY, ImageFilter? input) =>
        GetObject(SkiaApi.sk_imagefilter_new_erode(radiusX, radiusY, input?.Handle ?? IntPtr.Zero,
            IntPtr.Zero));

    //TODO: others
}
#endif