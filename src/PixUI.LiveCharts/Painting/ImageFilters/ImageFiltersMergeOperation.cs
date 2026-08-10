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
using System.Linq;

namespace PixUI.LiveCharts.Painting;

/// <summary>
/// Merges multiple image filters.
/// </summary>
/// <seealso cref="ImageFilter" />
/// <remarks>
/// Initializes a new instance of the <see cref="ImageFiltersMergeOperation"/> class.
/// </remarks>
/// <param name="imageFilters">The image filters.</param>
public class ImageFiltersMergeOperation(ImageFilter[] imageFilters) : ImageFilter(s_key)
{
    internal static object s_key = new();

    private ImageFilter[] Filters { get; } = imageFilters;

    /// <inheritdoc cref="ImageFilter.CreateNative()"/>
    public override SKImageFilter CreateNative()
    {
        throw new NotImplementedException("ImageFiltersMergeOperation.CreateNative() is not implemented");
        // var natives = new SKImageFilter[Filters.Length];
        // for (var i = 0; i < natives.Length; i++)
        //     natives[i] = Filters[i].CreateNative();
        //
        // var merged = PixUI.ImageFilter.CreateMerge(natives);
        //
        // // CreateMerge takes its own reference to each child, so the transient child handles can be
        // // released here; only the merged filter is returned, and the paint owns and disposes it.
        // foreach (var native in natives)
        //     native.Dispose();
        //
        // return merged;
    }

    /// <inheritdoc cref="ImageFilter.Transitionate(float, ImageFilter)"/>
    protected override ImageFilter Transitionate(float progress, ImageFilter target)
    {
        if (target is not ImageFiltersMergeOperation merge) return target;

        if (merge.Filters.Length != Filters.Length)
            throw new Exception("The image filters must have the same length");

        var filters = new ImageFilter[Filters.Length];

        var hasNull = false;
        for (var i = 0; i < Filters.Length; i++)
        {
            var transitionated = Transitionate(Filters[i], merge.Filters[i], progress);
            filters[i] = transitionated!; // ! ignored, will be filtered out later
            if (transitionated is null) hasNull = true;
        }

        return new ImageFiltersMergeOperation(
            hasNull ? [.. filters.Where(x => x is not null)] : filters);
    }
}