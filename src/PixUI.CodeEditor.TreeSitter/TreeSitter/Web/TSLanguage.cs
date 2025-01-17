#if __WEB__
using System;
using PixUI;

namespace CodeEditor
{
    [TSType("CodeEditor.TSLanguage")]
    public sealed class TSLanguage
    {
        [TSRename("query")]
        public TSQuery Query(string source) => throw new Exception();
    }

    [TSType("CodeEditor.TSCSharpLanguage")]
    public static class TSCSharpLanguage
    {
        public static TSLanguage Get() => throw new Exception();
    }
}
#endif