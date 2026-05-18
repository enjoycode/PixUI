using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI;

public delegate Widget ListPopupItemBuilder<in T>(T data, int index, State<int> selectState);

/// <summary>
/// 列表弹窗，可通过键盘或鼠标选择指定项，并且支持条件过滤
/// </summary>
public class ListPopup<T> : Popup
{
    public ListPopup(Overlay overlay, ListPopupItemBuilder<T> itemBuilder,
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

        _selectState.AddListener(OnSelectChanged);
    }

    private readonly ListViewController<T> _listViewController;
    private readonly ListPopupItemBuilder<T> _itemBuilder;
    private readonly Card _child;
    private readonly int _maxShowItems; //最多可显示多少个
    private readonly float _itemExtent;

    private readonly State<int> _selectState = -1;
    private bool _selectByTap = true;
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
        _selectState.Value = -1; //reset it
        _listViewController.DataSource = value;
    }

    private Widget BuildItem(T data, int index)
    {
        return new SelectableItem(index, _selectState)
        {
            Width = _child.Width, Height = _itemExtent,
            Child = _itemBuilder(data, index, _selectState)
        };
    }

    public void TrySelectFirst()
    {
        if (_listViewController.DataSource != null && _listViewController.DataSource.Count > 0)
        {
            _selectByTap = false;
            _selectState.Value = 0;
            _selectByTap = true;
            _listViewController.ScrollController.OffsetY = 0;
        }
    }

    /// <summary>
    /// 用于显示前初始化选择的项
    /// </summary>
    public void InitSelect(T item) => _selectState.Value = _listViewController.DataSource!.IndexOf(item);

    public void UpdateFilter(Predicate<T> predicate)
    {
        Relayout(); //强制自己重新布局
        // @ts-ignore
        ChangeDataSource(_fullDataSource?.Where(t => predicate(t)).ToList());
    }

    public void ClearFilter()
    {
        Relayout(); //强制自己重新布局
        ChangeDataSource(_fullDataSource);
    }

    #region ====EventHandlers====

    private void OnSelectChanged(State state)
    {
        if (_selectByTap && _selectState.Value >= 0)
        {
            OnSelectionChanged?.Invoke(DataSource![_selectState.Value]);
            Hide();
        }
    }

    private void OnKeysUp()
    {
        if (_selectState.Value <= 0) return;
        _selectByTap = false;
        _selectState.Value -= 1;
        _selectByTap = true;
        _listViewController.ScrollTo(_selectState.Value);
    }

    private void OnKeysDown()
    {
        if (_selectState.Value == DataSource!.Count - 1) return;
        _selectByTap = false;
        _selectState.Value += 1;
        _selectByTap = true;
        _listViewController.ScrollTo(_selectState.Value);
    }

    private void OnKeysReturn()
    {
        if (_selectState.Value >= 0)
        {
            OnSelectionChanged?.Invoke(DataSource![_selectState.Value]);
            Hide();
        }
    }

    #endregion

    #region ====Overrides====

    public override void VisitChildren<TVisitor>(ref TVisitor visitor) => visitor.Visit(_child);

    protected override void OnLayout(Size maxSize)
    {
        if (DataSource == null) return;

        //计算弹窗的高度
        var maxHeight = Math.Min(_itemExtent * _maxShowItems, DataSource.Count * _itemExtent);
        var cardMarginTotalHeight = _child.Margin == null
            ? Card.DefaultMargin * 2
            : _child.Margin.Value.Top + _child.Margin.Value.Bottom;

        _child.PerformLayout(_child.Width!.Value, maxHeight + cardMarginTotalHeight);
        SetLayoutSize(_child.W, _child.H);
    }

    #endregion

    #region ====EventHook====

    public override EventPreviewResult PreviewEvent(EventType type, object? e)
    {
        if (_listViewController.DataSource == null || _listViewController.DataSource.Count == 0)
            return EventPreviewResult.NotProcessed; //暂简单判断无数据不拦截

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