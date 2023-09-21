﻿// The MIT License(MIT)
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
using System.Linq;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveCharts.Drawing;
using LiveCharts.Drawing.Geometries;
using LiveCharts.Painting;
using LiveCharts.SKCharts.Helpers;
using LiveCharts.VisualElements;
using LiveChartsCore;
using LiveChartsCore.VisualElements;


namespace LiveCharts.SKCharts;

/// <summary>
/// Defines the default legend for a chart.
/// </summary>
public class SKDefaultLegend : IChartLegend<SkiaDrawingContext>
{
    private static readonly int s_zIndex = 10050;

    // marked as internal only for testing purposes
    internal readonly StackPanel<RoundedRectangleGeometry, SkiaDrawingContext> _stackPanel = new()
    {
        Padding = new Padding(15, 4),
        HorizontalAlignment = Align.Start,
        VerticalAlignment = Align.Middle
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="SKDefaultLegend"/> class.
    /// </summary>
    public SKDefaultLegend()
    {
        FontPaint = SolidColorPaint.MakeByColor(new SKColor(30, 30, 30, 255));
    }

    /// <summary>
    /// Gets or sets the legend font paint.
    /// </summary>
    public IPaint<SkiaDrawingContext>? FontPaint { get; set; }

    /// <summary>
    /// Gets or sets the fonts size.
    /// </summary>
    public double TextSize { get; set; } = 15;

    /// <inheritdoc cref="IChartLegend{TDrawingContext}.Draw(Chart{TDrawingContext})"/>
    public void Draw(Chart<SkiaDrawingContext> chart)
    {
        var legendPosition = chart.GetLegendPosition();

        _stackPanel.X = legendPosition.X;
        _stackPanel.Y = legendPosition.Y;

        chart.AddVisual(_stackPanel);
        if (chart.LegendPosition == LegendPosition.Hidden) chart.RemoveVisual(_stackPanel);
    }

    /// <inheritdoc cref="IChartLegend{TDrawingContext}.Measure(Chart{TDrawingContext})"/>
    public LvcSize Measure(Chart<SkiaDrawingContext> chart)
    {
        BuildLayout(chart);
        return _stackPanel.Measure(chart);
    }

    private void BuildLayout(Chart<SkiaDrawingContext> chart)
    {
        if (chart.View.LegendTextPaint is not null) FontPaint = chart.View.LegendTextPaint;
        if (chart.View.LegendTextSize is not null) TextSize = chart.View.LegendTextSize.Value;

        if (FontPaint is not null) FontPaint.ZIndex = s_zIndex + 1;

        _stackPanel.Orientation = chart.LegendPosition is LegendPosition.Left or LegendPosition.Right
            ? ContainerOrientation.Vertical
            : ContainerOrientation.Horizontal;

        if (_stackPanel.Orientation == ContainerOrientation.Horizontal)
        {
            _stackPanel.MaxWidth = chart.ControlSize.Width;
            _stackPanel.MaxHeight = double.MaxValue;
        }
        else
        {
            _stackPanel.MaxWidth = double.MaxValue;
            _stackPanel.MaxHeight = chart.ControlSize.Height;
        }

        foreach (var visual in _stackPanel.Children.ToArray())
        {
            _ = _stackPanel.Children.Remove(visual);
            chart.RemoveVisual(visual);
        }

        foreach (var series in chart.Series.Where(x => x.IsVisibleAtLegend))
        {
            _stackPanel.Children.Add(new StackPanel<RectangleGeometry, SkiaDrawingContext>
            {
                Padding = new Padding(12, 6),
                VerticalAlignment = Align.Middle,
                HorizontalAlignment = Align.Middle,
                Children =
                {
                    series.GetMiniaturesSketch().AsDrawnControl(s_zIndex),
                    new LabelVisual
                    {
                        Text = series.Name ?? string.Empty,
                        Paint = FontPaint,
                        TextSize = TextSize,
                        Padding = new Padding(8, 0, 0, 0),
                        MaxWidth = (float)LiveChartsCore.LiveCharts.DefaultSettings.MaxTooltipsAndLegendsLabelsWidth,
                        VerticalAlignment = Align.Start,
                        HorizontalAlignment = Align.Start,
                        ClippingMode = ClipMode.None
                    }
                }
            });
        }
    }
}