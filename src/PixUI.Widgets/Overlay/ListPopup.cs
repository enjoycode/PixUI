using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI;

public delegate Widget ListPopupItemBuilder<T>(T data, int index, State<bool> isHover,
    State<bool> isSelected);

internal readonly struct ItemState
{
    internal readonly State<bool> HoverState;
    internal readonly State<bool> SelectedState;

    public ItemState(State<bool> hoverState, State<bool> selectedState)
    {
        HoverState = hoverState;
        SelectedState = selectedState;
    }

#if __WEB__
        internal static readonly ItemState Empty = new ItemState(false, false);
        internal ItemState Clone() => new ItemState(HoverState, SelectedState);
#endif
}

internal sealed class ListPopupItemWidget : SingleChildWidget, IMouseRegion
{
    private readonly State<bool> _hoverState;
    private readonly State<bool> _selectedState;

    internal ListPopupItemWidget(int index, State<bool> hoverState, State<bool> selectedState,
        Action<int> onSelect)
    {
        _hoverState = Bind(hoverState, RepaintOnStateChanged);
        _selectedState = selectedState;

        MouseRegion = new MouseRegion(() => Cursors.Hand);
        MouseRegion.HoverChanged += isHover => hoverState.Value = isHover;
        MouseRegion.PointerTap += e => onSelect(index);
    }

    public MouseRegion MouseRegion { get; private set; }

    public override void Layout(float availableWidth, float availableHeight)
    {
        //始终为上级指定的宽高
        var fixedWidth = Width!.Value;
        var fixedHeight = Height!.Value;
        if (Child != null)
        {
            Child.Layout(fixedWidth, fixedHeight);
            Child.SetPosition(0, (fixedHeight - Child.H) / 2f); //暂上下居中
        }
        SetSize(fixedWidth, fixedHeight);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_selectedState.Value)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PaintUtils.Shared(Theme.FocusedColor));
        else if (_hoverState.Value)
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PaintUtils.Shared(Theme.AccentColor));

        base.Paint(canvas, area);
    }
}

/// <summary>
/// 列表弹窗，可通过键盘或鼠标选择指定项，并且支持条件过滤
/// </summary>
public class ListPopup<T> : Popup
{
    public ListPopup(Overlay overlay,
        ListPopupItemBuilder<T> itemBuilder,
        float popupWidth, float itemExtent, int maxShowItems = 5) : base(overlay)
    {
        _itemExtent = itemExtent;
        _maxShowItems = maxShowItems;
        _itemBuilder = itemBuilder;
        _listViewController = new ListViewController<T>();
        _child = new Card
        {
            Width = popupWidth, Elevation = 8,
            Child = new ListView<T>(BuildItem, null, _listViewController)
        };
        _child.Parent = this;
    }

    private readonly ListViewController<T> _listViewController;
    private readonly ListPopupItemBuilder<T> _itemBuilder;
    private readonly Card _child;
    private readonly int _maxShowItems; //最多可显示多少个
    private readonly float _itemExtent;
    private ItemState[]? _itemStates;

    private int _selectedIndex = -1;
    private IList<T>? _fullDataSource;

    public IList<T>? DataSource
    {
        get => _listViewController.DataSource;
        set
        {
            _fullDataSource = value; //reset it
            ChangeDataSource(value);
        }
    }

    public Action<T?>? OnSelectionChanged { get; set; }

    private void ChangeDataSource(IList<T>? value)
    {
        if (value != null)
        {
            _itemStates = new ItemState[value.Count];
            for (var i = 0; i < value.Count; i++)
            {
                _itemStates[i] = new ItemState(false, false);
            }
        }

        _selectedIndex = -1; //reset it
        _listViewController.DataSource = value;
    }

    private Widget BuildItem(T data, int index)
    {
        var states = _itemStates![index];

        return new ListPopupItemWidget(index, states.HoverState, states.SelectedState, OnSelectByTap)
        {
            Width = _child.Width, Height = _itemExtent,
            Child = _itemBuilder(data, index, states.HoverState, states.SelectedState)
        };
    }

    public void TrySelectFirst()
    {
        if (_listViewController.DataSource != null && _listViewController.DataSource.Count > 0)
        {
            Select(0, false);
            _listViewController.ScrollController.OffsetY = 0;
        }
    }

    /// <summary>
    /// 用于显示前初始化选择的项
    /// </summary>
    public void InitSelect(T item)
    {
        var index = _listViewController.DataSource!.IndexOf(item);
        if (index < 0) return;

        _selectedIndex = index;
        _itemStates![_selectedIndex].SelectedState.Value = true;
    }

    private void Select(int index, bool raiseChangedEvent = false)
    {
        if (_selectedIndex == index) return;

        if (_selectedIndex >= 0)
            _itemStates![_selectedIndex].SelectedState.Value = false;

        _selectedIndex = index;

        if (_selectedIndex >= 0)
            _itemStates![_selectedIndex].SelectedState.Value = true;

        if (raiseChangedEvent)
            OnSelectionChanged?.Invoke(DataSource![index]);

        Invalidate(InvalidAction.Repaint); //force repaint
    }

    public void UpdateFilter(Predicate<T> predicate)
    {
        Invalidate(InvalidAction.Relayout); //强制自己重新布局
        // @ts-ignore
        ChangeDataSource(_fullDataSource?.Where(t => predicate(t)).ToList());
    }

    public void ClearFilter()
    {
        Invalidate(InvalidAction.Relayout); //强制自己重新布局
        ChangeDataSource(_fullDataSource);
    }

    #region ====EventHandlers====

    private void OnSelectByTap(int index)
    {
        Select(index, true);
        Hide();
    }

    private void OnKeysUp()
    {
        if (_selectedIndex <= 0) return;
        Select(_selectedIndex - 1, false);
        _listViewController.ScrollTo(_selectedIndex);
    }

    private void OnKeysDown()
    {
        if (_selectedIndex == DataSource!.Count - 1) return;
        Select(_selectedIndex + 1, false);
        _listViewController.ScrollTo(_selectedIndex);
    }

    private void OnKeysReturn()
    {
        if (_selectedIndex >= 0)
        {
            OnSelectionChanged?.Invoke(DataSource![_selectedIndex]);
            Hide();
        }
    }

    #endregion

    #region ====Overrides====

    public override void VisitChildren(Func<Widget, bool> action) => action(_child);

    public override void Layout(float availableWidth, float availableHeight)
    {
        if (DataSource == null) return;

        //计算弹窗的高度
        var maxHeight = Math.Min(_itemExtent * _maxShowItems, DataSource.Count * _itemExtent);
        var cardMarginTotalHeight = _child.Margin == null
            ? Card.DefaultMargin * 2
            : _child.Margin.Value.Top + _child.Margin.Value.Bottom;

        _child.Layout(_child.Width!.Value, maxHeight + cardMarginTotalHeight);
        SetSize(_child.W, _child.H);
    }

    #endregion

    #region ====EventHook====

    public override EventPreviewResult PreviewEvent(EventType type, object? e)
    {
        if (type == EventType.KeyDown)
        {
            var keyEvent = (KeyEvent)e!;
            if (keyEvent.KeyCode == Keys.Up)
            {
                OnKeysUp();
                return EventPreviewResult.NoDispatch;
            }

            if (keyEvent.KeyCode == Keys.Down)
            {
                OnKeysDown();
                return EventPreviewResult.NoDispatch;
            }

            if (keyEvent.KeyCode == Keys.Return)
            {
                OnKeysReturn();
                return EventPreviewResult.NoDispatch;
            }
        }

        return base.PreviewEvent(type, e);
    }

    #endregion
}