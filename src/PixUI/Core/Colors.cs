using System;

namespace PixUI;

public static class Colors
{
    public static readonly Color White = new Color(255, 255, 255);
    public static readonly Color Black = new Color(0, 0, 0);
    public static readonly Color Red = new Color(255, 0, 0);
    public static readonly Color Blue = new Color(0, 0, 255);
    public static readonly Color Green = new Color(0, 255, 0);
    public static readonly Color Gray = new Color(0xFF5F6368);
    public static readonly Color Transparent = new Color(0, 0, 0, 0);

    private static Random? _random;

    public static Color Random(byte alpha = 255)
    {
        _random ??= new Random();
        var randomValue = (uint)(_random.Next(0, 1 << 24) | (alpha << 24));
        return new Color(randomValue);
    }
}