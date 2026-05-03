using System.Diagnostics.CodeAnalysis;

namespace PixUI;

[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
public static class MatrixUtils
{

    /// <summary>
    /// Returns the given [transform] matrix as an [Offset], if the matrix is
    /// nothing but a 2D translation. Otherwise, returns null
    /// </summary>
    public static Offset? GetAsTranslation(in Matrix4 transform)
    {
        if (transform.M00 == 1 &&
            transform.M01 == 0 &&
            transform.M02 == 0 &&
            transform.M03 == 0 &&
            transform.M10 == 0 &&
            transform.M11 == 1 &&
            transform.M12 == 0 &&
            transform.M13 == 0 &&
            transform.M20 == 0 &&
            transform.M21 == 0 &&
            transform.M22 == 1 &&
            transform.M23 == 0 &&
            transform.M32 == 0 &&
            transform.M33 == 1)
        {
            return new Offset(transform.M30, transform.M31);
        }

        return null;
    }
        
    /// <summary>
    /// Applies the given matrix as a perspective transform to the given point.
    /// </summary>
    /// <remarks>
    /// This function assumes the given point has a z-coordinate of 0.0. The
    /// z-coordinate of the result is ignored.
    ///
    /// While not common, this method may return (NaN, NaN), iff the given `point`
    /// results in a "point at infinity" in homogeneous coordinates after applying
    /// the `transform`.
    /// </remarks>
    public static Offset TransformPoint(in Matrix4 transform, float x, float y)
    {
        // Directly simulate the transform of the vector (x, y, 0, 1),
        // dropping the resulting Z coordinate, and normalizing only if needed.
            
        var rx = transform.M00 * x + transform.M10 * y + transform.M30;
        var ry = transform.M01 * x + transform.M11 * y + transform.M31;
        var rw = transform.M03 * x + transform.M13 * y + transform.M33;

        return rw == 1.0 ? new Offset(rx, ry) : new Offset(rx / rw, ry / rw);
    }
    
    /// <summary>
    /// Removes the "perspective" component from `transform`.
    /// </summary>
    /// <remarks>
    /// When applying the resulting transform matrix to a point with a
    /// z-coordinate of zero (which is generally assumed for all points
    /// represented by an [Offset]), the other coordinates will get transformed as
    /// before, but the new z-coordinate is going to be zero again. This is
    /// achieved by setting the third column and third row of the matrix to "0, 0, 1, 0".
    /// </remarks>
    public static Matrix4 RemovePerspectiveTransform(Matrix4 transform)
    {
        var vector = new Vector4(0, 0, 1, 0);
        transform.SetColumn(2, vector);
        transform.SetRow(2, vector);
        return transform;
    }
}