using LiveChartsCore;
using LiveChartsCore.VisualElements;
using PixUI.LiveCharts.Drawing.Geometries;

namespace PixUI.LiveCharts.VisualElements;

public sealed class SVGVisual: GeometryVisual<VariableSVGPathGeometry>
{
    /// <summary>
    /// Gets or sets the SVG path.
    /// </summary>
    public SKPath? Path { get; set => SetProperty(ref field, value); }

    /// <inheritdoc cref="VisualElement.OnInvalidated(Chart)"/>
    protected internal override void OnInvalidated(Chart chart)
    {
        base.OnInvalidated(chart);
        if (_geometry is not null) _geometry.Path = Path;
    }
}
