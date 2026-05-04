#if !__WEB__
namespace PixUI;

public readonly struct FontStyle
{
    public readonly FontWeight? Weight;
    public readonly FontSlant? Slant;
    //TODO: 与skia定义一致

    public FontStyle()
    {
        Weight = FontWeight.Normal;
        Slant = FontSlant.Upright;
    }

    public FontStyle(FontWeight weight, FontSlant slant)
    {
        Weight = weight;
        Slant = slant;
    }
}
#endif