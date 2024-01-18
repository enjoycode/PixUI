using AppKit;
using CoreGraphics;

namespace PixUI.Platform.Mac;

public sealed class MacWindow : NativeWindow
{
    private nint _windowNumber;
    private TextInputView? _inputView;

    internal NSWindow? NSWindow { get; private set; }

    public override float ScaleFactor =>
        (float)(NSWindow?.Screen ?? NSScreen.MainScreen).BackingScaleFactor;

    public MacWindow(Widget child) : base(child) { }

    public bool InitWindow()
    {
        if (NSWindow != null) return true;

        // Create a delegate to track certain events
        var windowDelegate = new WindowDelegate(this);
        // Create Cocoa window TODO:选项
        var windowRect = new CGRect(100, 600 - 50, 911, 666);
        var windowStyle = NSWindowStyle.Titled | NSWindowStyle.Closable |
                          NSWindowStyle.Resizable |
                          NSWindowStyle.Miniaturizable;
        NSWindow = new NSWindow(windowRect, windowStyle, NSBackingStore.Buffered, false);
        NSWindow.Delegate = windowDelegate;
        // Create MainView
        var view = new MainView(this);
        NSWindow.ContentView = view;
        NSWindow.MakeFirstResponder(view);
        NSWindow.AcceptsMouseMovedEvents = true;
        NSWindow.Restorable = false;

        //TODO: add to app's windows map
        _windowNumber = NSWindow.WindowNumber;

        return true;
    }

    public override bool Attach(BackendType backendType)
    {
        InitWindow();

        //TODO: 根据backendType创建相应的Context
        WindowContext =
            new MacMetalWindowContext(NSWindow!.ContentView!, this, new DisplayParams());

        OnBackendCreated();
        return true;
    }

    protected override void Show()
    {
        NSWindow!.OrderFront(null);
        NSApplication.SharedApplication.ActivateIgnoringOtherApps(true);
        NSWindow.MakeKeyAndOrderFront(NSApplication.SharedApplication);
    }

    internal void CloseWindow()
    {
        if (NSWindow == null) return;

        RootWidget.Dispose();
        Detach();

        //TODO:判断最后一个
        NSApplication.SharedApplication.Terminate(NSWindow);

        NSWindow.Close();
        NSWindow = null;
    }

    public override void SetTextInputRect(Rect rect) => _inputView?.SetInputRect(rect);

    public override void StartTextInput()
    {
        if (_inputView != null) return;

        var mainView = NSWindow!.ContentView!;
        _inputView = new TextInputView();
        mainView.AddSubview(_inputView);
        NSWindow.MakeFirstResponder(_inputView);
    }

    public override void StopTextInput()
    {
        if (_inputView == null) return;

        _inputView.RemoveFromSuperview();
        _inputView.Dispose();
        _inputView = null;
    }
}