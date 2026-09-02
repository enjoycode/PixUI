namespace PixUI;

public abstract class InputBorder : ShapeBorder
{
    public static readonly InputBorder DefaultOutline = new OutlineInputBorder(null, BorderRadius.Circular(4));
    public static readonly InputBorder DefaultUnderline = new UnderlineInputBorder(0xFF9B9B9B);

    public BorderSide BorderSide { get; protected set; }

    public override EdgeInsets Dimensions => EdgeInsets.All(BorderSide.Width);

    public InputBorder(BorderSide? borderSide)
    {
        BorderSide = borderSide ?? BorderSide.Empty;
    }
}

public sealed class OutlineInputBorder : InputBorder
{
    public OutlineInputBorder(BorderSide? borderSide = null, BorderRadius? borderRadius = null, float gapPadding = 4.0f)
        : base(borderSide ?? new BorderSide(new Color(0xFF9B9B9B)))
    {
        if (gapPadding < 0)
            throw new ArgumentOutOfRangeException(nameof(gapPadding));

        BorderRadius = borderRadius ?? BorderRadius.All(Radius.Circular(4.0f));
        GapPadding = gapPadding;
    }

    public BorderRadius BorderRadius { get; private set; }
    public float GapPadding { get; private set; }

    public override IPath GetOuterPath(in Rect rect)
    {
        throw new NotImplementedException();
    }

    public override IPath GetInnerPath(in Rect rect)
    {
        throw new NotImplementedException();
    }

    public override void LerpTo(ShapeBorder? to, ShapeBorder tween, double t)
    {
        if (to is OutlineInputBorder other)
        {
            var temp = (OutlineInputBorder)tween;
            temp.BorderRadius = BorderRadius.Lerp(BorderRadius, other.BorderRadius, t)!.Value;
            temp.BorderSide = BorderSide.Lerp(BorderSide, other.BorderSide, t);
            temp.GapPadding = other.GapPadding;
        }
        else
        {
            base.LerpTo(to, tween, t);
        }
    }

    public override ShapeBorder Clone()
        => new OutlineInputBorder(BorderSide, BorderRadius, GapPadding);

    public override void Paint(ICanvas canvas, in Rect rect, in Color? fillColor = null)
    {
        var outer = BorderRadius.ToRRect(rect);
        outer.Deflate(BorderSide.Width / 2f, BorderSide.Width / 2f);

        if (fillColor != null)
            canvas.DrawRRect(outer, PixUI.Paint.Shared(fillColor.Value));

        var paint = PixUI.Paint.Shared();
        BorderSide.ApplyPaint(paint);
        paint.IsAntialias = true; //TODO: no radius no need
        canvas.DrawRRect(outer, paint);
    }
}

public sealed class UnderlineInputBorder : InputBorder
{
    public UnderlineInputBorder(Color color) : base(new BorderSide(color)) { }

    public UnderlineInputBorder(BorderSide? borderSide) : base(borderSide) { }

    public override IPath GetOuterPath(in Rect rect)
    {
        throw new NotImplementedException();
    }

    public override IPath GetInnerPath(in Rect rect)
    {
        throw new NotImplementedException();
    }

    public override void LerpTo(ShapeBorder? to, ShapeBorder tween, double t)
    {
        if (to is UnderlineInputBorder other)
        {
            var temp = (UnderlineInputBorder)tween;
            temp.BorderSide = BorderSide.Lerp(BorderSide, other.BorderSide, t);
        }
        else
        {
            base.LerpTo(to, tween, t);
        }
    }

    public override void Paint(ICanvas canvas, in Rect rect, in Color? fillColor = null)
    {
        if (fillColor != null)
            canvas.DrawRect(rect, PixUI.Paint.Shared(fillColor));

        var paint = PixUI.Paint.Shared();
        BorderSide.ApplyPaint(paint);
        canvas.DrawLine(rect.Left, rect.Bottom, rect.Right, rect.Bottom, paint);
    }

    public override ShapeBorder Clone() => new UnderlineInputBorder(BorderSide);
}