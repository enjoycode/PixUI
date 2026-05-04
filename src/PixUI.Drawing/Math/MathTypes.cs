namespace PixUI;

public partial struct Point
{
    public static readonly Point Empty;

    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly float Length => (float)Math.Sqrt(X * X + Y * Y);

    public readonly float LengthSquared => X * X + Y * Y;

    public void Offset(float dx, float dy)
    {
        X += dx;
        Y += dy;
    }

    public PointI ToPoint() => new PointI((int)X, (int)Y); //TODO: rename to ToPointI

    public readonly override string ToString() => $"{{X={X}, Y={Y}}}";

    public static Point Normalize(Point point)
    {
        var ls = point.X * point.X + point.Y * point.Y;
        var invNorm = 1.0 / Math.Sqrt(ls);
        return new Point((float)(point.X * invNorm), (float)(point.Y * invNorm));
    }

    public static float Distance(Point point, Point other)
    {
        var dx = point.X - other.X;
        var dy = point.Y - other.Y;
        var ls = dx * dx + dy * dy;
        return (float)Math.Sqrt(ls);
    }

    public static float DistanceSquared(Point point, Point other)
    {
        var dx = point.X - other.X;
        var dy = point.Y - other.Y;
        return dx * dx + dy * dy;
    }

    public static Point Reflect(Point point, Point normal)
    {
        var dot = point.X * point.X + point.Y * point.Y;
        return new Point(
            point.X - 2.0f * dot * normal.X,
            point.Y - 2.0f * dot * normal.Y);
    }

    public static Point Add(Point pt, SizeI sz) => pt + sz;
    public static Point Add(Point pt, Size sz) => pt + sz;
    public static Point Add(Point pt, PointI sz) => pt + sz;
    public static Point Add(Point pt, Point sz) => pt + sz;

    public static Point Subtract(Point pt, SizeI sz) => pt - sz;
    public static Point Subtract(Point pt, Size sz) => pt - sz;
    public static Point Subtract(Point pt, PointI sz) => pt - sz;
    public static Point Subtract(Point pt, Point sz) => pt - sz;

    public static Point operator +(Point pt, SizeI sz) =>
        new Point(pt.X + sz.Width, pt.Y + sz.Height);

    public static Point operator +(Point pt, Size sz) =>
        new Point(pt.X + sz.Width, pt.Y + sz.Height);

    public static Point operator +(Point pt, PointI sz) =>
        new Point(pt.X + sz.X, pt.Y + sz.Y);

    public static Point operator +(Point pt, Point sz) =>
        new Point(pt.X + sz.X, pt.Y + sz.Y);

    public static Point operator -(Point pt, SizeI sz) =>
        new Point(pt.X - sz.Width, pt.Y - sz.Height);

    public static Point operator -(Point pt, Size sz) =>
        new Point(pt.X - sz.Width, pt.Y - sz.Height);

    public static Point operator -(Point pt, PointI sz) =>
        new Point(pt.X - sz.X, pt.Y - sz.Y);

    public static Point operator -(Point pt, Point sz) =>
        new Point(pt.X - sz.X, pt.Y - sz.Y);
}

public partial struct PointI
{
    public static readonly PointI Empty;

    public PointI(SizeI sz)
    {
        X = sz.Width;
        Y = sz.Height;
    }

    public PointI(int x, int y)
    {
        X = x;
        Y = y;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly int Length => (int)Math.Sqrt(X * X + Y * Y);

    public readonly int LengthSquared => X * X + Y * Y;

    public void Offset(PointI p)
    {
        X += p.X;
        Y += p.Y;
    }

    public void Offset(int dx, int dy)
    {
        X += dx;
        Y += dy;
    }

    public readonly override string ToString() => $"{{X={X},Y={Y}}}";

    public static PointI Normalize(PointI point)
    {
        var ls = point.X * point.X + point.Y * point.Y;
        var invNorm = 1.0 / Math.Sqrt(ls);
        return new PointI((int)(point.X * invNorm), (int)(point.Y * invNorm));
    }

    public static float Distance(PointI point, PointI other)
    {
        var dx = point.X - other.X;
        var dy = point.Y - other.Y;
        var ls = dx * dx + dy * dy;
        return (float)Math.Sqrt(ls);
    }

    public static float DistanceSquared(PointI point, PointI other)
    {
        var dx = point.X - other.X;
        var dy = point.Y - other.Y;
        return dx * dx + dy * dy;
    }

    public static PointI Reflect(PointI point, PointI normal)
    {
        var dot = point.X * point.X + point.Y * point.Y;
        return new PointI(
            (int)(point.X - 2.0f * dot * normal.X),
            (int)(point.Y - 2.0f * dot * normal.Y));
    }

    public static PointI Ceiling(Point value)
    {
        int x, y;
        checked
        {
            x = (int)Math.Ceiling(value.X);
            y = (int)Math.Ceiling(value.Y);
        }

        return new PointI(x, y);
    }

    public static PointI Round(Point value)
    {
        int x, y;
        checked
        {
            x = (int)Math.Round(value.X);
            y = (int)Math.Round(value.Y);
        }

        return new PointI(x, y);
    }

    public static PointI Truncate(Point value)
    {
        int x, y;
        checked
        {
            x = (int)value.X;
            y = (int)value.Y;
        }

        return new PointI(x, y);
    }

    public static PointI Add(PointI pt, SizeI sz) => pt + sz;
    public static PointI Add(PointI pt, PointI sz) => pt + sz;

    public static PointI Subtract(PointI pt, SizeI sz) => pt - sz;
    public static PointI Subtract(PointI pt, PointI sz) => pt - sz;

    public static PointI operator +(PointI pt, SizeI sz) =>
        new PointI(pt.X + sz.Width, pt.Y + sz.Height);

    public static PointI operator +(PointI pt, PointI sz) =>
        new PointI(pt.X + sz.X, pt.Y + sz.Y);

    public static PointI operator -(PointI pt, SizeI sz) =>
        new PointI(pt.X - sz.Width, pt.Y - sz.Height);

    public static PointI operator -(PointI pt, PointI sz) =>
        new PointI(pt.X - sz.X, pt.Y - sz.Y);

    public static explicit operator SizeI(PointI p) =>
        new SizeI(p.X, p.Y);

    public static implicit operator Point(PointI p) =>
        new Point(p.X, p.Y);
}

public partial struct Point3
{
    public static readonly Point3 Empty;

    public Point3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly override string ToString() => $"{{X={X}, Y={Y}, Z={Z}}}";

    public static Point3 Add(Point3 pt, Point3 sz) => pt + sz;

    public static Point3 Subtract(Point3 pt, Point3 sz) => pt - sz;

    public static Point3 operator +(Point3 pt, Point3 sz) =>
        new Point3(pt.X + sz.X, pt.Y + sz.Y, pt.Z + sz.Z);

    public static Point3 operator -(Point3 pt, Point3 sz) =>
        new Point3(pt.X - sz.X, pt.Y - sz.Y, pt.Z - sz.Z);
}

public partial struct Size
{
    public static readonly Size Empty;

    public Size(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public Size(Point pt)
    {
        Width = pt.X;
        Height = pt.Y;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly Point ToPoint() =>
        new Point(Width, Height);

    public readonly SizeI ToSizeI()
    {
        int w, h;
        checked
        {
            w = (int)Width;
            h = (int)Height;
        }

        return new SizeI(w, h);
    }

    public readonly override string ToString() =>
        $"{{Width={Width}, Height={Height}}}";

    public static Size Add(Size sz1, Size sz2) => sz1 + sz2;

    public static Size Subtract(Size sz1, Size sz2) => sz1 - sz2;

    public static Size operator +(Size sz1, Size sz2) =>
        new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);

    public static Size operator -(Size sz1, Size sz2) =>
        new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);

    public static explicit operator Point(Size size) =>
        new Point(size.Width, size.Height);

    public static implicit operator Size(SizeI size) =>
        new Size(size.Width, size.Height);
}

public partial struct SizeI
{
    public static readonly SizeI Empty;

    public SizeI(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public SizeI(PointI pt)
    {
        Width = pt.X;
        Height = pt.Y;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly PointI ToPointI() => new PointI(Width, Height);

    public readonly override string ToString() =>
        $"{{Width={Width}, Height={Height}}}";

    public static SizeI Add(SizeI sz1, SizeI sz2) => sz1 + sz2;

    public static SizeI Subtract(SizeI sz1, SizeI sz2) => sz1 - sz2;

    public static SizeI Round(Size value) => new((int)Math.Round(value.Width), (int)Math.Round(value.Height));

    public static SizeI operator +(SizeI sz1, SizeI sz2) =>
        new SizeI(sz1.Width + sz2.Width, sz1.Height + sz2.Height);

    public static SizeI operator -(SizeI sz1, SizeI sz2) =>
        new SizeI(sz1.Width - sz2.Width, sz1.Height - sz2.Height);

    public static explicit operator PointI(SizeI size) =>
        new PointI(size.Width, size.Height);
}

public partial struct Rect
{
    public static readonly Rect Empty;

    public Rect(float left, float top, float right, float bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }

    public readonly float MidX => (Left + Right) / 2;

    public readonly float MidY => (Top + Bottom) / 2;

    public float Width
    {
        readonly get => Right - Left;
        set => Right = value + Left;
    }

    internal float HarfWidth => FloatUtils.MidPoint(-Left, Right);

    public float Height
    {
        readonly get => Bottom - Top;
        set => Bottom = Top + value;
    }

    internal float HarfHeight => FloatUtils.MidPoint(-Top, Bottom);

    public readonly bool IsEmpty => !(Left < Right && Top < Bottom);

    public Size Size
    {
        readonly get => new Size(Width, Height);
        set
        {
            Right = Left + value.Width;
            Bottom = Top + value.Height;
        }
    }

    public Point Location
    {
        readonly get => new Point(Left, Top);
        set => this = Create(value, Size);
    }

    public Point Center => new((Left + Right) / 2, (Top + Bottom) / 2);

    public readonly Rect Standardized
    {
        get
        {
            // return new Rect(Math.Min(Left, Right), Math.Min(Top, Bottom), 
            //     Math.Max(Left, Right), Math.Max(Top, Bottom));
            if (Left > Right)
            {
                return Top > Bottom
                    ? new Rect(Right, Bottom, Left, Top)
                    : new Rect(Right, Top, Left, Bottom);
            }

            return Top > Bottom
                ? new Rect(Left, Bottom, Right, Top)
                : new Rect(Left, Top, Right, Bottom);
        }
    }

    public bool IsStandardized() => Left <= Right && Top <= Bottom;

    /// <summary>
    /// Returns true if all values in the rectangle are finite.
    /// </summary>
    /// <returns>true if no member is infinite or NaN</returns>
    public readonly bool IsFinite() =>
        float.IsFinite(Left) && float.IsFinite(Top) && float.IsFinite(Right) && float.IsFinite(Bottom);

    public readonly Rect AspectFit(Size size) => AspectResize(size, true);

    public readonly Rect AspectFill(Size size) => AspectResize(size, false);

    private readonly Rect AspectResize(Size size, bool fit)
    {
        if (size.Width == 0 || size.Height == 0 || Width == 0 || Height == 0)
            return FromLTWH(MidX, MidY, 0, 0);

        var aspectWidth = size.Width;
        var aspectHeight = size.Height;
        var imgAspect = aspectWidth / aspectHeight;
        var fullRectAspect = Width / Height;

        var compare = fit ? (fullRectAspect > imgAspect) : (fullRectAspect < imgAspect);
        if (compare)
        {
            aspectHeight = Height;
            aspectWidth = aspectHeight * imgAspect;
        }
        else
        {
            aspectWidth = Width;
            aspectHeight = aspectWidth / imgAspect;
        }

        var aspectLeft = MidX - (aspectWidth / 2f);
        var aspectTop = MidY - (aspectHeight / 2f);

        return FromLTWH(aspectLeft, aspectTop, aspectWidth, aspectHeight);
    }

    public static RectI Ceiling(Rect value)
    {
        int x, y, w, h;
        checked
        {
            x = (int)Math.Ceiling(value.X);
            y = (int)Math.Ceiling(value.Y);
            w = (int)Math.Ceiling(value.Width);
            h = (int)Math.Ceiling(value.Height);
        }

        return RectI.FromLTWH(x, y, w, h);
    }

    public void Inflate(float dx, float dy)
    {
        Left -= dx;
        Top -= dy;
        Right += dx;
        Bottom += dy;
    }

    public static Rect Inflate(in Rect rect, float x, float y)
    {
        var res = rect;
        res.Inflate(x, y);
        return res;
    }

    public void Deflate(float dx, float dy) => Inflate(-dx, -dy);

    public static Rect Deflate(in Rect rect, float dx, float dy) => Inflate(rect, -dx, -dy);

    public static Rect Intersect(Rect a, Rect b)
    {
        if (!a.IntersectsWithInclusive(b))
        {
            return Empty;
        }

        return new Rect(
            Math.Max(a.Left, b.Left),
            Math.Max(a.Top, b.Top),
            Math.Min(a.Right, b.Right),
            Math.Min(a.Bottom, b.Bottom));
    }

    public void Intersect(Rect rect) => this = Intersect(this, rect);

    public static Rect Union(Rect a, Rect b) =>
        new Rect(
            Math.Min(a.Left, b.Left),
            Math.Min(a.Top, b.Top),
            Math.Max(a.Right, b.Right),
            Math.Max(a.Bottom, b.Bottom));

    public void Union(Rect rect) => this = Union(this, rect);

    public static implicit operator Rect(RectI r) => new Rect(r.Left, r.Top, r.Right, r.Bottom);

    public readonly bool Contains(Point point) => ContainsPoint(point.X, point.Y);

    public readonly bool ContainsPoint(float x, float y) =>
        x >= Left && x < Right && y >= Top && y < Bottom;

    public readonly bool ContainsRect(Rect rect) =>
        Left <= rect.Left && Right >= rect.Right &&
        Top <= rect.Top && Bottom >= rect.Bottom;

    public readonly bool IntersectsWith(float x, float y, float w, float h) =>
        Left < (x + w) && Right > x && Top < (y + h) && Bottom > y;

    public readonly bool IntersectsWith(Rect rect) =>
        IntersectsWith(rect.Left, rect.Top, rect.Width, rect.Height);

    public readonly bool IntersectsWithInclusive(Rect rect) =>
        Left <= rect.Right && Right >= rect.Left && Top <= rect.Bottom && Bottom >= rect.Top;

    public void Offset(float x, float y)
    {
        Left += x;
        Top += y;
        Right += x;
        Bottom += y;
    }

    public void Offset(Point pos) => Offset(pos.X, pos.Y);

    public readonly override string ToString() => $"{{X={Left},Y={Top},W={Width},H={Height}}}";

    public static Rect Create(Point location, Size size) =>
        FromLTWH(location.X, location.Y, size.Width, size.Height);

    public static Rect FromLTWH(float x, float y, float width, float height) =>
        new(x, y, x + width, y + height);
}

public partial struct RectI
{
    public static readonly RectI Empty;

    public static RectI FromLTWH(int x, int y, int width, int height) =>
        new(x, y, x + width, y + height);

    public RectI(int left, int top, int right, int bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }

    public readonly int MidX => Left + (Width / 2);

    public readonly int MidY => Top + (Height / 2);

    public int Width
    {
        readonly get => Right - Left;
        set => Right = Left + value;
    }

    public int Height
    {
        readonly get => Bottom - Top;
        set => Bottom = Top + value;
    }

    public readonly bool IsEmpty => this == Empty;

    public SizeI Size
    {
        readonly get => new SizeI(Width, Height);
        set
        {
            Right = Left + value.Width;
            Bottom = Top + value.Height;
        }
    }

    public PointI Location
    {
        readonly get => new PointI(Left, Top);
        set => this = Create(value, Size);
    }

    public readonly RectI Standardized
    {
        get
        {
            if (Left > Right)
            {
                if (Top > Bottom)
                    return new RectI(Right, Bottom, Left, Top);

                return new RectI(Right, Top, Left, Bottom);
            }

            if (Top > Bottom)
                return new RectI(Left, Bottom, Right, Top);

            return new RectI(Left, Top, Right, Bottom);
        }
    }

    public readonly RectI AspectFit(SizeI size) =>
        Truncate(((Rect)this).AspectFit(size));

    public readonly RectI AspectFill(SizeI size) =>
        Truncate(((Rect)this).AspectFill(size));

    public static RectI Ceiling(Rect value) =>
        Ceiling(value, false);

    public static RectI Ceiling(Rect value, bool outwards)
    {
        int x, y, r, b;
        checked
        {
            x = (int)(outwards && value.Width > 0
                ? Math.Floor(value.Left)
                : Math.Ceiling(value.Left));
            y = (int)(outwards && value.Height > 0
                ? Math.Floor(value.Top)
                : Math.Ceiling(value.Top));
            r = (int)(outwards && value.Width < 0
                ? Math.Floor(value.Right)
                : Math.Ceiling(value.Right));
            b = (int)(outwards && value.Height < 0
                ? Math.Floor(value.Bottom)
                : Math.Ceiling(value.Bottom));
        }

        return new RectI(x, y, r, b);
    }

    public static RectI Inflate(RectI rect, int x, int y)
    {
        var r = new RectI(rect.Left, rect.Top, rect.Right, rect.Bottom);
        r.Inflate(x, y);
        return r;
    }

    public void Inflate(SizeI size) =>
        Inflate(size.Width, size.Height);

    public void Inflate(int width, int height)
    {
        Left -= width;
        Top -= height;
        Right += width;
        Bottom += height;
    }

    public static RectI Intersect(RectI a, RectI b)
    {
        if (!a.IntersectsWithInclusive(b))
            return Empty;

        return new RectI(
            Math.Max(a.Left, b.Left),
            Math.Max(a.Top, b.Top),
            Math.Min(a.Right, b.Right),
            Math.Min(a.Bottom, b.Bottom));
    }

    public void Intersect(RectI rect) =>
        this = Intersect(this, rect);

    public static RectI Round(Rect value)
    {
        int x, y, r, b;
        checked
        {
            x = (int)Math.Round(value.Left);
            y = (int)Math.Round(value.Top);
            r = (int)Math.Round(value.Right);
            b = (int)Math.Round(value.Bottom);
        }

        return new RectI(x, y, r, b);
    }

    public static RectI Floor(Rect value) => Floor(value, false);

    public static RectI Floor(Rect value, bool inwards)
    {
        int x, y, r, b;
        checked
        {
            x = (int)(inwards && value.Width > 0
                ? Math.Ceiling(value.Left)
                : Math.Floor(value.Left));
            y = (int)(inwards && value.Height > 0
                ? Math.Ceiling(value.Top)
                : Math.Floor(value.Top));
            r = (int)(inwards && value.Width < 0
                ? Math.Ceiling(value.Right)
                : Math.Floor(value.Right));
            b = (int)(inwards && value.Height < 0
                ? Math.Ceiling(value.Bottom)
                : Math.Floor(value.Bottom));
        }

        return new RectI(x, y, r, b);
    }

    public static RectI Truncate(Rect value)
    {
        int x, y, r, b;
        checked
        {
            x = (int)value.Left;
            y = (int)value.Top;
            r = (int)value.Right;
            b = (int)value.Bottom;
        }

        return new RectI(x, y, r, b);
    }

    public static RectI Union(RectI a, RectI b) =>
        new RectI(
            Math.Min(a.Left, b.Left),
            Math.Min(a.Top, b.Top),
            Math.Max(a.Right, b.Right),
            Math.Max(a.Bottom, b.Bottom));

    public void Union(RectI rect) =>
        this = Union(this, rect);

    public readonly bool Contains(int x, int y) =>
        (x >= Left) && (x < Right) && (y >= Top) && (y < Bottom);

    public readonly bool Contains(PointI pt) =>
        Contains(pt.X, pt.Y);

    public readonly bool Contains(RectI rect) =>
        (Left <= rect.Left) && (Right >= rect.Right) &&
        (Top <= rect.Top) && (Bottom >= rect.Bottom);

    public readonly bool IntersectsWith(RectI rect) =>
        (Left < rect.Right) && (Right > rect.Left) && (Top < rect.Bottom) &&
        (Bottom > rect.Top);

    public readonly bool IntersectsWithInclusive(RectI rect) =>
        (Left <= rect.Right) && (Right >= rect.Left) && (Top <= rect.Bottom) &&
        (Bottom >= rect.Top);

    public void Offset(int x, int y)
    {
        Left += x;
        Top += y;
        Right += x;
        Bottom += y;
    }

    public void Offset(PointI pos) => Offset(pos.X, pos.Y);

    public readonly override string ToString() =>
        $"{{Left={Left},Top={Top},Width={Width},Height={Height}}}";

    public static RectI Create(SizeI size) =>
        Create(PointI.Empty.X, PointI.Empty.Y, size.Width, size.Height);

    public static RectI Create(PointI location, SizeI size) =>
        Create(location.X, location.Y, size.Width, size.Height);

    public static RectI Create(int width, int height) =>
        new RectI(PointI.Empty.X, PointI.Empty.X, width, height);

    public static RectI Create(int x, int y, int width, int height) =>
        new RectI(x, y, x + width, y + height);
}