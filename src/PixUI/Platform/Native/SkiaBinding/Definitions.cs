using System;

#if !__WEB__
namespace PixUI
{
    public enum SKFontStyleWeight
    {
        Invisible = 0,
        Thin = 100,
        ExtraLight = 200,
        Light = 300,
        Normal = 400,
        Medium = 500,
        SemiBold = 600,
        Bold = 700,
        ExtraBold = 800,
        Black = 900,
        ExtraBlack = 1000,
    }

    public enum SKFontStyleWidth
    {
        UltraCondensed = 1,
        ExtraCondensed = 2,
        Condensed = 3,
        SemiCondensed = 4,
        Normal = 5,
        SemiExpanded = 6,
        Expanded = 7,
        ExtraExpanded = 8,
        UltraExpanded = 9,
    }

    public static partial class SkiaExtensions
    {
        // SkImageInfo.cpp - SkColorTypeBytesPerPixel
        public static int GetBytesPerPixel(this ColorType colorType) =>
            colorType switch
            {
                // 0
                //ColorType.Unknown => 0,
                // 1
                ColorType.Alpha8 => 1,
                ColorType.Gray8 => 1,
                // 2
                ColorType.Rgb565 => 2,
                // ColorType.Argb4444 => 2,
                // ColorType.Rg88 => 2,
                // ColorType.Alpha16 => 2,
                // ColorType.AlphaF16 => 2,
                // 4
                ColorType.Bgra8888 => 4,
                // ColorType.Bgra1010102 => 4,
                // ColorType.Bgr101010x => 4,
                ColorType.Rgba8888 => 4,
                // ColorType.Rgb888x => 4,
                ColorType.Rgba1010102 => 4,
                ColorType.Rgb101010x => 4,
                // ColorType.Rg1616 => 4,
                // ColorType.RgF16 => 4,
                // 8
                // ColorType.RgbaF16Clamped => 8,
                ColorType.RgbaF16 => 8,
                // ColorType.Rgba16161616 => 8,
                // 16
                ColorType.RgbaF32 => 16,
                //
                _ => throw new ArgumentOutOfRangeException(nameof(colorType)),
            };
    }
}
#endif