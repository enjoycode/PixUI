using System;

namespace PixUI;

public sealed class Tab : SingleChildWidget, IMouseRegion
{
    internal Tab()
    {
        MouseRegion = new MouseRegion(() => Cursors.Hand, false);
        MouseRegion.HoverChanged += _OnHoverChanged;

        IsSelected.AddListener(RepaintOnStateChanged); //TODO:待TabBar实现指示器后移除
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
            Repaint();
    }

    public override bool IsOpaque
    {
        get
        {
            if (IsSelected.Value)
                return TabBar.SelectedColor is { IsOpaque: true };
            if (_isHover)
                return TabBar.HoverColor is { IsOpaque: true };
            return false;
        }
    }

    protected internal override Size LayoutSize
    {
        get
        {
            //重写因为可能TabView的布局空间不够TabBar指定的高度造成的overflow
            var tabView = Parent?.Parent;
            //TODO: 还需要根据TabBar的方向判断处理
            return tabView == null
                ? base.LayoutSize
                : new(base.LayoutSize.Width, Math.Min(tabView.LayoutSize.Height, base.LayoutSize.Height));
        }
    }

    protected override void OnLayout(Size maxSize)
    {
        if (Child == null)
        {
            SetLayoutSize(0, 0);
            return;
        }

        Child!.PerformLayout(maxSize);

        if (TabBar.Scrollable)
        {
            SetLayoutSize(Child.W, maxSize.Height);
            Child.SetLayoutLocation(0, (maxSize.Height - Child.H) / 2);
        }
        else
        {
            SetLayoutSize(maxSize);
            Child.SetLayoutLocation((maxSize.Width - Child.W) / 2, (maxSize.Height - Child.H) / 2);
        }
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        //根据状态画背景色
        if (IsSelected.Value)
        {
            if (TabBar.SelectedColor != null)
                canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), Paint.Shared(TabBar.SelectedColor.Value));
        }
        else if (_isHover)
        {
            if (TabBar.HoverColor != null)
                canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), Paint.Shared(TabBar.HoverColor.Value));
        }

        if (Child == null) return;

        PaintChild(Child, canvas, area);

        //TODO:暂在这里画指示器，待TabBar实现后移除
        if (IsSelected.Value)
            canvas.DrawRect(Rect.FromLTWH(0, AvailableSize.Height - 4, W, 4), Paint.Shared(Theme.FocusedColor));
    }
}