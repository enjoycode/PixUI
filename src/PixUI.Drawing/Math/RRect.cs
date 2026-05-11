using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PixUI;

public enum RoundRectType // must be int32
{
    Empty = 0,
    Rect = 1,
    Oval = 2,
    Simple = 3,
    NinePatch = 4,
    Complex = 5,
}

[StructLayout(LayoutKind.Sequential)]
public struct RRect
{
    #region ====Factory====

    public static RRect FromRectAndCorner(Rect rect,
        Radius? topLeft = null, Radius? topRight = null,
        Radius? bottomLeft = null, Radius? bottomRight = null)
    {
        var result = new RRect();
        Span<Radius> radius = stackalloc Radius[4];
        radius[0] = topLeft ?? Radius.Empty;
        radius[1] = topRight ?? Radius.Empty;
        radius[2] = bottomRight ?? Radius.Empty;
        radius[3] = bottomLeft ?? Radius.Empty;

        result.SetRectRadii(ref rect, radius);

        return result;
    }

    public static RRect FromRectAndRadius(Rect rect, float radiusX, float radiusY)
    {
        var result = new RRect();
        result.SetRectXY(ref rect, radiusX, radiusY);
        return result;
    }

    #endregion

    #region ====MemoryLayout====

    private Rect _rect;
    private CornerRadius _radius;
    private RoundRectType _type = RoundRectType.Empty;

    public RRect()
    {
        _rect = default;
        _radius = default;
    }

    private const int UpperLeftCorner = 0;
    private const int UpperRightCorner = 1;
    private const int LowerRightCorner = 2;
    private const int LowerLeftCorner = 3;

    [InlineArray(4)]
    private struct CornerRadius
    {
        private Radius _radius;
    }

    public bool IsEmpty() => _type == RoundRectType.Empty;

    /// <summary>
    /// Initializes fRect. If the passed in rect is not finite or empty the rrect will be fully
    /// </summary>
    /// <returns>initialized and false is returned. Otherwise, just fRect is initialized and true is returned.</returns>
    private bool InitializeRect(ref readonly Rect rect)
    {
        // Check this before sorting because sorting can hide nans.
        if (!rect.IsFinite())
        {
            this = new RRect();
            return false;
        }

        _rect = rect.Standardized;
        if (_rect.IsEmpty)
        {
            _radius[0] = _radius[1] = _radius[2] = _radius[3] = Radius.Empty;
            _type = RoundRectType.Empty;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Sets bounds to sorted rect, and sets corner radii to zero.
    /// If set bounds has width and height, and sets type to RoundRectType;
    /// otherwise, sets type to RoundRectType.Empty.
    /// </summary>
    private void SetRect(ref readonly Rect rect)
    {
        if (!InitializeRect(in rect)) return;

        _radius[0] = _radius[1] = _radius[2] = _radius[3] = Radius.Empty;
        _type = RoundRectType.Rect;

        Debug.Assert(IsValid());
    }

    private void SetRectXY(ref readonly Rect rect, float xRad, float yRad)
    {
        if (!InitializeRect(in rect))
            return;

        if (!(float.IsFinite(xRad) && float.IsFinite(yRad)))
            xRad = yRad = 0; // devolve into a simple rect

        if (_rect.Width < xRad + xRad || _rect.Height < yRad + yRad)
        {
            // At most one of these two divides will be by zero, and neither numerator is zero.
            var scale = Math.Min(_rect.Width / (xRad + xRad), _rect.Height / (yRad + yRad));
            Debug.Assert(scale < 1f);
            xRad *= scale;
            yRad *= scale;
        }

        if (xRad <= 0 || yRad <= 0)
        {
            SetRect(rect); // all corners are square in this case
            return;
        }

        for (var i = 0; i < 4; ++i)
        {
            _radius[i] = Radius.Elliptical(xRad, yRad);
        }

        _type = RoundRectType.Simple;
        if (xRad >= _rect.HarfWidth && yRad >= _rect.HarfHeight)
        {
            _type = RoundRectType.Oval;
            // TODO: assert that all the x&y radii are already W/2 & H/2
        }

        Debug.Assert(IsValid());
    }

    private void SetRectRadii(ref readonly Rect rect, ReadOnlySpan<Radius> radii)
    {
        Debug.Assert(radii.Length == 4);

        if (!InitializeRect(in rect))
            return;

        if (!IsFinite(radii))
        {
            SetRect(in rect); // devolve into a simple rect
            return;
        }

        radii.CopyTo(_radius);

        if (ClampToZero(_radius))
        {
            SetRect(in rect);
            return;
        }

        ScaleRadii();

        if (!IsValid())
        {
            SetRect(in rect);
            return;
        }
    }

    private bool ScaleRadii()
    {
        // Proportionally scale down all radii to fit. Find the minimum ratio
        // of a side and the radii on that side (for all four sides) and use
        // that to scale down _all_ the radii. This algorithm is from the
        // W3 spec (http://www.w3.org/TR/css3-background/) section 5.5 - Overlapping
        // Curves:
        // "Let f = min(Li/Si), where i is one of { top, right, bottom, left },
        //   Si is the sum of the two corresponding radii of the corners on side i,
        //   and top = bottom = the width of the box,
        //   and left = right = the height of the box.
        // If f < 1, then all corner radii are reduced by multiplying them by f."
        double scale = 1.0;

        // The sides of the rectangle may be larger than a float.
        double width = (double)_rect.Right - (double)_rect.Left;
        double height = (double)_rect.Bottom - (double)_rect.Top;
        scale = ComputeMinScale(_radius[0].X, _radius[1].X, width, scale);
        scale = ComputeMinScale(_radius[1].Y, _radius[2].Y, height, scale);
        scale = ComputeMinScale(_radius[2].X, _radius[3].X, width, scale);
        scale = ComputeMinScale(_radius[3].Y, _radius[0].Y, height, scale);

        FlushToZero(ref _radius[0][0], ref _radius[1][0]);
        FlushToZero(ref _radius[1][1], ref _radius[2][1]);
        FlushToZero(ref _radius[2][0], ref _radius[3][0]);
        FlushToZero(ref _radius[3][1], ref _radius[0][1]);

        if (scale < 1.0)
        {
            AdjustRadii(width, scale, ref _radius[0][0], ref _radius[1][0]);
            AdjustRadii(height, scale, ref _radius[1][1], ref _radius[2][1]);
            AdjustRadii(width, scale, ref _radius[2][0], ref _radius[3][0]);
            AdjustRadii(height, scale, ref _radius[3][1], ref _radius[0][1]);
        }

        // adjust radii may set x or y to zero; set companion to zero as well
        ClampToZero(_radius);

        // May be simple, oval, or complex, or become a rect/empty if the radii adjustment made them 0
        ComputeType();

        // TODO:  Why can't we assert this here?
        //SkASSERT(this->isValid());

        return scale < 1.0;
    }

    private void ComputeType()
    {
        if (_rect.IsEmpty)
        {
            Debug.Assert(_rect.IsStandardized());
            for (var i = 0; i < 4; ++i)
            {
                Debug.Assert(_radius[i] == Radius.Empty);
            }

            _type = RoundRectType.Empty;
            Debug.Assert(IsValid());
            return;
        }

        bool allRadiiEqual = true; // are all x radii equal and all y radii?
        bool allCornersSquare = 0 == _radius[0].X || 0 == _radius[0].Y;

        for (var i = 1; i < 4; ++i)
        {
            if (0 != _radius[i].X && 0 != _radius[i].Y)
            {
                // if either radius is zero the corner is square so both have to
                // be non-zero to have a rounded corner
                allCornersSquare = false;
            }

            if (_radius[i].X != _radius[i - 1].X || _radius[i].Y != _radius[i - 1].Y)
            {
                allRadiiEqual = false;
            }
        }

        if (allCornersSquare)
        {
            _type = RoundRectType.Rect;
            Debug.Assert(IsValid());
            return;
        }

        if (allRadiiEqual)
        {
            if (_radius[0].X >= _rect.HarfWidth && _radius[0].Y >= _rect.HarfHeight)
                _type = RoundRectType.Oval;
            else
                _type = RoundRectType.Simple;

            Debug.Assert(IsValid());
            return;
        }

        _type = RadiiAreNinePatch(_radius) ? RoundRectType.NinePatch : RoundRectType.Complex;

        if (!IsValid())
        {
            SetRect(_rect);
            Debug.Assert(IsValid());
        }
    }

    private bool IsValid()
    {
        if (!AreRectAndRadiiValid(ref _rect, _radius))
        {
            return false;
        }

        var allRadiiZero = (0 == _radius[0].X && 0 == _radius[0].Y);
        var allCornersSquare = (0 == _radius[0].X || 0 == _radius[0].Y);
        var allRadiiSame = true;

        for (var i = 1; i < 4; ++i)
        {
            if (0 != _radius[i].X || 0 != _radius[i].Y)
            {
                allRadiiZero = false;
            }

            if (_radius[i].X != _radius[i - 1].X || _radius[i].Y != _radius[i - 1].Y)
            {
                allRadiiSame = false;
            }

            if (0 != _radius[i].X && 0 != _radius[i].Y)
            {
                allCornersSquare = false;
            }
        }

        var patchesOfNine = RadiiAreNinePatch(_radius);

        switch (_type)
        {
            case RoundRectType.Empty:
                if (!_rect.IsEmpty || !allRadiiZero || !allRadiiSame || !allCornersSquare)
                    return false;

                break;
            case RoundRectType.Rect:
                if (_rect.IsEmpty || !allRadiiZero || !allRadiiSame || !allCornersSquare)
                    return false;

                break;
            case RoundRectType.Oval:
                if (_rect.IsEmpty || allRadiiZero || !allRadiiSame || allCornersSquare)
                    return false;

                for (var i = 0; i < 4; ++i)
                {
                    if (!_radius[i].X.NearlyEqual(_rect.HarfWidth) || !_radius[i].Y.NearlyEqual(_rect.HarfHeight))
                        return false;
                }

                break;
            case RoundRectType.Simple:
                if (_rect.IsEmpty || allRadiiZero || !allRadiiSame || allCornersSquare)
                    return false;
                break;
            case RoundRectType.NinePatch:
                if (_rect.IsEmpty || allRadiiZero || allRadiiSame || allCornersSquare || !patchesOfNine)
                    return false;
                break;
            case RoundRectType.Complex:
                if (_rect.IsEmpty || allRadiiZero || allRadiiSame || allCornersSquare || patchesOfNine)
                    return false;
                break;
        }

        return true;
    }

    private static bool IsFinite(ReadOnlySpan<Radius> radius)
    {
        for (var i = 0; i < radius.Length; i++)
        {
            if (!(float.IsFinite(radius[i].X) && float.IsFinite(radius[i].Y)))
                return false;
        }

        return true;
    }

    private static bool AreRadiusCheckPredicatesValid(float rad, float min, float max) =>
        min <= max && rad <= max - min && min + rad <= max && max - rad >= min && rad >= 0;

    private static bool AreRectAndRadiiValid(ref Rect rect, ReadOnlySpan<Radius> radius)
    {
        if (!rect.IsFinite() || !rect.IsStandardized())
            return false;

        for (var i = 0; i < radius.Length; i++)
        {
            if (!AreRadiusCheckPredicatesValid(radius[i].X, rect.Left, rect.Right) ||
                !AreRadiusCheckPredicatesValid(radius[i].Y, rect.Top, rect.Bottom))
            {
                return false;
            }
        }

        return true;
    }

    private static bool RadiiAreNinePatch(ReadOnlySpan<Radius> radius)
    {
        return radius[UpperLeftCorner].X == radius[LowerLeftCorner].X &&
               radius[UpperLeftCorner].Y == radius[UpperRightCorner].Y &&
               radius[UpperRightCorner].X == radius[LowerRightCorner].X &&
               radius[LowerLeftCorner].Y == radius[LowerRightCorner].Y;
    }

    private static bool ClampToZero(Span<Radius> radius)
    {
        var allCornersSquare = true;

        // Clamp negative radii to zero
        for (var i = 0; i < 4; ++i)
        {
            if (radius[i].X <= 0 || radius[i].Y <= 0)
            {
                // In this case we are being a little fast & loose. Since one of
                // the radii is 0 the corner is square. However, the other radii
                // could still be non-zero and play in the global scale factor
                // computation.
                radius[i] = Radius.Empty;
                // radius[i].X = 0;
                // radius[i].Y = 0;
            }
            else
            {
                allCornersSquare = false;
            }
        }

        return allCornersSquare;
    }

    /// <summary>
    /// These parameters intentionally double. if one of the
    /// radii is huge while the other is small, single precision math can completely
    /// miss the fact that a scale is required.
    /// </summary>
    private static double ComputeMinScale(double rad1, double rad2, double limit, double curMin)
    {
        return (rad1 + rad2) > limit ? Math.Min(curMin, limit / (rad1 + rad2)) : curMin;
    }

    /// <summary>
    /// If we can't distinguish one of the radii relative to the other, force it to zero so it
    /// doesn't confuse us later.
    /// </summary>
    private static void FlushToZero(ref float a, ref float b)
    {
        Debug.Assert(a >= 0 && b >= 0);
        if (a + b == a) b = 0;
        else if (a + b == b) a = 0;
    }

    private static void AdjustRadii(double limit, double scale, ref float a, ref float b)
    {
        Debug.Assert(scale < 1.0 && scale > 0.0);

        a = (float)(a * scale);
        b = (float)(b * scale);

        if (a + b > limit)
        {
            ref float minRadius = ref a;
            ref float maxRadius = ref b;

            // Force minRadius to be the smaller of the two.
            if (minRadius > maxRadius)
            {
                minRadius = ref b;
                maxRadius = ref a;
            }

            // newMinRadius must be float in order to give the actual value of the radius.
            // The newMinRadius will always be smaller than limit. The largest that minRadius can be
            // is 1/2 the ratio of minRadius : (minRadius + maxRadius), therefore in the resulting
            // division, minRadius can be no larger than 1/2 limit + ULP.
            float newMinRadius = minRadius;
            float newMaxRadius = (float)(limit - newMinRadius);

            // Reduce newMaxRadius an ulp at a time until it fits. This usually never happens,
            // but if it does it could be 1 or 2 times. In certain pathological cases it could be
            // more. Max iterations seen so far is 17.
            while (newMaxRadius + newMinRadius > limit)
            {
                newMaxRadius = FloatUtils.NextAfter(newMaxRadius, 0.0f);
            }

            maxRadius = newMaxRadius;
        }

        Debug.Assert(a >= 0.0f && b >= 0.0f, $"a: {a}, b: {b}, limit: {limit}, scale: {scale}");
        Debug.Assert(a + b <= limit, $"\nlimit: {limit}, sum: {a + b}, a: {a}, b: {b}, scale: {scale}");
    }

    #endregion

    // public float Width => SkiaApi.sk_rrect_get_width(Handle);

    // public float Height => SkiaApi.sk_rrect_get_height(Handle);

    public bool AllCornersCircular => CheckAllCornersCircular(FloatUtils.NearlyZero);

    public bool CheckAllCornersCircular(float tolerance)
    {
        var ul = _radius[UpperLeftCorner];
        var ur = _radius[UpperRightCorner];
        var lr = _radius[LowerRightCorner];
        var ll = _radius[LowerLeftCorner];

        return ul.X.NearlyEqual(ul.Y, tolerance) &&
               ur.X.NearlyEqual(ur.Y, tolerance) &&
               lr.X.NearlyEqual(lr.Y, tolerance) &&
               ll.X.NearlyEqual(ll.Y, tolerance);
    }

    // public void SetOval(Rect rect)
    // {
    //     SkiaApi.sk_rrect_set_oval(Handle, &rect);
    // }
    //
    // public void SetNinePatch(Rect rect, float leftRadius, float topRadius, float rightRadius,
    //     float bottomRadius)
    // {
    //     SkiaApi.sk_rrect_set_nine_patch(Handle, &rect, leftRadius, topRadius, rightRadius,
    //         bottomRadius);
    // }

    // public bool Contains(Rect rect)
    // {
    //     return SkiaApi.sk_rrect_contains(Handle, &rect);
    // }

    public void Deflate(float dx, float dy) => this = Deflate(this, dx, dy);

    public static RRect Deflate(in RRect src, float dx, float dy)
    {
        var dst = new RRect(); //Empty
        var r = Rect.Deflate(src._rect, dx, dy);
        bool degenerate = false;
        if (r.Right <= r.Left)
        {
            degenerate = true;
            r.Left = r.Right = FloatUtils.Ave(r.Left, r.Right);
        }

        if (r.Bottom <= r.Top)
        {
            degenerate = true;
            r.Top = r.Bottom = FloatUtils.Ave(r.Top, r.Bottom);
        }

        if (degenerate)
            return dst;
        if (!r.IsFinite())
            return dst;

        Span<Radius> dstRadii = stackalloc Radius[4];
        ReadOnlySpan<Radius> srcRadii = src._radius;
        srcRadii.CopyTo(dstRadii);
        for (var i = 0; i < 4; i++)
        {
            if (dstRadii[i].X != 0.0f)
                dstRadii[i][0] -= dx;
            if (dstRadii[i].Y != 0.0f)
                dstRadii[i][1] -= dy;
        }

        dst.SetRectRadii(ref r, dstRadii);
        return dst;
    }

    public void Inflate(float dx, float dy) => this = Deflate(this, -dx, -dy);

    public static RRect Inflate(in RRect src, float dx, float dy) => Deflate(src, -dx, -dy);

    public void Shift(float dx, float dy) => _rect.Offset(dx, dy);
}