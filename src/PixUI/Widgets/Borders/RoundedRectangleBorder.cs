namespace PixUI;

public sealed class RoundedRectangleBorder : OutlinedBorder
{
    public readonly BorderRadius BorderRadius;

    public RoundedRectangleBorder(BorderSide? side = null, BorderRadius? borderRadius = null)
        : base(side)
    {
        BorderRadius = borderRadius ?? BorderRadius.Empty;
    }

    public override IPath GetOuterPath(in Rect rect)
    {
        var rrect = BorderRadius.ToRRect(rect);
        rrect.Deflate(Side.Width, Side.Width);
        var path = Path.Create();
        path.AddRRect(rrect);
        return path;
    }

    public override IPath GetInnerPath(in Rect rect)
    {
        var rrect = BorderRadius.ToRRect(rect);
        var path = Path.Create();
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

    public override void Paint(ICanvas canvas, in Rect rect, in Color? fillColor = null)
    {
        if (Side.Style == BorderStyle.None)
            return;

        var width = Side.Width;
        if (width == 0)
        {
            var paint = PixUI.Paint.Shared();
            Side.ApplyPaint(paint);
            var rrect = BorderRadius.ToRRect(rect);
            canvas.DrawRRect(rrect, paint);
        }
        else
        {
            var outer = BorderRadius.ToRRect(rect);
            var inner = outer;
            inner.Deflate(width, width);
            var paint = PixUI.Paint.Shared(Side.Color);
            canvas.DrawDRRect(outer, inner, paint);
        }
    }
}