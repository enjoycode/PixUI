using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using Microsoft.JSInterop;
using static PixUI.Platform.Blazor.InputUtils;

namespace PixUI.Platform.Blazor;

[SupportedOSPlatform("browser")]
internal static partial class WebBrowser
{
    #region ====Imports====

    [JSImport("globalThis.PixUI.PushWebHistory")]
    internal static partial void PushWebHistory(string fullPath, int index);

    [JSImport("globalThis.PixUI.ReplaceWebHistory")]
    internal static partial void ReplaceWebHistory(string fullPath, int index);

    [JSImport("globalThis.PixUI.StartTextInput")]
    internal static partial void StartTextInput();

    [JSImport("globalThis.PixUI.StopTextInput")]
    internal static partial void StopTextInput();

    [JSImport("globalThis.PixUI.SetInputRect")]
    internal static partial void SetInputRect(float x, float y, float width, float height);

    [JSImport("globalThis.PixUI.ClipboardWriteText")]
    internal static partial Task ClipboardWriteText(string text);

    [JSImport("globalThis.PixUI.ClipboardReadText")]
    internal static partial Task<string?> ClipboardReadText();

    [JSImport("globalThis.PixUI.SetCursor")]
    internal static partial void SetCursor(string cursor);

    [JSImport("globalThis.PixUI.PostInvalidateEvent")]
    internal static partial void PostInvalidateEvent();

    [JSImport("globalThis.PixUI.OpenFile")]
    private static partial Task<JSObject> OpenFileInternal(bool allowMultiple, string[] accepts);

    #endregion

    #region ====Exports====

#if SK_GRAPHITE
    [JSExport]
    private static void OnInvalidate(int onScreenTextureId) =>
        ((BlazorApplication)UIApplication.Current).RunInvalidateRequest(onScreenTextureId);
#else
    [JSExport]
    private static void OnInvalidate() =>
        ((BlazorApplication)UIApplication.Current).RunInvalidateRequest();
#endif

    [JSExport]
    private static void OnMouseMove(int buttons, int x, int y, int dx, int dy)
    {
        var args = PointerEvent.UseDefault(ConvertButtons(buttons), x, y, dx, dy);
        BlazorApplication.Window.OnPointerMove(args);
    }

    [JSExport]
    private static void OnMouseMoveOutWindow()
    {
        BlazorApplication.Window.OnPointerMoveOutWindow();
    }

    [JSExport]
    private static void OnMouseDown(int button, int x, int y, int dx, int dy)
    {
        var args = PointerEvent.UseDefault(ConvertButton(button), x, y, dx, dy);
        BlazorApplication.Window.OnPointerDown(args);
    }

    [JSExport]
    private static void OnMouseUp(int button, int x, int y, int dx, int dy)
    {
        var args = PointerEvent.UseDefault(ConvertButton(button), x, y, dx, dy);
        BlazorApplication.Window.OnPointerUp(args);
    }

    [JSExport]
    public static void OnScroll(int x, int y, int dx, int dy)
    {
        var args = ScrollEvent.Make(x, y, dx, dy);
        BlazorApplication.Window.OnScroll(args);
    }

    [JSExport]
    private static void OnKeyDown(string key, string code, bool alt, bool control, bool shift, bool meta)
    {
        var args = KeyEvent.UseDefault(ConvertKeys(key, code, alt, control, shift, meta));
        BlazorApplication.Window.OnKeyDown(args);
    }

    [JSExport]
    private static void OnKeyUp(string key, string code, bool alt, bool control, bool shift, bool meta)
    {
        var args = KeyEvent.UseDefault(ConvertKeys(key, code, alt, control, shift, meta));
        BlazorApplication.Window.OnKeyUp(args);
    }

    [JSExport]
    private static void OnTextInput(string text) => BlazorApplication.Window.OnTextInput(text);

#if SK_GRAPHITE
#else
    [JSExport]
    public static void OnResize(int width, int height, float ratio) =>
        BlazorApplication.Window.OnResize(width, height, ratio);
#endif

    [JSInvokable]
    public static void RouteGoto(int historyId) => BlazorApplication.Window.RouteGoto(historyId);

    [JSInvokable]
    public static void RoutePush(string path) => BlazorApplication.Window.RoutePush(path);

    [JSInvokable]
    public static int NewRouteId() => BlazorApplication.Window.NewRouteId();

    [JSInvokable]
    public static async Task OnDropFile(int x, int y, string name, int size, string type,
        IJSStreamReference jsStreamReference)
    {
        try
        {
            await using var stream =
                await jsStreamReference.OpenReadStreamAsync(maxAllowedSize: 1024 * 1024 /*TODO: 全局配置*/);
            BlazorApplication.Window.OnDropFile(x, y, name, size, type, stream);
        }
        finally
        {
            await jsStreamReference.DisposeAsync();
        }
    }

    #endregion
}