using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Motion;
using PixUI;
using PixUI.LiveCharts;
using PixUI.LiveCharts.Drawing;

namespace LiveChartsGeneratedCode;

public abstract partial class SourceGenDrawnView : Widget
{
    protected SourceGenDrawnView()
    {
        _motionCanvas = new MotionCanvas(this);
    }

    private readonly MotionCanvas _motionCanvas;

    public virtual bool DesignerMode => false;
    public virtual bool IsDarkMode => false;

    public void InvokeOnUIThread(Action action) => UIApplication.Current.BeginInvoke(action);

    #region ====Widget Overrides====

    protected override void OnMounted()
    {
        base.OnMounted();
        _motionCanvas.OnDrawnViewMounted();
        OnDrawnViewLoaded();
    }

    protected override void OnUnmounted()
    {
        base.OnUnmounted();
        _motionCanvas.OnDrawnViewUnmounted();
        OnDrawnViewUnloaded();
    }

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        var backColor = (this as IChartView)?.BackColor.AsSKColor() ?? Color.Empty;
        CoreCanvas.DrawFrame(new SkiaSharpDrawingContext(CoreCanvas, canvas, backColor));
    }

    #endregion

    #region ====IDrawnView====

    public CoreMotionCanvas CoreCanvas => _motionCanvas.CanvasCore;

    public LvcSize ControlSize => new(LayoutSize.Width, LayoutSize.Height);

    #endregion
}