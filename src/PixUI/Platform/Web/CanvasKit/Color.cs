#if __WEB__
using System;

namespace PixUI
{
    [TSType("PixUI.Color")]
    public readonly struct Color
    {
        public static readonly Color Empty;

        public Color(byte red, byte green, byte blue)
        {
            Red = Green = Blue = Alpha = 0;
        }

        public Color(byte red, byte green, byte blue, byte alpha)
        {
            Red = Green = Blue = Alpha = 0;
        }

        public Color(uint value)
        {
            Red = Green = Blue = Alpha = 0;
        }

        public byte Red { get; }

        public byte Green { get; }

        public byte Blue { get; }

        public byte Alpha { get; }

        public bool IsOpaque => false;

        public Color WithAlpha(byte alpha) => this;

        public static Color? Lerp(Color? a, Color? b, double t) => ColorUtils.Lerp(a, b, t);
        
        public static bool operator ==(Color left, Color right) => false;

        public static bool operator !=(Color left, Color right) => false;
    }
}
#endif