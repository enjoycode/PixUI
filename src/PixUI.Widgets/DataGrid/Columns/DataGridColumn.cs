using System;

namespace PixUI;

public abstract class DataGridColumn<T>
{
    protected DataGridColumn(string label)
    {
        Label = label;
    }

    /// <summary>
    /// 列标题
    /// </summary>
    public readonly string Label;

    public ColumnWidth Width { get; set; } = ColumnWidth.Auto();
    public CellStyle? HeaderCellStyle { get; set; }
    public CellStyle? CellStyle { get; set; }
    public Func<T, int, CellStyle>? CellStyleGetter { get; set; }

    /// <summary>
    /// 是否支持合并同一列内相同内容的单元格
    /// </summary>
    public bool AutoMergeCells { get; set; }

    /// <summary>
    /// 是否冻结列
    /// </summary>
    public bool Frozen { get; set; }

    internal DataGridGroupColumn<T>? Parent;
    internal int HeaderRowIndex => Parent == null ? 0 : Parent.HeaderRowIndex + 1;

    #region ----Cached Properties----

    //缓存的计算后的列宽度(像素)
    private float _cachedWidth = 0f;

    //缓存布局后的位置信息,相对于DataGrid.X
    //    |-----DataGrid Width=100, ScrollOffsetX=10-----|
    // |--Col1 Width=30--|--ColOthers----------------|
    // |->Col1.CacheLeft
    //    |->Col1.CachedVisibleLeft
    //                   |->Col1.CachedVisibleRight

    internal float CachedLeft = 0f;
    internal float CachedVisibleLeft = 0f;
    internal float CachedVisibleRight = 0f;

    #endregion

    /// <summary>
    /// 经过布局计算后的实际像素宽度
    /// </summary>
    internal virtual float LayoutWidth => Width.Type == ColumnWidthType.Fixed ? Width.Value : _cachedWidth;

    /// <summary>
    /// 非分组列计算实际像素宽度
    /// </summary>
    internal void CalcWidth(float leftWidth, int leftColumns, float rowHeight)
    {
        var widthChanged = false;
        if (Width.Type == ColumnWidthType.Percent)
        {
            var newWidth = Math.Max(leftWidth / Width.Value, Width.MinValue);
            widthChanged = newWidth != _cachedWidth;
            _cachedWidth = newWidth;
        }
        else if (Width.Type == ColumnWidthType.Auto)
        {
            var newWidth = Math.Max(leftWidth / leftColumns, Width.MinValue);
            widthChanged = newWidth != _cachedWidth;
            _cachedWidth = newWidth;
        }

        if (widthChanged) OnWidthChanged(_cachedWidth, rowHeight);
    }

    /// <summary>
    /// 改变列宽或重设数据源后清除所有缓存
    /// </summary>
    protected internal virtual void ClearAllCache() { }

    protected internal virtual void OnWidthChanged(float width, float height) => ClearAllCache();

    /// <summary>
    /// 清除指定单元格的缓存
    /// </summary>
    protected internal virtual void ClearCacheAt(int rowIndex) { }

    /// <summary>
    /// 滚动后清除相关缓存
    /// </summary>
    protected internal virtual void ClearCacheOnScroll(bool isScrollDown, int rowIndex) { }

    /// <summary>
    ///  画标题，允许子类特殊绘制(如CheckBoxColumn)
    /// </summary>
    protected internal virtual void PaintHeader(Canvas canvas, Rect cellRect, DataGridTheme theme)
    {
        var cellStyle = HeaderCellStyle ?? theme.DefaultHeaderCellStyle;

        //画背景色
        if (cellStyle.FillColor != null)
        {
            var paint = PaintUtils.Shared(cellStyle.FillColor.Value);
            canvas.DrawRect(cellRect, paint);
        }

        //画文本
        using var ph = DataGridPainter.BuildCellParagraph(cellRect,
            cellStyle, theme.DefaultHeaderCellStyle, Label, 2);
        DataGridPainter.PaintCellParagraph(canvas, cellRect, cellStyle, ph);
    }

    /// <summary>
    /// 画单元格背景，由具体实现根据需要调用
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="controller"></param>
    /// <param name="rowIndex"></param>
    /// <param name="cellRect"></param>
    /// <returns>单元格样式，优先级: CellStyleGetter > Column.CellStyle > Default</returns>
    protected CellStyle PaintCellBackground(Canvas canvas, DataGridController<T> controller, int rowIndex,
        in Rect cellRect)
    {
        var theme = controller.Theme;
        //根据设置获取单元格样式
        var cellStyle = CellStyleGetter != null
            ? CellStyleGetter(controller.DataView![rowIndex], rowIndex)
            : CellStyle ?? controller.Theme.DefaultRowCellStyle;

        //画单元格背景
        if (!AutoMergeCells /*合并单元格时不支持*/ && theme.StripeRows && rowIndex % 2 != 0)
        {
            var paint = PaintUtils.Shared(theme.StripeFillColor);
            canvas.DrawRect(cellRect, paint);
        }
        else
        {
            var fillColor = cellStyle.FillColor ?? theme.DefaultRowCellStyle.FillColor;
            if (fillColor.HasValue)
            {
                var paint = PaintUtils.Shared(fillColor.Value);
                canvas.DrawRect(cellRect, paint);
            }
        }

        return cellStyle;
    }

    /// <summary>
    /// 画单元格内容
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="controller"></param>
    /// <param name="rowIndex"></param>
    /// <param name="cellRect"></param>
    protected internal virtual void PaintCell(Canvas canvas, DataGridController<T> controller,
        int rowIndex, Rect cellRect) { }

    /// <summary>
    /// 尝试向上合并单元格
    /// </summary>
    protected internal virtual int TryMergeUp(DataGridController<T> controller, int currentRow) => 0;

    /// <summary>
    /// 尝试向下合并单元格
    /// </summary>
    protected internal virtual int TryMergeDown(DataGridController<T> controller, int currentRow) => 0;
}