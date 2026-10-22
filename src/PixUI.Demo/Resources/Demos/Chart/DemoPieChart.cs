using LiveChartsCore;
using LiveChartsCore.Measure;
using PixUI.LiveCharts;
using PixUI.LiveCharts.Painting;

public class DemoPieChart : View
{
    public DemoPieChart()
    {
        Child = new Center
        {
            Child = new Card
            {
                Width = 600,
                Height = 300,
                Child = new PieChart
                {
                    Series = _pieSeries,
                    LegendPosition = LegendPosition.Right,
                }
            }
        };
    }
    
    private static readonly float[] _data1 = [3, 2, 5, 6, 4, 1, 2];
    
    private readonly IEnumerable<ISeries> _pieSeries = _data1.AsPieSeries((value, s) =>
    {
        // here you can configure the series assigned to each value.
        s.Name = $"S{value}";
        s.DataLabelsPaint = new SolidColorPaint { Color = new Color(30, 30, 30) };
        s.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
        //s.DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})";
        s.DataLabelsFormatter = p => $"{p.StackedValue.Share:P2}";
    });
}