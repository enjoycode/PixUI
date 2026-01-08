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
    private static readonly BlazorCursor WArrow = new("auto");
    private static readonly BlazorCursor WHand = new("pointer");
    private static readonly BlazorCursor WIbeam = new("text");
    private static readonly BlazorCursor WResizeLR = new("ew-resize");
    private static readonly BlazorCursor WResizeUD = new("ns-resize");

    public Cursor Arrow => WArrow;
    public Cursor Hand => WHand;
    public Cursor IBeam => WIbeam;
    public Cursor ResizeLR => WResizeLR;
    public Cursor ResizeUD => WResizeUD;

    public void SetCursor(Cursor cursor) =>
        ((IJSInProcessRuntime)BlazorApplication.JSRuntime).InvokeVoid("PixUI.SetCursor", ((BlazorCursor)cursor).Name);
}