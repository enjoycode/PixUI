#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.ParagraphBuilder")]
    public sealed class ParagraphBuilder : IDisposable
    {
        [TSCustomInterceptor("CanvasKitCtor")]
        public ParagraphBuilder(ParagraphStyle style) { }

        [TSRename("pushStyle")]
        public void PushStyle(TextStyle textStyle) { }

        [TSRename("addText")]
        public void AddText(string text) { }

        [TSRename("addPlaceholder")]
        public void AddPlaceholder(float width, float height, PlaceholderAlignment alignment) { }

        [TSRename("pop")]
        public void Pop() { }

        [TSRename("build")]
        public Paragraph Build() => throw new Exception();

        [TSRename("delete")]
        public void Dispose() { }
    }
}
#endif