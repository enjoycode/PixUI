namespace PixUI;

public interface IRender
{
    #region ====Font====

    IFontCollection FontCollection { get; }

    #endregion

    #region ====Paint====

    IPaint PaintShared(in Color? color = null, PaintStyle style = PaintStyle.Fill, float strokeWidth = 1);

    #endregion

    #region ====Image====

    IImage? ImageFromEncodedData(byte[] data);

    IImage? ImageFromEncodedData(Stream data);

    #endregion
}