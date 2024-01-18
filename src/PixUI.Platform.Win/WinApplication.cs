using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixUI.Platform.Win;

public sealed class WinApplication : UIApplication
{
    private static readonly Thread _uiThread = Thread.CurrentThread;

    public static void Run(Widget child)
    {
        // Init SynchronizationContext
        SynchronizationContext.SetSynchronizationContext(new WinSynchronizationContext());
        // init platform supports
        Cursor.PlatformCursors = new WinCursors();
        Clipboard.Init(new WinClipboard());
        // init for hidpi
        WinApi.Win32SetProcessDPIAware();

        // 测试加载字体
        //var fontStream = File.OpenRead("MiSans-Regular.ttf");
        //var fontData = SKData.Create(fontStream);
        //FontCollection.Instance.RegisterTypefaceToAsset(fontData, "MiSans", false);

        // create and run application
        var app = new WinApplication();
        Current = app;
        app.RunInternal(child);
    }

    private void RunInternal(Widget child)
    {
        // Create root & native window
        WinWindow.OnInvalidateRequest = OnInvalidateRequest;
        var win = new WinWindow(child);
        MainWindow = win;
        // window.InitWindow();
        win.Attach(NativeWindow.BackendType.Raster);

        // Run EventLoop
        var msg = new MSG();
        do
        {
            var ret = WinApi.Win32GetMessage(ref msg, IntPtr.Zero, 0, 0);
            if (ret == 0)
                break;
            if (ret == -1)
                break;

            if (msg.message == Msg.WM_QUIT)
                break;

            // 自定义消息处理
            if (msg.message == Msg.WM_ASYNC_MESSAGE)
            {
                var gcHandle = GCHandle.FromIntPtr(msg.lParam);
                var action = (Action)gcHandle.Target!;
                gcHandle.Free();
                action();
            }
            else
            {
                WinApi.Win32TranslateMessage(ref msg);
                WinApi.Win32DispatchMessage(ref msg);
            }

        } while (true);

        Console.WriteLine("Application exit.");
    }

    public static bool InvokeRequired => Thread.CurrentThread != _uiThread;

    public override void BeginInvoke(Action action)
    {
        //HWND_BROADCAST = 0xFFFF
        var win = (WinWindow)MainWindow;
        var gcHandle = GCHandle.Alloc(action);
        var ok = WinApi.Win32PostMessage(win.HWND, Msg.WM_ASYNC_MESSAGE, IntPtr.Zero, GCHandle.ToIntPtr(gcHandle));
        if (!ok)
        {
            gcHandle.Free();
            Console.WriteLine("Can't post async message to event loop");
        }
    }

    public override void PostInvalidateEvent()
    {
        if (MainWindow == null) return;

        var win = (WinWindow)MainWindow; //TODO: to correct window
        var res = WinApi.Win32InvalidateRect(win.HWND, IntPtr.Zero, false);
        if (res == IntPtr.Zero)
            Console.WriteLine("Can't post update message to event loop");
    }
}