using AppKit;
using Foundation;

namespace PixUI.Platform.Mac;

internal sealed class WindowDelegate : NSWindowDelegate
{
    private readonly MacWindow _window;

    public WindowDelegate(MacWindow window)
    {
        _window = window;
    }

    public override void DidResize(NSNotification notification)
    {
        var view = _window.NSWindow!.ContentView!;
        var scaleFactor = _window.ScaleFactor;
        _window.OnResize((int)(view.Bounds.Size.Width * scaleFactor), (int)(view.Bounds.Size.Height * scaleFactor));
    }

    public override bool WindowShouldClose(NSObject sender)
    {
        _window.CloseWindow();
        return false; //已由上句手工关掉
    }
}