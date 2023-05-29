#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.Font")]
    public sealed class Font : IDisposable
    {
        [TSTemplate("new CanvasKit.Font({1}, {2})")]
        public Font(Typeface typeface, float size) { }

        [TSTemplate("{0}.getGlyphIDs(String.fromCharCode({1}))[0]")]
        public ushort GetGlyphId(int codepoint) => 0;

        [TSRename("delete")]
        public void Dispose() { }
    }
}
#endif