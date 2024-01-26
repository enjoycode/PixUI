using System;
using System.Diagnostics;

namespace PixUI;

public static class DragDropManager
{
    private static IDraggable? _dragging;
    private static IDroppable? _dropping;
    private static DragEvent? _dragEvent;

    internal static bool MaybeDrag(UIWindow window, ref HitTestEntry? pointDownEntry, PointerEvent e)
    {
        if (pointDownEntry?.Widget is not IDraggable draggable) return false;

        if (_dragging == null)
        {
            if (!draggable.AllowDrag()) return false;

            _dragging = draggable;
            _dragEvent = new DragEvent();
            _dragging.OnDragStart(_dragEvent);
            //check TransferItem is null, if null stop dragging
        }
        else
        {
            //TODO: 考虑不一样重新启动DragStart
            Debug.Assert(ReferenceEquals(draggable, _dragging));
        }

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
        var hitEntry = window.NewHitResult.LastEntry;
        window.NewHitResult.Reset();

        if (hitEntry == null || ReferenceEquals(hitEntry.Value.Widget, _dragging))
        {
            _dropping = null;
        }
        else if (ReferenceEquals(hitEntry.Value.Widget, _dropping))
        {
            var localPt = hitEntry.Value.ToLocalPoint(e.X, e.Y);
            _dropping.OnDragOver(_dragEvent!, new(localPt.Dx, localPt.Dy));
        }
        else
        {
            if (hitEntry.Value.Widget is IDroppable droppable)
            {
                
            }
        }

        return true;
    }
}