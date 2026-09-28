using System.Runtime.Versioning;

namespace PixUI.Platform.Blazor;

[SupportedOSPlatform("browser")]
public abstract class WebWindow : UIWindow
{
    protected WebWindow(Widget child, string? initRoutePath = null) : base(child, initRoutePath) { }

    protected internal abstract void CreateOnScreenSurface(int onScreenTextureId);

    internal void FirstShow()
    {
        RootWidget.PerformLayout(new(Width, Height));
        Overlay.PerformLayout(new(Width, Height));

        RootWidget.Mount();
        RootWidget.Repaint();
    }

#if SK_GRAPHITE
    internal abstract void OnResize(int width, int height, float ratio, int onScreenTextureId, int offScreenTextureId);
#else
    internal abstract void OnResize(int width, int height, float ratio);
#endif

    public override void StartTextInput() => WebBrowser.StartTextInput();

    public override void StopTextInput() => WebBrowser.StopTextInput();

    public override void SetTextInputRect(Rect rect) =>
        WebBrowser.SetInputRect(rect.X, rect.Y, rect.Width, rect.Height);

    internal void RouteGoto(int historyId) => RouteHistoryManager.Goto(historyId);

    internal void RoutePush(string path) => RouteHistoryManager.Push(path);

    internal int NewRouteId() => RouteHistoryManager.NewIdForPush();
}