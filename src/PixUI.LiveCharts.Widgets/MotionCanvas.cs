using System;
using LiveChartsCore.Motion;

namespace PixUI.LiveCharts;

internal sealed class MotionCanvas : IRenderMode
{
    static MotionCanvas()
    {
        _ = LiveChartsSkiaSharp.EnsureInitialized();
        LiveChartsCore.LiveCharts.RenderingSettings.TryUseVSync = false; //暂禁用
    }

    public MotionCanvas(Widget chartView)
    {
        _chartView = chartView;
        // CanvasCore.DisableAnimations = true;
    }

    private readonly Widget _chartView;
    private IFrameTicker _ticker = null!;

    public CoreMotionCanvas CanvasCore { get; } = new();

    internal void OnDrawnViewMounted()
    {
        _ticker = new AsyncLoopTicker();
        _ticker.InitializeTicker(CanvasCore, this);
        // _chartView.Repaint();
    }

    internal void OnDrawnViewUnmounted()
    {
        _ticker.DisposeTicker();
        // CanvasCore.Dispose();
    }

    #region ====IRenderMode====

    event CoreMotionCanvas.FrameRequestHandler IRenderMode.FrameRequest
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }

    void IRenderMode.InitializeRenderMode(CoreMotionCanvas canvas) =>
        throw new NotImplementedException();

    void IRenderMode.InvalidateRenderer() => _chartView.Repaint();

    void IRenderMode.DisposeRenderMode() => throw new NotImplementedException();

    #endregion
}