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
using LiveCharts.Drawing;
using PixUI;


namespace LiveCharts.Painting;

/// <summary>
/// Defines a set of geometries that will be painted using a radial gradient shader.
/// </summary>
/// <seealso cref="Paint" />
public class RadialGradientPaint : Paint
{
    private SkiaDrawingContext? _drawingContext;
    private readonly SKColor[] _gradientStops;
    private readonly SKPoint _center;
    private readonly float _radius;
    private readonly float[]? _colorPos;
    private readonly SKShaderTileMode _tileMode;

    /// <summary>
    /// Initializes a new instance of the <see cref="RadialGradientPaint"/> class.
    /// </summary>
    /// <param name="gradientStops">The gradient stops.</param>
    /// <param name="center">
    /// The center point of the gradient, both X and Y in the range of 0 to 1, where 0 is the start of the axis and 1 the end,
    /// default is (0.5, 0.5).
    /// </param>
    /// <param name="radius">
    /// The radius, in the range of 0 to 1, where 1 is the minimum of both Width and Height of the chart, default is 0.5.
    /// </param>
    /// <param name="colorPos">
    /// An array of integers in the range of 0 to 1.
    /// These integers indicate the relative positions of the colors, You can set that argument to null to equally
    /// space the colors, default is null.
    /// </param>
    /// <param name="tileMode">
    /// The shader tile mode, default is <see cref="SKShaderTileMode.Repeat"/>.
    /// </param>
    public RadialGradientPaint(
        SKColor[] gradientStops,
        SKPoint? center = null,
        float radius = 0.5f,
        float[]? colorPos = null,
        SKShaderTileMode tileMode = SKShaderTileMode.Repeat)
    {
        _gradientStops = gradientStops;
        if (center is null) _center = new SKPoint(0.5f, 0.5f);
        _radius = radius;
        _colorPos = colorPos;
        _tileMode = tileMode;
    }

    // /// <summary>
    // /// Initializes a new instance of the <see cref="RadialGradientPaint"/> class.
    // /// </summary>
    // /// <param name="centerColor">Color of the center.</param>
    // /// <param name="outerColor">Color of the outer.</param>
    // public RadialGradientPaint(SKColor centerColor, SKColor outerColor)
    //     : this(new[] { centerColor, outerColor }) { }

    /// <inheritdoc cref="IPaint{TDrawingContext}.CloneTask" />
    public override IPaint<SkiaDrawingContext> CloneTask()
    {
        return new RadialGradientPaint(_gradientStops, _center, _radius, _colorPos, _tileMode)
        {
            Style = Style,
            IsStroke = IsStroke,
            IsFill = IsFill,
            Color = Color,
            IsAntialias = IsAntialias,
            StrokeThickness = StrokeThickness,
            StrokeCap = StrokeCap,
            StrokeJoin = StrokeJoin,
            StrokeMiter = StrokeMiter,
            FontFamily = FontFamily,
            SKFontStyle = SKFontStyle,
            SKTypeface = SKTypeface,
            PathEffect = PathEffect?.Clone(),
            ImageFilter = ImageFilter?.Clone()
        };
    }

    /// <inheritdoc cref="IPaint{TDrawingContext}.InitializeTask(TDrawingContext)" />
    public override void InitializeTask(SkiaDrawingContext drawingContext)
    {
        _skiaPaint ??= new SKPaint();

        var size = GetDrawRectangleSize(drawingContext);
        var center = new SKPoint(size.Left + _center.X * size.Width, size.Top + _center.Y * size.Height);
        var r = size.Left + size.Width > size.Top + size.Height
            ? size.Top + size.Height
            : size.Left + size.Width;
        r *= _radius;

        _skiaPaint.Shader = SKShader.CreateRadialGradient(
                center,
                r,
                _gradientStops,
                _colorPos,
                _tileMode);

        _skiaPaint.AntiAlias = IsAntialias;
        _skiaPaint.Style = PaintStyle.Stroke;
        _skiaPaint.StrokeWidth = StrokeThickness;
        _skiaPaint.StrokeCap = StrokeCap;
        _skiaPaint.StrokeJoin = StrokeJoin;
        _skiaPaint.StrokeMiter = StrokeMiter;
        _skiaPaint.Style = IsStroke ? SKPaintStyle.Stroke : SKPaintStyle.Fill;

        //if (HasCustomFont) _skiaPaint.Typeface = GetSKTypeface();

        if (PathEffect is not null)
        {
            PathEffect.CreateEffect(drawingContext);
            _skiaPaint.PathEffect = PathEffect.SKPathEffect;
        }

        if (ImageFilter is not null)
        {
            ImageFilter.CreateFilter(drawingContext);
            _skiaPaint.ImageFilter = ImageFilter.SKImageFilter;
        }

        var clip = GetClipRectangle(drawingContext.MotionCanvas);
        if (clip != LvcRectangle.Empty)
        {
            drawingContext.Canvas.Save();
            drawingContext.Canvas.ClipRect(SKRect.FromLTWH(clip.X, clip.Y, clip.Width, clip.Height), ClipOp.Intersect, true);
            _drawingContext = drawingContext;
        }

        drawingContext.Paint = _skiaPaint;
        drawingContext.PaintTask = this;
    }

    /// <inheritdoc cref="IPaint{TDrawingContext}.RestoreOpacityMask(TDrawingContext, IPaintable{TDrawingContext})" />
    public override void RestoreOpacityMask(SkiaDrawingContext context, IPaintable<SkiaDrawingContext> geometry)
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc cref="IPaint{TDrawingContext}.ApplyOpacityMask(TDrawingContext, IPaintable{TDrawingContext})" />
    public override void ApplyOpacityMask(SkiaDrawingContext context, IPaintable<SkiaDrawingContext> geometry)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public override void Dispose()
    {
        // Note #301222
        // Disposing typefaces could cause render issues.
        // Does this causes memory leaks?
        // Should the user dispose typefaces manually?
        //if (HasCustomFont && _skiaPaint != null) _skiaPaint.Typeface.Dispose();
        PathEffect?.Dispose();
        ImageFilter?.Dispose();

        if (_drawingContext is not null && GetClipRectangle(_drawingContext.MotionCanvas) != LvcRectangle.Empty)
        {
            _drawingContext.Canvas.Restore();
            _drawingContext = null;
        }

        base.Dispose();
    }

    private SKRect GetDrawRectangleSize(SkiaDrawingContext drawingContext)
    {
        var clip = GetClipRectangle(drawingContext.MotionCanvas);

        return clip == LvcRectangle.Empty
            ? new SKRect(0, 0, drawingContext.Width, drawingContext.Width)
            : new SKRect(clip.X, clip.Y, clip.Width, clip.Height);
    }
}
