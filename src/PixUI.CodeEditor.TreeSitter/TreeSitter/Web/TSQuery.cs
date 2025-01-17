#if __WEB__
using System;
using PixUI;

namespace CodeEditor
{
    [TSType("CodeEditor.TSQueryCapture")]
    public sealed class TSQueryCapture
    {
        public TSSyntaxNode node = null!;
    }


    [TSType("CodeEditor.TSQuery")]
    public sealed class TSQuery : IDisposable
    {
        [TSRename("captures")]
        public TSQueryCapture[] Captures(TSSyntaxNode node, Point? startPosition = null,
            Point? endPosition = null) => throw new Exception();
        
        [TSRename("delete")]
        public void Dispose() {}
    }
}
#endif