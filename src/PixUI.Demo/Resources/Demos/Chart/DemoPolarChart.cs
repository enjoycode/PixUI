using LiveChartsCore;
using LiveChartsCore.Drawing;
using PixUI.LiveCharts;
using PixUI.LiveCharts.Painting;

public class DemoPolarChart : View
{
    public DemoPolarChart()
    {
        Child = new Center
        {
            Child = new Container
            {
                Width = 400,
                Height = 400,
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
        };
    }
    
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
    
}