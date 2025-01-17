#if __WEB__
using PixUI;
using System;

namespace CodeEditor
{
    [TSType("CodeEditor.TSTree")]
    public class TSTree : IDisposable
    {
        [TSRename("rootNode")]
        public TSSyntaxNode Root => throw new Exception();
        
        [TSRename("edit")]
        internal void Edit(TSEdit edit) {}
        
        [TSRename("delete")]
        public void Dispose() {}
    }
}
#endif