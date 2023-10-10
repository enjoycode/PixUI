using System;

namespace PixUI;

/// <summary>
/// 实现此接口的Widget接收处理Mouse事件
/// </summary>
[TSInterfaceOf]
public interface IMouseRegion
{
    public MouseRegion MouseRegion { get; }
}

public sealed class MouseRegion
{
    /// <summary>
    /// Hover时的Cursor
    /// </summary>
    public readonly Func<Cursor>? Cursor;

    /// <summary>
    /// false会继续检测子级嵌套的MouseRegion
    /// </summary>
    public bool Opaque { get; set; }

    public event Action<PointerEvent>? PointerDown;
    public event Action<PointerEvent>? PointerUp;
    public event Action<PointerEvent>? PointerMove;
    public event Action<PointerEvent>? PointerTap;
    public event Action<bool>? HoverChanged;
    public event Action<IDataTransferItem>? Drop;

    private readonly Func<IDataTransferItem, bool>? _allowDrop;

    public MouseRegion(Func<Cursor>? cursor = null, bool opaque = true, Func<IDataTransferItem, bool>? allowDrop = null)
    {
        Cursor = cursor;
        Opaque = opaque;
        _allowDrop = allowDrop;
    }

    public bool AllowDrop(IDataTransferItem item) => _allowDrop != null && _allowDrop(item);

    internal void RaisePointerMove(PointerEvent theEvent) => PointerMove?.Invoke(theEvent);

    internal void RaisePointerDown(PointerEvent theEvent) => PointerDown?.Invoke(theEvent);

    internal void RaisePointerUp(PointerEvent theEvent) => PointerUp?.Invoke(theEvent);

    internal void RaisePointerTap(PointerEvent theEvent) => PointerTap?.Invoke(theEvent);

    internal void RaiseDrop(IDataTransferItem item) => Drop?.Invoke(item);

    internal void RaiseHoverChanged(bool hover)
    {
        if (Cursor != null)
            PixUI.Cursor.Current = hover ? Cursor() : Cursors.Arrow;
        HoverChanged?.Invoke(hover);
    }

    internal void RestoreHoverCursor()
    {
        if (Cursor != null)
            PixUI.Cursor.Current = Cursor();
    }
}