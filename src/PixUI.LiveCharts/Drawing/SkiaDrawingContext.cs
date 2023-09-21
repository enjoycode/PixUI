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
using LiveChartsCore.Motion;
using LiveCharts.Painting;


namespace LiveCharts.Drawing;

/// <summary>
/// Defines a skia sharp drawing context.
/// </summary>
/// <seealso cref="DrawingContext" />
public sealed class SkiaDrawingContext : DrawingContext
{
    private readonly bool _clearOnBegingDraw;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkiaDrawingContext"/> class.
    /// </summary>
    /// <param name="motionCanvas">The motion canvas.</param>
    /// <param name="canvas">The canvas.</param>
    /// <param name="clearOnBegingDraw">Indicates whether the canvas is cleared on frame draw.</param>
    public SkiaDrawingContext(
        MotionCanvas<SkiaDrawingContext> motionCanvas,
        int width, int height,
        SKCanvas canvas,
        bool clearOnBegingDraw = true)
    {
        MotionCanvas = motionCanvas;
        Width = width;
        Height = height;
        Canvas = canvas;
        PaintTask = null!;
        Paint = null!;
        _clearOnBegingDraw = clearOnBegingDraw;
    }

    /// <summary>
    /// Gets or sets the motion canvas.
    /// </summary>
    /// <value>
    /// The motion canvas.
    /// </value>
    public MotionCanvas<SkiaDrawingContext> MotionCanvas { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    // /// <summary>
    // /// Gets or sets the surface.
    // /// </summary>
    // /// <value>
    // /// The surface.
    // /// </value>
    // public SKSurface Surface { get; set; }

    /// <summary>
    /// Gets or sets the canvas.
    /// </summary>
    /// <value>
    /// The canvas.
    /// </value>
    public SKCanvas Canvas { get; set; }

    /// <summary>
    /// Gets or sets the paint task.
    /// </summary>
    /// <value>
    /// The paint task.
    /// </value>
    public Paint PaintTask { get; set; }

    /// <summary>
    /// Gets or sets the paint.
    /// </summary>
    /// <value>
    /// The paint.
    /// </value>
    public SKPaint Paint { get; set; }

    /// <summary>
    /// Gets or sets the background.
    /// </summary>
    public SKColor Background { get; set; } = SKColor.Empty;

    /// <inheritdoc cref="DrawingContext.OnBegingDraw"/>
    public override void OnBeginDraw()
    {
        //if (_clearOnBegingDraw) Canvas.Clear();
        if (Background != SKColor.Empty)
        {
            Canvas.DrawRect(SKRect.FromLTWH(0, 0, Width, Height), PixUI.PaintUtils.Shared(Background));
        }
    }
}