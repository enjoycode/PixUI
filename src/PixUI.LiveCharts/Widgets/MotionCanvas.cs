using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveCharts.Drawing;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Motion;
using PixUI;

namespace LiveCharts;

internal sealed class MotionCanvas
{
    public MotionCanvas(Widget chartView)
    {
        _chartView = chartView;
    }

    private readonly Widget _chartView;
    private bool _isDrawingLoopRunning = false;
    private List<PaintSchedule<SkiaDrawingContext>> _paintTasksSchedule = new();

    public List<PaintSchedule<SkiaDrawingContext>> PaintTasks
    {
        get => _paintTasksSchedule;
        set
        {
            _paintTasksSchedule = value;
            OnPaintTasksChanged();
        }
    }

    public double MaxFps { get; set; } = 65;

    public MotionCanvas<SkiaDrawingContext> CanvasCore { get; } = new();

    internal void CanvasCore_Invalidated(MotionCanvas<SkiaDrawingContext> sender) => RunDrawingLoop();

    private async void RunDrawingLoop()
    {
        if (_isDrawingLoopRunning) return;
        _isDrawingLoopRunning = true;

        var ts = TimeSpan.FromSeconds(1 / MaxFps);
        while (!CanvasCore.IsValid && _isDrawingLoopRunning)
        {
            _chartView.Invalidate(InvalidAction.Repaint);
            await Task.Delay((int)ts.TotalMilliseconds);
        }

        _isDrawingLoopRunning = false;
    }

    internal void StopDrawingLoop() => _isDrawingLoopRunning = false;

    private void OnPaintTasksChanged()
    {
        var tasks = new HashSet<IPaint<SkiaDrawingContext>>();

        foreach (var item in _paintTasksSchedule)
        {
            item.PaintTask.SetGeometries(CanvasCore, item.Geometries);
            _ = tasks.Add(item.PaintTask);
        }

        CanvasCore.SetPaintTasks(tasks);
    }
}