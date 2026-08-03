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

namespace PixUI.LiveCharts.Painting;

/// <summary>
/// Creates a stroke dash effect.
/// </summary>
/// <seealso cref="PathEffect" />
/// <remarks>
/// Initializes a new instance of the <see cref="DashEffect"/> class.
/// </remarks>
public class DashEffect(float[] dashArray, float phase = 0)
    : PathEffect(s_key)
{
    internal static object s_key = new();

    private float[] DashArray { get; } = dashArray;
    private float Phase { get; } = phase;

    /// <inheritdoc cref="PathEffect.CreateNative()"/>
    public override SKPathEffect CreateNative() => PixUI.PathEffect.CreateDash(DashArray, Phase)!;

    /// <inheritdoc cref="PathEffect.Transitionate(float, PathEffect)"/>
    public override PathEffect? Transitionate(float progress, PathEffect? target)
    {
        if (target is not DashEffect dashEffect) return target;

        if (DashArray.Length != dashEffect.DashArray.Length)
            throw new Exception("The dash arrays must have the same length");

        var dashArray = new float[DashArray.Length];
        for (var i = 0; i < dashArray.Length; i++)
            dashArray[i] = DashArray[i] + (dashEffect.DashArray[i] - DashArray[i]) * progress;

        var phase = Phase + (dashEffect.Phase - Phase) * progress;

        return new DashEffect(dashArray, phase);
    }
}
