using System;
using System.IO;
using System.Text.Json;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Geometries;
using LiveCharts.Drawing.Segments;
using LiveCharts.Painting;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Geo;
using LiveChartsCore.Kernel;

namespace PixUI.Demo;

public sealed class WuxiMap : Widget
{
    public WuxiMap()
    {
        try
        {
            var streamReader = new StreamReader(ResourceLoad.LoadStream("Resources.wuxi.json"));
            _geoJson = System.Text.Json.JsonSerializer.Deserialize<GeoJsonFile>(
                streamReader.ReadToEnd(),
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                });
            _mapLayer = new MapLayer<SkiaDrawingContext>("default",
                new SolidColorPaint { Color = Colors.Black },
                new SolidColorPaint { Color = Colors.Gray });
            if (_geoJson != null)
            {
                _mapLayer.AddFile(_geoJson);
                //计算边框
                foreach (var landDefinition in _mapLayer.Lands.Values)
                {
                    _boundsLeft = double.IsNaN(_boundsLeft)
                        ? landDefinition.MinBounds[0]
                        : Math.Min(landDefinition.MinBounds[0], _boundsLeft);
                    _boundsTop = double.IsNaN(_boundsTop)
                        ? landDefinition.MinBounds[1]
                        : Math.Min(landDefinition.MinBounds[1], _boundsTop);
                    _boundsRight = double.IsNaN(_boundsRight)
                        ? landDefinition.MaxBounds[0]
                        : Math.Max(landDefinition.MaxBounds[0], _boundsRight);
                    _boundsBottom = double.IsNaN(_boundsBottom)
                        ? landDefinition.MaxBounds[1]
                        : Math.Max(landDefinition.MaxBounds[1], _boundsBottom);
                }
            }
        }
        catch (Exception ex)
        {
            _geoJson = null;
            Log.Warn("Can't load geo json for wuxi city");
        }
    }

    private readonly GeoJsonFile? _geoJson;
    private readonly MapLayer<SkiaDrawingContext> _mapLayer = null!;
    private MapProjector _projector = null!;
    private Matrix4 _matrix = Matrix4.CreateIdentity();
    private float _scale = 1f;
    private double _boundsLeft = double.NaN;
    private double _boundsTop = double.NaN;
    private double _boundsRight = double.NaN;
    private double _boundsBottom = double.NaN;

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        // var oldWidth = W;
        // var oldHeight = H;

        SetSize(width, height);

        _projector = Maps.BuildProjector(MapProjection.Mercator, new[] { W, H });

        foreach (var landDefinition in _mapLayer.Lands.Values)
        {
            foreach (var landData in landDefinition.Data)
            {
                HeatPathShape shape;
                if (landData.Shape is null)
                    landData.Shape = shape = new HeatPathShape { IsClosed = true };
                else
                    shape = (HeatPathShape)landData.Shape;
                shape.ClearCommands();

                var isFirst = true;

                foreach (var point in landData.Coordinates)
                {
                    var p = _projector.ToMap(point);

                    var x = p.X;
                    var y = p.Y;

                    if (isFirst)
                    {
                        _ = shape.AddLast(new MoveToPathCommand { X = x, Y = y });
                        isFirst = false;
                        continue;
                    }

                    _ = shape.AddLast(new LineSegment { X = x, Y = y });
                }
            }
        }

        //计算缩放系数及居中位移
        // var center = projector.ToMap(new LvcPointD(120.357298, 31.585559));
        // var ox = W / 2f - center.X;
        // var oy = H / 2f - center.Y;

        var min = _projector.ToMap(new LvcPointD(_boundsLeft, _boundsTop));
        var max = _projector.ToMap(new LvcPointD(_boundsRight, _boundsBottom));
        var cx = (max.X - min.X) / 2f + min.X;
        var cy = (max.Y - min.Y) / 2f + min.Y;
        var ox = W / 2f - cx;
        var oy = H / 2f - cy;

        _scale = _projector.MapWidth / (max.X - min.X); //180f;
        _matrix = Matrix4.CreateTranslation(W / 2f, H / 2f);
        _matrix.Scale(_scale, _scale);
        _matrix.Translate(-(W / 2f), -(H / 2f));
        _matrix.Translate(ox, oy);
    }

    protected override void BeforePaint(Canvas canvas, bool onlyTransform = false, Rect? dirtyRect = null)
    {
        canvas.Save();
        if (X != 0 || Y != 0)
            canvas.Translate(X, Y);
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);

        canvas.Concat(_matrix);
    }

    protected override void AfterPaint(Canvas canvas) => canvas.Restore();

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        using var fillPaint = new Paint()
        {
            Color = new Color(0xFF295EBC),
            Style = PaintStyle.Fill,
            ImageFilter = ImageFilter.CreateDropShadow(2 / _scale, 2 / _scale,
                4 / _scale, 4 / _scale, Colors.Black, null)
        };
        var strokePaint = PaintUtils.Shared(Colors.Black, PaintStyle.Stroke, 2f / _scale);

        foreach (var landDefinition in _mapLayer.Lands.Values)
        {
            foreach (var landData in landDefinition.Data)
            {
                if (landData.Shape is HeatPathShape heatPathShape)
                {
                    using var path = new Path();
                    var temp = heatPathShape.FirstCommand;
                    while (temp != null)
                    {
                        temp.Value.Execute(path, 0, heatPathShape);
                        temp = temp.Next;
                    }

                    if (heatPathShape.IsClosed)
                        path.Close();

                    canvas.DrawPath(path, strokePaint);
                    canvas.DrawPath(path, fillPaint);
                }
            }
        }

        //画名称
        if (_geoJson is { Features: not null })
        {
            foreach (var feature in _geoJson.Features)
            {
                if (feature.Properties?["name"] is JsonElement { ValueKind: JsonValueKind.String } name &&
                    feature.Properties?["centroid"] is JsonElement { ValueKind: JsonValueKind.Array } center)
                {
                    var cp = _projector.ToMap(new LvcPointD(center[0].GetDouble(), center[1].GetDouble()));
                    using var ph =
                        TextPainter.BuildParagraph(name.GetString()!, float.MaxValue, 10 / _scale, Colors.White);
                    canvas.DrawParagraph(ph, cp.X - ph.MaxIntrinsicWidth / 2f, cp.Y);
                }
            }
        }
    }
}