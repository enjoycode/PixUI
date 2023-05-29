namespace PixUI;

[TSType("CanvasKit.PaintStyle")]
public enum PaintStyle
{
    Fill = 0,
    Stroke = 1,
    //StrokeAndFill = 2,
}

[TSType("CanvasKit.StrokeCap")]
public enum StrokeCap
{
    Butt = 0,
    Round = 1,
    Square = 2,
}

[TSType("CanvasKit.StrokeJoin")]
public enum StrokeJoin
{
    Miter = 0,
    Round = 1,
    Bevel = 2,
}

[TSType("CanvasKit.ClipOp")]
public enum ClipOp
{
    Difference = 0,
    Intersect = 1,
}

[TSType("CanvasKit.PathOp")]
public enum PathOp
{
    Difference = 0,
    Intersect = 1,
    Union = 2,
    Xor = 3,
    ReverseDifference = 4,
}

[TSType("CanvasKit.BlendMode")]
public enum BlendMode
{
    Clear = 0,
    Src = 1,
    Dst = 2,
    SrcOver = 3,
    DstOver = 4,
    SrcIn = 5,
    DstIn = 6,
    SrcOut = 7,
    DstOut = 8,
    SrcATop = 9,
    DstATop = 10,
    Xor = 11,
    Plus = 12,
    Modulate = 13,
    Screen = 14,
    Overlay = 15,
    Darken = 16,
    Lighten = 17,
    ColorDodge = 18,
    ColorBurn = 19,
    HardLight = 20,
    SoftLight = 21,
    Difference = 22,
    Exclusion = 23,
    Multiply = 24,
    Hue = 25,
    Saturation = 26,
    Color = 27,
    Luminosity = 28,
}

[TSType("CanvasKit.ColorType")]
public enum ColorType
{
    //Unknown = 0,
    Alpha8 = 1,
    Rgb565 = 2,

    //Argb4444 = 3,
    Rgba8888 = 4,

    //Rgb888x = 5,
    Bgra8888 = 6,
    Rgba1010102 = 7,
    Rgb101010x = 8,
    Gray8 = 9,
    RgbaF16 = 10,

    //RgbaF16Clamped = 11,
    RgbaF32 = 12,
    //Rg88 = 13,
    //AlphaF16 = 14,
    //RgF16 = 15,
    //Alpha16 = 16,
    //Rg1616 = 17,
    //Rgba16161616 = 18,
    //Bgra1010102 = 19,
    //Bgr101010x = 20,
}

[TSType("CanvasKit.AlphaType")]
public enum AlphaType
{
    Unknown = 0,
    Opaque = 1,
    Premul = 2,
    Unpremul = 3,
}

[TSType("CanvasKit.BlurStyle")]
public enum BlurStyle
{
    Normal = 0,
    Solid = 1,
    Outer = 2,
    Inner = 3,
}

[TSType("CanvasKit.FontSlant")]
public enum FontSlant
{
    Upright = 0,
    Italic = 1,
    Oblique = 2,
}

[TSType("CanvasKit.FontWeight")]
public enum FontWeight
{
    Invisible = 0,
    Thin,
    ExtraLight,
    Light,
    Normal,
    Medium,
    SemiBold,
    Bold,
    ExtraBold,
    Black,
    ExtraBlack,
}

[TSType("CanvasKit.TextDirection")]
public enum TextDirection
{
    RTL,
    LTR
}

public enum TextBaseline
{
    /// <summary>
    /// The horizontal line used to align the bottom of glyphs for alphabetic characters.
    /// </summary>
    Alphabetic,

    /// <summary>
    /// The horizontal line used to align ideographic characters.
    /// </summary>
    Ideographic
}

[TSType("CanvasKit.TextAlign")]
public enum TextAlign
{
    Left = 0,
    Right = 1,
    Center = 2,
}

[TSType("CanvasKit.Affinity")]
public enum Affinity
{
    Upstream,
    Downstream
}

[TSType("CanvasKit.RectHeightStyle")]
public enum BoxHeightStyle
{
    /// <summary>
    /// Provide tight bounding boxes that fit heights per run.
    /// </summary>
    Tight,

    /// <summary>
    /// The height of the boxes will be the maximum height of all runs in the
    /// line. All rects in the same line will be the same height.
    /// </summary>
    Max,

    /// <summary>
    /// Extends the top and/or bottom edge of the bounds to fully cover any line
    /// spacing. The top edge of each line should be the same as the bottom edge
    /// of the line above. There should be no gaps in vertical coverage given any
    /// ParagraphStyle line_height.
    ///
    /// The top and bottom of each rect will cover half of the
    /// space above and half of the space below the line.
    /// </summary>
    IncludeLineSpacingMiddle,

    /// <summary>
    /// The line spacing will be added to the top of the rect.
    /// </summary>
    IncludeLineSpacingTop,

    /// <summary>
    /// The line spacing will be added to the bottom of the rect.
    /// </summary>
    IncludeLineSpacingBottom,

    Strut
}

[TSType("CanvasKit.RectWidthStyle")]
public enum BoxWidthStyle
{
    /// <summary>
    /// Provide tight bounding boxes that fit widths to the runs of each line
    /// independently.
    /// </summary>
    Tight,

    /// <summary>
    /// Extends the width of the last rect of each line to match the position of
    /// the widest rect over all the lines.
    /// </summary>
    Max
}

[TSType("CanvasKit.Path1DEffectStyle")]
public enum Path1DPathEffectStyle
{
    Translate = 0,
    Rotate = 1,
    Morph = 2,
}

[TSType("CanvasKit.TileMode")]
public enum TileMode
{
    Clamp = 0,
    Repeat = 1,
    Mirror = 2,
    Decal = 3,
}

[TSType("CanvasKit.ColorChannel")]
public enum ColorChannel
{
    R = 0,
    G = 1,
    B = 2,
    A = 3,
}

[TSType("CanvasKit.EncodedImageFormat")]
public enum EncodedImageFormat
{
    // Bmp = 0,
    // Gif = 1,
    // Ico = 2,
    Jpeg = 3,
    Png = 4,

    // Wbmp = 5,
    Webp = 6,
    // Pkm = 7,
    // Ktx = 8,
    // Astc = 9,
    // Dng = 10,
    // Heif = 11,
}

[TSType("CanvasKit.PlaceholderAlignment")]
public enum PlaceholderAlignment
{
    Baseline,
    AboveBaseline,
    BelowBaseline,
    Top,
    Bottom,
    Middle
}