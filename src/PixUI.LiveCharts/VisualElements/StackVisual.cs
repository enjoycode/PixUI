using LiveChartsCore.Drawing;
using LiveChartsCore.VisualElements;
using PixUI.LiveCharts.Drawing;
using PixUI.LiveCharts.Drawing.Geometries;

namespace PixUI.LiveCharts.VisualElements;

public sealed class StackVisual<TBackgroundGeometry> : StackPanel<TBackgroundGeometry, SkiaSharpDrawingContext>
    where TBackgroundGeometry : BoundedDrawnGeometry, new() { }

public sealed class StackVisual : StackPanel<RectangleGeometry, SkiaSharpDrawingContext> { }