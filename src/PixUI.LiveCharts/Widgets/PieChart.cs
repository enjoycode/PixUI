using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveCharts;
using LiveCharts.Drawing;
using LiveChartsCore.VisualElements;

namespace LiveCharts;

public sealed class PieChart : ChartView, IPieChartView<SkiaDrawingContext>
{
    public PieChart(IChartTooltip<SkiaDrawingContext>? tooltip = null,
        IChartLegend<SkiaDrawingContext>? legend = null) : base(tooltip, legend)
    {
        _seriesObserver = new CollectionDeepObserver<ISeries>(
            (s, e) => OnPropertyChanged(),
            (s, e) => OnPropertyChanged(),
            true);

        Series = new ObservableCollection<ISeries>();
        VisualElements = new ObservableCollection<ChartElement<SkiaDrawingContext>>();

        // var c = Controls[0].Controls[0];
        // c.MouseDown += OnMouseDown;
    }

    #region ====Fields====

    private readonly CollectionDeepObserver<ISeries> _seriesObserver;
    private IEnumerable<ISeries> _series = new List<ISeries>();
    private bool _isClockwise = true;
    private double _initialRotation;
    private double _maxAngle = 360;
    private double? _total;

    #endregion

    #region ====ChartView Overrides====

    protected override void InitializeCore()
    {
        core = new PieChart<SkiaDrawingContext>(
            this, config => config.UseDefaults(), CanvasCore, true);
        if (DesignerMode) return;
        core.Update();
    }

    public override IEnumerable<ChartPoint> GetPointsAt(LvcPoint point,
        TooltipFindingStrategy strategy = TooltipFindingStrategy.Automatic)
    {
#if __WEB__
        var cc = (PieChart<SkiaDrawingContext>)core;
#else
        if (core is not PieChart<SkiaDrawingContext> cc) throw new Exception("core not found");
#endif        

        if (strategy == TooltipFindingStrategy.Automatic)
            strategy = cc.Series.GetTooltipFindingStrategy();

        return cc.Series.SelectMany(series => series.FindHitPoints(cc, point, strategy));
    }

    public override IEnumerable<VisualElement<SkiaDrawingContext>> GetVisualsAt(LvcPoint point)
    {
#if __WEB__
        var cc = (PieChart<SkiaDrawingContext>)core;
        return cc.VisualElements.SelectMany(visual =>
                ((VisualElement<SkiaDrawingContext>)visual).IsHitBy(core, point));
#else
        return core is not PieChart<SkiaDrawingContext> cc
            ? throw new Exception("core not found")
            : cc.VisualElements.SelectMany(visual =>
                ((VisualElement<SkiaDrawingContext>)visual).IsHitBy(core, point));
#endif        
    }

    #endregion

    #region ====IPieChartView====

    public PieChart<SkiaDrawingContext> Core => (PieChart<SkiaDrawingContext>)core!;

    public IEnumerable<ISeries> Series
    {
        get => _series;
        set
        {
            _seriesObserver?.Dispose(_series);
            _seriesObserver?.Initialize(value);
            _series = value;
            OnPropertyChanged();
        }
    }

    public double InitialRotation
    {
        get => _initialRotation;
        set
        {
            _initialRotation = value;
            OnPropertyChanged();
        }
    }

    public double MaxAngle
    {
        get => _maxAngle;
        set
        {
            _maxAngle = value;
            OnPropertyChanged();
        }
    }

    public double? Total
    {
        get => _total;
        set
        {
            _total = value;
            OnPropertyChanged();
        }
    }

    public bool IsClockwise
    {
        get => _isClockwise;
        set
        {
            _isClockwise = value;
            OnPropertyChanged();
        }
    }

    #endregion
}