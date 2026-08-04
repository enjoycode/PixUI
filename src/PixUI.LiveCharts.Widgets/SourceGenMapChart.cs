using PixUI;

namespace LiveChartsGeneratedCode;

public abstract partial class SourceGenMapChart : SourceGenDrawnView, IMouseRegion, IPaintEmptyClip /*, IScrollable*/
{
    protected SourceGenMapChart()
    {
        InitializeChartControl();

        MouseRegion = new MouseRegion();
        MouseRegion.PointerDown += OnMouseDown;
        MouseRegion.PointerUp += OnMouseUp;
        MouseRegion.PointerMove += OnMouseMove;
        MouseRegion.HoverChanged += OnHoverChanged;
    }

    public MouseRegion MouseRegion { get; }

    /// <inheritdoc />
    protected override void OnDrawnViewSizeChanged() => CoreChart?.Update();

    /// <inheritdoc />
    protected override void OnDrawnViewLoaded() => CoreChart?.Load();

    /// <inheritdoc />
    protected override void OnDrawnViewUnloaded() => CoreChart?.Unload();

    public void ClearOrStopPaint(ICanvas canvas)
    {
        //不能简单停止MotionCanvas.DrawingLoop，因为可能动画进入前clip区域为空
        //所以应该继续绘制至有效状态
        OnPaint(canvas);
    }

    #region ====Mouse Events====

    private void OnMouseMove(PointerEvent e) => CoreChart.InvokePointerMove(new(e.X, e.Y));

    private void OnMouseDown(PointerEvent e) => CoreChart.InvokePointerDown(new(e.X, e.Y), false);

    private void OnMouseUp(PointerEvent e) =>
        CoreChart.InvokePointerUp(new(e.X, e.Y), false);

    private void OnHoverChanged(bool isHovering)
    {
        if (!isHovering) CoreChart.InvokePointerLeft();
    }

    #endregion
}