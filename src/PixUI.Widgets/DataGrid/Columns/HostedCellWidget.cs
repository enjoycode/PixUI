using System;

namespace PixUI;

/// <summary>
/// 用于DataGridHostColumn承载单元格组件
/// </summary>
internal sealed class HostedCellWidget : Widget
{
    public HostedCellWidget(IScrollable dataGridBody, IDataGridHostColumn column, Widget child, float offsetYToBody)
    {
        IsLayoutTight = false; //充满单元格
        child.Parent = this;
        _hostedWidget = child;
        _dataGridBody = dataGridBody;
        _column = column;
        _offsetYToBody = offsetYToBody;
    }

    private readonly Widget _hostedWidget;

    /// <summary>
    /// 不包含滚动偏移量的相对于DataGridBody的Y位置
    /// </summary>
    private readonly float _offsetYToBody;

    private readonly IScrollable _dataGridBody;
    private readonly IDataGridHostColumn _column;

    //注意X加上滚动偏移量用于抵消
    protected internal override float X => _column.LeftToDataGrid + _dataGridBody.ScrollOffsetX;

    protected internal override float Y => _offsetYToBody;

    public override void VisitChildren(Func<Widget, bool> action) => action(_hostedWidget);

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        SetSize(width, height);
        _hostedWidget.Layout(width, height);
        //TODO:根据对齐方式设置位置，暂简单居中
        _hostedWidget.SetPosition((width - _hostedWidget.W) / 2, 0);
    }

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false, Rect? dirtyRect = null)
    {
        //这里需要重写处理滚动量,不用考虑列是否冻结，因取X值时会抵消横向滚动量
        base.BeforePaint(canvas, onlyTransform, dirtyRect);
        canvas.Translate(-_dataGridBody.ScrollOffsetX, -_dataGridBody.ScrollOffsetY);
    }

    protected internal override void AfterPaint(Canvas canvas)
    {
        canvas.Translate(_dataGridBody.ScrollOffsetX, _dataGridBody.ScrollOffsetY);
        base.AfterPaint(canvas);
    }
}