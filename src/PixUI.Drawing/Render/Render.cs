namespace PixUI;

public interface IRender
{
    #region ====Font====

    IFontCollection FontCollection { get; }

    #endregion

    #region ====Paint====

    IPaint MakePaint();

    IPaint PaintShared(in Color? color = null, PaintStyle style = PaintStyle.Fill, float strokeWidth = 1);

    #endregion

    #region ====Image====

    IImage? ImageFromEncodedData(byte[] data);

    IImage? ImageFromEncodedData(Stream data);

    #endregion

    IPath MakePath();

    IPathEffect? PathEffectCreateDash(float[] intervals, float phase);
}

public static class Render
{
    public static IRender Provider { get; private set; } = null!;

    public static void Init(IRender provider)
    {
        Provider = provider;
    }
}