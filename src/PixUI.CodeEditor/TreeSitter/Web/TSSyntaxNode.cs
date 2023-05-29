#if __WEB__
using PixUI;

namespace CodeEditor
{
    [TSType("CodeEditor.TSSyntaxNode")]
    public sealed class TSSyntaxNode
    {
        public int id => 0;

        [TSRename("typeId")]
        public ushort TypeId => 0;

        [TSRename("type")]
        public string Type => "";

        [TSRename("parent")]
        public TSSyntaxNode? Parent => null;

        [TSRename("childCount")]
        public int ChildCount => 0;

        [TSRename("children")]
        public TSSyntaxNode[] Children {get;}

        [TSRename("startIndex")]
        public int StartIndex => 0;

        [TSRename("endIndex")]
        public int EndIndex => 0;

        [TSRename("startPosition")]
        public TSPoint StartPosition => TSPoint.Empty;

        [TSRename("endPosition")]
        public TSPoint EndPosition => TSPoint.Empty;

        [TSRename("hasError")]
        public bool HasError() => false;

        [TSRename("isNamed")]
        public bool IsNamed() => false;

        [TSRename("nextNamedSibling")]
        public TSSyntaxNode? NextNamedSibling => null;
        
        [TSRename("namedDescendantForPosition")]
        internal TSSyntaxNode? NamedDescendantForPosition(TSPoint start, TSPoint? end = null) => null;
    }
}
#endif