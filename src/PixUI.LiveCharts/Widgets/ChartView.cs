using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.Motion;
using LiveCharts;
using LiveCharts.Drawing;
using LiveCharts.SKCharts;
using LiveChartsCore.VisualElements;
using PixUI;
using LCC = LiveChartsCore;

namespace LiveCharts;

public abstract class ChartView : Widget, IMouseRegion, IChartView<SkiaDrawingContext>
{
    protected ChartView(IChartTooltip<SkiaDrawingContext>? tooltip,
        IChartLegend<SkiaDrawingContext>? legend)
    {
        if (tooltip != null) this.tooltip = tooltip;
        if (legend != null) this.legend = legend;

        if (!LCC.LiveCharts.IsConfigured)
            LCC.LiveCharts.Configure(config => config.UseDefaults());

        InitializeCore();

        _visualsObserver = new CollectionDeepObserver<ChartElement<SkiaDrawingContext>>(
            (s, e) => OnPropertyChanged(),
            (s, e) => OnPropertyChanged(), true);

        if (core is null) throw new Exception("Core not found!");
        // core.Measuring += OnCoreMeasuring;
        // core.UpdateStarted += OnCoreUpdateStarted;
        // core.UpdateFinished += OnCoreUpdateFinished;

        MouseRegion = new MouseRegion();
        MouseRegion.PointerMove += e => core?.InvokePointerMove(new LvcPoint(e.X, e.Y));
        MouseRegion.HoverChanged += hover =>
        {
            if (!hover) core?.InvokePointerLeft();
        };
    }

    #region ====Fields====

    protected LiveChartsCore.Chart<SkiaDrawingContext>? core;

    protected IChartLegend<SkiaDrawingContext>? legend = new SKDefaultLegend();

    protected IChartTooltip<SkiaDrawingContext>? tooltip = new SKDefaultTooltip();

    private LegendPosition _legendPosition = LiveChartsCore.LiveCharts.DefaultSettings.LegendPosition;
    private Margin? _drawMargin = null;
    private TooltipPosition _tooltipPosition = LiveChartsCore.LiveCharts.DefaultSettings.TooltipPosition;
    private VisualElement<SkiaDrawingContext>? _title;
    private readonly CollectionDeepObserver<ChartElement<SkiaDrawingContext>> _visualsObserver;

    private IEnumerable<ChartElement<SkiaDrawingContext>> _visuals =
        new List<ChartElement<SkiaDrawingContext>>();

    private IPaint<SkiaDrawingContext>? _legendTextPaint =
        (IPaint<SkiaDrawingContext>?)LiveChartsCore.LiveCharts.DefaultSettings.LegendTextPaint;

    private IPaint<SkiaDrawingContext>? _legendBackgroundPaint =
        (IPaint<SkiaDrawingContext>?)LiveChartsCore.LiveCharts.DefaultSettings.LegendBackgroundPaint;

    private double? _legendTextSize = LiveChartsCore.LiveCharts.DefaultSettings.LegendTextSize;

    private IPaint<SkiaDrawingContext>? _tooltipTextPaint =
        (IPaint<SkiaDrawingContext>?)LiveChartsCore.LiveCharts.DefaultSettings.TooltipTextPaint;

    private IPaint<SkiaDrawingContext>? _tooltipBackgroundPaint =
        (IPaint<SkiaDrawingContext>?)LiveChartsCore.LiveCharts.DefaultSettings.TooltipBackgroundPaint;

    private double? _tooltipTextSize = LiveChartsCore.LiveCharts.DefaultSettings.TooltipTextSize;

    #endregion

    #region ====IChartView====

    public IChart CoreChart => core!;
    public bool DesignerMode => false;

    public LvcColor BackColor { get; set; } = new(255, 255, 255, 0);

    public LvcSize ControlSize
    {
        get
        {
            // return the full control size as a workaround when the legend is not set.
            // for some reason WinForms has not loaded the correct size at this point when the control loads.
            return LegendPosition == LegendPosition.Hidden
                ? new LvcSize { Width = W, Height = H }
                : new LvcSize { Width = W, Height = H };
        }
    }

    public Margin? DrawMargin
    {
        get => _drawMargin;
        set
        {
            _drawMargin = value;
            OnPropertyChanged();
        }
    }

    public TimeSpan AnimationsSpeed { get; set; } = LCC.LiveCharts.DefaultSettings.AnimationsSpeed;

    public Func<float, float>? EasingFunction { get; set; } = LCC.LiveCharts.DefaultSettings.EasingFunction;

    public TimeSpan UpdaterThrottler { get; set; } = LCC.LiveCharts.DefaultSettings.UpdateThrottlingTimeout;

    public LegendPosition LegendPosition
    {
        get => _legendPosition;
        set
        {
            _legendPosition = value;
            OnPropertyChanged();
        }
    }

    public TooltipPosition TooltipPosition
    {
        get => _tooltipPosition;
        set
        {
            _tooltipPosition = value;
            OnPropertyChanged();
        }
    }

    public event ChartPointsHandler? DataPointerDown;
    public event ChartPointHandler? ChartPointPointerDown;

    public void OnDataPointerDown(IEnumerable<ChartPoint> points, LvcPoint pointer)
    {
        throw new NotImplementedException();
    }

    // public object SyncContext
    // {
    //     get => CoreCanvas.Sync;
    //     // set
    //     // {
    //     //     CoreCanvas.Sync = value;
    //     //     OnPropertyChanged();
    //     // }
    // }

    public void InvokeOnUIThread(Action action)
    {
        if (!IsMounted) return;

        UIApplication.Current.BeginInvoke(action);
    }

    //void IChartView.Invalidate() => CoreCanvas.Invalidate();

    public IPaint<SkiaDrawingContext>? LegendTextPaint
    {
        get => _legendTextPaint;
        set
        {
            _legendTextPaint = value;
            OnPropertyChanged();
        }
    }

    public IPaint<SkiaDrawingContext>? LegendBackgroundPaint
    {
        get => _legendBackgroundPaint;
        set
        {
            _legendBackgroundPaint = value;
            OnPropertyChanged();
        }
    }

    public double? LegendTextSize
    {
        get => _legendTextSize;
        set
        {
            _legendTextSize = value;
            OnPropertyChanged();
        }
    }

    public IPaint<SkiaDrawingContext>? TooltipTextPaint
    {
        get => _tooltipTextPaint;
        set
        {
            _tooltipTextPaint = value;
            OnPropertyChanged();
        }
    }

    public IPaint<SkiaDrawingContext>? TooltipBackgroundPaint
    {
        get => _tooltipBackgroundPaint;
        set
        {
            _tooltipBackgroundPaint = value;
            OnPropertyChanged();
        }
    }

    public double? TooltipTextSize
    {
        get => _tooltipTextSize;
        set
        {
            _tooltipTextSize = value;
            OnPropertyChanged();
        }
    }

    public VisualElement<SkiaDrawingContext>? Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    public event ChartEventHandler<SkiaDrawingContext>? Measuring;
    public event ChartEventHandler<SkiaDrawingContext>? UpdateStarted;
    public event ChartEventHandler<SkiaDrawingContext>? UpdateFinished;
    public event VisualElementHandler<SkiaDrawingContext>? VisualElementsPointerDown;

    public bool AutoUpdateEnabled { get; set; } = true;
    public MotionCanvas<SkiaDrawingContext> CoreCanvas => CanvasCore;

    public IChartLegend<SkiaDrawingContext>? Legend
    {
        get => legend;
        set => legend = value;
    }

    public IChartTooltip<SkiaDrawingContext>? Tooltip
    {
        get => tooltip;
        set => tooltip = value;
    }

    public IEnumerable<ChartElement<SkiaDrawingContext>> VisualElements
    {
        get => _visuals;
        set
        {
            _visualsObserver?.Dispose(_visuals);
            _visualsObserver?.Initialize(value);
            _visuals = value;
            OnPropertyChanged();
        }
    }

    public void ShowTooltip(IEnumerable<ChartPoint> points)
    {
        if (tooltip is null || core is null) return;

        tooltip.Show(points, core);
    }

    public void HideTooltip()
    {
        if (tooltip is null || core is null) return;

        core.ClearTooltipData();
        tooltip.Hide();
    }

    public void OnVisualElementPointerDown(IEnumerable<VisualElement<SkiaDrawingContext>> visualElements,
        LvcPoint pointer)
    {
        throw new NotImplementedException();
    }

    public abstract IEnumerable<ChartPoint> GetPointsAt(LvcPoint point,
        TooltipFindingStrategy strategy = TooltipFindingStrategy.Automatic);

    public abstract IEnumerable<VisualElement<SkiaDrawingContext>> GetVisualsAt(LvcPoint point);

    #endregion

    #region ====MotionCanvas====

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

    private void CanvasCore_Invalidated(MotionCanvas<SkiaDrawingContext> sender) => RunDrawingLoop();

    private async void RunDrawingLoop()
    {
        if (_isDrawingLoopRunning) return;
        _isDrawingLoopRunning = true;

        var ts = TimeSpan.FromSeconds(1 / MaxFps);
        while (!CanvasCore.IsValid)
        {
            Invalidate(InvalidAction.Repaint);
            await Task.Delay((int)ts.TotalMilliseconds);
        }

        _isDrawingLoopRunning = false;
    }

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

    #endregion

    #region ====Widget Overrides====

    public MouseRegion MouseRegion { get; }

    protected override void OnMounted()
    {
        base.OnMounted();
        core?.Load();

        CanvasCore.Invalidated += CanvasCore_Invalidated;
    }

    protected override void OnUnmounted()
    {
        base.OnUnmounted();

        CanvasCore.Invalidated -= CanvasCore_Invalidated;
        CanvasCore.Dispose();

        if (tooltip is IDisposable disposableTooltip)
            disposableTooltip.Dispose();
        core?.Unload();
        OnUnloading();
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        var oldWidth = W;
        var oldHeight = H;

        SetSize(width, height);

        if (core != null && !core.IsFirstDraw && (oldWidth != W || oldHeight != H))
            core.Update();
    }


    private SkiaDrawingContext? _drawCtx;

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        canvas.Save();
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);

        if (_drawCtx == null)
        {
            _drawCtx = new SkiaDrawingContext(CanvasCore, (int)W, (int)H, canvas)
                { Background = BackColor.AsSKColor() };
        }
        else
        {
            _drawCtx.Width = (int)W;
            _drawCtx.Height = (int)H;
            _drawCtx.Background = BackColor.AsSKColor();
        }

        CanvasCore.DrawFrame(_drawCtx);

        canvas.Restore();
    }

    #endregion

    protected abstract void InitializeCore();

    protected virtual void OnUnloading() { }

    protected void OnPropertyChanged()
    {
        if (core is null || ((IChartView)this).DesignerMode) return;
        core.Update();
    }
}