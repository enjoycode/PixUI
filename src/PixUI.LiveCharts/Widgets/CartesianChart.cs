using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveCharts;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Geometries;
using LiveCharts.Painting;
using LiveChartsCore.VisualElements;

namespace LiveCharts;

public sealed class CartesianChart : ChartView, ICartesianChartView<SkiaDrawingContext>
{
    public CartesianChart(IChartTooltip<SkiaDrawingContext>? tooltip = null,
        IChartLegend<SkiaDrawingContext>? legend = null) : base(tooltip, legend)
    {
        _seriesObserver =
            new CollectionDeepObserver<ISeries>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);
        _xObserver =
            new CollectionDeepObserver<ICartesianAxis>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged,
                true);
        _yObserver =
            new CollectionDeepObserver<ICartesianAxis>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged,
                true);
        _sectionsObserver = new CollectionDeepObserver<Section<SkiaDrawingContext>>(
            OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);

        XAxes = new List<ICartesianAxis>()
        {
            //LiveCharts.DefaultSettings.GetProvider<SkiaSharpDrawingContext>().GetDefaultCartesianAxis()
            new Axis()
        };
        YAxes = new List<ICartesianAxis>()
        {
            //LiveCharts.DefaultSettings.GetProvider<SkiaSharpDrawingContext>().GetDefaultCartesianAxis()
            new Axis()
        };
        Series = new ObservableCollection<ISeries>();
        VisualElements = new ObservableCollection<ChartElement<SkiaDrawingContext>>();

        // var c = Controls[0].Controls[0];
        // c.MouseDown += OnMouseDown;
        // c.MouseWheel += OnMouseWheel;
        // c.MouseUp += OnMouseUp;
    }

    #region ====Fields====

    private readonly CollectionDeepObserver<ISeries> _seriesObserver;
    private readonly CollectionDeepObserver<ICartesianAxis> _xObserver;
    private readonly CollectionDeepObserver<ICartesianAxis> _yObserver;
    private readonly CollectionDeepObserver<Section<SkiaDrawingContext>> _sectionsObserver;
    private IEnumerable<ISeries> _series = new List<ISeries>();
    private IEnumerable<ICartesianAxis> _xAxes = new List<Axis> { new() };
    private IEnumerable<ICartesianAxis> _yAxes = new List<Axis> { new() };
    private IEnumerable<Section<SkiaDrawingContext>> _sections = new List<Section<SkiaDrawingContext>>();
    private DrawMarginFrame<SkiaDrawingContext>? _drawMarginFrame;
    private TooltipFindingStrategy _tooltipFindingStrategy = LiveChartsCore.LiveCharts.DefaultSettings.TooltipFindingStrategy;

    #endregion

    #region ====ChartView Overrides====

    protected override void InitializeCore()
    {
        var zoomingSection = new RectangleGeometry();
        var zoomingSectionPaint = new SolidColorPaint
        {
            IsFill = true,
            Color = new SKColor(33, 150, 243, 50),
            ZIndex = int.MaxValue
        };
        zoomingSectionPaint.AddGeometryToPaintTask(CoreCanvas, zoomingSection);
        CoreCanvas.AddDrawableTask(zoomingSectionPaint);

        core = new CartesianChart<SkiaDrawingContext>(
            this, config => config.UseDefaults(), CoreCanvas, zoomingSection);
        if (((IChartView)this).DesignerMode) return;
        core.Update();
    }

    public override IEnumerable<ChartPoint> GetPointsAt(LvcPoint point,
        TooltipFindingStrategy strategy = TooltipFindingStrategy.Automatic)
    {
#if __WEB__
        var cc = (CartesianChart<SkiaDrawingContext>)core;
#else
        if (core is not CartesianChart<SkiaDrawingContext> cc) throw new Exception("core not found");
#endif        

        if (strategy == TooltipFindingStrategy.Automatic)
            strategy = cc.Series.GetTooltipFindingStrategy();

        return cc.Series.SelectMany(series => series.FindHitPoints(cc, point, strategy));
    }

    public override IEnumerable<VisualElement<SkiaDrawingContext>> GetVisualsAt(LvcPoint point)
    {
#if __WEB__
        var cc = (CartesianChart<SkiaDrawingContext>)core;
        return cc.VisualElements.SelectMany(visual =>
                ((VisualElement<SkiaDrawingContext>)visual).IsHitBy(core, point));
#else
        return core is not CartesianChart<SkiaDrawingContext> cc
            ? throw new Exception("core not found")
            : cc.VisualElements.SelectMany(visual =>
                ((VisualElement<SkiaDrawingContext>)visual).IsHitBy(core, point));
#endif        
    }

    #endregion

    #region ====ICartesianChartView====

    public CartesianChart<SkiaDrawingContext> Core => (CartesianChart<SkiaDrawingContext>)core!;

    public IEnumerable<ICartesianAxis> XAxes
    {
        get => _xAxes;
        set
        {
            _xObserver?.Dispose(_xAxes);
            _xObserver?.Initialize(value);
            _xAxes = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<ICartesianAxis> YAxes
    {
        get => _yAxes;
        set
        {
            _yObserver?.Dispose(_yAxes);
            _yObserver?.Initialize(value);
            _yAxes = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<Section<SkiaDrawingContext>> Sections
    {
        get => _sections;
        set
        {
            _sectionsObserver?.Dispose(_sections);
            _sectionsObserver?.Initialize(value);
            _sections = value;
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

    public DrawMarginFrame<SkiaDrawingContext>? DrawMarginFrame
    {
        get => _drawMarginFrame;
        set
        {
            _drawMarginFrame = value;
            OnPropertyChanged();
        }
    }

    public TooltipFindingStrategy TooltipFindingStrategy
    {
        get => _tooltipFindingStrategy;
        set
        {
            _tooltipFindingStrategy = value;
            OnPropertyChanged();
        }
    }

    public ZoomAndPanMode ZoomMode { get; set; } = LiveChartsCore.LiveCharts.DefaultSettings.ZoomMode;

    public double ZoomingSpeed { get; set; } = LiveChartsCore.LiveCharts.DefaultSettings.ZoomSpeed;

    public LvcPointD ScalePixelsToData(LvcPointD point, int xAxisIndex = 0, int yAxisIndex = 0)
    {
#if __WEB__
        var cc = (CartesianChart<SkiaDrawingContext>)core;
#else
        if (core is not CartesianChart<SkiaDrawingContext> cc) throw new Exception("core not found");
#endif        
        var xScaler = Scaler.Make(cc.DrawMarginLocation, cc.DrawMarginSize, cc.XAxes[xAxisIndex]);
        var yScaler = Scaler.Make(cc.DrawMarginLocation, cc.DrawMarginSize, cc.YAxes[yAxisIndex]);

        return new LvcPointD(xScaler.ToChartValues(point.X), yScaler.ToChartValues(point.Y));
    }

    public LvcPointD ScaleDataToPixels(LvcPointD point, int xAxisIndex = 0, int yAxisIndex = 0)
    {
#if __WEB__
        var cc = (CartesianChart<SkiaDrawingContext>)core;
#else
        if (core is not CartesianChart<SkiaDrawingContext> cc) throw new Exception("core not found");
#endif        

        var xScaler = Scaler.Make(cc.DrawMarginLocation, cc.DrawMarginSize, cc.XAxes[xAxisIndex]);
        var yScaler = Scaler.Make(cc.DrawMarginLocation, cc.DrawMarginSize, cc.YAxes[yAxisIndex]);

        return new LvcPointD(xScaler.ToPixels(point.X), yScaler.ToPixels(point.Y));
    }

    #endregion

    private void OnDeepCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => OnPropertyChanged();

    private void OnDeepCollectionPropertyChanged(object? sender, PropertyChangedEventArgs e) => OnPropertyChanged();
}