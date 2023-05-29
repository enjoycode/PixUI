// The MIT License(MIT)
//
// Copyright(c) 2021 Alberto Rodriguez Orozco & LiveCharts Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections.Generic;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Geometries;
using LiveCharts.Painting;
using LiveCharts.SKCharts.Helpers;
using LiveCharts.VisualElements;
using LiveChartsCore.VisualElements;


namespace LiveCharts.SKCharts;

/// <inheritdoc cref="IChartLegend{TDrawingContext}" />
public class SKDefaultLegend : IChartLegend<SkiaDrawingContext>, IImageControl
{
    private static readonly int s_zIndex = 10050;
    private ContainerOrientation _orientation = ContainerOrientation.Vertical;
    private StackPanel<RoundedRectangleGeometry, SkiaDrawingContext>? _stackPanel;

    private readonly DoubleDict<IChartSeries<SkiaDrawingContext>, VisualElement<SkiaDrawingContext>> _activeSeries =
        new();

    private List<VisualElement<SkiaDrawingContext>> _toRemoveSeries = new();
    private IPaint<SkiaDrawingContext>? _backgroundPaint;

    /// <summary>
    /// Initializes a new instance of the <see cref="SKDefaultLegend"/> class.
    /// </summary>
    public SKDefaultLegend()
    {
        FontPaint = new SolidColorPaint { Color = new SKColor(30, 30, 30, 255) };
    }

    /// <inheritdoc cref="IImageControl.Size"/>
    public LvcSize Size { get; set; }

    /// <summary>
    /// Gets or sets the legend font paint.
    /// </summary>
    public IPaint<SkiaDrawingContext>? FontPaint { get; set; }

    /// <summary>
    /// Gets or sets the background paint.
    /// </summary>
    public IPaint<SkiaDrawingContext>? BackgroundPaint
    {
        get => _backgroundPaint;
        set
        {
            _backgroundPaint = value;
            if (value is not null)
            {
                value.IsFill = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets the fonts size.
    /// </summary>
    public double TextSize { get; set; } = 15;

    void IChartLegend<SkiaDrawingContext>.Draw(LiveChartsCore.Chart<SkiaDrawingContext> chart)
    {
        if (chart.Legend is null || chart.LegendPosition == LegendPosition.Hidden) return;

        Measure(chart);

        if (_stackPanel is null) return;
        if (BackgroundPaint is not null) BackgroundPaint.ZIndex = s_zIndex;
        if (FontPaint is not null) FontPaint.ZIndex = s_zIndex + 1;

        var actualChartSize = chart.ControlSize;

        // this seems a constant layout issue...
        // ToDo:
        // this is a workaround to force the legend to be drawn in the correct position
        // It seems that this value is constant, it seems to not be affected by the font size or the stack panel properties.
        // is this and SkiaSharp measure issue?
        // is it a LiveCharts issue?
        var iDontKnowWhyThis = 17;

        if (chart.LegendPosition == LegendPosition.Top)
        {
            chart.Canvas.StartPoint = new LvcPoint(0, Size.Height);
            _stackPanel.X = actualChartSize.Width * 0.5f - Size.Width * 0.5f;
            _stackPanel.Y = -Size.Height;
        }

        if (chart.LegendPosition == LegendPosition.Bottom)
        {
            _stackPanel.X = actualChartSize.Width * 0.5f - Size.Width * 0.5f;
            _stackPanel.Y = actualChartSize.Height;
        }

        if (chart.LegendPosition == LegendPosition.Left)
        {
            chart.Canvas.StartPoint = new LvcPoint(Size.Width, 0);
            _stackPanel.X = -Size.Width;
            _stackPanel.Y = actualChartSize.Height * 0.5f - Size.Height * 0.5f;
        }

        if (chart.LegendPosition == LegendPosition.Right)
        {
            _stackPanel.X = actualChartSize.Width - iDontKnowWhyThis;
            _stackPanel.Y = actualChartSize.Height * 0.5f - Size.Height * 0.5f;
        }

        chart.AddVisual(_stackPanel);

        foreach (var visual in _toRemoveSeries)
        {
            _ = _stackPanel.Children.Remove(visual);
            chart.RemoveVisual(visual);
            if (_activeSeries.TryGetValue(visual, out var series)) _ = _activeSeries.Remove(series);
        }
    }

    private void BuildLayout(LiveChartsCore.Chart<SkiaDrawingContext> chart)
    {
        if (chart.View.LegendBackgroundPaint is not null) BackgroundPaint = chart.View.LegendBackgroundPaint;
        if (chart.View.LegendTextPaint is not null) FontPaint = chart.View.LegendTextPaint;
        if (chart.View.LegendTextSize is not null) TextSize = chart.View.LegendTextSize.Value;

#if __WEB__
        _orientation = chart.LegendPosition == LegendPosition.Left || chart.LegendPosition == LegendPosition.Right
            ? ContainerOrientation.Vertical
            : ContainerOrientation.Horizontal;
#else
        _orientation = chart.LegendPosition is LegendPosition.Left or LegendPosition.Right
            ? ContainerOrientation.Vertical
            : ContainerOrientation.Horizontal;
#endif        

        _stackPanel ??=
            new StackPanel<RoundedRectangleGeometry, SkiaDrawingContext>(() => new RoundedRectangleGeometry())
            {
                Padding = Padding.All(0),
                HorizontalAlignment = Align.Start,
                VerticalAlignment = Align.Middle,
            };

        _stackPanel.Orientation = _orientation;
        _stackPanel.BackgroundPaint = BackgroundPaint;

        _toRemoveSeries = new List<VisualElement<SkiaDrawingContext>>(_stackPanel.Children);

        foreach (var series in chart.ChartSeries)
        {
            if (!series.IsVisibleAtLegend) continue;

            var seriesMiniatureVisual = GetSeriesVisual(series);
            _ = _toRemoveSeries.Remove(seriesMiniatureVisual);
        }
    }

    /// <inheritdoc cref="IImageControl.Measure(IChart)"/>
    public void Measure(IChart chart)
    {
        var skiaChart = (LiveChartsCore.Chart<SkiaDrawingContext>)chart;
        BuildLayout(skiaChart);
        if (_stackPanel is null) return;
        Size = _stackPanel.Measure(skiaChart, null, null);
    }

    private VisualElement<SkiaDrawingContext> GetSeriesVisual(IChartSeries<SkiaDrawingContext> series)
    {
        if (_activeSeries.TryGetValue(series, out var seriesPanel)) return seriesPanel;

        var sketch = series.GetMiniatresSketch();
        var relativePanel = sketch.AsDrawnControl();

        var sp = new StackPanel<RoundedRectangleGeometry, SkiaDrawingContext>(() => new RoundedRectangleGeometry())
        {
            Padding = new Padding(15, 4, 15, 4),
            VerticalAlignment = Align.Middle,
            HorizontalAlignment = Align.Middle,
#if !__WEB__            
            Children =
            {
                relativePanel,
                new LabelVisual
                {
                    Text = series.Name ?? string.Empty,
                    Paint = FontPaint,
                    TextSize = TextSize,
                    Padding = new Padding(8, 0, 0, 0),
                    VerticalAlignment = Align.Start,
                    HorizontalAlignment = Align.Start
                }
            }
#endif            
        };
#if __WEB__        
        sp.Children.Add(relativePanel);
        sp.Children.Add(new LabelVisual
        {
            Text = series.Name ?? string.Empty,
            Paint = FontPaint,
            TextSize = TextSize,
            Padding = new Padding(8, 0, 0, 0),
            VerticalAlignment = Align.Start,
            HorizontalAlignment = Align.Start
        });
#endif

        _ = _stackPanel?.Children.Add(sp);
        _activeSeries.Add(series, sp);

        return sp;
    }
}