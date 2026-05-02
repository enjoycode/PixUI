using System.Runtime.InteropServices;

namespace PixUI.Platform.Win;

internal sealed class WinClipboard : IPlatformClipboard
{
    public ValueTask<string?> ReadText()
    {
        var win = (WinWindow)UIWindow.Current;
        if (!WinApi.Win32OpenClipboard(win.HWND))
        {
            Console.WriteLine("Can't open clipboard");
            return new ValueTask<string?>((string?)null);
        }

        var hMem = WinApi.Win32GetClipboardData((uint)ClipboardFormats.CF_UNICODETEXT);
        if (hMem == IntPtr.Zero)
            return new ValueTask<string?>((string?)null);

        var src = WinApi.Win32GlobalLock(hMem);
        var text = Marshal.PtrToStringUni(src);
        WinApi.Win32GlobalUnlock(hMem);

        WinApi.Win32CloseClipboard();

        return new ValueTask<string?>(text);
    }

    public ValueTask WriteText(string text)
    {
        var win = (WinWindow)UIWindow.Current;
        if (!WinApi.Win32OpenClipboard(win.HWND))
        {
            Console.WriteLine("Can't open clipboard");
            return ValueTask.CompletedTask;
        }

        unsafe
        {
            fixed (char* src = text)
            {
                var srcSpan = new Span<byte>(src, text.Length * 2);
                var hMem = WinApi.CopyToMoveableMemory(srcSpan, true);
                WinApi.Win32EmptyClipboard(); //TODO: check need it?
                WinApi.Win32SetClipboardData((uint)ClipboardFormats.CF_UNICODETEXT, hMem);
            }
        }
        WinApi.Win32CloseClipboard();

        return ValueTask.CompletedTask;
    }
}