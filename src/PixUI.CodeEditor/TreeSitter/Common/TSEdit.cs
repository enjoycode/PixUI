using System.Runtime.InteropServices;

namespace CodeEditor;

[StructLayout(LayoutKind.Sequential)]
internal struct TSEdit
{
    internal uint startIndex;
    internal uint oldEndIndex;
    internal uint newEndIndex;
    internal TSPoint startPosition;
    internal TSPoint oldEndPosition;
    internal TSPoint newEndPosition;

#if __WEB__
    public TSEdit Clone()
    {
        var clone = new TSEdit();
        clone.startIndex = startIndex;
        clone.oldEndIndex = oldEndIndex;
        clone.newEndIndex = newEndIndex;
        clone.startPosition = startPosition;
        clone.oldEndPosition = oldEndPosition;
        clone.newEndPosition = newEndPosition;
        return clone;
    }
#endif
}