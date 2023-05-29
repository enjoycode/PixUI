#if !__WEB__
using System;

namespace PixUI
{
    public readonly struct SKFontStyle //TODO:合并至FontStyle
    {
        public static SKFontStyle Make(bool bold, bool italic)
        {
            if (bold && italic)
                return new SKFontStyle(SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, FontSlant.Italic);
            if (bold)
                return new SKFontStyle(SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, FontSlant.Upright);
            if (italic)
                return new SKFontStyle(SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, FontSlant.Italic);
            return new SKFontStyle();
        }
        
        private readonly int _fValue;

        public SKFontStyle()
            : this(SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, FontSlant.Upright) { }

        public SKFontStyle(SKFontStyleWeight weight, SKFontStyleWidth width, FontSlant slant)
            : this((int)weight, (int)width, (int)slant) { }

        private SKFontStyle(int weight, int width, int slant)
        {
            _fValue = weight + (width << 16) + (slant << 24);
        }

        public SKFontStyleWeight Weight => (SKFontStyleWeight)(_fValue & 0xFFFF);

        public SKFontStyleWidth Width => (SKFontStyleWidth)((_fValue >> 16) & 0xFF);

        public FontSlant Slant => (FontSlant)((_fValue >> 24) & 0xFF);
    }
}
#endif