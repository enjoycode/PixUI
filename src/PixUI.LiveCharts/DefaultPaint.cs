#if !__WEB__
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
using System.Collections.Generic;
using System.Linq;
using LiveChartsCore.Drawing;
using LiveChartsCore.Motion;
using LiveCharts.Drawing;

namespace LiveCharts;

/// <summary>
/// Defines a default paint task.
/// </summary>
/// <seealso cref="IPaint{TDrawingContext}" />
[Obsolete("This is an obsolete class to make maps work, thgey are out-dated and will behave differently in the future.")]
public class DefaultPaint : IPaint<SkiaDrawingContext>
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance is stroke.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is stroke; otherwise, <c>false</c>.
    /// </value>
    public bool IsStroke { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is fill.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is fill; otherwise, <c>false</c>.
    /// </value>
    public bool IsFill { get; set; }

    /// <summary>
    /// Gets or sets the index of the z.
    /// </summary>
    /// <value>
    /// The index of the z.
    /// </value>
    public double ZIndex { get; set; }

    /// <summary>
    /// Gets or sets the stroke thickness.
    /// </summary>
    /// <value>
    /// The stroke thickness.
    /// </value>
    public float StrokeThickness { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is completed.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is completed; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the current time.
    /// </summary>
    /// <value>
    /// The current time.
    /// </value>
    public long CurrentTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is removed when all the transitions complete.
    /// </summary>
    /// <value>
    ///   <c>true</c> if remove on completed; otherwise, <c>false</c>.
    /// </value>
    public bool RemoveOnCompleted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is paused.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is paused; otherwise, <c>false</c>.
    /// </value>
    public bool IsPaused { get; set; }

    /// <summary>
    /// Gets or sets the font family.
    /// </summary>
    public string? FontFamily { get; set; }

    /// <inheritdoc cref="IAnimatable.MotionProperties"/>
    public Dictionary<string, IMotionProperty> MotionProperties => throw new NotImplementedException();

    /// <summary>
    /// Adds the geometry to paint task.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    /// <param name="geometry">The geometry.</param>
    public void AddGeometryToPaintTask(MotionCanvas<SkiaDrawingContext> canvas, IDrawable<SkiaDrawingContext> geometry) { }

    /// <summary>
    /// Clones the task.
    /// </summary>
    /// <returns></returns>
    public IPaint<SkiaDrawingContext> CloneTask()
    {
        return this;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() { }

    /// <summary>
    /// Gets the geometries.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    /// <returns></returns>
    public IEnumerable<IDrawable<SkiaDrawingContext>> GetGeometries(MotionCanvas<SkiaDrawingContext> canvas)
    {
        return Enumerable.Empty<IDrawable<SkiaDrawingContext>>();
    }

    /// <summary>
    /// Initializes the task.
    /// </summary>
    /// <param name="context">The context.</param>
    public void InitializeTask(SkiaDrawingContext context) { }

    /// <summary>
    /// Removes the geometry from pain task.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    /// <param name="geometry">The geometry.</param>
    public void RemoveGeometryFromPainTask(MotionCanvas<SkiaDrawingContext> canvas, IDrawable<SkiaDrawingContext> geometry) { }

    /// <summary>
    /// Removes all geometry from paint task.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    public void ClearGeometriesFromPaintTask(MotionCanvas<SkiaDrawingContext> canvas) { }

    /// <summary>
    /// Sets the geometries.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    /// <param name="geometries">The geometries.</param>
    public void SetGeometries(MotionCanvas<SkiaDrawingContext> canvas, HashSet<IDrawable<SkiaDrawingContext>> geometries) { }

    /// <inheritdoc cref="IAnimatable.SetTransition(Animation?, string[])"/>
    public void SetTransition(Animation? animation, params string[]? propertyName) { }

    /// <inheritdoc cref="IAnimatable.RemoveTransition(string[])"/>
    public void RemoveTransition(params string[]? propertyName) { }

    /// <inheritdoc cref="IAnimatable.CompleteTransition(string[])"/>
    public void CompleteTransition(params string[]? propertyName) { }

    /// <inheritdoc cref="IPaint{TDrawingContext}.ApplyOpacityMask(TDrawingContext, IPaintable{TDrawingContext})" />
    public void ApplyOpacityMask(SkiaDrawingContext context, IPaintable<SkiaDrawingContext> geometry) { }

    /// <inheritdoc cref="IPaint{TDrawingContext}.RestoreOpacityMask(TDrawingContext, IPaintable{TDrawingContext})" />
    public void RestoreOpacityMask(SkiaDrawingContext context, IPaintable<SkiaDrawingContext> geometry) { }

    /// <inheritdoc cref="IPaint{TDrawingContext}.GetClipRectangle(MotionCanvas{TDrawingContext})" />
    public LvcRectangle GetClipRectangle(MotionCanvas<SkiaDrawingContext> canvas)
    {
        return LvcRectangle.Empty;
    }

    /// <inheritdoc cref="IPaint{TDrawingContext}.SetClipRectangle(MotionCanvas{TDrawingContext}, LvcRectangle)" />
    public void SetClipRectangle(MotionCanvas<SkiaDrawingContext> canvas, LvcRectangle value) { }

    /// <inheritdoc cref="IPaint{TDrawingContext}.ReleaseCanvas(MotionCanvas{TDrawingContext})" />
    public void ReleaseCanvas(MotionCanvas<SkiaDrawingContext> canvas) { }
}
#endif
