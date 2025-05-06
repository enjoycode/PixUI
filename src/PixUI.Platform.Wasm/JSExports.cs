using System.Runtime.InteropServices.JavaScript;

namespace PixUI.Platform.Wasm;

public partial class JSExports
{
    [JSExport]
    internal static void InitAppArgs(int glHandle, int width, int height, float pixelRatio) =>
        WasmApplication.InitArgs(glHandle, width, height, pixelRatio);

    [JSExport]
    internal static void OnInvalidate() => ((WasmApplication)UIApplication.Current).RunInvalidateRequest();
}