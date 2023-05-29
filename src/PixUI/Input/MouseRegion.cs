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
    public readonly bool Opaque;

    public event Action<PointerEvent>? PointerDown;
    public event Action<PointerEvent>? PointerUp;
    public event Action<PointerEvent>? PointerMove;
    public event Action<PointerEvent>? PointerTap;
    public event Action<bool>? HoverChanged;

    public MouseRegion(Func<Cursor>? cursor = null, bool opaque = true)
    {
        Cursor = cursor;
        Opaque = opaque;
    }

    internal void RaisePointerMove(PointerEvent theEvent) => PointerMove?.Invoke(theEvent);

    internal void RaisePointerDown(PointerEvent theEvent) => PointerDown?.Invoke(theEvent);

    internal void RaisePointerUp(PointerEvent theEvent) => PointerUp?.Invoke(theEvent);

    internal void RaisePointerTap(PointerEvent theEvent) => PointerTap?.Invoke(theEvent);

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