namespace PixUI;

/// <summary>
/// Base class for shape outlines.
/// </summary>
public abstract class ShapeBorder
{
    public abstract EdgeInsets Dimensions { get; }

    /// <summary>
    /// Create a [Path] that describes the outer edge of the border.
    /// </summary>
    public abstract Path GetOuterPath(in Rect rect);

    /// <summary>
    /// Create a [Path] that describes the inner edge of the border.
    /// </summary>
    public abstract Path GetInnerPath(in Rect rect);

    public virtual void LerpTo(ShapeBorder? to, ShapeBorder tween, double t)
    {
        //TODO: if (b == null) ScaleTo(1.0 - t)
    }

    /// <summary>
    /// Paints the border within the given [Rect] on the given [Canvas].
    /// </summary>
    public abstract void Paint(Canvas canvas, in Rect rect, in Color? fillColor = null);

    public abstract ShapeBorder Clone();
}