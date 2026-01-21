using System;

namespace PixUI;

/// <summary>
/// 可点击选择的组件
/// </summary>
public sealed class SelectableItemWidget : SingleChildWidget, IMouseRegion
{
    private readonly State<bool> _hoverState;
    private readonly State<bool> _selectedState;

    public SelectableItemWidget(int index, State<bool> hoverState, State<bool> selectedState, Action<int> onSelect)
    {
        Bind(ref _hoverState!, hoverState, RepaintOnStateChanged);
        _selectedState = selectedState;

        MouseRegion = new MouseRegion(() => Cursors.Hand);
        MouseRegion.HoverChanged += isHover => hoverState.Value = isHover;
        MouseRegion.PointerTap += _ => onSelect(index);
    }

    public MouseRegion MouseRegion { get; private set; }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var fixedWidth = Width?.Value ?? availableWidth;
        var fixedHeight = Height?.Value ?? 20;
        if (Child != null)
        {
            Child.Layout(fixedWidth, fixedHeight);
            Child.SetPosition(0, (fixedHeight - Child.H) / 2f); //暂上下居中
        }

        SetSize(fixedWidth, fixedHeight);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //TODO: 根据样式属性绘制选择状态及Hover状态
        if (_selectedState.Value)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(Theme.FocusedColor));
        else if (_hoverState.Value)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(Theme.AccentColor));

        base.Paint(canvas, area);
    }
}