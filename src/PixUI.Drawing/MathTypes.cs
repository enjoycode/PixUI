#if !__WEB__
using System;

namespace PixUI;

public partial struct Point
{
    public static readonly Point Empty;

    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly float Length => (float)Math.Sqrt(x * x + y * y);

    public readonly float LengthSquared => x * x + y * y;

    public void Offset(float dx, float dy)
    {
        x += dx;
        y += dy;
    }

    public PointI ToPoint() => new PointI((int)X, (int)Y); //TODO: rename to ToPointI

    public readonly override string ToString() => $"{{X={x}, Y={y}}}";

    public static Point Normalize(Point point)
    {
        var ls = point.x * point.x + point.y * point.y;
        var invNorm = 1.0 / Math.Sqrt(ls);
        return new Point((float)(point.x * invNorm), (float)(point.y * invNorm));
    }

    public static float Distance(Point point, Point other)
    {
        var dx = point.x - other.x;
        var dy = point.y - other.y;
        var ls = dx * dx + dy * dy;
        return (float)Math.Sqrt(ls);
    }

    public static float DistanceSquared(Point point, Point other)
    {
        var dx = point.x - other.x;
        var dy = point.y - other.y;
        return dx * dx + dy * dy;
    }

    public static Point Reflect(Point point, Point normal)
    {
        var dot = point.x * point.x + point.y * point.y;
        return new Point(
            point.x - 2.0f * dot * normal.x,
            point.y - 2.0f * dot * normal.y);
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
        new Point(pt.x + sz.Width, pt.y + sz.Height);

    public static Point operator +(Point pt, Size sz) =>
        new Point(pt.x + sz.Width, pt.y + sz.Height);

    public static Point operator +(Point pt, PointI sz) =>
        new Point(pt.x + sz.X, pt.y + sz.Y);

    public static Point operator +(Point pt, Point sz) =>
        new Point(pt.x + sz.X, pt.y + sz.Y);

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
        x = sz.Width;
        y = sz.Height;
    }

    public PointI(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly int Length => (int)Math.Sqrt(x * x + y * y);

    public readonly int LengthSquared => x * x + y * y;

    public void Offset(PointI p)
    {
        x += p.X;
        y += p.Y;
    }

    public void Offset(int dx, int dy)
    {
        x += dx;
        y += dy;
    }

    public readonly override string ToString() => $"{{X={x},Y={y}}}";

    public static PointI Normalize(PointI point)
    {
        var ls = point.x * point.x + point.y * point.y;
        var invNorm = 1.0 / Math.Sqrt(ls);
        return new PointI((int)(point.x * invNorm), (int)(point.y * invNorm));
    }

    public static float Distance(PointI point, PointI other)
    {
        var dx = point.x - other.x;
        var dy = point.y - other.y;
        var ls = dx * dx + dy * dy;
        return (float)Math.Sqrt(ls);
    }

    public static float DistanceSquared(PointI point, PointI other)
    {
        var dx = point.x - other.x;
        var dy = point.y - other.y;
        return dx * dx + dy * dy;
    }

    public static PointI Reflect(PointI point, PointI normal)
    {
        var dot = point.x * point.x + point.y * point.y;
        return new PointI(
            (int)(point.x - 2.0f * dot * normal.x),
            (int)(point.y - 2.0f * dot * normal.y));
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
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly override string ToString() => $"{{X={x}, Y={y}, Z={z}}}";

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
        w = width;
        h = height;
    }

    public Size(Point pt)
    {
        w = pt.X;
        h = pt.Y;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly Point ToPoint() =>
        new Point(w, h);

    public readonly SizeI ToSizeI()
    {
        int w, h;
        checked
        {
            w = (int)this.w;
            h = (int)this.h;
        }

        return new SizeI(w, h);
    }

    public readonly override string ToString() =>
        $"{{Width={w}, Height={h}}}";

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
        w = width;
        h = height;
    }

    public SizeI(PointI pt)
    {
        w = pt.X;
        h = pt.Y;
    }

    public readonly bool IsEmpty => this == Empty;

    public readonly PointI ToPointI() => new PointI(w, h);

    public readonly override string ToString() =>
        $"{{Width={w}, Height={h}}}";

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
        this.left = left;
        this.right = right;
        this.top = top;
        this.bottom = bottom;
    }

    public readonly float MidX => (left + right) / 2;

    public readonly float MidY => (top + bottom) / 2;

    public float Width
    {
        get => right - left;
        set => right = value + left;
    }

    public float Height
    {
        get => bottom - top;
        set => bottom = top + value;
    }

    public readonly bool IsEmpty => this == Empty;

    public Size Size
    {
        get => new Size(Width, Height);
        set
        {
            right = left + value.Width;
            bottom = top + value.Height;
        }
    }

    public Point Location
    {
        readonly get => new Point(left, top);
        set => this = Rect.Create(value, Size);
    }

    public Point Center => new((left + right) / 2, (top + bottom) / 2);

    public readonly Rect Standardized
    {
        get
        {
            if (left > right)
            {
                return top > bottom
                    ? new Rect(right, bottom, left, top)
                    : new Rect(right, top, left, bottom);
            }

            return top > bottom
                ? new Rect(left, bottom, right, top)
                : new Rect(left, top, right, bottom);
        }
    }

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

    public void Inflate(float x, float y)
    {
        left -= x;
        top -= y;
        right += x;
        bottom += y;
    }

    public static Rect Inflate(Rect rect, float x, float y)
    {
        var res = rect;
        res.Inflate(x, y);
        return res;
    }

    public static Rect Intersect(Rect a, Rect b)
    {
        if (!a.IntersectsWithInclusive(b))
        {
            return Empty;
        }

        return new Rect(
            Math.Max(a.left, b.left),
            Math.Max(a.top, b.top),
            Math.Min(a.right, b.right),
            Math.Min(a.bottom, b.bottom));
    }

    public void Intersect(Rect rect) => this = Intersect(this, rect);

    public static Rect Union(Rect a, Rect b) =>
        new Rect(
            Math.Min(a.left, b.left),
            Math.Min(a.top, b.top),
            Math.Max(a.right, b.right),
            Math.Max(a.bottom, b.bottom));

    public void Union(Rect rect) => this = Union(this, rect);

    public static implicit operator Rect(RectI r) =>
        new Rect(r.Left, r.Top, r.Right, r.Bottom);

    public readonly bool Contains(Point point) => ContainsPoint(point.X, point.Y);

    public readonly bool ContainsPoint(float x, float y) =>
        x >= left && x < right && y >= top && y < bottom;

    public readonly bool ContainsRect(Rect rect) =>
        left <= rect.left && right >= rect.right &&
        top <= rect.top && bottom >= rect.bottom;

    public readonly bool IntersectsWith(float x, float y, float w, float h) =>
        left < (x + w) && right > x && top < (y + h) && bottom > y;

    public readonly bool IntersectsWith(Rect rect) =>
        IntersectsWith(rect.Left, rect.Top, rect.Width, rect.Height);

    public readonly bool IntersectsWithInclusive(Rect rect) =>
        left <= rect.right && right >= rect.left && top <= rect.bottom && bottom >= rect.top;

    public void Offset(float x, float y)
    {
        left += x;
        top += y;
        right += x;
        bottom += y;
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
        this.left = left;
        this.right = right;
        this.top = top;
        this.bottom = bottom;
    }

    public readonly int MidX => left + (Width / 2);

    public readonly int MidY => top + (Height / 2);

    public int Width
    {
        get => right - left;
        set => right = left + value;
    }

    public int Height
    {
        get => bottom - top;
        set => bottom = top + value;
    }

    public readonly bool IsEmpty => this == Empty;

    public SizeI Size
    {
        readonly get => new SizeI(Width, Height);
        set
        {
            right = left + value.Width;
            bottom = top + value.Height;
        }
    }

    public PointI Location
    {
        readonly get => new PointI(left, top);
        set => this = RectI.Create(value, Size);
    }

    public readonly RectI Standardized
    {
        get
        {
            if (left > right)
            {
                if (top > bottom)
                {
                    return new RectI(right, bottom, left, top);
                }
                else
                {
                    return new RectI(right, top, left, bottom);
                }
            }
            else
            {
                if (top > bottom)
                {
                    return new RectI(left, bottom, right, top);
                }
                else
                {
                    return new RectI(left, top, right, bottom);
                }
            }
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
        var r = new RectI(rect.left, rect.top, rect.right, rect.bottom);
        r.Inflate(x, y);
        return r;
    }

    public void Inflate(SizeI size) =>
        Inflate(size.Width, size.Height);

    public void Inflate(int width, int height)
    {
        left -= width;
        top -= height;
        right += width;
        bottom += height;
    }

    public static RectI Intersect(RectI a, RectI b)
    {
        if (!a.IntersectsWithInclusive(b))
            return Empty;

        return new RectI(
            Math.Max(a.left, b.left),
            Math.Max(a.top, b.top),
            Math.Min(a.right, b.right),
            Math.Min(a.bottom, b.bottom));
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
        (x >= left) && (x < right) && (y >= top) && (y < bottom);

    public readonly bool Contains(PointI pt) =>
        Contains(pt.X, pt.Y);

    public readonly bool Contains(RectI rect) =>
        (left <= rect.left) && (right >= rect.right) &&
        (top <= rect.top) && (bottom >= rect.bottom);

    public readonly bool IntersectsWith(RectI rect) =>
        (left < rect.right) && (right > rect.left) && (top < rect.bottom) &&
        (bottom > rect.top);

    public readonly bool IntersectsWithInclusive(RectI rect) =>
        (left <= rect.right) && (right >= rect.left) && (top <= rect.bottom) &&
        (bottom >= rect.top);

    public void Offset(int x, int y)
    {
        left += x;
        top += y;
        right += x;
        bottom += y;
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
#endif