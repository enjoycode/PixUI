using Microsoft.JSInterop;

namespace PixUI.Platform.Blazor;

public sealed class BlazorCursor : Cursor
{
    internal BlazorCursor(string name)
    {
        Name = name;
    }

    internal readonly string Name;
}

public sealed class BlazorCursors : IPlatformCursors
{
    private static readonly BlazorCursor _arrow = new("auto");
    private static readonly BlazorCursor _hand = new("pointer");
    private static readonly BlazorCursor _ibeam = new("text");
    private static readonly BlazorCursor _resizeLR = new("e-resize");
    private static readonly BlazorCursor _resizeUD = new("s-resize");

    public Cursor Arrow => _arrow;
    public Cursor Hand => _hand;
    public Cursor IBeam => _ibeam;
    public Cursor ResizeLR => _resizeLR;
    public Cursor ResizeUD => _resizeUD;

    public void SetCursor(Cursor cursor) =>
        ((IJSInProcessRuntime)BlazorApplication.JSRuntime).InvokeVoid("SetCursor", ((BlazorCursor)cursor).Name);
}