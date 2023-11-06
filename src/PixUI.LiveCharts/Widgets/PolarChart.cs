using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using LiveCharts.Drawing;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.VisualElements;

namespace LiveCharts;

public sealed class PolarChart : ChartView, IPolarChartView<SkiaDrawingContext>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PolarChart"/> class.
    /// </summary>
    public PolarChart() : this(null, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PolarChart"/> class.
    /// </summary>
    /// <param name="tooltip">The default tool tip control.</param>
    /// <param name="legend">The default legend control.</param>
    public PolarChart(IChartTooltip<SkiaDrawingContext>? tooltip = null,
        IChartLegend<SkiaDrawingContext>? legend = null)
        : base(tooltip, legend)
    {
        _seriesObserver =
            new CollectionDeepObserver<ISeries>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);
        _angleObserver =
            new CollectionDeepObserver<IPolarAxis>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);
        _radiusObserver =
            new CollectionDeepObserver<IPolarAxis>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);

        AngleAxes = new List<IPolarAxis>()
        {
            LiveChartsCore.LiveCharts.DefaultSettings.GetProvider<SkiaDrawingContext>().GetDefaultPolarAxis()
        };
        RadiusAxes = new List<IPolarAxis>()
        {
            LiveChartsCore.LiveCharts.DefaultSettings.GetProvider<SkiaDrawingContext>().GetDefaultPolarAxis()
        };
        Series = new ObservableCollection<ISeries>();
        VisualElements = new ObservableCollection<ChartElement<SkiaDrawingContext>>();
    }

    private bool _fitToBounds = false;
    private double _totalAngle = 360;
    private double _innerRadius;
    private double _initialRotation = LiveChartsCore.LiveCharts.DefaultSettings.PolarInitialRotation;
    private readonly CollectionDeepObserver<ISeries> _seriesObserver;
    private readonly CollectionDeepObserver<IPolarAxis> _angleObserver;
    private readonly CollectionDeepObserver<IPolarAxis> _radiusObserver;
    private IEnumerable<ISeries> _series = new List<ISeries>();
    private IEnumerable<IPolarAxis> _angleAxes = new List<PolarAxis>();
    private IEnumerable<IPolarAxis> _radiusAxes = new List<PolarAxis>();

    public bool FitToBounds
    {
        get => _fitToBounds;
        set
        {
            _fitToBounds = value;
            OnPropertyChanged();
        }
    }

    public double TotalAngle
    {
        get => _totalAngle;
        set
        {
            _totalAngle = value;
            OnPropertyChanged();
        }
    }

    public double InnerRadius
    {
        get => _innerRadius;
        set
        {
            _innerRadius = value;
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

    public IEnumerable<IPolarAxis> AngleAxes
    {
        get => _angleAxes;
        set
        {
            _angleObserver?.Dispose(_angleAxes);
            _angleObserver?.Initialize(value);
            _angleAxes = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<IPolarAxis> RadiusAxes
    {
        get => _radiusAxes;
        set
        {
            _radiusObserver?.Dispose(_radiusAxes);
            _radiusObserver?.Initialize(value);
            _radiusAxes = value;
            OnPropertyChanged();
        }
    }

    PolarChart<SkiaDrawingContext> IPolarChartView<SkiaDrawingContext>.Core =>
        core is null ? throw new Exception("core not found") : (PolarChart<SkiaDrawingContext>)core;

    protected override void InitializeCore()
    {
        core = new PolarChart<SkiaDrawingContext>(
            this, config => config.UseDefaults(), CoreCanvas, true);
        if (((IChartView)this).DesignerMode) return;
        core.Update();
    }

    public LvcPointD ScalePixelsToData(LvcPointD point, int angleAxisIndex = 0, int radiusAxisIndex = 0)
    {
        if (core is not PolarChart<SkiaDrawingContext> cc) throw new Exception("core not found");

        var scaler = new PolarScaler(
            cc.DrawMarginLocation, cc.DrawMarginSize, cc.AngleAxes[angleAxisIndex], cc.RadiusAxes[radiusAxisIndex],
            cc.InnerRadius, cc.InitialRotation, cc.TotalAnge);

        return scaler.ToChartValues(point.X, point.Y);
    }

    public LvcPointD ScaleDataToPixels(LvcPointD point, int angleAxisIndex = 0, int radiusAxisIndex = 0)
    {
        if (core is not PolarChart<SkiaDrawingContext> cc) throw new Exception("core not found");

        var scaler = new PolarScaler(
            cc.DrawMarginLocation, cc.DrawMarginSize, cc.AngleAxes[angleAxisIndex], cc.RadiusAxes[radiusAxisIndex],
            cc.InnerRadius, cc.InitialRotation, cc.TotalAnge);

        var r = scaler.ToPixels(point.X, point.Y);

        return new LvcPointD { X = (float)r.X, Y = (float)r.Y };
    }

    public override IEnumerable<ChartPoint> GetPointsAt(LvcPoint point,
        TooltipFindingStrategy strategy = TooltipFindingStrategy.Automatic)
    {
        if (core is not PolarChart<SkiaDrawingContext> cc) throw new Exception("core not found");

        if (strategy == TooltipFindingStrategy.Automatic)
            strategy = cc.Series.GetTooltipFindingStrategy();

        return cc.Series.SelectMany(series => series.FindHitPoints(cc, point, strategy));
    }

    public override IEnumerable<VisualElement<SkiaDrawingContext>> GetVisualsAt(LvcPoint point)
    {
        return core is not PolarChart<SkiaDrawingContext> cc
            ? throw new Exception("core not found")
            : cc.VisualElements.SelectMany(visual => ((VisualElement<SkiaDrawingContext>)visual).IsHitBy(core, point));
    }

    private void OnDeepCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => OnPropertyChanged();

    private void OnDeepCollectionPropertyChanged(object? sender, PropertyChangedEventArgs e) => OnPropertyChanged();
}