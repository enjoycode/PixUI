using System;
using System.Diagnostics;

namespace PixUI;

public static class DragDropManager
{
    /// <summary>
    /// 开始拖动的阀值
    /// </summary>
    private const int DRAG_SIZE = 4;

    private static IDraggable? _dragging;
    private static HitTestEntry? _dropHitEntry;

    private static Overlay? _overlay;
    private static readonly DraggingDecorator Decorator = new();

    internal static DragEvent? DragEvent { get; private set; }

    internal static IDroppable? Dropping { get; private set; }

    internal static Matrix4 HitTransform => _dropHitEntry!.Value.Transform;

    internal static bool MaybeStart(UIWindow window, ref HitTestEntry? pointDownEntry, PointerEvent e)
    {
        if (pointDownEntry?.Widget is not IDraggable draggable) return false;

        if (_dragging == null)
        {
            if (!draggable.AllowDrag()) return false;
            if (Math.Abs(e.X - window.LastPointerDown.X) < DRAG_SIZE &&
                Math.Abs(e.Y - window.LastPointerDown.Y) < DRAG_SIZE)
                return false;

            _dragging = draggable;
            DragEvent = new DragEvent();
            _dragging.OnDragStart(DragEvent);
            //check TransferItem is null, if null set to draggable
            if (DragEvent.TransferItem == null!)
                DragEvent.TransferItem = _dragging;
            if (DragEvent.DragHintImage == null!)
                throw new NotImplementedException(); //TODO: build default hit image

            ShowDecorator();
            Log.Debug($"[{_dragging}] start drag...");
            return true;
        }

        //TODO: 考虑不一样重新启动DragStart
        Debug.Assert(ReferenceEquals(draggable, _dragging));

        #region ----考虑优化----

        // // Check pointer in IDraggable
        // var winX = e.X;
        // var winY = e.Y;
        // var localPt = pointDownEntry.Value.ToLocalPoint(winX, winY);
        // var widget = (Widget) pointDownEntry.Value.Widget;
        // if (widget.ContainsPoint(localPt.Dx, localPt.Dy))
        // {
        //     _dropping = null;
        //     return true;
        // }
        //
        // // Check pointer in current IDroppable
        // if (_dropping != null)
        // { }

        #endregion

        // HitTest for IDroppable
        window.NewHitTest(e.X, e.Y);
        _dropHitEntry = window.NewHitResult.LastEntry;
        window.NewHitResult.Reset();

        if (_dropHitEntry == null || ReferenceEquals(_dropHitEntry.Value.Widget, _dragging))
        {
            LeaveOldDropping();
        }
        else if (ReferenceEquals(_dropHitEntry.Value.Widget, Dropping))
        {
            var localPt = _dropHitEntry.Value.ToLocalPoint(e.X, e.Y);
            Dropping.OnDragOver(DragEvent!, new(localPt.Dx, localPt.Dy));
            Log.Debug($"Drag over 2 [{Dropping}]");
        }
        else if (_dropHitEntry.Value.Widget is IDroppable droppable)
        {
            LeaveOldDropping();
            if (droppable.AllowDrop(DragEvent!))
            {
                Dropping = droppable;
                var localPt = _dropHitEntry.Value.ToLocalPoint(e.X, e.Y);
                Dropping.OnDragOver(DragEvent!, new(localPt.Dx, localPt.Dy));
                Log.Debug($"Drag over 1 [{Dropping}]");
            }
        }

        Decorator.Repaint();

        return true;
    }

    private static void LeaveOldDropping()
    {
        Dropping?.OnDragLeave(DragEvent!);
        DragEvent!.DropEffect = DropEffect.None;
        Dropping = null;
    }

    internal static bool MaybeStop(PointerEvent e)
    {
        if (_dragging == null) return false;

        HideDecorator();

        _dragging.OnDragEnd(DragEvent!);
        if (DragEvent!.DropEffect != DropEffect.None)
        {
            var localPt = _dropHitEntry!.Value.ToLocalPoint(e.X, e.Y);
            Dropping?.OnDrop(DragEvent, new(localPt.Dx, localPt.Dy));
        }

        _dragging = null;
        Dropping = null;
        DragEvent = null;
        return true;
    }

    private static void ShowDecorator()
    {
        if (_dragging == null) return;
        _overlay = ((Widget)_dragging).Overlay;
        _overlay?.Show(Decorator);
    }

    private static void HideDecorator() => _overlay?.Remove(Decorator);
}