#if !__WEB__
using System;

namespace PixUI;

public sealed class TextStyle : SKObject
{
    private TextStyle(IntPtr handle, bool owns) : base(handle, owns) { }

    public TextStyle() : this(SkiaApi.sk_text_style_new(), true)
    {
        if (OperatingSystem.IsBrowser())
            SetFontFamilies(new[] { FontCollection.DefaultFamilyName });
    }

    protected override void DisposeNative() => SkiaApi.sk_text_style_delete(Handle);

    public Color Color
    {
        get => SkiaApi.sk_text_style_get_color(Handle);
        set => SkiaApi.sk_text_style_set_color(Handle, value);
    }

    public float FontSize
    {
        get => SkiaApi.sk_text_style_get_font_size(Handle);
        set => SkiaApi.sk_text_style_set_font_size(Handle, value);
    }

    public FontStyle FontStyle
    {
        get
        {
            var slant = (FontSlant)SkiaApi.sk_text_style_get_font_style(Handle);
            var weight = (FontWeight)SkiaApi.sk_text_style_get_font_weight(Handle);
            return new FontStyle(weight, slant);
        }
        set
        {
            if (value.Weight != null)
                SkiaApi.sk_text_style_set_font_weight(Handle, (int)value.Weight);
            if (value.Slant != null)
                SkiaApi.sk_text_style_set_font_style(Handle, (int)value.Slant);
        }
    }

    public float Height
    {
        get => SkiaApi.sk_text_style_get_height(Handle);
        set => SkiaApi.sk_text_style_set_height(Handle, value);
    }

    public TextBaseline TextBaseline
    {
        get => (TextBaseline)SkiaApi.sk_text_style_get_text_baseline(Handle);
        set => SkiaApi.sk_text_style_set_text_baseline(Handle, (int)value);
    }

    public unsafe void AddShadow(Shadow shadow)
    {
        SkiaApi.sk_text_style_add_shadow(Handle, &shadow);
    }

    public void ResetShadows() => SkiaApi.sk_text_style_reset_shadows(Handle);

    public unsafe void SetFontFamilies(string[] familyNames)
    {
        var names = new SKString[familyNames.Length];
        for (var i = 0; i < familyNames.Length; i++)
        {
            names[i] = new SKString(familyNames[i]);
        }

        fixed (IntPtr* ptrs = stackalloc IntPtr[names.Length])
        {
            for (var i = 0; i < names.Length; i++)
            {
                ptrs[i] = names[i].Handle;
            }

            SkiaApi.sk_text_style_set_font_families(Handle, new IntPtr(ptrs), familyNames.Length);
        }

        for (var i = 0; i < names.Length; i++)
        {
            names[i].Dispose();
        }
    }
}

public readonly struct FontStyle
{
    public readonly FontWeight? Weight;
    public readonly FontSlant? Slant;

    public FontStyle(FontWeight weight, FontSlant slant)
    {
        Weight = weight;
        Slant = slant;
    }
}
#endif