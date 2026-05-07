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
                return TabBar.SelectedColor != null && TabBar.SelectedColor.Value.IsOpaque;
            if (_isHover)
                return TabBar.HoverColor != null && TabBar.HoverColor.Value.IsOpaque;
            return false;
        }
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        if (Child == null)
        {
            SetSize(0, 0);
            return;
        }

        Child!.Layout(maxSize.Width, maxSize.Height);

        if (TabBar.Scrollable)
        {
            SetSize(Child.W, maxSize.Height);
            Child.SetPosition(0, (maxSize.Height - Child.H) / 2);
        }
        else
        {
            SetSize(maxSize.Width, maxSize.Height);
            Child.SetPosition((maxSize.Width - Child.W) / 2, (maxSize.Height - Child.H) / 2);
        }
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        //根据状态画背景色
        if (IsSelected.Value)
        {
            if (TabBar.SelectedColor != null)
                canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(TabBar.SelectedColor.Value));
        }
        else if (_isHover)
        {
            if (TabBar.HoverColor != null)
                canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(TabBar.HoverColor.Value));
        }

        if (Child == null) return;

        canvas.Translate(Child!.X, Child.Y);
        Child.OnPaint(canvas, area?.ToChild(Child));
        canvas.Translate(-Child.X, -Child.Y);

        //TODO:暂在这里画指示器，待TabBar实现后移除
        if (IsSelected.Value)
            canvas.DrawRect(Rect.FromLTWH(0, H - 4, W, 4), PixUI.Paint.Shared(Theme.FocusedColor));
    }
}