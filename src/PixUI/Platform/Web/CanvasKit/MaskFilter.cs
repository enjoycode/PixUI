#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.MaskFilter")]
    public sealed class MaskFilter
    {
        private MaskFilter() { }

        [TSTemplate("CanvasKit.MaskFilter.MakeBlur({1},{2},false)")]
        public static MaskFilter CreateBlur(BlurStyle blurStyle, float sigma)
            => throw new Exception();

        [TSTemplate("PixUI.ConvertRadiusToSigma({1})")]
        public static float ConvertRadiusToSigma(float radius) => 0;
    }
}
#endif