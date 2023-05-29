#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.ColorSpace")]
    public class ColorSpace //TODO: IEquatable<ColorSpace>
    {
        public static ColorSpace SRGB => throw new Exception();
    }
}

#endif