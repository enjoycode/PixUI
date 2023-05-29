namespace PixUI;

/// <summary>
/// A ShapeBorder that draws an outline with the width and color specified by [side].
/// </summary>
public abstract class OutlinedBorder : ShapeBorder
{
    public readonly BorderSide Side;

    public override EdgeInsets Dimensions => EdgeInsets.All(Side.Width);

    public OutlinedBorder(BorderSide? side)
    {
        Side = side ?? BorderSide.Empty;
    }

    /// <summary>
    /// Returns a copy of this OutlinedBorder that draws its outline with the
    /// specified [side], if [side] is non-null.
    /// </summary>
    public abstract OutlinedBorder CopyWith(BorderSide? side);
}