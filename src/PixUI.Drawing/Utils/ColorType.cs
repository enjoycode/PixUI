namespace PixUI;

public static class ColorTypeExtension
{
    public static int GetBytesPerPixel(this ColorType colorType) => colorType switch
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