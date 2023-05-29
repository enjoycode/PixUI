using System;
using System.Collections.Generic;

namespace PixUI;

/// <summary>
/// 管理打开的子菜单（可能多级）
/// </summary>
internal sealed class PopupMenuStack : Popup
{
    private readonly Action _closeAll;

    public PopupMenuStack(Overlay overlay, Action closeAll) : base(overlay)
    {
        _closeAll = closeAll;
    }

    private readonly List<PopupMenu> _children = new List<PopupMenu>();

    internal bool HasChild => _children.Count > 0;

    #region ====PopupMenu Manager====

    internal void Add(PopupMenu child)
    {
        child.Parent = this;
        _children.Add(child);

        if (_children.Count == 1)
            Show();
        else
            Invalidate(InvalidAction.Repaint);
    }

    internal bool TryCloseSome(MenuItemWidget newHoverItem)
    {
        //判断子级回到任意上级
        for (int i = _children.Count - 1; i >= 0; i--)
        {
            if (ReferenceEquals(newHoverItem, _children[i].Owner))
            {
                CloseTo(newHoverItem.Depth);
                return true; //调用者不需要后续操作
            }
        }

        var lastMenuItem = _children[_children.Count - 1].Owner;
        //判断是否同级
        if (newHoverItem.Depth == lastMenuItem?.Depth)
        {
            CloseTo(newHoverItem.Depth - 1);
        }
        //再判断是否直接子级
        else if (lastMenuItem != null && !IsChildOf(newHoverItem, lastMenuItem))
        {
            CloseTo(newHoverItem.Depth - 1);
        }

        return false;
    }

    private static bool IsChildOf(MenuItemWidget child, MenuItemWidget parent)
    {
        var isChild = false;
        foreach (var item in parent.MenuItem.Children!)
        {
            if (ReferenceEquals(item, child.MenuItem))
            {
                isChild = true;
                break;
            }
        }

        return isChild;
    }

    private void CloseTo(int depth)
    {
        var needInvalidate = false;
        for (var i = _children.Count - 1; i >= 0; i--)
        {
            if (i == depth)
                break;

            _children[i].Parent = null;
            _children.RemoveAt(i);
            needInvalidate = true;
        }

        if (needInvalidate)
            Invalidate(InvalidAction.Repaint);
    }

    #endregion

    #region ====Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        foreach (var child in _children)
        {
            if (action(child)) break;
        }
    }

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        foreach (var child in _children)
        {
            if (HitTestChild(child, x, y, result)) return true;
        }

        return false;
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        //do nothing,每个弹出的子菜单在加入前已经手动布局过
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        foreach (var child in _children)
        {
            canvas.Translate(child.X, child.Y);
            child.Paint(canvas, area);
            canvas.Translate(-child.X, -child.Y);
        }
    }

    #endregion

    #region ====EventHook====

    public override EventPreviewResult PreviewEvent(EventType type, object? e)
    {
        //TODO: 判断ESC键及其他
        if (type == EventType.PointerDown)
        {
            var pointerEvent = (PointerEvent)e!;
            //判断是否在所有子菜单区域外，是则移除所有
            var someOneContains = false;
            foreach (var child in _children)
            {
                if (child.ContainsPoint(pointerEvent.X, pointerEvent.Y))
                {
                    someOneContains = true;
                    break;
                }
            }

            if (!someOneContains)
            {
                _closeAll();
                return EventPreviewResult.Processed;
            }
        }

        return base.PreviewEvent(type, e);
    }

    #endregion
}