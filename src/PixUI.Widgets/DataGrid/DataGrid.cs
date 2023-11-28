using System;
using System.Collections.Generic;
using System.Linq;

namespace PixUI;

public sealed class DataGrid<T> : Widget, IMouseRegion
{
    public DataGrid(DataGridController<T> controller)
    {
        _controller = controller;
        _controller.Attach(this);

        Body = new DataGridBody<T>(_controller);
        Body.Parent = this;

        MouseRegion = new MouseRegion(null, false);
        MouseRegion.PointerMove += _controller.OnPointerMove;
        MouseRegion.PointerDown += _controller.OnPointerDown;
    }

    private readonly DataGridController<T> _controller;
    internal readonly DataGridBody<T> Body;
    private DataGridHeader<T>? _header;
    private DataGridFooter<T>? _footer;
    private DataGridTheme _theme = DataGridTheme.Default;

    public bool ShowHeader { get; init; } = true;
    public bool ShowFooter { get; init; } = false;

    public DataGridTheme Theme
    {
        get => _theme;
        set
        {
            _theme = value;
            _controller.Refresh();
        }
    }

    public DataGridColumns<T> Columns => _controller.Columns;

    public MouseRegion MouseRegion { get; }

    #region ====Overrides====

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (_header != null && action(_header)) return;
        if (action(Body)) return;
        if (_footer != null) action(_footer);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        SetSize(width, height);

        var bodyHeight = height;

        if (ShowHeader)
        {
            if (_header == null)
            {
                _header = new DataGridHeader<T>(_controller);
                _header.Parent = this;
            }

            _header.Layout(width, height);
            _header.SetPosition(0, 0);
            bodyHeight -= _header.H;
        }

        if (ShowFooter)
        {
            if (_footer == null)
            {
                _footer = new DataGridFooter<T>(_controller);
                _footer.Parent = this;
            }

            _footer.Layout(width, bodyHeight);
            _footer.SetPosition(0, H - _footer.H);
            bodyHeight -= _footer.H;
        }

        Body.Layout(width, bodyHeight);
        Body.SetPosition(0, _header?.H ?? 0);

        _controller.CalcColumnsWidth(new Size(width, height));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var size = new Size(W, H);
        //clip first
        canvas.Save();
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);

        //TODO: 暂每次计算可见列
        _controller.LayoutVisibleColumns(size);

        PaintChildren(canvas, area);

        canvas.Restore();
    }

    #endregion
}