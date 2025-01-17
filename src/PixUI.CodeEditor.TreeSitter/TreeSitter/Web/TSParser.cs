#if __WEB__
using System;
using PixUI;

namespace CodeEditor
{
    [TSType("CodeEditor.TSParser")]
    public sealed class TSParser : IDisposable
    {
        [TSTemplate("new window.TreeSitter()")]
        public TSParser() {}

        [TSPropertyToGetSet]
        public TSLanguage Language { get; set; }
        
        [TSRename("delete")]
        public void Dispose() {}
    }
}
#endif