using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PixUI.Platform.Win;

public sealed class WinWindow : NativeWindow
{
    public WinWindow(Widget child) : base(child) { }
    internal IntPtr HWND { get; private set; }
    private float _scaleFactor = 1;
    public override float ScaleFactor => _scaleFactor;
    private static bool _hasRegisterWinCls = false;
    private static readonly WndProcFunc _wndProc = new WndProcFunc(WndProc);
    internal static Action OnInvalidateRequest = null!;

    private char _highSurrogate; //cache for WM_CHAR

    private void InitWindow()
    {
        var x = 50;
        var y = 50;
        var width = (int)(950 * _scaleFactor);
        var height = (int)(705 * _scaleFactor);

        var className = RegisterWindowClass();
        HWND = WinApi.Win32CreateWindow(WindowExStyles.WS_EX_APPWINDOW, className, "Demo",
            WindowStyles.WS_OVERLAPPEDWINDOW,
            x, y, width, height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        if (HWND == IntPtr.Zero)
            throw new Exception("Can't create native window");
    }

    public override bool Attach(BackendType backendType)
    {
        var systemDpi = WinApi.Win32GetDpiForSystem();
        _scaleFactor = systemDpi / 96f;

        InitWindow();

        //WindowContext = new WinRasterWindowContext(this, new DisplayParams());
        WindowContext = new WinD3D12WindowContext(this, new DisplayParams());
        OnBackendCreated();

        return true;
    }

    protected override void Show()
    {
        WinApi.Win32ShowWindow(HWND, WindowPlacementFlags.SW_SHOWNORMAL);
        //WinApi.Win32UpdateWindow(HWND);
    }

    private static string RegisterWindowClass()
    {
        var className = $"PixUIApp";
        if (_hasRegisterWinCls) return className;
        _hasRegisterWinCls = true;

        var wndClass = new WNDCLASS();
        wndClass.style = 1 | 2 | 0x00000020;//CS_HREDRAW | CS_VREDRAW | CS_OWNDC;
        wndClass.lpfnWndProc = _wndProc;
        wndClass.cbClsExtra = 0;
        wndClass.cbWndExtra = 0;
        wndClass.hbrBackground = (IntPtr)(GetSysColorIndex.COLOR_WINDOW + 1);
        wndClass.hCursor = WinApi.Win32LoadCursor(IntPtr.Zero, LoadCursorType.IDC_ARROW);
        wndClass.hIcon = IntPtr.Zero;
        wndClass.hInstance = IntPtr.Zero;
        wndClass.lpszClassName = className;
        wndClass.lpszMenuName = "";

        var result = WinApi.Win32RegisterClass(ref wndClass);
        if (!result)
            throw new Exception("Can't register window class");

        return className;
    }

    private static IntPtr WndProc(IntPtr hWnd, Msg msg, IntPtr wParam, IntPtr lParam)
    {
        var eventHandled = IntPtr.Zero;
        var ps = new PAINTSTRUCT();

        var win = (WinWindow)Current; //TODO:暂单窗体
        if (hWnd != win.HWND)
            return WinApi.Win32DefWindowProc(hWnd, msg, wParam, lParam);

        switch (msg)
        {
            case Msg.WM_CLOSE:
                WinApi.Win32PostQuitMessage(0);
                return eventHandled;
            case Msg.WM_WINDOWPOSCHANGED:
            {
                WinApi.Win32GetClientRect(win.HWND, out var rect);
                var newWidth = rect.right - rect.left;
                var newHeight = rect.bottom - rect.top;
                if (win.WindowContext!.Width != newWidth || win.WindowContext.Height != newHeight)
                    win.OnResize(newWidth, newHeight);
                return WinApi.Win32DefWindowProc(hWnd, msg, wParam, lParam);
            }
            case Msg.WM_MOUSEMOVE:
            {
                var xPos = (lParam.ToInt32() & 0xFFFF) / win.ScaleFactor;
                var yPos = (lParam.ToInt32() >> 16) / win.ScaleFactor;
                var buttons = GetButtonsFromWParam(wParam.ToInt64());
                //Console.WriteLine($"MouseMove: pos=[{xPos}, {yPos}] btn={buttons}");
                win.OnPointerMove(PointerEvent.UseDefault(buttons, xPos, yPos, xPos - win.LastMouseX, yPos - win.LastMouseY));
                return eventHandled;
            }
            case Msg.WM_NCMOUSEMOVE:
            {
                win.OnPointerMoveOutWindow();
                return eventHandled;
            }
            case Msg.WM_MOUSEWHEEL:
            {
                var xPos = (lParam.ToInt32() & 0xFFFF) / win.ScaleFactor;
                var yPos = (lParam.ToInt32() >> 16) / win.ScaleFactor;
                var delta = (short)(wParam.ToInt64() >> 16);
                //Console.WriteLine($"MouseWheel: {delta}");
                win.OnScroll(ScrollEvent.Make(xPos, yPos, 0, -delta));
                return eventHandled;
            }
            case Msg.WM_MOUSEHWHEEL:
            {
                var xPos = (lParam.ToInt32() & 0xFFFF) / win.ScaleFactor;
                var yPos = (lParam.ToInt32() >> 16) / win.ScaleFactor;
                var delta = (short)(wParam.ToInt64() >> 16);
                win.OnScroll(ScrollEvent.Make(xPos, yPos, delta, 0));
                return eventHandled;
            }
            case Msg.WM_LBUTTONDOWN:
            case Msg.WM_RBUTTONDOWN:
            {
                var xPos = (lParam.ToInt32() & 0xFFFF) / win.ScaleFactor;
                var yPos = (lParam.ToInt32() >> 16) / win.ScaleFactor;
                win.OnPointerDown(PointerEvent.UseDefault(GetButtonsFromWParam(wParam.ToInt64()), xPos, yPos, 0, 0));
                return eventHandled;
            }
            case Msg.WM_LBUTTONUP:
            case Msg.WM_RBUTTONUP:
            {
                var xPos = (lParam.ToInt32() & 0xFFFF) / win.ScaleFactor;
                var yPos = (lParam.ToInt32() >> 16) / win.ScaleFactor;
                win.OnPointerUp(PointerEvent.UseDefault(GetButtonsFromWParam(wParam.ToInt64()), xPos, yPos, 0, 0));
                return eventHandled;
            }
            case Msg.WM_SETCURSOR:
                if (WinCursors.Current == null || (lParam.ToInt32() & 0xFFFF) != 1 /*HTCLIENT*/)
                    return WinApi.Win32DefWindowProc(hWnd, msg, wParam, lParam);
                WinApi.Win32SetCursor(((WinCursor)WinCursors.Current).Handle);
                return (IntPtr)1;
            case Msg.WM_KEYDOWN:
            case Msg.WM_SYSKEYDOWN:
            {
                var keys = (Keys)wParam.ToInt32();
                win.OnKeyDown(KeyEvent.UseDefault(keys | GetModifierKeys()));
                return eventHandled;
            }
            case Msg.WM_KEYUP:
            case Msg.WM_SYSKEYUP:
            {
                var keys = (Keys)wParam.ToInt32();
                win.OnKeyUp(KeyEvent.UseDefault(keys | GetModifierKeys()));
                return eventHandled;
            }
            case Msg.WM_CHAR:
            {
                var ch = (char)wParam.ToInt32();
                //Don't post text events for unprintable characters
                if (ch < ' ' || ch == 127)
                    return eventHandled;

                if (IsHighSurrogate(ch))
                {
                    win._highSurrogate = ch;
                }
                else if (IsHighSurrogate(win._highSurrogate) && IsLowSurrogate(ch))
                {
                    var text = string.Create<object?>(2, null, (span, arg) =>
                    {
                        span[0] = win._highSurrogate;
                        span[1] = ch;
                    });
                    win._highSurrogate = (char)0;
                }
                else
                {
                    win.OnTextInput(ch.ToString());
                }
                return eventHandled;
            }
            case Msg.WM_PAINT:
                //Console.WriteLine($"Got WM_PAINT {DateTime.Now}");
                WinApi.Win32BeginPaint(win.HWND, ref ps);
                OnInvalidateRequest();
                WinApi.Win32EndPaint(win.HWND, ref ps);
                return eventHandled;
            default:
                return WinApi.Win32DefWindowProc(hWnd, msg, wParam, lParam);
        }
    }

    private static PointerButtons GetButtonsFromWParam(long param)
    {
        PointerButtons buttons = PointerButtons.None;

        if ((param & 0x0001) != 0)
            buttons |= PointerButtons.Left;
        if ((param & 0x0010) != 0)
            buttons |= PointerButtons.Middle;
        if ((param & 0x0002) != 0)
            buttons |= PointerButtons.Right;

        return buttons;
    }

    private static Keys GetModifierKeys()
    {
        Keys key_state = Keys.None;
        if ((WinApi.Win32GetKeyState(VirtualKeys.VK_SHIFT) & 0x8000) != 0)
            key_state |= Keys.Shift;
        if ((WinApi.Win32GetKeyState(VirtualKeys.VK_CONTROL) & 0x8000) != 0)
            key_state |= Keys.Control;
        if ((WinApi.Win32GetKeyState(VirtualKeys.VK_MENU) & 0x8000) != 0)
            key_state |= Keys.Alt;
        return key_state;
    }

    private static bool IsHighSurrogate(char ch) => (ch >= 0xd800) && (ch <= 0xdbff);

    private static bool IsLowSurrogate(char ch) => (ch >= 0xdc00) && (ch <= 0xdfff);
}