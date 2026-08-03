using System;
using System.Threading.Tasks;
using LiveChartsCore.Motion;

namespace PixUI.LiveCharts;

internal sealed class MotionCanvas
{
    public MotionCanvas(Widget chartView)
    {
        _chartView = chartView;
    }

    private readonly Widget _chartView;
    private bool _isDrawingLoopRunning;

    public double MaxFps { get; set; } = 65;

    public CoreMotionCanvas CanvasCore { get; } = new();

    internal void CanvasCore_Invalidated(CoreMotionCanvas sender) => RunDrawingLoop();

    private async void RunDrawingLoop()
    {
        if (_isDrawingLoopRunning) return;
        _isDrawingLoopRunning = true;

        var ts = TimeSpan.FromSeconds(1 / MaxFps);
        while (!CanvasCore.IsValid)
        {
            _chartView.Repaint();
            await Task.Delay((int)ts.TotalMilliseconds);
        }

        _isDrawingLoopRunning = false;
    }
}