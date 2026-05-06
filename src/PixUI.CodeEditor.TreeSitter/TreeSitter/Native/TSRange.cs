#if !__WEB__
using System.Runtime.InteropServices;

namespace PixUI.CodeEditor;

[StructLayout(LayoutKind.Sequential)]
public struct TSRange
{
    public TSPoint StartPosition;
    public TSPoint EndPosition;
    public uint StartIndex;
    public uint EndIndex;

    public override string ToString() => $"[{StartPosition} - {EndPosition}]";
}
#endif