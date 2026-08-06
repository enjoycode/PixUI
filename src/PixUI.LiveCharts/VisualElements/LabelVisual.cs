using LiveChartsCore.VisualElements;
using PixUI.LiveCharts.Drawing.Geometries;

namespace PixUI.LiveCharts.VisualElements;

public class LabelVisual : BaseLabelVisual<LabelGeometry>
{
    /// <summary>
    /// The default values used for the Xaml generator.
    /// </summary>
    public static LabelVisual DefaultValues { get; } = new();
}