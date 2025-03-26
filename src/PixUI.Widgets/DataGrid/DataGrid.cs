using System;

namespace PixUI;

public sealed class DataGrid<T> : Widget, IDroppable
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
        MouseRegion.HoverChanged += OnHoverChanged;
    }

    private readonly DataGridController<T> _controller;
    internal readonly DataGridBody<T> Body;
    internal DataGridHeader<T>? Header { get; private set; }
    internal DataGridFooter<T>? Footer { get; private set; }

    public bool ShowHeader { get; init; } = true;
    private bool ShowFooter => Footer != null;

    public DataGridTheme Theme
    {
        get => _controller.Theme;
        set => _controller.Theme = value;
    }

    public DataGridColumns<T> Columns => _controller.Columns;

    public DataGridFooterCell[]? FooterCells
    {
        get => Footer?.Cells;
        set
        {
            var hasOld = Footer != null;
            if (hasOld)
            {
                Footer!.Parent = null;
                Footer.Dispose();
                Footer = null;
            }

            if ((value == null || value.Length == 0))
            {
                if (hasOld) Relayout();
            }
            else
            {
                Footer = new DataGridFooter<T>(_controller);
                Footer.Parent = this;
                Footer.Cells = value;
                if (hasOld) Footer.Repaint();
                else Relayout();
            }
        }
    }

    public MouseRegion MouseRegion { get; }

    private void OnHoverChanged(bool hover)
    {
        if (hover)
            Body.ScrollBars.Show();
        else
            Body.ScrollBars.Hide(false);
    }

    #region ====DragDrop====

    public bool AllowDrop
    {
        get => _controller.AllowDrop;
        set => _controller.AllowDrop = value;
    }

    public Func<DragEvent, bool>? OnAllowDrop
    {
        get => _controller.OnAllowDrop;
        set => _controller.OnAllowDrop = value;
    }

    public Action<DragEvent>? OnDrop
    {
        get => _controller.OnDrop;
        set => _controller.OnDrop = value;
    }

    bool IDroppable.AllowDrop(DragEvent dragEvent) => AllowDrop;

    void IDroppable.OnDragOver(DragEvent dragEvent, Point local)
    {
        dragEvent.DropEffect = DropEffect.Move;
        dragEvent.DropPosition = DropPosition.In;

        var allowDrop = true;
        if (_controller.OnAllowDrop != null)
            allowDrop = _controller.OnAllowDrop(dragEvent);
        if (!allowDrop)
        {
            dragEvent.DropEffect = DropEffect.None;
            dragEvent.DropHintImage = null;
        }
        // else
        // {
        //     //TODO:考虑缓存dropHintImage
        //     // dragEvent.DropHintImage = BuildDropHintImage(dragEvent.DropPosition);
        // }
    }

    void IDroppable.OnDragLeave(DragEvent dragEvent) { }

    void IDroppable.OnDrop(DragEvent dragEvent, Point local) => _controller.OnDrop?.Invoke(dragEvent);

    #endregion

    #region ====Overrides====

    protected override void OnMounted()
    {
        base.OnMounted();
        _controller.Columns.OnMounted();
    }

    public override void VisitChildren(Func<Widget, bool> action)
    {
        if (Header != null && action(Header)) return;
        if (action(Body)) return;
        if (Footer != null) action(Footer);
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        SetSize(maxSize.Width, maxSize.Height);

        var bodyHeight = maxSize.Height;

        if (ShowHeader)
        {
            if (Header == null)
            {
                Header = new DataGridHeader<T>(_controller);
                Header.Parent = this;
            }

            Header.Layout(maxSize.Width, maxSize.Height);
            Header.SetPosition(0, 0);
            bodyHeight -= Header.H;
        }

        if (ShowFooter)
        {
            Footer!.Layout(maxSize.Width, bodyHeight);
            Footer.SetPosition(0, H - Footer.H);
            bodyHeight -= Footer.H;
        }

        Body.Layout(maxSize.Width, bodyHeight);
        Body.SetPosition(0, Header?.H ?? 0);

        _controller.CalcColumnsWidth(maxSize);
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