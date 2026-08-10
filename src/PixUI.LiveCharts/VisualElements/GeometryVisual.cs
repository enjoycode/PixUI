using LiveChartsCore.Drawing;
using PixUI.LiveCharts.Drawing.Geometries;

namespace PixUI.LiveCharts.VisualElements;

public class GeometryVisual<TGeometry> : LiveChartsCore.VisualElements.GeometryVisual<TGeometry, LabelGeometry>
    where TGeometry : BoundedDrawnGeometry, new() { }