#if !__WEB__
using System;
using System.Runtime.InteropServices;
using CodeEditor;
using PixUI;

namespace CodeEditor;

public sealed class ParserInput : IDisposable
{
    private readonly ITextBuffer _textBuffer;
    private readonly IntPtr _nativeBuffer;
    private const int NativeInputSize = 2048;

    public ParserInput(ITextBuffer textBuffer)
    {
        _textBuffer = textBuffer;
        _nativeBuffer = Marshal.AllocHGlobal(NativeInputSize);
    }

    /// <summary>
    /// TreeSitter回调读取代码
    /// </summary>
    [UnmanagedCallersOnly]
    public static unsafe void* Read(void* payload, uint byteIndex, TSPoint* position, uint* bytesRead)
    {
        Log.Debug($"ParserInput.Read: index={byteIndex}");

        var gcHandle = GCHandle.FromIntPtr(new IntPtr(payload));
        var input = (ParserInput)gcHandle.Target!;

        var offset = (int)(byteIndex / SyntaxParser.ParserEncoding); //utf16
        if (offset >= input._textBuffer.Length)
        {
            *bytesRead = 0;
            return IntPtr.Zero.ToPointer();
        }

        //TODO: 优化避免复制，暂简单实现
        var count = Math.Min(NativeInputSize / 2, input._textBuffer.Length - offset);
        var dest = new Span<char>(input._nativeBuffer.ToPointer(), NativeInputSize / 2);
        input._textBuffer.CopyTo(dest, offset, count);

        *bytesRead = (uint)count * SyntaxParser.ParserEncoding;
        return input._nativeBuffer.ToPointer();
    }

    public void Dispose() => Marshal.FreeHGlobal(_nativeBuffer);
}
#endif