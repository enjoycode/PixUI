namespace PixUI.Drawing.Skia;

public enum PathConvexity
{
    Unknown = 0,
    Convex = 1,
    Concave = 2,
}

public unsafe class SKPath : SKObject, ISKSkipObjectRegistration, IPath
{
    private SKPath(IntPtr handle, bool owns) : base(handle, owns) { }

    public SKPath() : this(SkiaApi.sk_path_new(), true)
    {
        if (Handle == IntPtr.Zero)
            throw new InvalidOperationException("Unable to create a new SKPath instance.");
    }

    protected override void DisposeNative() => SkiaApi.sk_path_delete(Handle);

    public SKPath Clone() => new SKPath(SkiaApi.sk_path_clone(this.Handle), true);

    public PathFillType FillType
    {
        get => SkiaApi.sk_path_get_filltype(Handle);
        set => SkiaApi.sk_path_set_filltype(Handle, value);
    }

    public PathConvexity Convexity
    {
        get => IsConvex ? PathConvexity.Convex : PathConvexity.Concave;
    }

    public bool IsConvex => SkiaApi.sk_path_is_convex(Handle);

    public bool IsConcave => !IsConvex;

    public bool IsEmpty() => VerbCount == 0;

    public bool IsOval => SkiaApi.sk_path_is_oval(Handle, null);

    public bool IsRoundRect => SkiaApi.sk_path_is_rrect(Handle, IntPtr.Zero);

    public bool IsLine => SkiaApi.sk_path_is_line(Handle, null);

    public bool IsRect => SkiaApi.sk_path_is_rect(Handle, null, null, null);

    public SKPathSegmentMask SegmentMasks =>
        (SKPathSegmentMask)SkiaApi.sk_path_get_segment_masks(Handle);

    public int VerbCount => SkiaApi.sk_path_count_verbs(Handle);

    public int PointCount => SkiaApi.sk_path_count_points(Handle);

    public Point this[int index] => GetPoint(index);

    public Point[] Points => GetPoints(PointCount);

    public Rect Bounds
    {
        get
        {
            Rect rect;
            SkiaApi.sk_path_get_bounds(Handle, &rect);
            return rect;
        }
    }

    public Rect TightBounds
    {
        get
        {
            if (GetTightBounds(out var rect))
            {
                return rect;
            }
            else
            {
                return Rect.Empty;
            }
        }
    }

    public Rect GetOvalBounds()
    {
        Rect bounds;
        if (SkiaApi.sk_path_is_oval(Handle, &bounds))
        {
            return bounds;
        }
        else
        {
            return Rect.Empty;
        }
    }

    public RRect? GetRoundRect()
    {
        throw new NotImplementedException();
        // var rrect = new RRect();
        // var result = SkiaApi.sk_path_is_rrect(Handle, rrect.Handle);
        // if (result)
        //     return rrect;
        //
        // rrect.Dispose();
        // return null;
    }

    public Point[]? GetLine()
    {
        var temp = new Point[2];
        fixed (Point* t = temp)
        {
            var result = SkiaApi.sk_path_is_line(Handle, t);
            return result ? temp : null;
        }
    }

    public Rect GetRect() =>
        GetRect(out var isClosed, out var direction);

    public Rect GetRect(out bool isClosed, out PathDirection direction)
    {
        byte c;
        fixed (PathDirection* d = &direction)
        {
            Rect rect;
            var result = SkiaApi.sk_path_is_rect(Handle, &rect, &c, d);
            isClosed = c > 0;
            if (result)
            {
                return rect;
            }
            else
            {
                return Rect.Empty;
            }
        }
    }

    public bool IsClosed()
    {
        //暂简单判断最后一个
        return SkiaApi.sk_path_is_last_contour_closed(Handle);
    }

    public bool TryGetLastPoint(out Point point)
    {
        Point res;
        var has = SkiaApi.sk_path_get_last_point(Handle, &res);
        point = res;
        return has;
    }

    public Point GetPoint(int index)
    {
        if (index < 0 || index >= PointCount)
            throw new ArgumentOutOfRangeException(nameof(index));

        Point point;
        SkiaApi.sk_path_get_point(Handle, index, &point);
        return point;
    }

    public Point[] GetPoints(int max)
    {
        var points = new Point[max];
        GetPoints(points, max);
        return points;
    }

    public int GetPoints(Point[] points, int max)
    {
        fixed (Point* p = points)
        {
            return SkiaApi.sk_path_get_points(Handle, p, max);
        }
    }

    public bool Contains(float x, float y) =>
        SkiaApi.sk_path_contains(Handle, x, y);

    public bool IsVisible(Point point) => Contains(point.X, point.Y);

    public void Offset(float dx, float dy) =>
        Transform(Matrix3.CreateTranslation(dx, dy));

    public void Rewind() => SkiaApi.sk_path_rewind(Handle);

    public void Reset() => SkiaApi.sk_path_reset(Handle);

    public bool GetBounds(out Rect rect)
    {
        var isEmpty = IsEmpty();
        if (isEmpty)
        {
            rect = Rect.Empty;
        }
        else
        {
            fixed (Rect* r = &rect)
            {
                SkiaApi.sk_path_get_bounds(Handle, r);
            }
        }

        return !isEmpty;
    }

    public Rect ComputeTightBounds()
    {
        Rect rect;
        SkiaApi.sk_path_compute_tight_bounds(Handle, &rect);
        return rect;
    }

    public void Transform(Matrix3 matrix) =>
        SkiaApi.sk_path_transform(Handle, &matrix);

    public void Transform(Matrix3 matrix, SKPath destination)
    {
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));

        SkiaApi.sk_path_transform_to_dest(Handle, &matrix, destination.Handle);
    }

    public Iterator CreateIterator(bool forceClose) =>
        new Iterator(this, forceClose);

    public RawIterator CreateRawIterator() =>
        new RawIterator(this);

    public IPath? GetOutlinePath(float strokeWidth)
    {
        if (strokeWidth <= 0)
            throw new ArgumentOutOfRangeException(nameof(strokeWidth), "Stroke width must be positive.");

        using var strokePaint = new SKPaint();
        strokePaint.Style = PaintStyle.Stroke;
        strokePaint.StrokeWidth = strokeWidth;
        strokePaint.StrokeCap = StrokeCap.Butt;
        strokePaint.StrokeJoin = StrokeJoin.Miter;
        strokePaint.IsAntialias = true;

        var pathBuilder = new SKPathBuilder();
        var ok = SkiaApi.sk_paint_get_fill_path(Handle, strokePaint.Handle, pathBuilder.Handle);
        if (!ok)
        {
            pathBuilder.Dispose();
            return null;
        }

        return pathBuilder.Detach();
    }

    public bool Op(IPath other, PathOp op)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        return SkiaApi.sk_pathop_op(Handle, ((SKPath)other).Handle, op, Handle);
    }

    public bool Simplify(SKPath result)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));

        return SkiaApi.sk_pathop_simplify(Handle, result.Handle);
    }

    public SKPath? Simplify()
    {
        var result = new SKPath();
        if (Simplify(result))
        {
            return result;
        }
        else
        {
            result.Dispose();
            return null;
        }
    }

    public bool GetTightBounds(out Rect result)
    {
        fixed (Rect* r = &result)
        {
            //TODO: use SkiaApi.sk_path_compute_tight_bounds(Handle, r);
            return SkiaApi.sk_pathop_tight_bounds(Handle, r);
        }
    }

    public bool ToWinding(SKPath result)
    {
        if (result == null)
            throw new ArgumentNullException(nameof(result));

        return SkiaApi.sk_pathop_as_winding(Handle, result.Handle);
    }

    public SKPath? ToWinding()
    {
        var result = new SKPath();
        if (ToWinding(result))
        {
            return result;
        }

        result.Dispose();
        return null;
    }

    public string ToSvgPathData()
    {
        using var str = new SKString();
        SkiaApi.sk_path_to_svg_string(Handle, str.Handle);
        return (string)str;
    }

    public static SKPath ParseSvgPathData(string svgPath)
    {
        var path = new SKPath();
        var success = SkiaApi.sk_path_parse_svg_string(path.Handle, svgPath);
        if (!success)
        {
            path.Dispose();
            path = null;
        }

        return path;
    }

    public static Point[] ConvertConicToQuads(Point p0, Point p1, Point p2, float w, int pow2)
    {
        ConvertConicToQuads(p0, p1, p2, w, out var pts, pow2);
        return pts;
    }

    public static int ConvertConicToQuads(Point p0, Point p1, Point p2, float w, out Point[] pts, int pow2)
    {
        var quadCount = 1 << pow2;
        var ptCount = 2 * quadCount + 1;
        pts = new Point[ptCount];
        return ConvertConicToQuads(p0, p1, p2, w, pts, pow2);
    }

    public static int ConvertConicToQuads(Point p0, Point p1, Point p2, float w, Point[] pts, int pow2)
    {
        if (pts == null)
            throw new ArgumentNullException(nameof(pts));
        fixed (Point* ptsptr = pts)
        {
            return SkiaApi.sk_path_convert_conic_to_quads(&p0, &p1, &p2, w, ptsptr, pow2);
        }
    }

    internal static SKPath? GetObject(IntPtr handle, bool owns = true) =>
        handle == IntPtr.Zero ? null : new SKPath(handle, owns);

    public sealed class Iterator : SKObject, ISKSkipObjectRegistration
    {
        private readonly SKPath _path;

        internal Iterator(SKPath path, bool forceClose)
            : base(SkiaApi.sk_path_create_iter(path.Handle, forceClose ? 1 : 0), true)
        {
            this._path = path;
        }

        protected override void DisposeNative() => SkiaApi.sk_path_iter_destroy(Handle);

        public SKPathVerb Next(Point[] points) =>
            Next(new Span<Point>(points));

        public SKPathVerb Next(Span<Point> points)
        {
            if (points.Length != 4)
                throw new ArgumentException("Must be an array of four elements.", nameof(points));

            fixed (Point* p = points)
            {
                return SkiaApi.sk_path_iter_next(Handle, p);
            }
        }

        public float ConicWeight() => SkiaApi.sk_path_iter_conic_weight(Handle);

        public bool IsCloseLine() => SkiaApi.sk_path_iter_is_close_line(Handle) != 0;

        public bool IsCloseContour() => SkiaApi.sk_path_iter_is_closed_contour(Handle) != 0;
    }

    public sealed class RawIterator : SKObject, ISKSkipObjectRegistration
    {
        private readonly SKPath _path;

        internal RawIterator(SKPath path)
            : base(SkiaApi.sk_path_create_rawiter(path.Handle), true)
        {
            this._path = path;
        }

        protected override void Dispose(bool disposing) =>
            base.Dispose(disposing);

        protected override void DisposeNative() => SkiaApi.sk_path_rawiter_destroy(Handle);

        public SKPathVerb Next(Point[] points) => Next(new Span<Point>(points));

        public SKPathVerb Next(Span<Point> points)
        {
            if (points.Length != 4)
                throw new ArgumentException("Must be an array of four elements.", nameof(points));
            fixed (Point* p = points)
            {
                return SkiaApi.sk_path_rawiter_next(Handle, p);
            }
        }

        public float ConicWeight() => SkiaApi.sk_path_rawiter_conic_weight(Handle);

        public SKPathVerb Peek() => SkiaApi.sk_path_rawiter_peek(Handle);
    }

    public sealed class OpBuilder : SKObject, ISKSkipObjectRegistration
    {
        public OpBuilder()
            : base(SkiaApi.sk_opbuilder_new(), true) { }

        public void Add(SKPath path, PathOp op) =>
            SkiaApi.sk_opbuilder_add(Handle, path.Handle, op);

        public bool Resolve(SKPath result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            return SkiaApi.sk_opbuilder_resolve(Handle, result.Handle);
        }

        protected override void DisposeNative() => SkiaApi.sk_opbuilder_destroy(Handle);
    }
}