using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using PixUI;

namespace LiveChartsGeneratedCode;

public abstract partial class SourceGenChart : SourceGenDrawnView, IMouseRegion, IPaintEmptyClip
{
    protected SourceGenChart()
    {
        InitializeChartControl();
        InitializeObservedProperties();

        MouseRegion = new MouseRegion();
        MouseRegion.PointerDown += OnMouseDown;
        MouseRegion.PointerUp += OnMouseUp;
        // MouseRegion.PointerTap += OnMouseClick;
        MouseRegion.PointerMove += OnMouseMove;
        MouseRegion.HoverChanged += OnHoverChanged;
    }

    public MouseRegion MouseRegion { get; }

    LvcColor IChartView.BackColor => new(255, 255, 255, 255);

    protected override void OnDrawnViewSizeChanged() => CoreChart.Update();

    protected override void OnDrawnViewLoaded()
    {
        StartObserving();
        CoreChart.Load();
    }

    protected override void OnDrawnViewUnloaded()
    {
        StopObserving();
        CoreChart.Unload();
    }

    public void ClearOrStopPaint(ICanvas canvas)
    {
        //不能简单停止MotionCanvas.DrawingLoop，因为可能动画进入前clip区域为空
        //所以应该继续绘制至有效状态
        OnPaint(canvas);
    }

    #region ====Mouse Events====

    private void OnMouseMove(PointerEvent e) => CoreChart.InvokePointerMove(new(e.X, e.Y));

    private void OnMouseDown(PointerEvent e)
    {
        //TODO: if (ModifierKeys > 0) return;
        CoreChart.InvokePointerDown(new(e.X, e.Y), e.Buttons == PointerButtons.Right);
    }

    private void OnMouseUp(PointerEvent e) =>
        CoreChart.InvokePointerUp(new(e.X, e.Y), e.Buttons == PointerButtons.Right);

    private void OnHoverChanged(bool isHovering)
    {
        if (!isHovering) CoreChart.InvokePointerLeft();
    }

    #endregion
}