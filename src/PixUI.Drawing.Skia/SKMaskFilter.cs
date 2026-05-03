#if !__WEB__

namespace PixUI;

public sealed class SKMaskFilter : SKObject, ISKReferenceCounted
{
    private const float BlurSigmaScale = 0.57735f;
    public const int TableMaxLength = 256;

    private SKMaskFilter(IntPtr handle, bool owns) : base(handle, owns) { }

    internal static SKMaskFilter? GetObject(IntPtr handle) =>
        GetOrAddObject(handle, (h, o) => new SKMaskFilter(h, o));

    public static float ConvertRadiusToSigma(float radius) =>
        radius > 0 ? BlurSigmaScale * radius + 0.5f : 0.0f;

    public static float ConvertSigmaToRadius(float sigma) =>
        sigma > 0.5f ? (sigma - 0.5f) / BlurSigmaScale : 0.0f;

    public static SKMaskFilter? CreateBlur(BlurStyle blurStyle, float sigma)
        => GetObject(SkiaApi.sk_maskfilter_new_blur(blurStyle, sigma));
}
#endif