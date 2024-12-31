using System.Runtime.InteropServices;

namespace CodeEditor;

[StructLayout(LayoutKind.Sequential)]
public struct TSEdit
{
    public uint startIndex;
    public uint oldEndIndex;
    public uint newEndIndex;
    public TSPoint startPosition;
    public TSPoint oldEndPosition;
    public TSPoint newEndPosition;

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