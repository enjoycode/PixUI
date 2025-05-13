using System.Runtime.InteropServices.JavaScript;
using static PixUI.Platform.Blazor.InputUtils;

namespace PixUI.Platform.Wasm;

public partial class JSExports
{
    [JSExport]
    internal static void InitAppArgs(int glHandle, int width, int height, float pixelRatio) =>
        WasmApplication.InitArgs(glHandle, width, height, pixelRatio);

    [JSExport]
    internal static void OnInvalidate() => ((WasmApplication)UIApplication.Current).RunInvalidateRequest();

    [JSExport]
    internal static void OnMouseDown(int button, float x, float y, float dx, float dy)
    {
        //暂在这里强制HitTest
        var pointEvent = PointerEvent.UseDefault(PointerButtons.None, x, y, dx, dy);
        WasmApplication.Window.OnPointerMove(pointEvent);

        var args = PointerEvent.UseDefault(ConvertButton(button), x, y, dx, dy);
        WasmApplication.Window.OnPointerDown(args);
    }

    [JSExport]
    internal static void OnMouseUp(int button, float x, float y, float dx, float dy)
    {
        var args = PointerEvent.UseDefault(ConvertButton(button), x, y, dx, dy);
        WasmApplication.Window.OnPointerUp(args);
    }
}