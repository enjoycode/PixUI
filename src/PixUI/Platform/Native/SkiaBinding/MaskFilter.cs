#if !__WEB__

using System;

namespace PixUI
{
    public sealed class MaskFilter : SKObject, ISKReferenceCounted
    {
        private const float BlurSigmaScale = 0.57735f;
        public const int TableMaxLength = 256;

        private MaskFilter(IntPtr handle, bool owns) : base(handle, owns) { }

        internal static MaskFilter? GetObject(IntPtr handle) =>
            GetOrAddObject(handle, (h, o) => new MaskFilter(h, o));

        public static float ConvertRadiusToSigma(float radius) =>
            radius > 0 ? BlurSigmaScale * radius + 0.5f : 0.0f;

        public static float ConvertSigmaToRadius(float sigma) =>
            sigma > 0.5f ? (sigma - 0.5f) / BlurSigmaScale : 0.0f;

        public static MaskFilter? CreateBlur(BlurStyle blurStyle, float sigma)
            => GetObject(SkiaApi.sk_maskfilter_new_blur(blurStyle, sigma));
    }
}
#endif