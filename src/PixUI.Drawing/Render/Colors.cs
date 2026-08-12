using System;

namespace PixUI;

public static class Colors
{
    public static Color White => 0xFFFFFFFF;
    public static Color Black => 0xFF000000;
    public static Color Red => 0xFFFF0000;
    public static Color Blue => 0xFF0000FF;
    public static Color Green => 0xFF00FF00;
    public static Color Yellow => 0xFFFFFF00;
    public static Color Brown => 0xFFA52A2A;

    public static Color Lime => 0xFF00FF00;
    public static Color Cyan => 0xFF00FFFF;
    public static Color Magenta => 0xFFFF00FF;
    public static Color Orange => 0xFFFFA500;
    public static Color Purple => 0xFF800080;
    public static Color Gray => 0xFF5F6368;
    public static Color LightGray => 0xFFD3D3D3;
    public static Color DarkGray => 0xFFA9A9A9;
    public static Color Silver = 0xFFC0C0C0;
    public static Color Transparent => 0x00FFFFFF;

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