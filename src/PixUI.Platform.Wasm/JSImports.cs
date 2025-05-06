using System.Runtime.InteropServices.JavaScript;

namespace PixUI.Platform.Wasm;

public partial class JSImports
{
    [JSImport("node.process.version", "main.mjs")]
    internal static partial string GetNodeVersion();
}