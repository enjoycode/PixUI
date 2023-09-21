using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Drawing;
using LiveCharts.Painting;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Geo;
using LiveChartsCore.Kernel;
using LiveChartsCore.Motion;
using LCC = LiveChartsCore;
using PixUI;

namespace LiveCharts;

public sealed class GeoMap : Widget, IMouseRegion, IGeoMapView<SkiaDrawingContext>, IPaintEmptyClip
{
    public GeoMap()
    {
        _motionCanvas = new MotionCanvas(this);

        LCC.LiveCharts.Configure(config => config.UseDefaults());

        _core = new GeoMap<SkiaDrawingContext>(this);

        _seriesObserver = new CollectionDeepObserver<IGeoSeries>(
            (sender, e) => _core.Update(),
            (sender, e) => _core.Update(),
            true);

        MouseRegion = new MouseRegion();
        MouseRegion.PointerMove += e => _core.InvokePointerMove(new LvcPoint(e.X, e.Y));
        MouseRegion.HoverChanged += hover =>
        {
            if (!hover) _core.InvokePointerLeft();
        };
    }

    #region ====Fields====

    private readonly MotionCanvas _motionCanvas;
    private readonly GeoMap<SkiaDrawingContext> _core;
    private CollectionDeepObserver<IGeoSeries> _seriesObserver;
    private IEnumerable<IGeoSeries> _series = Enumerable.Empty<IGeoSeries>();
    private CoreMap<SkiaDrawingContext> _activeMap;
    private MapProjection _mapProjection = MapProjection.Default;

    private IPaint<SkiaDrawingContext>? _stroke = new SolidColorPaint
        { IsStroke = true, Color = new Color(255, 255, 255, 255) };

    private IPaint<SkiaDrawingContext>? _fill = new SolidColorPaint
        { IsFill = true, Color = new Color(240, 240, 240, 255) };

    private object? _viewCommand = null;

    #endregion

    #region ====IGeoMapView====

    public bool DesignerMode => false;

    public object SyncContext { get; set; } = new();

    public MotionCanvas<SkiaDrawingContext> Canvas => _motionCanvas.CanvasCore;

    public bool AutoUpdateEnabled { get; set; } = true;

    public object? ViewCommand
    {
        get => _viewCommand;
        set
        {
            _viewCommand = value;
            if (value is not null) _core.ViewTo(value);
        }
    }

    public CoreMap<SkiaDrawingContext> ActiveMap
    {
        get => _activeMap;
        set
        {
            _activeMap = value;
            OnPropertyChanged();
        }
    }

    float IGeoMapView<SkiaDrawingContext>.Width => W;

    float IGeoMapView<SkiaDrawingContext>.Height => H;

    public MapProjection MapProjection
    {
        get => _mapProjection;
        set
        {
            _mapProjection = value;
            OnPropertyChanged();
        }
    }

    public IPaint<SkiaDrawingContext>? Stroke
    {
        get => _stroke;
        set
        {
            if (value is not null) value.IsStroke = true;
            _stroke = value;
            OnPropertyChanged();
        }
    }

    public IPaint<SkiaDrawingContext>? Fill
    {
        get => _fill;
        set
        {
            if (value is not null) value.IsFill = true;
            _fill = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<IGeoSeries> Series
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

    public void InvokeOnUIThread(Action action)
    {
        if (!IsMounted) return;

        UIApplication.Current.BeginInvoke(action);
    }

    private void OnPropertyChanged() => _core.Update();

    #endregion

    #region ====Widget Overries====

    public MouseRegion MouseRegion { get; }

    protected override void OnMounted()
    {
        base.OnMounted();

        _motionCanvas.CanvasCore.Invalidated += _motionCanvas.CanvasCore_Invalidated;
    }

    protected override void OnUnmounted()
    {
        base.OnUnmounted();

        _motionCanvas.CanvasCore.Invalidated -= _motionCanvas.CanvasCore_Invalidated;
        _motionCanvas.CanvasCore.Dispose();

        _core.Unload();
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        var oldWidth = W;
        var oldHeight = H;

        SetSize(width, height);

        if (HasLayout && (oldWidth != W || oldHeight != H))
            _core.Update();
    }

    private SkiaDrawingContext? _drawCtx;

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        canvas.Save();
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);

        if (_drawCtx == null)
        {
            _drawCtx = new SkiaDrawingContext(_motionCanvas.CanvasCore, (int)W, (int)H, canvas);
            //{ Background = BackColor.AsSKColor() };
        }
        else
        {
            _drawCtx.Canvas = canvas;
            _drawCtx.Width = (int)W;
            _drawCtx.Height = (int)H;
            //_drawCtx.Background = BackColor.AsSKColor();
        }

        _motionCanvas.CanvasCore.DrawFrame(_drawCtx);

        canvas.Restore();
    }

    void IPaintEmptyClip.ClearOrStopPaint(Canvas canvas)
    {
        //暂直接停止DrawingLoop
        _motionCanvas.StopDrawingLoop();
    }

    #endregion
}