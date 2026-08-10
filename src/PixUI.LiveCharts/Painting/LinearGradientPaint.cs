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

using LiveChartsCore.Drawing;
using LiveChartsCore.Generators;
using PixUI.LiveCharts.Drawing;

namespace PixUI.LiveCharts.Painting;

/// <summary>
/// Defines a set of geometries that will be painted using a linear gradient shader.
/// </summary>
/// <seealso cref="SkiaPaint" />
public partial class LinearGradientPaint : SkiaPaint
{
    private readonly SKShaderTileMode _tileMode;
    private SKShader? _shader;
    internal IColorFilter? _opacityFilter;
    internal float _opacityFilterAlpha = -1f;
    private SKRect _activeClip = new();

    // Inputs used to build the cached shader; the shader is rebuilt when any of these change.
    // While animating, the motion getters return a fresh interpolated array/point each frame
    // (so a rebuild happens every frame); once settled they return the stored values and the
    // cached shader is reused.
    private SKColor[]? _builtStops;
    private float[]? _builtColorPos;
    private SKPoint _builtStart;
    private SKPoint _builtEnd;
    private SKRect _builtClip;

    /// <summary>
    /// Default start point.
    /// </summary>
    public static readonly SKPoint DefaultStartPoint = new(0, 0.5f);

    /// <summary>
    /// Default end point.
    /// </summary>
    public static readonly SKPoint DefaultEndPoint = new(1, 0.5f);

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearGradientPaint"/> class.
    /// </summary>
    /// <param name="gradientStops">The gradient stops.</param>
    /// <param name="startPoint">
    /// The start point, both X and Y in the range of 0 to 1, where 0 is the start of the axis and 1 the end.
    /// </param>
    /// <param name="endPoint">
    /// The end point, both X and Y in the range of 0 to 1, where 0 is the start of the axis and 1 the end.
    /// </param>
    /// <param name="colorPos">
    /// An array of floats in the range of 0 to 1.
    /// These floats indicate the relative positions of the colors, you can set that argument to null to equally
    /// space the colors, default is null.
    /// </param>
    /// <param name="tileMode">
    /// The shader tile mode, default is <see cref="SKShaderTileMode.Clamp"/>.
    /// </param>
    public LinearGradientPaint(
        SKColor[] gradientStops,
        SKPoint startPoint,
        SKPoint endPoint,
        float[]? colorPos = null,
        SKShaderTileMode tileMode = SKShaderTileMode.Clamp)
    {
        _GradientStopsMotionProperty = new(gradientStops);
        _StartPointMotionProperty = new(startPoint);
        _EndPointMotionProperty = new(endPoint);
        _ColorPosMotionProperty = new(colorPos);
        _tileMode = tileMode;
    }

    /// <summary>
    /// Gets or sets the gradient stops.
    /// </summary>
    [MotionProperty]
    public partial SKColor[] GradientStops { get; set; }

    /// <summary>
    /// Gets or sets the start point, both X and Y in the range of 0 to 1.
    /// </summary>
    [MotionProperty]
    public partial SKPoint StartPoint { get; set; }

    /// <summary>
    /// Gets or sets the end point, both X and Y in the range of 0 to 1.
    /// </summary>
    [MotionProperty]
    public partial SKPoint EndPoint { get; set; }

    /// <summary>
    /// Gets or sets the relative positions of the colors, in the range of 0 to 1, or null to space them equally.
    /// </summary>
    [MotionProperty]
    public partial float[]? ColorPos { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearGradientPaint"/> class.
    /// </summary>
    /// <param name="gradientStops">The gradient stops.</param>
    public LinearGradientPaint(SKColor[] gradientStops)
        : this(gradientStops, DefaultStartPoint, DefaultEndPoint) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearGradientPaint"/> class.
    /// </summary>
    /// <param name="startColor">The start color.</param>
    /// <param name="endColor">The end color.</param>
    /// <param name="startPoint">
    /// The start point, both X and Y in the range of 0 to 1, where 0 is the start of the axis and 1 the end.
    /// </param>
    /// <param name="endPoint">
    /// The end point, both X and Y in the range of 0 to 1, where 0 is the start of the axis and 1 the end.
    /// </param>
    public LinearGradientPaint(SKColor startColor, SKColor endColor, SKPoint startPoint, SKPoint endPoint)
        : this([startColor, endColor], startPoint, endPoint) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearGradientPaint"/> class.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="end">The end.</param>
    public LinearGradientPaint(SKColor start, SKColor end)
        : this(start, end, DefaultStartPoint, DefaultEndPoint) { }

    /// <inheritdoc cref="Paint.CloneTask" />
    public override LiveChartsCore.Painting.Paint CloneTask()
    {
        var clone = new LinearGradientPaint(GradientStops, StartPoint, EndPoint, ColorPos, _tileMode);
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

        // Previously this allocated an SKColorFilter every call and `RestoreOpacityMask`
        // dropped the reference without disposing — the native handle leaked to GC
        // finalization. Cache by opacity value; for static or coarsely-quantized opacity
        // (most cases) the native filter is created once and reused.
        if (_opacityFilter is null || _opacityFilterAlpha != opacity)
        {
            _opacityFilter?.Dispose();
            _opacityFilter = PixUI.ColorFilter.CreateBlendMode(
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
        var startPoint = StartPoint;
        var endPoint = EndPoint;

        if (_shader is not null &&
            ReferenceEquals(stops, _builtStops) &&
            ReferenceEquals(colorPos, _builtColorPos) &&
            startPoint == _builtStart &&
            endPoint == _builtEnd &&
            _activeClip == _builtClip)
            return _shader;

        _builtStops = stops;
        _builtColorPos = colorPos;
        _builtStart = startPoint;
        _builtEnd = endPoint;
        _builtClip = _activeClip;

        var xf = _activeClip.Location.X;
        var xt = xf + _activeClip.Width;

        var yf = _activeClip.Location.Y;
        var yt = yf + _activeClip.Height;

        var start = new SKPoint(xf + (xt - xf) * startPoint.X, yf + (yt - yf) * startPoint.Y);
        var end = new SKPoint(xf + (xt - xf) * endPoint.X, yf + (yt - yf) * endPoint.Y);

        _shader?.Dispose();

        return
            _shader = Shader.CreateLinearGradient(start, end, stops, colorPos, _tileMode)!;
    }
}
