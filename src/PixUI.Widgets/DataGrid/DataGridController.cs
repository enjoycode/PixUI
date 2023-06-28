using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI;

public sealed class DataGridController<T> /* where T : notnull*/
{
    public DataGridController()
    {
        Columns = new DataGridColumns<T>(this);
    }

    internal readonly ScrollController ScrollController = new(ScrollDirection.Both);

    private DataGrid<T>? _owner;

    internal void Attach(DataGrid<T> dataGrid) => _owner = dataGrid;

    public DataGridTheme Theme => _owner!.Theme;

    internal DataGrid<T> DataGrid => _owner!;

    public DataGridColumns<T> Columns { get; }

    #region ----Layout Properties----

    /// <summary>
    /// Header的行数，不包含分组列始终为1
    /// </summary>
    internal int HeaderRows { get; private set; } = 1;

    internal float HeaderRowHeight { get; private set; } = 35f;

    internal float TotalHeaderHeight => HeaderRows * HeaderRowHeight;

    internal float TotalRowsHeight => DataView == null ? 0f : DataView.Count * Theme.RowHeight;

    internal float TotalColumnsWidth => _cachedLeafColumns.Sum(c => c.LayoutWidth);

    /// <summary>
    /// 是否包含冻结列
    /// </summary>
    internal bool HasFrozen { get; private set; }

    internal float ScrollDeltaY => ScrollController.OffsetY % Theme.RowHeight;

    /// <summary>
    /// 第一行可见行索引号
    /// </summary>
    internal int VisibleStartRowIndex =>
        (int)Math.Truncate(ScrollController.OffsetY / Theme.RowHeight);

    /// <summary>
    /// 可见行总数
    /// </summary>
    internal int VisibleRows =>
        (int)Math.Ceiling(Math.Max(0, DataGrid.H - TotalHeaderHeight) / Theme.RowHeight);

    #endregion

    #region ----DataSource Properties----

    private IList<T>? _dataSource;

    /// <summary>
    /// 数据源
    /// </summary>
    public IList<T> DataSource
    {
        set
        {
            var oldEmpty = _dataSource == null ? true : _dataSource.Count == 0;
            var newEmpty = value == null ? true : value.Count == 0;

            _dataSource = value;
            ClearAllCache();

            if (oldEmpty && newEmpty) return;
            _owner?.Invalidate(InvalidAction.Repaint);
        }
    }

    public IList<T>? DataView
    {
        get
        {
            //TODO: sort and filter
            return _dataSource;
        }
    }

    #endregion

    #region ----Cached Properties----

    // 所有非分组的列集合
    private readonly IList<DataGridColumn<T>> _cachedLeafColumns = new List<DataGridColumn<T>>();

    private readonly IList<DataGridColumn<T>> _cachedVisibleColumns = new List<DataGridColumn<T>>();

    // 缓存的组件尺寸
    private Size _cachedWidgetSize = new Size(0, 0);

    private float _cachedScrollLeft = 0.0f;
    private float _cachedScrollRight = 0.0f;

    private DataGridHitTestResult<T>? _cachedHitInHeader;
    private DataGridHitTestResult<T>? _cachedHitInRows;

    #endregion

    #region ----Selection----

    private readonly List<int> _selectedRows = new();

    public event Action? SelectionChanged;

    public int CurrentRowIndex => _selectedRows.Count > 0 ? _selectedRows[0] : -1;

    /// <summary>
    /// 返回监测当前选择的行的状态
    /// </summary>
    public State<T?> ObserveCurrentRow()
    {
        var state = new RxProperty<T?>(
            () =>
            {
                if (DataView == null || _selectedRows.Count == 0)
                {
                    object? nullValue = null;
                    return (T?)nullValue; //Don't use default(T) for Web
                }

                return DataView[_selectedRows[0]];
            },
            newRow =>
            {
                if (newRow == null)
                {
                    ClearSelection();
                    return;
                }

                var index = DataView!.IndexOf(newRow);
                SelectAt(index);
            },
            false
        );
        SelectionChanged += () => state.NotifyValueChanged();

        return state;
    }

    public void SelectAt(int index)
    {
        var oldRowIndex = CurrentRowIndex;
        //_cachedHitInRows = new DataGridHitTestResult<T>(Columns[0], index);
        var newRowIndex = index; //_cachedHitInRows != null ? _cachedHitInRows.Value.RowIndex : -1;
        TrySelectRow(oldRowIndex, newRowIndex);
    }

    public void ClearSelection()
    {
        _selectedRows.Clear();
        _cachedHitInRows = null;
        SelectionChanged?.Invoke();
    }

    private void TrySelectRow(int oldRowIndex, int newRowIndex)
    {
        if (oldRowIndex == newRowIndex) return;

        _selectedRows.Clear();
        if (newRowIndex != -1)
            _selectedRows.Add(newRowIndex);
        SelectionChanged?.Invoke();
    }

    #endregion

    #region ====Event Handles====

    public void Invalidate() => _owner?.Invalidate(InvalidAction.Repaint);

    private void ClearAllCache()
    {
        //TODO:暂所有列，考虑仅可见列
        foreach (var column in _cachedLeafColumns)
        {
            column.ClearAllCache();
        }
    }

    internal void ClearCacheOnScroll(bool isScrollDown, int rowIndex)
    {
        //Console.WriteLine($"---------->ClearCache: down={isScrollDown} row={rowIndex}");
        foreach (var column in _cachedLeafColumns /*TODO:暂所有列，考虑仅可见列*/)
        {
            column.ClearCacheOnScroll(isScrollDown, rowIndex);
        }
    }

    internal void OnPointerMove(PointerEvent e)
    {
        if (e.Buttons == PointerButtons.Left && e.DeltaX != 0 && _cachedHitInHeader != null)
        {
            //TODO:根据列宽定义改变
            var col = _cachedHitInHeader.Value.Column;
            if (col.Width.Type == ColumnWidthType.Fixed)
            {
                var delta = e.DeltaX;
                var newWidth = col.Width.Value + delta;
                col.Width.ChangeValue(newWidth);
                col.ClearAllCache(); //固定列暂需要
                if (delta < 0 && ScrollController.OffsetX > 0)
                {
                    //减小需要重设滚动位置
                    ScrollController.OffsetX = Math.Max(ScrollController.OffsetX + delta, 0f);
                }

                //重新计算所有列宽并重绘
                CalcColumnsWidth(_cachedWidgetSize, true);
                _owner?.Invalidate(InvalidAction.Repaint);
            }
        }
    }

    internal void OnPointerDown(PointerEvent e)
    {
        if (e.Y <= TotalHeaderHeight)
        {
            //do nothing now
            return;
        }

        //TODO:暂仅支持单选
        var oldRowIndex = CurrentRowIndex;
        //TODO: 如果移动端则需要 _cachedHitInRows = HitTestInRows(e.X, e.Y);
        var newRowIndex = _cachedHitInRows != null ? _cachedHitInRows.Value.RowIndex : -1;
        TrySelectRow(oldRowIndex, newRowIndex);
        //TODO: if (res == _cachedHitInRows) return;

        //检查是否需要自动滚动
        // var needScroll = false;
        if (_cachedHitInRows != null && (_cachedHitInRows.Value.ScrollDeltaX != 0 ||
                                         _cachedHitInRows.Value.ScrollDeltaY != 0))
        {
            // needScroll = true;
            ScrollController.OffsetX += _cachedHitInRows.Value.ScrollDeltaX;
            ScrollController.OffsetY += _cachedHitInRows.Value.ScrollDeltaY;
        }

        // if (needScroll)
        _owner?.Invalidate(InvalidAction.Repaint);
    }

    internal bool HitTestInHeader(float x, float y)
    {
        if (y <= TotalHeaderHeight)
        {
            foreach (var col in _cachedVisibleColumns)
            {
                if (col.CachedVisibleLeft <= x && x <= col.CachedVisibleRight)
                {
                    var isColumnResizer = col.CachedVisibleRight - x <= 5;
                    _cachedHitInHeader = new DataGridHitTestResult<T>(col, -1, 0f, 0f, isColumnResizer);
                    break;
                }
            }

            if (_cachedHitInHeader != null && _cachedHitInHeader.Value.IsColumnResizer)
                Cursor.Current = Cursors.ResizeLR;
            else
                Cursor.Current = Cursors.Arrow;

            return true;
        }

        //没有命中
        if (_cachedHitInHeader != null)
        {
            Cursor.Current = Cursors.Arrow;
            _cachedHitInHeader = null;
        }

        return false;
    }

    internal DataGridHitTestResult<T>? HitTestInRows(float x, float y)
    {
        _cachedHitInRows = HitTestInRowsInternal(x, y);
        return _cachedHitInRows;
    }

    private DataGridHitTestResult<T>? HitTestInRowsInternal(float x, float y)
    {
        //TODO:先判断仍旧在缓存的范围内，是则直接返回

        if (DataView == null || DataView.Count == 0) return null;

        var scrollX = 0f;
        var scrollY = 0f;

        var rowIndex = (int)Math.Truncate((y - TotalHeaderHeight + ScrollController.OffsetY) / Theme.RowHeight);
        //判断是否超出范围
        if (rowIndex >= DataView.Count)
            return _cachedHitInRows;

        var deltaY = ScrollDeltaY;
        if (deltaY != 0)
        {
            if (rowIndex == VisibleStartRowIndex)
            {
                scrollY = -deltaY;
            } //TODO: rowIndex == visibleEndRowIndex
        }

        foreach (var col in _cachedVisibleColumns)
        {
            if (col.CachedVisibleLeft <= x && x <= col.CachedVisibleRight)
            {
                if (col.CachedVisibleLeft != col.CachedLeft)
                {
                    scrollX = col.CachedLeft - col.CachedVisibleLeft;
                }
                else if (col.CachedVisibleRight != col.CachedLeft + col.LayoutWidth)
                {
                    scrollX = col.CachedLeft + col.LayoutWidth - col.CachedVisibleRight;
                }

                return new DataGridHitTestResult<T>(col, rowIndex, scrollX, scrollY);
            }
        }

        return null;
    }

    #endregion

    #region ====Layout Methods====

    internal void RelayoutIfMounted()
    {
        if (_owner is { IsMounted: true })
            _owner.Invalidate(InvalidAction.Relayout);
    }

    /// <summary>
    ///  计算所有列宽度
    /// </summary>
    internal void CalcColumnsWidth(in Size widgetSize, bool force = false)
    {
        var needCalc = _cachedWidgetSize.Width != widgetSize.Width;
        if (ScrollController.OffsetX > 0 && widgetSize.Width > _cachedWidgetSize.Width)
        {
            //如果变宽了且有横向滚动，需要扣减
            var deltaX = widgetSize.Width - _cachedWidgetSize.Width;
            ScrollController.OffsetX = Math.Max(ScrollController.OffsetX - deltaX, 0);
        }

        _cachedWidgetSize = widgetSize;
        if (!needCalc && !force) return;

        //先计算固定宽度列
        var fixedColumns = _cachedLeafColumns
            .Where(c => c.Width.Type == ColumnWidthType.Fixed).ToArray();
        var fixedWidth = fixedColumns.Sum(c => c.Width.Value);
        var leftWidth = _cachedWidgetSize.Width - fixedWidth;
        var leftColumns = _cachedLeafColumns.Count - fixedColumns.Length;

        //再计算百分比列宽度
        var percentColumns = _cachedLeafColumns
            .Where(c => c.Width.Type == ColumnWidthType.Percent).ToArray();
        var percentWidth = percentColumns.Sum(c =>
        {
            c.CalcWidth(leftWidth, leftColumns);
            return c.LayoutWidth;
        });
        leftWidth -= percentWidth;
        leftColumns -= percentColumns.Length;

        //最后计算自动列宽
        var autoColumns = _cachedLeafColumns
            .Where(c => c.Width.Type == ColumnWidthType.Auto).ToArray();
        foreach (var col in autoColumns)
        {
            col.CalcWidth(leftWidth, leftColumns);
        }
    }

    /// <summary>
    /// 计算可视列及其位置
    /// </summary>
    internal IList<DataGridColumn<T>> LayoutVisibleColumns(Size size)
    {
        _cachedVisibleColumns.Clear();

        var colStartIndex = 0;
        var colEndIndex = _cachedLeafColumns.Count - 1;
        var remainWidth = size.Width;
        var offsetX = 0f;
        var needScroll = size.Width < TotalColumnsWidth;
        var insertIndex = 0;

        if (needScroll && HasFrozen)
        {
            //先计算左侧冻结列
            for (var i = 0; i < _cachedLeafColumns.Count; i++)
            {
                var col = _cachedLeafColumns[i];
                if (!col.Frozen)
                {
                    colStartIndex = i;
                    break;
                }

                col.CachedLeft = col.CachedVisibleLeft = offsetX;
                col.CachedVisibleRight = col.CachedLeft + col.LayoutWidth;
                _cachedVisibleColumns.Insert(insertIndex++, col);

                offsetX += col.LayoutWidth;
            }

            remainWidth -= offsetX;
            if (remainWidth <= 0) return _cachedVisibleColumns;

            //再计算右侧冻结列
            var rightOffsetX = 0.0f;
            for (var i = _cachedLeafColumns.Count - 1; i >= 0; i--)
            {
                var col = _cachedLeafColumns[i];
                if (!col.Frozen)
                {
                    colEndIndex = i;
                    break;
                }

                col.CachedLeft = size.Width - rightOffsetX - col.LayoutWidth;
                col.CachedVisibleLeft = col.CachedLeft;
                col.CachedVisibleRight = col.CachedLeft + col.LayoutWidth;
                _cachedVisibleColumns.Add(col);

                rightOffsetX += col.LayoutWidth;
                if (remainWidth - rightOffsetX <= 0) return _cachedVisibleColumns;
            }

            remainWidth -= rightOffsetX;
            if (remainWidth <= 0) return _cachedVisibleColumns;
        }

        _cachedScrollLeft = offsetX;
        _cachedScrollRight = offsetX + remainWidth;

        if (ScrollController.OffsetX > 0)
        {
            var skipWidth = 0.0f;
            for (var i = colStartIndex; i <= colEndIndex; i++)
            {
                var col = _cachedLeafColumns[i];
                skipWidth += col.LayoutWidth;
                if (skipWidth <= ScrollController.OffsetX) continue;

                colStartIndex = i;
                offsetX = offsetX - ScrollController.OffsetX + (skipWidth - col.LayoutWidth);
                break;
            }
        }

        for (var i = colStartIndex; i <= colEndIndex; i++)
        {
            var col = _cachedLeafColumns[i];
            col.CachedLeft = offsetX;
            col.CachedVisibleLeft = Math.Max(_cachedScrollLeft, col.CachedLeft);
            col.CachedVisibleRight = Math.Min(_cachedScrollRight, col.CachedLeft + col.LayoutWidth);
            _cachedVisibleColumns.Insert(insertIndex++, col);

            // print("${col.label} offsetX=$offsetX VL=${col.cachedVisibleLeft} VR=${col.cachedVisibleRight}");

            offsetX += col.LayoutWidth;
            if (offsetX >= _cachedScrollRight) break;
        }

        return _cachedVisibleColumns;
    }

    internal Rect GetScrollClipRect(float top, float height) =>
        Rect.FromLTWH(_cachedScrollLeft, top, _cachedScrollRight - _cachedScrollLeft, height);

    internal Rect? GetCurrentRowRect()
    {
        if (_selectedRows.Count == 0) return null;

        var top = TotalHeaderHeight + (_selectedRows[0] - VisibleStartRowIndex) * Theme.RowHeight - ScrollDeltaY;
        return new Rect(1, top + 1, _owner!.W - 2, top + Theme.RowHeight - 1);
    }

    /// <summary>
    /// 获取当前选择的单元格的边框
    /// </summary>
    internal Rect? GetCurrentCellRect()
    {
        if (_cachedHitInRows == null || _cachedHitInRows.Value.RowIndex == -1)
            return null;

        var hitColumn = _cachedHitInRows.Value.Column;
        var top = TotalHeaderHeight +
            (_cachedHitInRows.Value.RowIndex - VisibleStartRowIndex) * Theme.RowHeight - ScrollDeltaY;
        return new Rect(hitColumn.CachedVisibleLeft + 1, top + 1,
            hitColumn.CachedVisibleRight - 2, top + Theme.RowHeight - 1);
    }

    internal void ClearLeafColumns() => _cachedLeafColumns.Clear();

    internal void GetLeafColumns(DataGridColumn<T> column, bool? parentFrozen)
    {
        if (parentFrozen != null)
            column.Frozen = parentFrozen.Value;

        if (column is DataGridGroupColumn<T> groupColumn)
        {
            HeaderRows += 1;
            foreach (var child in groupColumn.Children)
            {
                child.Parent = groupColumn;
                GetLeafColumns(child, column.Frozen);
            }
        }
        else
        {
            _cachedLeafColumns.Add(column);
        }
    }

    internal void RemoveLeafColumns(DataGridColumn<T> column)
    {
        if (column is DataGridGroupColumn<T> groupColumn)
        {
            HeaderRows -= 1;
            foreach (var child in groupColumn.Children)
            {
                RemoveLeafColumns(child);
            }
        }
        else
        {
            _cachedLeafColumns.Remove(column);
        }
    }

    internal void CheckHasFrozen()
    {
        //TODO:纠正一些错误的冻结列设置,如全部冻结，中间有冻结等
        HasFrozen = _cachedLeafColumns.Any(c => c.Frozen);
    }

    #endregion

    #region ====Add / Remove / Refresh=====

    public void Add(T item)
    {
        _dataSource!.Add(item);
        //TODO: refresh DataView
        _owner?.Invalidate(InvalidAction.Repaint);
    }

    public void Remove(T item)
    {
        var indexInDataView = DataView!.IndexOf(item);
        RemoveAt(indexInDataView);
    }

    public void RemoveAt(int index)
    {
        var rowIndex = index;
        if (!ReferenceEquals(DataView, _dataSource))
        {
            var rowInView = DataView![index];
            DataView.RemoveAt(index);
            rowIndex = _dataSource!.IndexOf(rowInView);
        }

        _dataSource!.RemoveAt(rowIndex);
        ClearSelection(); //TODO: rowIndex when in selection
        ClearAllCache(); //TODO:仅移除并重设缓存
        _owner?.Invalidate(InvalidAction.Repaint);
    }

    /// <summary>
    /// 清除缓存并重绘
    /// </summary>
    public void Refresh()
    {
        ClearAllCache();
        _owner?.Invalidate(InvalidAction.Repaint);
    }

    #endregion
}