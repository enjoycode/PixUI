#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.Typeface")]
    public sealed class Typeface : IDisposable
    {
        [TSRename("delete")]
        public void Dispose() {}
    }
}
#endif