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

using LiveCharts.Drawing;
using LiveChartsCore.Drawing;
using LiveChartsCore.Motion;

namespace LiveCharts.Drawing.Geometries;

/// <summary>
/// Defines a rounded rectangle geometry.
/// </summary>
/// <seealso cref="SizedGeometry" />
public class RoundedRectangleGeometry : SizedGeometry, IRoundedGeometry<SkiaDrawingContext>
{
    private readonly PointMotionProperty _borderRadius;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoundedRectangleGeometry"/> class.
    /// </summary>
    public RoundedRectangleGeometry()
    {
        _borderRadius = RegisterMotionProperty(new PointMotionProperty(nameof(BorderRadius), new LvcPoint(8f, 8f)));
    }

    /// <inheritdoc cref="IRoundedGeometry{TDrawingContext}.BorderRadius"/>
    public LvcPoint BorderRadius
    {
        get => _borderRadius.GetMovement(this);
        set => _borderRadius.SetMovement(value, this);
    }

    /// <inheritdoc cref="Geometry.OnDraw(SkiaSharpDrawingContext, SKPaint)" />
    public override void OnDraw(SkiaDrawingContext context, SKPaint paint)
    {
        using var rrect =
            PixUI.RRect.FromRectAndRadius(SKRect.FromLTWH(X, Y, Width, Height), BorderRadius.X, BorderRadius.Y);
        context.Canvas.DrawRRect(rrect, paint);
    }
}