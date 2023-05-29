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

using System;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Motion;
using LiveCharts.Drawing;
using LiveCharts.Painting;


namespace LiveCharts;

/// <summary>
/// Defines the drawing extensions.
/// </summary>
public static class DrawingFluentExtensions
{
    /// <summary>
    /// Initializes a drawing in the given canvas, it is just a simplified API to draw in the chart.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public static DrawingCanvas Draw(this MotionCanvas<SkiaDrawingContext> canvas)
    {
        return new DrawingCanvas(canvas);
    }

    // /// <summary>
    // /// Initializes a drawing in the canvas of the given chart.
    // /// </summary>
    // /// <param name="chart">The chart.</param>
    // public static Drawing Draw(this IChartView chart)
    // {
    //     return Draw(((LiveChartsCore.Chart<SkiaSharpDrawingContext>)chart.CoreChart).Canvas);
    // }
}

/// <summary>
/// Defines the Drawing class.
/// </summary>
public sealed class DrawingCanvas
{
    private IPaint<SkiaDrawingContext>? _selectedPaint;

    /// <summary>
    /// Initializes a new instance of the Drawing class.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public DrawingCanvas(MotionCanvas<SkiaDrawingContext> canvas)
    {
        Canvas = canvas;
    }

    /// <summary>
    /// Gets the canvas.
    /// </summary>
    public MotionCanvas<SkiaDrawingContext> Canvas { get; }

    /// <summary>
    /// Selects the specified paint.
    /// </summary>
    /// <param name="paint">The paint.</param>
    /// <returns>The current drawing instance.</returns>
    public DrawingCanvas SelectPaint(IPaint<SkiaDrawingContext> paint)
    {
        _selectedPaint = paint;
        Canvas.AddDrawableTask(_selectedPaint);

        return this;
    }

    /// <summary>
    /// Selects the specified color.
    /// </summary>
    /// <param name="color">The color to draw with.</param>
    /// <param name="strokeWidth">The stroke width.</param>
    /// <param name="isFill">Indicates whether the geometries are filled with the specified color.</param>
    /// <returns>The current drawing instance.</returns>
    public DrawingCanvas SelectColor(SKColor color, float? strokeWidth = null, bool? isFill = null)
    {
        strokeWidth ??= 1;
        isFill ??= false;
        var paint = SolidColorPaint.MakeByColorAndStroke(color, strokeWidth.Value);
        paint.IsFill = isFill.Value;

        return SelectPaint(paint);
    }

    /// <summary>
    /// Sets the clip rectangle of the selected paint.
    /// </summary>
    /// <returns></returns>
    public DrawingCanvas SetClip(LvcRectangle? clipRectangle)
    {
        if (clipRectangle is null) return this;
        if (_selectedPaint is null)
            throw new Exception(
                "There is no paint selected, please select a paint (By calling a Select method) to add the geometry to.");

        _selectedPaint.SetClipRectangle(Canvas, clipRectangle.Value);
        return this;
    }

    /// <summary>
    /// Draws the specified object.
    /// </summary>
    /// <param name="drawable">The drawable.</param>
    /// <returns></returns>
    public DrawingCanvas Draw(IDrawable<SkiaDrawingContext> drawable)
    {
        if (_selectedPaint is null)
            throw new Exception(
                "There is no paint selected, please select a paint (By calling a Select method) to add the geometry to.");

        _selectedPaint.AddGeometryToPaintTask(Canvas, drawable);

        return this;
    }
}