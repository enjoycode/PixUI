using Microsoft.JSInterop;
using static PixUI.Platform.Blazor.InputUtils;

namespace PixUI.Platform.Blazor;

public static class JSApi
{
    [JSInvokable]
    public static void OnInvalidate() => ((BlazorApplication)UIApplication.Current).RunInvaldateRequest();

    [JSInvokable]
    public static void OnMouseMove(int buttons, int x, int y, int dx, int dy)
    {
        var args = PointerEvent.UseDefault(ConvertButtons(buttons), x, y, dx, dy);
        BlazorApplication.Window.OnPointerMove(args);
    }

    [JSInvokable]
    public static void OnMouseMoveOutWindow()
    {
        BlazorApplication.Window.OnPointerMoveOutWindow();
    }

    [JSInvokable]
    public static void OnMouseDown(int buttons, int x, int y, int dx, int dy)
    {
        var args = PointerEvent.UseDefault(ConvertButtons(buttons), x, y, dx, dy);
        BlazorApplication.Window.OnPointerDown(args);
    }

    [JSInvokable]
    public static void OnMouseUp(int buttons, int x, int y, int dx, int dy)
    {
        var args = PointerEvent.UseDefault(ConvertButtons(buttons), x, y, dx, dy);
        BlazorApplication.Window.OnPointerUp(args);
    }

    [JSInvokable]
    public static void OnScroll(int x, int y, int dx, int dy)
    {
        var args = ScrollEvent.Make(x, y, dx, dy);
        BlazorApplication.Window.OnScroll(args);
    }

    [JSInvokable]
    public static void OnKeyDown(string key, string code, bool alt, bool shift, bool control)
    {
        var args = KeyEvent.UseDefault(ConvertKeys(key, code, alt, control, shift));
        BlazorApplication.Window.OnKeyDown(args);
    }

    [JSInvokable]
    public static void OnKeyUp(string key, string code, bool alt, bool shift, bool control)
    {
        var args = KeyEvent.UseDefault(ConvertKeys(key, code, alt, control, shift));
        BlazorApplication.Window.OnKeyUp(args);
    }

    [JSInvokable]
    public static void OnTextInput(string text) => BlazorApplication.Window.OnTextInput(text);

    [JSInvokable]
    public static void OnResize(int width, int height, float ratio) =>
        BlazorApplication.Window.OnResize(width, height, ratio);

    [JSInvokable]
    public static void RouteGoto(int historyId) => BlazorApplication.Window.RouteGoto(historyId);

    [JSInvokable]
    public static void RoutePush(string path) => BlazorApplication.Window.RoutePush(path);

    [JSInvokable]
    public static int NewRouteId() => BlazorApplication.Window.NewRouteId();
}