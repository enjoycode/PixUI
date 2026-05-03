#if !__WEB__

namespace PixUI;

public sealed class SKMaskFilter : SKObject, ISKReferenceCounted, IMaskFilter
{
    //public const int TableMaxLength = 256;

    private SKMaskFilter(IntPtr handle, bool owns) : base(handle, owns) { }

    internal static SKMaskFilter? GetObject(IntPtr handle) =>
        GetOrAddObject(handle, (h, o) => new SKMaskFilter(h, o));

    public static SKMaskFilter? CreateBlur(BlurStyle blurStyle, float sigma)
        => GetObject(SkiaApi.sk_maskfilter_new_blur(blurStyle, sigma));
}
#endif