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

using System.Collections.Generic;

namespace PixUI.LiveCharts.Painting;

/// <summary>
/// A wrapper object for skia sharp path effects.
/// </summary>
public abstract class PathEffect(object key)
{
    private readonly object _key = key;
    private static readonly Dictionary<object, PathEffect> s_defaultEffects = new()
    {
        { DashEffect.s_key, new DashEffect([1, 0], 0) }
    };

    /// <summary>
    /// Builds the native Skia path effect from this effect's parameters. The returned object is owned
    /// by the caller (the <see cref="SkiaPaint"/>), which caches and disposes it — the effect itself
    /// holds no native resource, so it is a lightweight, shareable value.
    /// </summary>
    /// <returns>A new native path effect.</returns>
    public abstract SKPathEffect CreateNative();

    /// <summary>
    /// Returns the effect interpolated toward <paramref name="target"/> at the given progress.
    /// </summary>
    /// <param name="progress">The progress.</param>
    /// <param name="target">The end target.</param>
    /// <returns>A new interpolated effect.</returns>
    public abstract PathEffect? Transitionate(float progress, PathEffect? target);

    /// <summary>
    /// Adds a default filter.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="effect">The effect.</param>
    public static void AddDefaultEffect(byte key, PathEffect effect) =>
        s_defaultEffects[key] = effect;

    internal static PathEffect? Transitionate(PathEffect? from, PathEffect? to, float progress)
    {
        if (from is null && to is null) return null;

        var key = (from ?? to)!._key;

        // Transition a null endpoint to/from the effect's registered no-op default (e.g. a dash
        // fades to "no dash"). An effect without a registered default — e.g. a custom self-animating
        // effect — falls back to the present endpoint instead of throwing.
        from ??= s_defaultEffects.TryGetValue(key, out var dFrom) ? dFrom : to;
        to ??= s_defaultEffects.TryGetValue(key, out var dTo) ? dTo : from;

        return from!.Transitionate(progress, to);
    }
}
