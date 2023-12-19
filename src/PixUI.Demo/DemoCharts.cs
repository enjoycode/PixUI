using System.Collections.Generic;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveCharts;
using LiveCharts.Drawing;
using LiveCharts.Painting;
using LiveCharts.Painting.ImageFilters;
using LiveCharts.VisualElements;
using LiveChartsCore.Drawing;
using LiveChartsCore.Geo;

namespace PixUI.Demo;

public sealed class DemoCharts : View
{
    private static float[] data1 = { 3, 2, 5, 6, 4, 1, 2 };
    private static float[] data2 = { 2, 1, 3, 5, 3, 4, 6 };

    private ISeries[] series =
    {
        new ColumnSeries<float> { Values = data1, },
        new LineSeries<float> { Values = data2, Fill = null },
    };

    private IEnumerable<ISeries> pieSeries = data1.AsLiveChartsPieSeries((value, s) =>
    {
        // here you can configure the series assigned to each value.
        s.Name = $"S{value}";
        s.DataLabelsPaint = new SolidColorPaint { Color = new Color(30, 30, 30) };
        s.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
        //s.DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})";
        s.DataLabelsFormatter = p => $"{p.StackedValue.Share:P2}";
    });

    private ISeries[] polarSeries =
    {
        new PolarLineSeries<int>
        {
            Values = new[] { 7, 5, 7, 5, 6 },
            LineSmoothness = 0,
            GeometrySize = 0,
            Fill = new SolidColorPaint { Color = Colors.Blue.WithAlpha(90) }
        },
        new PolarLineSeries<int>
        {
            Values = new[] { 2, 7, 5, 9, 7 },
            LineSmoothness = 1,
            GeometrySize = 0,
            Fill = new SolidColorPaint { Color = Colors.Red.WithAlpha(90) }
        }
    };

    private PolarAxis[] polarAngleAxes =
    {
        new PolarAxis
        {
            // LabelsRotation = LiveChartsCore.LiveCharts.TangentAngle,
            LabelsBackground = LvcColor.Empty,
            Labels = new[] { "first", "second", "third", "forth", "fifth" }
        }
    };

    private LabelVisual title = new LabelVisual()
    {
        Text = "My Chart Title",
        TextSize = 25,
        Padding = new LiveChartsCore.Drawing.Padding(15),
        Paint = new SolidColorPaint { Color = Colors.Gray }
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
        var min = projector.ToMap(minBounds);
        var max = projector.ToMap(maxBounds);
        var cx = (max.X - min.X) / 2f + min.X;
        var cy = (max.Y - min.Y) / 2f + min.Y;
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
                                Series = series,
                                //Title = title,
                            }
                        },
                        new Card
                        {
                            Width = 600,
                            Height = 300,
                            Child = new PieChart
                            {
                                Series = pieSeries,
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
                                    Child = new Transform(matrix)
                                    {
                                        Child = new GeoMap()
                                        {
                                            Width = mapWidth,
                                            Height = mapHeight,
                                            MapProjection = projection,
                                            Stroke = new SolidColorPaint { Color = Colors.Green },
                                            Fill = new SolidColorPaint
                                            {
                                                Color = Colors.Red,
                                                // ImageFilter = new DropShadow(
                                                //     2 / scale, 2 / scale, 6 / scale, 6 / scale,
                                                //     Colors.Black /*new Color(50, 0, 0, 100)*/
                                                // )
                                            },
                                            ActiveMap = Maps.GetMapFromStreamReader<SkiaDrawingContext>(
                                                new StreamReader(geoJson))
                                        }
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
                                    Series = polarSeries,
                                    AngleAxes = polarAngleAxes,
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