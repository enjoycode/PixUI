namespace PixUI;

/// <summary>
/// 可点击选择的列表项组件
/// </summary>
public sealed class SelectableItem : SingleChildWidget, IMouseRegion
{
    public SelectableItem(int index, State<int> selectState)
    {
        _index = index;
        _selectState = selectState;
        _selectState.AddListener(OnSelectChanged);
        _isSelected = _selectState.Value == _index;

        MouseRegion = new MouseRegion(() => Cursors.Hand);
        MouseRegion.HoverChanged += OnHoverChanged;
        MouseRegion.PointerTap += _ => selectState.Value = _index;
    }

    private readonly State<int> _selectState;
    private readonly int _index;
    private bool _isHover;
    private bool _isSelected;

    public MouseRegion MouseRegion { get; }

    private void OnHoverChanged(bool isHover)
    {
        _isHover = isHover;
        Repaint();
    }

    private void OnSelectChanged(State state)
    {
        if (_isSelected)
        {
            _isSelected = false;
            Repaint();
        }

        if (_selectState.Value == _index)
        {
            _isSelected = true;
            Repaint();
        }
    }

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
        if (_isSelected)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), Paint.Shared(Theme.FocusedColor));
        else if (_isHover)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), Paint.Shared(Theme.AccentColor));

        base.OnPaint(canvas, area);
    }
}