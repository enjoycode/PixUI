using System.Collections.Generic;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveCharts;
using LiveCharts.Drawing;
using LiveCharts.Painting;
using LiveCharts.VisualElements;
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
                            Child = new CartesianChart
                            {
                                Series = series,
                                //Title = title,
                                Width = 400,
                                Height = 300,
                            }
                        },
                        new Card
                        {
                            Child = new PieChart
                            {
                                Series = pieSeries,
                                LegendPosition = LegendPosition.Right,
                                Width = 600,
                                Height = 300,
                            }
                        }
                    }
                },

                new GeoMap()
                {
                    Width = 500,
                    Height = 300,
                    MapProjection = MapProjection.Mercator,
                    ActiveMap = Maps.GetMapFromStreamReader<SkiaDrawingContext>(new StreamReader(geoJson))
                }
            }
        };
    }
}