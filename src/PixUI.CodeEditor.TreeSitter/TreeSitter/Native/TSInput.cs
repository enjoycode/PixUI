#if !__WEB__

using System.Runtime.InteropServices;

namespace PixUI.CodeEditor;

[StructLayout(LayoutKind.Sequential)]
public struct TSInput
{
    public IntPtr payload;

    //public delegate* unmanaged<void*, uint, long, uint*, void*> read;
    public IntPtr read;
    public TsInputEncoding encoding;
}
#endif