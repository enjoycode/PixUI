#if !__WEB__
using System;

namespace PixUI
{
    public unsafe class RRect : SKObject, ISKSkipObjectRegistration //TODO: change to struct
    {
        private static Point RadiusToPoint(Radius? radius)
            => radius == null ? Point.Empty : new Point(radius.Value.X, radius.Value.Y);

        public static RRect FromRectAndCorner(Rect rect,
            Radius? topLeft = null, Radius? topRight = null,
            Radius? bottomLeft = null, Radius? bottomRight = null)
        {
            var rrect = new RRect();
            fixed (Point* pts = stackalloc Point[4])
            {
                pts[0] = RadiusToPoint(topLeft);
                pts[1] = RadiusToPoint(topRight);
                pts[2] = RadiusToPoint(bottomRight);
                pts[3] = RadiusToPoint(bottomLeft);

                SkiaApi.sk_rrect_set_rect_radii(rrect.Handle, &rect, pts);
            }

            return rrect;
        }

        public static RRect FromRectAndRadius(Rect rect, float radiusX, float radiusY)
        {
            return new RRect(rect, radiusX, radiusY);
        }

        public static RRect FromCopy(RRect from)
            => new RRect(SkiaApi.sk_rrect_new_copy(from.Handle), true);

        private RRect(IntPtr handle, bool owns) : base(handle, owns) { }

        public RRect() : this(SkiaApi.sk_rrect_new(), true)
        {
            if (Handle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to create a new SKRoundRect instance.");
            }

            SetEmpty();
        }

        // public RRect(Rect rect) : this(SkiaApi.sk_rrect_new(), true)
        // {
        //     if (Handle == IntPtr.Zero)
        //         throw new InvalidOperationException("Unable to create a new SKRoundRect instance.");
        //     SetRect(rect);
        // }

        // public RRect(Rect rect, float radius) : this(rect, radius, radius) { }

        private RRect(Rect rect, float xRadius, float yRadius)
            : this(SkiaApi.sk_rrect_new(), true)
        {
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Unable to create a new SKRoundRect instance.");

            SetRect(rect, xRadius, yRadius);
        }

        protected override void DisposeNative() => SkiaApi.sk_rrect_delete(Handle);

        public Rect Rect
        {
            get
            {
                Rect rect;
                SkiaApi.sk_rrect_get_rect(Handle, &rect);
                return rect;
            }
        }

        public Point[] Radii => new[]
        {
            GetRadii(SKRoundRectCorner.UpperLeft),
            GetRadii(SKRoundRectCorner.UpperRight),
            GetRadii(SKRoundRectCorner.LowerRight),
            GetRadii(SKRoundRectCorner.LowerLeft),
        };

        public SKRoundRectType Type => SkiaApi.sk_rrect_get_type(Handle);

        public float Width => SkiaApi.sk_rrect_get_width(Handle);

        public float Height => SkiaApi.sk_rrect_get_height(Handle);

        public bool IsValid => SkiaApi.sk_rrect_is_valid(Handle);

        public bool AllCornersCircular => CheckAllCornersCircular(Utils.NearlyZero);

        public bool CheckAllCornersCircular(float tolerance)
        {
            var ul = GetRadii(SKRoundRectCorner.UpperLeft);
            var ur = GetRadii(SKRoundRectCorner.UpperRight);
            var lr = GetRadii(SKRoundRectCorner.LowerRight);
            var ll = GetRadii(SKRoundRectCorner.LowerLeft);

            return
                Utils.NearlyEqual(ul.X, ul.Y, tolerance) &&
                Utils.NearlyEqual(ur.X, ur.Y, tolerance) &&
                Utils.NearlyEqual(lr.X, lr.Y, tolerance) &&
                Utils.NearlyEqual(ll.X, ll.Y, tolerance);
        }

        public void SetEmpty()
        {
            SkiaApi.sk_rrect_set_empty(Handle);
        }

        public void SetRect(Rect rect)
        {
            SkiaApi.sk_rrect_set_rect(Handle, &rect);
        }

        public void SetRect(Rect rect, float xRadius, float yRadius)
        {
            SkiaApi.sk_rrect_set_rect_xy(Handle, &rect, xRadius, yRadius);
        }

        public void SetOval(Rect rect)
        {
            SkiaApi.sk_rrect_set_oval(Handle, &rect);
        }

        public void SetNinePatch(Rect rect, float leftRadius, float topRadius, float rightRadius,
            float bottomRadius)
        {
            SkiaApi.sk_rrect_set_nine_patch(Handle, &rect, leftRadius, topRadius, rightRadius,
                bottomRadius);
        }

        public void SetRectRadii(Rect rect, Point[] radii)
        {
            if (radii == null)
                throw new ArgumentNullException(nameof(radii));
            if (radii.Length != 4)
                throw new ArgumentException("Radii must have a length of 4.", nameof(radii));

            fixed (Point* r = radii)
            {
                SkiaApi.sk_rrect_set_rect_radii(Handle, &rect, r);
            }
        }

        public bool Contains(Rect rect)
        {
            return SkiaApi.sk_rrect_contains(Handle, &rect);
        }

        public Point GetRadii(SKRoundRectCorner corner)
        {
            Point radii;
            SkiaApi.sk_rrect_get_radii(Handle, corner, &radii);
            return radii;
        }

        public void Deflate(float dx, float dy)
        {
            SkiaApi.sk_rrect_inset(Handle, dx, dy);
        }

        public void Inflate(float dx, float dy)
        {
            SkiaApi.sk_rrect_outset(Handle, dx, dy);
        }

        public void Shift(float dx, float dy)
        {
            SkiaApi.sk_rrect_offset(Handle, dx, dy);
        }

        public bool TryTransform(Matrix3 matrix, out RRect? transformed)
        {
            var destHandle = SkiaApi.sk_rrect_new();
            if (SkiaApi.sk_rrect_transform(Handle, &matrix, destHandle))
            {
                transformed = new RRect(destHandle, true);
                return true;
            }

            SkiaApi.sk_rrect_delete(destHandle);
            transformed = null;
            return false;
        }

        public RRect? Transform(Matrix3 matrix)
        {
            return TryTransform(matrix, out var transformed) ? transformed : null;
        }
    }
}
#endif