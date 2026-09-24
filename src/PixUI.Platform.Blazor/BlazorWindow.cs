using System.Runtime.Versioning;

namespace PixUI.Platform.Blazor;

[SupportedOSPlatform("browser")]
public abstract class BlazorWindow : UIWindow
{
    protected BlazorWindow(Widget child, string? initRoutePath = null) : base(child, initRoutePath) { }

    protected internal abstract void CreateOnScreenSurface(int onScreenTextureId);

    internal void FirstShow()
    {
        RootWidget.PerformLayout(new(Width, Height));
        Overlay.PerformLayout(new(Width, Height));

        RootWidget.Mount();
        RootWidget.Repaint();
    }

    internal void OnResize(int width, int height, float ratio)
    {
        //TODO:
    }

    public override void StartTextInput() => WebBrowser.StartTextInput();

    public override void StopTextInput() => WebBrowser.StopTextInput();

    public override void SetTextInputRect(Rect rect) =>
        WebBrowser.SetInputRect(rect.X, rect.Y, rect.Width, rect.Height);

    internal void RouteGoto(int historyId) => RouteHistoryManager.Goto(historyId);

    internal void RoutePush(string path) => RouteHistoryManager.Push(path);

    internal int NewRouteId() => RouteHistoryManager.NewIdForPush();
}