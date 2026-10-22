using LiveChartsCore.Geo;
using LiveChartsCore.Drawing.Segments;
using PixUI.LiveCharts.Drawing.Geometries;
using PixUI.LiveCharts.Painting;
using System.Text.Json;

public class DemoWuxiMap : View
{
    public DemoWuxiMap()
    {
        try
        {
            var assembly = System.Reflection.Assembly.Load("PixUI.Demo");
            var cityStream = assembly.GetManifestResourceStream("PixUI.Demo.Resources.Wuxi.json");
            var areaStream = assembly.GetManifestResourceStream("PixUI.Demo.Resources.WuxiAreas.json");
            var jsonOpts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var cityJson = JsonSerializer.Deserialize<GeoJsonFile>(cityStream, jsonOpts);
            _cityLayer = new MapLayer("city",
                new SolidColorPaint { Color = Colors.Black },
                new SolidColorPaint { Color = Colors.Gray });
            _areasJson = JsonSerializer.Deserialize<GeoJsonFile>(areaStream, jsonOpts);
            _areasLayer = new MapLayer("default",
                new SolidColorPaint { Color = Colors.Black },
                new SolidColorPaint { Color = Colors.Gray });

            _cityLayer.AddFile(cityJson!);
            _areasLayer.AddFile(_areasJson!);

            //计算边框
            foreach (var landDefinition in _areasLayer.Lands.Values)
            {
                _boundsLeft = Math.Min(landDefinition.MinBounds[0], _boundsLeft);
                _boundsTop = Math.Min(landDefinition.MinBounds[1], _boundsTop);
                _boundsRight = Math.Max(landDefinition.MaxBounds[0], _boundsRight);
                _boundsBottom = Math.Max(landDefinition.MaxBounds[1], _boundsBottom);
            }
        }
        catch (Exception ex)
        {
            _areasJson = null;
            Notification.Error("Can't load geo json for wuxi city");
        }
    }
    
    
    private readonly GeoJsonFile? _areasJson;
    private readonly MapLayer _cityLayer = null!;
    private readonly MapLayer _areasLayer = null!;
    private MapProjector _projector = null!;
    private Matrix4 _matrix = Matrix4.CreateIdentity();
    private float _scale = 1f;
    private readonly double _boundsLeft = double.MaxValue;
    private readonly double _boundsTop = double.MaxValue;
    private readonly double _boundsRight = double.MinValue;
    private readonly double _boundsBottom = double.MinValue;

    protected override void OnLayout(Size maxSize)
    {
        // var oldWidth = W;
        // var oldHeight = H;

        SetLayoutSize(maxSize.Width, maxSize.Height);

        _projector = Maps.BuildProjector(MapProjection.Mercator, [W, H]);

        BuildShapes(_areasLayer, _projector);
        BuildShapes(_cityLayer, _projector);

        //计算缩放系数及居中位移
        // var center = _projector.ToMap(new LvcPointD(120.357298, 31.585559));
        // var ox = W / 2f - center.X;
        // var oy = H / 2f - center.Y;
        _projector.ToMap(_boundsLeft, _boundsTop, out var minX, out var minY);
        _projector.ToMap(_boundsRight, _boundsBottom, out var maxX, out var maxY);
        var cx = (maxX - minX) / 2f + minX;
        var cy = (maxY - minY) / 2f + minY;
        var ox = W / 2f - cx;
        var oy = H / 2f - cy;

        //_scale = Math.Min(_projector.MapWidth / Math.Abs(maxX - minX), _projector.MapHeight / Math.Abs(maxY - minY));
        _scale = _projector.MapHeight / Math.Abs(maxY - minY);
        _matrix = Matrix4.CreateTranslation(W / 2f, H / 2f);
        _matrix.Scale(_scale, _scale);
        _matrix.Translate(-(W / 2f), -(H / 2f));
        _matrix.Translate(ox, oy);
    }

    private static void BuildShapes(MapLayer layer, MapProjector projector)
    {
        //参考MapFactory.GenerateLands()
        foreach (var landDefinition in layer.Lands.Values)
        {
            foreach (var landData in landDefinition.Data)
            {
                LandAreaGeometry shape;
                if (landData.Shape is null)
                    landData.Shape = shape = new LandAreaGeometry();
                else
                    shape = (LandAreaGeometry)landData.Shape;
                shape.Commands.Clear();

                var isFirst = true;
                float xp = 0, yp = 0;

                foreach (var point in landData.Coordinates)
                {
                    projector.ToMap(point.X, point.Y, out var x, out var y);

                    if (isFirst)
                    {
                        xp = x;
                        yp = y;
                    }

                    _ = shape.Commands.AddLast(new Segment { Xi = xp, Yi = yp, Xj = x, Yj = y });
                }

                shape.MarkPathDirty();
            }
        }
    }

    protected override void BeforePaint(ICanvas canvas, bool onlyTransform = false, IDirtyArea? dirtyArea = null)
    {
        canvas.Save();
        canvas.Translate(X, Y);
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
        canvas.Concat(_matrix);
    }

    protected override void AfterPaint(ICanvas canvas) => canvas.Restore();

    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        const float s = 4;
        using var fillPaint = Paint.Create();
        fillPaint.Color = new Color(0xFF295EBC);
        fillPaint.Style = PaintStyle.Fill;
        fillPaint.ImageFilter = PixUI.ImageFilter.CreateDropShadow(s / _scale, s / _scale,
            s * 2 / _scale, s * 2 / _scale, Colors.Black, null);
        var strokePaint = Paint.Shared(Colors.Black, PaintStyle.Stroke, 2f / _scale);

        PaintLayer(_cityLayer, canvas, strokePaint, fillPaint);
        fillPaint.ImageFilter = null;
        PaintLayer(_areasLayer, canvas, strokePaint, fillPaint);

        //画区名称
        if (_areasJson is { Features: not null })
        {
            foreach (var feature in _areasJson.Features)
            {
                if (feature.Properties?["name"] is { ValueKind: JsonValueKind.String } name &&
                    feature.Properties?["centroid"] is { ValueKind: JsonValueKind.Array } center)
                {
                    _projector.ToMap(center[0].GetDouble(), center[1].GetDouble(), out var cpX, out var cpY);
                    using var ph =
                        TextPainter.BuildParagraph(name.GetString()!, float.MaxValue, 10 / _scale, Colors.White);
                    canvas.DrawParagraph(ph, cpX - ph.MaxIntrinsicWidth / 2f, cpY);
                }
            }
        }
    }

    private static void PaintLayer(MapLayer layer, ICanvas canvas, IPaint strokePaint, IPaint fillPaint)
    {
        foreach (var landDefinition in layer.Lands.Values)
        {
            foreach (var landData in landDefinition.Data)
            {
                if (landData.Shape is LandAreaGeometry shape)
                {
                    using var pathBuilder = PathBuilder.Create();
                    var isFirst = true;
                    foreach (var segment in shape.Commands)
                    {
                        if (isFirst)
                        {
                            pathBuilder.MoveTo(segment.Xi, segment.Yi);
                            isFirst = false;
                        }

                        pathBuilder.LineTo(segment.Xi, segment.Yi);
                    }

                    using var path = pathBuilder.Detach();
                    canvas.DrawPath(path, strokePaint);
                    canvas.DrawPath(path, fillPaint);
                }
            }
        }
    }
}