using System;

namespace PixUI;

public sealed class Tab : SingleChildWidget, IMouseRegion
{
    internal Tab()
    {
        MouseRegion = new MouseRegion(() => Cursors.Hand, false);
        MouseRegion.HoverChanged += _OnHoverChanged;

        Bind(IsSelected, BindingOptions.AffectsVisual); //TODO:待TabBar实现指示器后移除
    }

    public readonly State<bool> IsSelected = false;
    private bool _isHover;

    private ITabBar TabBar => (ITabBar)Parent!;
    public MouseRegion MouseRegion { get; }

    public Action<PointerEvent> OnTap
    {
        set => MouseRegion.PointerTap += value;
    }

    private void _OnHoverChanged(bool hover)
    {
        _isHover = hover;
        if (!IsSelected.Value) //已选中的不需要重绘
            Invalidate(InvalidAction.Repaint);
    }

    public override bool IsOpaque
    {
        get
        {
            if (IsSelected.Value)
                return TabBar.SelectedColor != null && TabBar.SelectedColor.Value.IsOpaque;
            if (_isHover)
                return TabBar.HoverColor != null && TabBar.HoverColor.Value.IsOpaque;
            return false;
        }
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        if (Child == null)
        {
            SetSize(0, 0);
            return;
        }

        Child!.Layout(width, height);

        if (TabBar.Scrollable)
        {
            SetSize(Child.W, height);
            Child.SetPosition(0, (height - Child.H) / 2);
        }
        else
        {
            SetSize(width, height);
            Child.SetPosition((width - Child.W) / 2, (height - Child.H) / 2);
        }
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //根据状态画背景色
        if (IsSelected.Value)
        {
            if (TabBar.SelectedColor != null)
                canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PaintUtils.Shared(TabBar.SelectedColor.Value));
        }
        else if (_isHover)
        {
            if (TabBar.HoverColor != null)
                canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PaintUtils.Shared(TabBar.HoverColor.Value));
        }

        if (Child == null) return;

        canvas.Translate(Child!.X, Child.Y);
        Child.Paint(canvas, area?.ToChild(Child));
        canvas.Translate(-Child.X, -Child.Y);

        //TODO:暂在这里画指示器，待TabBar实现后移除
        if (IsSelected.Value)
            canvas.DrawRect(Rect.FromLTWH(0, H - 4, W, 4), PaintUtils.Shared(Theme.FocusedColor));
    }
}