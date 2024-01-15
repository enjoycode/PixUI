#if !__WEB__

using System;
using System.Runtime.InteropServices;

namespace CodeEditor;

[StructLayout(LayoutKind.Sequential)]
internal struct TSInput
{
    public IntPtr payload;

    //public delegate* unmanaged<void*, uint, long, uint*, void*> read;
    public IntPtr read;
    public TsInputEncoding encoding;
}
#endif