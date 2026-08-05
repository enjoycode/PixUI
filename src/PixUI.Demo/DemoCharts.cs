using System.Collections.Generic;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.Measure;
using PixUI.LiveCharts;
using LiveChartsCore.Drawing;
using LiveChartsCore.Geo;
using PixUI.LiveCharts.Drawing.Geometries;
using PixUI.LiveCharts.Painting;

namespace PixUI.Demo;

public sealed class DemoCharts : View
{
    private static readonly float[] _data1 = [3, 2, 5, 6, 4, 1, 2];
    private static readonly float[] _data2 = [2, 1, 3, 5, 3, 4, 6];

    private readonly ISeries[] _series =
    {
        new ColumnSeries<float> { Values = _data1, },
        new LineSeries<float, StarGeometry> { Values = _data2, Fill = null },
    };

    private readonly IEnumerable<ISeries> _pieSeries = _data1.AsPieSeries((value, s) =>
    {
        // here you can configure the series assigned to each value.
        s.Name = $"S{value}";
        s.DataLabelsPaint = new SolidColorPaint { Color = new Color(30, 30, 30) };
        s.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
        //s.DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})";
        s.DataLabelsFormatter = p => $"{p.StackedValue.Share:P2}";
    });

    private readonly ISeries[] _polarSeries =
    {
        new PolarLineSeries<int>
        {
            Values = [7, 5, 7, 5, 6],
            LineSmoothness = 0,
            GeometrySize = 0,
            Fill = new SolidColorPaint { Color = Colors.Blue.WithAlpha(90) }
        },
        new PolarLineSeries<int>
        {
            Values = [2, 7, 5, 9, 7],
            LineSmoothness = 1,
            GeometrySize = 0,
            Fill = new SolidColorPaint { Color = Colors.Red.WithAlpha(90) }
        }
    };

    private readonly PolarAxis[] _polarAngleAxes =
    {
        new PolarAxis
        {
            // LabelsRotation = LiveChartsCore.LiveCharts.TangentAngle,
            LabelsBackground = LvcColor.Empty,
            Labels = ["first", "second", "third", "forth", "fifth"]
        }
    };

    public DemoCharts()
    {
        var geoJson = ResourceLoad.LoadStream("Resources.China.json");
        //var gcp1 = new LvcPointD(116.3683244, 39.915085);
        //var gcp2 = new LvcPointD(104.113164, 37.570667);
        //max 135.09567000000001, 53.563268999999998
        //min 73.502354999999994, 3.3971618700000001
        var maxBounds = new LvcPointD(135.09567000000001, 53.563268999999998);
        var minBounds = new LvcPointD(73.502354999999994, 3.3971618700000001);

        var scale = 5f;
        var mapWidth = 400f;
        var mapHeight = 300f;
        var projection = MapProjection.Mercator;
        var projector = Maps.BuildProjector(projection, new[] { mapWidth, mapHeight });

        //var center = projector.ToMap(gcp2);
        // var ox = mapWidth / 2f - center.X;
        // var oy = mapHeight / 2f - center.Y;
        projector.ToMap(minBounds.X, minBounds.Y, out var minX, out var minY);
        projector.ToMap(maxBounds.X, maxBounds.Y, out var maxX, out var maxY);
        var cx = (maxX - minX) / 2f + minX;
        var cy = (maxY - minY) / 2f + minY;
        var ox = mapWidth / 2f - cx;
        var oy = mapHeight / 2f - cy;

        var matrix = Matrix4.CreateTranslation(mapWidth / 2f, mapHeight / 2f);
        matrix.Scale(scale, scale);
        matrix.Translate(-(mapWidth / 2f), -(mapHeight / 2f));
        matrix.Translate(ox, oy);

        FillColor = Colors.Green;

        Child = new Column
        {
            Children =
            {
                new Row
                {
                    Children =
                    {
                        new Card
                        {
                            Width = 400,
                            Height = 300,
                            Child = new CartesianChart
                            {
                                Series = _series,
                                //Title = title,
                            }
                        },
                        new Card
                        {
                            Width = 600,
                            Height = 300,
                            Child = new PieChart
                            {
                                Series = _pieSeries,
                                LegendPosition = LegendPosition.Right,
                            }
                        }
                    }
                },

                new Row
                {
                    Children =
                    {
                        new Expanded
                        {
                            Child = new Card
                            {
                                Child = new Center
                                {
                                    DebugLabel = "ChartCenter",
                                    Child = new GeoMap()
                                    {
                                        Width = mapWidth,
                                        Height = mapHeight,
                                        MapProjection = projection,
                                        MinLongitude = minBounds.X,
                                        MinLatitude = minBounds.Y,
                                        MaxLongitude = maxBounds.X,
                                        MaxLatitude = maxBounds.Y,
                                        Stroke = new SolidColorPaint { Color = Colors.Green },
                                        Fill = new SolidColorPaint
                                        {
                                            Color = Colors.Red,
                                            // ImageFilter = new DropShadow(
                                            //     2 / scale, 2 / scale, 6 / scale, 6 / scale,
                                            //     Colors.Black /*new Color(50, 0, 0, 100)*/
                                            // )
                                        },
                                        ActiveMap = Maps.GetMapFromStreamReader(new StreamReader(geoJson))
                                    }
                                }
                            }
                        },
                        new Expanded
                        {
                            Child = new Card
                            {
                                Child = new PolarChart()
                                {
                                    Series = _polarSeries,
                                    AngleAxes = _polarAngleAxes,
                                    InitialRotation = -45,
                                }
                            }
                        }
                    }
                },
            }
        };
    }
}