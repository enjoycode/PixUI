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

using System;
using LiveChartsCore.Drawing;
using LiveChartsCore.Generators;
using PixUI.LiveCharts.Drawing;

namespace PixUI.LiveCharts.Painting;

/// <summary>
/// Defines a set of geometries that will be painted using a radial gradient shader.
/// </summary>
/// <seealso cref="SkiaPaint" />
public partial class RadialGradientPaint : SkiaPaint
{
    private readonly SKShaderTileMode _tileMode;
    private SKShader? _shader;
    internal IColorFilter? _opacityFilter;
    internal float _opacityFilterAlpha = -1f;
    private SKRect _activeClip = new();

    // Inputs used to build the cached shader; see LinearGradientPaint for the caching rationale.
    private SKColor[]? _builtStops;
    private float[]? _builtColorPos;
    private SKPoint _builtCenter;
    private float _builtRadius = -1f;
    private SKRect _builtClip;

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
    /// The shader tile mode, default is <see cref="SKShaderTileMode.Clamp"/>.
    /// </param>
    public RadialGradientPaint(
        SKColor[] gradientStops,
        SKPoint? center = null,
        float radius = 0.5f,
        float[]? colorPos = null,
        SKShaderTileMode tileMode = SKShaderTileMode.Clamp)
    {
        _GradientStopsMotionProperty = new(gradientStops);
        _CenterMotionProperty = new(center ?? new SKPoint(0.5f, 0.5f));
        _RadiusMotionProperty = new(radius);
        _ColorPosMotionProperty = new(colorPos);
        _tileMode = tileMode;
    }

    /// <summary>
    /// Gets or sets the gradient stops.
    /// </summary>
    [MotionProperty]
    public partial SKColor[] GradientStops { get; set; }

    /// <summary>
    /// Gets or sets the center point of the gradient, both X and Y in the range of 0 to 1.
    /// </summary>
    [MotionProperty]
    public partial SKPoint Center { get; set; }

    /// <summary>
    /// Gets or sets the radius, in the range of 0 to 1, where 1 is the minimum of the chart Width and Height.
    /// </summary>
    [MotionProperty]
    public partial float Radius { get; set; }

    /// <summary>
    /// Gets or sets the relative positions of the colors, in the range of 0 to 1, or null to space them equally.
    /// </summary>
    [MotionProperty]
    public partial float[]? ColorPos { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RadialGradientPaint"/> class.
    /// </summary>
    /// <param name="centerColor">Color of the center.</param>
    /// <param name="outerColor">Color of the outer.</param>
    public RadialGradientPaint(SKColor centerColor, SKColor outerColor)
        : this([centerColor, outerColor]) { }

    /// <inheritdoc cref="Paint.CloneTask" />
    public override LiveChartsCore.Painting.Paint CloneTask()
    {
        var clone = new RadialGradientPaint(GradientStops, Center, Radius, ColorPos, _tileMode);
        Map(this, clone);

        return clone;
    }

    internal override void OnPaintStarted(DrawingContext drawingContext, IDrawnElement? drawnElement)
    {
        var skiaContext = (SkiaSharpDrawingContext)drawingContext;
        _skiaPaint = UpdateSkiaPaint(skiaContext, drawnElement);

        var bounds = skiaContext.Canvas.ClipBounds; //skiaContext.Canvas.LocalClipBounds;
        _activeClip = new SKRect(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);

        _skiaPaint.Shader = GetShader();
    }

    internal override void ApplyOpacityMask(DrawingContext context, float opacity, IDrawnElement? drawnElement)
    {
        if (_skiaPaint is null || opacity > 0.99) return;

        // See LinearGradientPaint.ApplyOpacityMask — same leak fix: previously a fresh
        // SKColorFilter was created every call and Restore dropped the reference without
        // disposing. Cache by opacity; for steady-state opacity the native filter is
        // created once and reused.
        if (_opacityFilter is null || _opacityFilterAlpha != opacity)
        {
            _opacityFilter?.Dispose();
            _opacityFilter = ColorFilter.CreateBlendMode(
                new SKColor(255, 255, 255, (byte)(255 * opacity)),
                BlendMode.DstIn);
            _opacityFilterAlpha = opacity;
        }

        _skiaPaint.ColorFilter = _opacityFilter;
    }

    internal override void RestoreOpacityMask(DrawingContext context, float opacity, IDrawnElement? drawnElement)
    {
        if (_skiaPaint is null) return;

        _skiaPaint.ColorFilter = null;
    }

    internal override void DisposeTask()
    {
        base.DisposeTask();

        _shader?.Dispose();
        _shader = null;

        _opacityFilter?.Dispose();
        _opacityFilter = null;
        _opacityFilterAlpha = -1f;
    }

    private SKShader GetShader()
    {
        // Read the (possibly interpolated) values once; the getters advance any active transition.
        var stops = GradientStops;
        var colorPos = ColorPos;
        var centerPos = Center;
        var radius = Radius;

        if (_shader is not null &&
            ReferenceEquals(stops, _builtStops) &&
            ReferenceEquals(colorPos, _builtColorPos) &&
            centerPos == _builtCenter &&
            Math.Abs(radius - _builtRadius) < 1e-6f &&
            _activeClip == _builtClip)
            return _shader;

        _builtStops = stops;
        _builtColorPos = colorPos;
        _builtCenter = centerPos;
        _builtRadius = radius;
        _builtClip = _activeClip;

        var center = new SKPoint(_activeClip.Location.X + centerPos.X * _activeClip.Width,
            _activeClip.Location.Y + centerPos.Y * _activeClip.Height);
        var r = Math.Min(_activeClip.Width, _activeClip.Height) * radius;

        _shader?.Dispose();

        return _shader = Shader.CreateRadialGradient(center, r, stops, colorPos, _tileMode)!;
    }
}