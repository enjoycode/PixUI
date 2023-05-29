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

using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Drawing;
using LiveCharts.Drawing;
using LiveChartsCore.VisualElements;

namespace LiveCharts.VisualElements;

/// <summary>
/// Defines the visual elements extensions class.
/// </summary>
public static class VisualElementsExtensions
{
    /// <summary>
    /// Creates a relative panel control from a given sketch.
    /// </summary>
    public static RelativePanel<SkiaDrawingContext> AsDrawnControl(
        this Sketch<SkiaDrawingContext> sketch, int baseZIndex = 10050)
    {
        var relativePanel = new RelativePanel<SkiaDrawingContext>
        {
            Size = new LvcSize((float)sketch.Width, (float)sketch.Height)
        };

        foreach (var schedule in sketch.PaintSchedules)
        {
            foreach (var g in schedule.Geometries)
            {
                var sizedGeometry = (ISizedGeometry<SkiaDrawingContext>)g;
                var vgv = new VariableGeometryVisual(sizedGeometry)
                {
                    Width = sizedGeometry.Width,
                    Height = sizedGeometry.Height,
                };

                schedule.PaintTask.ZIndex = schedule.PaintTask.ZIndex + 1 + baseZIndex;

                if (schedule.PaintTask.IsFill) vgv.Fill = schedule.PaintTask;
                if (schedule.PaintTask.IsStroke) vgv.Stroke = schedule.PaintTask;
                _ = relativePanel.Children.Add(vgv);
            }
        }

        return relativePanel;
    }
}
