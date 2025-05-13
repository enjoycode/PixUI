namespace PixUI.Platform.Wasm;

public sealed class WasmCursor : Cursor
{
    internal WasmCursor(string name)
    {
        Name = name;
    }

    internal readonly string Name;
}

public sealed class WasmCursors : IPlatformCursors
{
    private static readonly WasmCursor _arrow = new("auto");
    private static readonly WasmCursor _hand = new("pointer");
    private static readonly WasmCursor _ibeam = new("text");
    private static readonly WasmCursor _resizeLR = new("ew-resize");
    private static readonly WasmCursor _resizeUD = new("ns-resize");

    public Cursor Arrow => _arrow;
    public Cursor Hand => _hand;
    public Cursor IBeam => _ibeam;
    public Cursor ResizeLR => _resizeLR;
    public Cursor ResizeUD => _resizeUD;

    public void SetCursor(Cursor cursor)
    {
        //TODO:
    }
}