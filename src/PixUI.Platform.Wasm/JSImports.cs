using System.Runtime.InteropServices.JavaScript;

namespace PixUI.Platform.Wasm;

public partial class JSImports
{
    private const string MODULE_NAME = "main.mjs";

    [JSImport("PixUI.PostInvalidateEvent", MODULE_NAME)]
    internal static partial void PostInvalidateEvent();
}