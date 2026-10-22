using LiveChartsCore;
using PixUI.LiveCharts;
using PixUI.LiveCharts.Drawing.Geometries;

public class DemoCartesianChart : View
{
    public DemoCartesianChart()
    {
        Child = new Center
        {
            Child = new Card
            {
                Width = 400,
                Height = 300,
                Child = new CartesianChart
                {
                    Series = _series,
                    //Title = title,
                }
            },
        };
    }
    
    private static readonly float[] _data1 = [3, 2, 5, 6, 4, 1, 2];
    private static readonly float[] _data2 = [2, 1, 3, 5, 3, 4, 6];

    private readonly ISeries[] _series =
    {
        new ColumnSeries<float> { Values = _data1, },
        new LineSeries<float, StarGeometry> { Values = _data2, Fill = null },
    };
    
}