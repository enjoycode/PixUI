#if __WEB__
using System;

namespace PixUI
{
    [TSType("PixUI.TextStyle")]
    public sealed class TextStyle : IDisposable
    {
        [TSCustomInterceptor("CanvasKitCtor")] public TextStyle() {}

        [TSRename("color")] public Color Color;

        [TSRename("fontSize")] public float FontSize;

        [TSRename("heightMultiplier")] public float Height;

        [TSRename("fontStyle")] public FontStyle FontStyle;

        [TSIgnoreMethodInvoke]
        public void Dispose() { }
    }

    [TSType("PixUI.FontStyle")]
    public readonly struct FontStyle
    {
        public readonly FontWeight? Weight;
        public readonly FontSlant? Slant;

        public FontStyle(FontWeight weight, FontSlant slant)
        {
            Weight = weight;
            Slant = slant;
        }
    }
}
#endif