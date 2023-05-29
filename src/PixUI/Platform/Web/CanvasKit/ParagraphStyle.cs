#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.ParagraphStyle")]
    public sealed class ParagraphStyle : IDisposable
    {
        [TSCustomInterceptor("CanvasKitCtor")] public ParagraphStyle() {}

        [TSRename("maxLines")] public uint MaxLines;

        [TSRename("textStyle")] public TextStyle TextStyle;
        
        [TSRename("textAlign")] public TextAlign TextAlign;

        [TSRename("heightMultiplier")] public float Height;

        [TSIgnoreMethodInvoke]
        public void Dispose() { }
    }
}
#endif