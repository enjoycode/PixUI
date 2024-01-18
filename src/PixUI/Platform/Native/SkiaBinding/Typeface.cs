#if !__WEB__
using System;

namespace PixUI;

public sealed class Typeface : SKObject, ISKReferenceCounted
{
    internal static Typeface? GetObject(IntPtr handle) =>
        GetOrAddObject(handle, (h, o) => new Typeface(h, o));

    public static Typeface? FromData(SKData data, int index = 0)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        return GetObject(SkiaApi.sk_typeface_create_from_data(data.Handle, index));
    }

    internal Typeface(IntPtr handle, bool owns) : base(handle, owns) { }

    public string FamilyName => (string)SKString.GetObject(SkiaApi.sk_typeface_get_family_name(Handle))!;

    public int FontWeight => SkiaApi.sk_typeface_get_font_weight(Handle);

    public int FontWidth => SkiaApi.sk_typeface_get_font_width(Handle);

    public FontSlant FontSlant => SkiaApi.sk_typeface_get_font_slant(Handle);

    // public bool IsBold => FontStyle.Weight >= (int)SKFontStyleWeight.SemiBold;
    //
    // public bool IsItalic => FontStyle.Slant != SKFontStyleSlant.Upright;

    public bool IsFixedPitch => SkiaApi.sk_typeface_is_fixed_pitch(Handle);

    public int UnitsPerEm => SkiaApi.sk_typeface_get_units_per_em(Handle);
}
#endif