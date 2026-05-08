using System;

namespace PixUI;

/// <summary>
/// 可点击选择的组件
/// </summary>
public sealed class SelectableItem : SingleChildWidget, IMouseRegion
{
    private readonly State<bool> _hoverState;
    private readonly State<bool> _selectedState;

    public SelectableItem(int index, State<bool> hoverState, State<bool> selectedState, Action<int> onSelect)
    {
        Bind(ref _hoverState!, hoverState, RepaintOnStateChanged);
        _selectedState = selectedState;

        MouseRegion = new MouseRegion(() => Cursors.Hand);
        MouseRegion.HoverChanged += isHover => hoverState.Value = isHover;
        MouseRegion.PointerTap += _ => onSelect(index);
    }

    public MouseRegion MouseRegion { get; private set; }

    protected override void OnLayout(Size maxSize)
    {
        var fixedWidth = Width?.Value ?? AvailableSize.Width;
        var fixedHeight = Height?.Value ?? 20;
        if (Child != null)
        {
            Child.PerformLayout(new(fixedWidth, fixedHeight));
            Child.SetLayoutLocation(0, (fixedHeight - Child.H) / 2f); //暂上下居中
        }

        SetLayoutSize(fixedWidth, fixedHeight);
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        //TODO: 根据样式属性绘制选择状态及Hover状态
        if (_selectedState.Value)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(Theme.FocusedColor));
        else if (_hoverState.Value)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(Theme.AccentColor));

        base.OnPaint(canvas, area);
    }
}