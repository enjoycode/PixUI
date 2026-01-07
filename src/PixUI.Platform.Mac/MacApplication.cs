using System;
using AppKit;
using Foundation;

namespace PixUI.Platform.Mac;

public sealed class MacApplication : UIApplication
{
    private MacApplication() { }

    #region ====Platform Providers====

    public override IPlatformCursors CursorsProvider { get; } = new MacCursors();
    public override IPlatformClipboard ClipboardProvider { get; } = new MacClipboard();
    public override IPlatformFileDialog FileDialogProvider => throw new NotImplementedException("FileDialogProvider");

    #endregion

    public override void PostInvalidateEvent()
        => NSRunLoop.Main.BeginInvokeOnMainThread(OnInvalidateRequest);

    public override void BeginInvoke(Action action)
        => NSRunLoop.Main.BeginInvokeOnMainThread(action);

    public static void Run(Widget child)
    {
        // create and run application
        var app = new MacApplication();
        Current = app;
        app.RunInternal(child);
    }

    private void RunInternal(Widget child)
    {
        NSApplication.Init();
        var nsApp = NSApplication.SharedApplication;
        nsApp.ActivationPolicy = NSApplicationActivationPolicy.Regular;

        // Set AppDelegate to catch certain global events
        var appDelegate = new AppDelegate();
        nsApp.Delegate = appDelegate;

        // Create root & native window
        var window = new MacWindow(child);
        // window.InitWindow();
        window.Attach(NativeWindow.BackendType.Metal);
        MainWindow = window;

        // Run EventLoop
        nsApp.Run();

        // process the events
        while (!appDelegate.IsDone)
        {
            var evt = nsApp.NextEvent(NSEventMask.AnyEvent, NSDate.DistantFuture,
                NSRunLoopMode.Default, true);
            nsApp.SendEvent(evt);
            evt.Dispose();
        }

        Console.WriteLine("Application exit.");
        // Never be here now.
        // app.Delegate = null;
        // app.Terminate(null);
        // app.Dispose();
    }
}