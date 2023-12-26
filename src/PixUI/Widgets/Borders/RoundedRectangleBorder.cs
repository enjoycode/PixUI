namespace PixUI;

public sealed class RoundedRectangleBorder : OutlinedBorder
{
    public readonly BorderRadius BorderRadius;

    public RoundedRectangleBorder(BorderSide? side = null, BorderRadius? borderRadius = null)
        : base(side)
    {
        BorderRadius = borderRadius ?? BorderRadius.Empty;
    }

    public override Path GetOuterPath(in Rect rect)
    {
        var rrect = BorderRadius.ToRRect(rect);
        rrect.Deflate(Side.Width, Side.Width);
        var path = new Path();
        path.AddRRect(rrect);
        return path;
    }

    public override Path GetInnerPath(in Rect rect)
    {
        var rrect = BorderRadius.ToRRect(rect);
        var path = new Path();
        path.AddRRect(rrect);
        return path;
    }

    public override void LerpTo(ShapeBorder? to, ShapeBorder tween, double t)
    {
        throw new System.NotImplementedException();
    }

    public override OutlinedBorder CopyWith(BorderSide? side)
        => new RoundedRectangleBorder(side ?? Side, BorderRadius /*TODO: need copy for web?*/);

    public override ShapeBorder Clone()
    {
        throw new System.NotImplementedException();
    }

    public override void Paint(Canvas canvas, in Rect rect, in Color? fillColor = null)
    {
        if (Side.Style == BorderStyle.None)
            return;

        var width = Side.Width;
        if (width == 0)
        {
            var paint = PixUI.Paint.Shared();
            Side.ApplyPaint(paint);
            using var rrect = BorderRadius.ToRRect(rect);
            canvas.DrawRRect(rrect, paint);
        }
        else
        {
            using var outer = BorderRadius.ToRRect(rect);
            using var inner = RRect.FromCopy(outer);
            inner.Deflate(width, width);
            var paint = PixUI.Paint.Shared(Side.Color);
            canvas.DrawDRRect(outer, inner, paint);
        }
    }
}