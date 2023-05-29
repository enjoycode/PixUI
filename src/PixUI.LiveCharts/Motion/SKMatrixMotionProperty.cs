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

using LiveChartsCore.Motion;


namespace LiveCharts.Motion;

/// <summary>
/// Defines a motion property to handle the <see cref="SKMatrix"/> type.
/// </summary>
public class SKMatrixMotionProperty : MotionProperty<SKMatrix>
{
    // /// <summary>
    // /// Initializes a new instance of the <see cref="SKMatrixMotionProperty"/> class.
    // /// </summary>
    // /// <param name="propertyName">The property name.</param>
    // public SKMatrixMotionProperty(string propertyName)
    //     : base(propertyName) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SKMatrixMotionProperty"/> class.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="matrix">The initial matrix.</param>
    public SKMatrixMotionProperty(string propertyName, SKMatrix matrix)
        : base(propertyName)
    {
        fromValue = matrix;
        toValue = matrix;
    }

    /// <inheritdoc cref="MotionProperty{T}.OnGetMovement(float)"/>
    protected override SKMatrix OnGetMovement(float progress)
    {
        return new SKMatrix( //TODO: check
            fromValue.M0 + progress * (toValue.M0 - fromValue.M0),
            fromValue.M1 + progress * (toValue.M1 - fromValue.M1),
            fromValue.M2 + progress * (toValue.M2 - fromValue.M2),
            fromValue.M3 + progress * (toValue.M3 - fromValue.M3),
            
            fromValue.M4 + progress * (toValue.M4 - fromValue.M4),
            fromValue.M5 + progress * (toValue.M5 - fromValue.M5),
            fromValue.M6 + progress * (toValue.M6 - fromValue.M6),
            fromValue.M7 + progress * (toValue.M7 - fromValue.M7),
            
            fromValue.M8 + progress * (toValue.M8 - fromValue.M8),
            fromValue.M9 + progress * (toValue.M9 - fromValue.M9),
            fromValue.M10 + progress * (toValue.M10 - fromValue.M10),
            fromValue.M11 + progress * (toValue.M11 - fromValue.M11),
            
            fromValue.M12 + progress * (toValue.M12 - fromValue.M12),
            fromValue.M13 + progress * (toValue.M13 - fromValue.M13),
            fromValue.M14 + progress * (toValue.M14 - fromValue.M14),
            fromValue.M15 + progress * (toValue.M15 - fromValue.M15)
            // fromValue.ScaleX + progress * (toValue.ScaleX - fromValue.ScaleX),
            // fromValue.SkewX + progress * (toValue.SkewX - fromValue.SkewX),
            // fromValue.TransX + progress * (toValue.TransX - fromValue.TransX),
            // fromValue.SkewY + progress * (toValue.SkewY - fromValue.SkewY),
            // fromValue.ScaleY + progress * (toValue.ScaleY - fromValue.ScaleY),
            // fromValue.TransY + progress * (toValue.TransY - fromValue.TransY),
            // fromValue.Persp0 + progress * (toValue.Persp0 - fromValue.Persp0),
            // fromValue.Persp1 + progress * (toValue.Persp1 - fromValue.Persp1),
            // fromValue.Persp2 + progress * (toValue.Persp2 - fromValue.Persp2)
        );
    }
}