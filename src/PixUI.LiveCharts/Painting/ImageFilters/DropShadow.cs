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

namespace PixUI.LiveCharts.Painting;

/// <summary>
/// Creates a drop shadow image filter.
/// </summary>
/// <seealso cref="ImageFilter" />
/// <remarks>
/// Initializes a new instance of the <see cref="DropShadow"/> class.
/// </remarks>
/// <param name="dx">The dx.</param>
/// <param name="dy">The dy.</param>
/// <param name="sigmaX">The sigma x.</param>
/// <param name="sigmaY">The sigma y.</param>
/// <param name="color">The color.</param>
public class DropShadow(float dx, float dy, float sigmaX, float sigmaY, SKColor color) : ImageFilter(s_key)
{
    internal static object s_key = new();

    // internal so the drawing context can floor a per-element shadow at the paint's own shadow.
    internal float Dx { get; } = dx;
    internal float Dy { get; } = dy;
    internal float SigmaX { get; } = sigmaX;
    internal float SigmaY { get; } = sigmaY;
    internal SKColor Color { get; } = color;

    /// <inheritdoc cref="ImageFilter.CreateNative()"/>
    public override SKImageFilter CreateNative() =>
        PixUI.ImageFilter.CreateDropShadow(Dx, Dy, SigmaX, SigmaY, Color, null)!;

    /// <inheritdoc cref="ImageFilter.Transitionate(float, ImageFilter)"/>
    protected override ImageFilter Transitionate(float progress, ImageFilter target)
    {
        var dropShadow = (DropShadow)target;

        return new DropShadow(
            Dx + (dropShadow.Dx - Dx) * progress,
            Dy + (dropShadow.Dy - Dy) * progress,
            SigmaX + (dropShadow.SigmaX - SigmaX) * progress,
            SigmaY + (dropShadow.SigmaY - SigmaY) * progress,
            new SKColor(
                (byte)(Color.Red + (dropShadow.Color.Red - Color.Red) * progress),
                (byte)(Color.Green + (dropShadow.Color.Green - Color.Green) * progress),
                (byte)(Color.Blue + (dropShadow.Color.Blue - Color.Blue) * progress),
                (byte)(Color.Alpha + (dropShadow.Color.Alpha - Color.Alpha) * progress)));
    }
}