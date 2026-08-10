using System;

namespace PixUI;

public static class Colors
{
    public static Color White => new(255, 255, 255);
    public static Color Black => new(0, 0, 0);
    public static Color Red => new(255, 0, 0);
    public static Color Blue => new(0, 0, 255);
    public static Color Green => new(0, 255, 0);
    public static Color Yellow => new(0xFFFFFF00);
    public static Color Lime => new(0xFF00FF00);
    public static Color Cyan => new(0xFF00FFFF);
    public static Color Magenta => new(0xFFFF00FF);
    public static Color Gray => new(0xFF5F6368);
    public static Color DarkGray => new(0xFFA9A9A9);
    public static Color Silver = new(0xFFC0C0C0);
    public static Color Transparent => new(0, 0, 0, 0);

    private static Random? _random;

    public static Color Random(byte alpha = 255)
    {
        _random ??= new Random();
        var randomValue = (uint)(_random.Next(0, 1 << 24) | (alpha << 24));
        return new Color(randomValue);
    }

    public static Color Dark(Color baseColor) => new HLSColor(baseColor).Darker(0.5f);

    public static Color DarkDark(Color baseColor) => new HLSColor(baseColor).Darker(1.0f);

    public static Color Light(Color baseColor) => new HLSColor(baseColor).Lighter(0.5f);

    public static Color LightLight(Color baseColor) => new HLSColor(baseColor).Lighter(1.0f);
}