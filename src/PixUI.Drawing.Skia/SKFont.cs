namespace PixUI.Drawing.Skia;

public sealed unsafe class SKFont : SKObject, IFont
{
    private SKFont(IntPtr handle, bool owns) : base(handle, owns) { }

    public SKFont(SKTypeface typeface, float sizeInPoints, float scaleX = 1, float skewX = 0)
        : this(SkiaApi.sk_font_new_with_values(typeface.Handle, sizeInPoints, scaleX, skewX), true)
    {
        if (Handle == IntPtr.Zero)
            throw new InvalidOperationException("Unable to create a new Font instance.");
    }

    protected override void DisposeNative() => SkiaApi.sk_font_delete(Handle);

    public string Name => ((SKTypeface)Typeface!).FamilyName;

    public bool ForceAutoHinting
    {
        get => SkiaApi.sk_font_is_force_auto_hinting(Handle);
        set => SkiaApi.sk_font_set_force_auto_hinting(Handle, value);
    }

    public bool EmbeddedBitmaps
    {
        get => SkiaApi.sk_font_is_embedded_bitmaps(Handle);
        set => SkiaApi.sk_font_set_embedded_bitmaps(Handle, value);
    }

    public bool Subpixel
    {
        get => SkiaApi.sk_font_is_subpixel(Handle);
        set => SkiaApi.sk_font_set_subpixel(Handle, value);
    }

    public bool LinearMetrics
    {
        get => SkiaApi.sk_font_is_linear_metrics(Handle);
        set => SkiaApi.sk_font_set_linear_metrics(Handle, value);
    }

    public bool Embolden
    {
        get => SkiaApi.sk_font_is_embolden(Handle);
        set => SkiaApi.sk_font_set_embolden(Handle, value);
    }

    public bool BaselineSnap
    {
        get => SkiaApi.sk_font_is_baseline_snap(Handle);
        set => SkiaApi.sk_font_set_baseline_snap(Handle, value);
    }

    public SKFontEdging Edging
    {
        get => SkiaApi.sk_font_get_edging(Handle);
        set => SkiaApi.sk_font_set_edging(Handle, value);
    }

    public SKFontHinting Hinting
    {
        get => SkiaApi.sk_font_get_hinting(Handle);
        set => SkiaApi.sk_font_set_hinting(Handle, value);
    }

    public ITypeface? Typeface
    {
        get => SKTypeface.GetObject(SkiaApi.sk_font_get_typeface(Handle));
        set => SkiaApi.sk_font_set_typeface(Handle, (value as SKTypeface)?.Handle ?? IntPtr.Zero);
    }

    /// <summary>
    /// Size in points
    /// </summary>
    public float Size
    {
        get => SkiaApi.sk_font_get_size(Handle);
        set => SkiaApi.sk_font_set_size(Handle, value);
    }

    public float ScaleX
    {
        get => SkiaApi.sk_font_get_scale_x(Handle);
        set => SkiaApi.sk_font_set_scale_x(Handle, value);
    }

    public float SkewX
    {
        get => SkiaApi.sk_font_get_skew_x(Handle);
        set => SkiaApi.sk_font_set_skew_x(Handle, value);
    }

    /// <summary>
    /// 以像素为单位的行距
    /// </summary>
    /// <remarks>
    /// 行距是两个连续文本行的基线之间的垂直距离。 因此，行距包括行之间的空格以及字符本身的高度。
    /// </remarks>
    public int Height => (int)Math.Ceiling(Spacing / 0.75); //暂按96dpi算

    public float Spacing => SkiaApi.sk_font_get_metrics(Handle, null);

    public FontMetrics GetMetrics()
    {
        var res = new FontMetrics();
        SkiaApi.sk_font_get_metrics(Handle, &res);
        return res;
    }

    public ushort GetGlyph(int codepoint) => SkiaApi.sk_font_unichar_to_glyph(Handle, codepoint);

    public float GetGlyphWidth(ushort glyphId)
    {
        float width = 0;
        SkiaApi.sk_font_get_widths_bounds(Handle, &glyphId, 1, &width, null, IntPtr.Zero);
        return width;
    }

    public Size MeasureString(string text)
    {
        fixed (char* ptr = text)
        {
            Rect bounds;
            var width = SkiaApi.sk_font_measure_text(Handle, ptr,
                new IntPtr(text.Length * 2), SKTextEncoding.Utf16,
                &bounds, IntPtr.Zero);
            return new Size(width /*bounds.Width*/, bounds.Height);
        }
    }

    public int CountGlyphs(ReadOnlySpan<byte> text, SKTextEncoding encoding)
    {
        fixed (void* t = text)
        {
            return CountGlyphs(t, text.Length, encoding);
        }
    }

    internal int CountGlyphs(void* text, int length, SKTextEncoding encoding)
    {
        if (!ValidateTextArgs(text, length, encoding))
            return 0;

        return SkiaApi.sk_font_text_to_glyphs(Handle, text, (IntPtr)length, encoding, null, 0);
    }

    internal ushort[] GetGlyphs(void* text, int length, SKTextEncoding encoding)
    {
        if (!ValidateTextArgs(text, length, encoding))
            return [];

        var n = CountGlyphs(text, length, encoding);
        if (n <= 0)
            return [];

        var glyphs = new ushort[n];
        GetGlyphs(text, length, encoding, glyphs);
        return glyphs;
    }

    internal void GetGlyphs(void* text, int length, SKTextEncoding encoding, Span<ushort> glyphs)
    {
        if (!ValidateTextArgs(text, length, encoding))
            return;

        fixed (ushort* gp = glyphs)
        {
            SkiaApi.sk_font_text_to_glyphs(Handle, text, (IntPtr)length, encoding, gp, glyphs.Length);
        }
    }

    public ushort[] GetGlyphs(string text)
    {
        var glyphs = new ushort[text.Length];
        fixed (ushort* glyphsPtr = glyphs)
        fixed (char* charPtr = text)
        {
            var glyphsCount = SkiaApi.sk_font_text_to_glyphs(Handle, charPtr, new IntPtr(text.Length * 2),
                SKTextEncoding.Utf16, glyphsPtr, glyphs.Length);
            if (glyphsCount == glyphs.Length)
                return glyphs;
            return glyphs.AsSpan(0, glyphsCount).ToArray();
        }
    }

    public void GetGlyphPositions(ReadOnlySpan<char> text, Span<Point> offsets, Point origin = default)
    {
        fixed (void* t = text)
        {
            GetGlyphPositions(t, text.Length * 2, SKTextEncoding.Utf16, offsets, origin);
        }
    }

    internal void GetGlyphPositions(void* text, int length, SKTextEncoding encoding, Span<Point> offsets, Point origin)
    {
        if (!ValidateTextArgs(text, length, encoding))
            return;

        var n = offsets.Length;
        if (n <= 0)
            return;

        using var glyphs = Utils.RentArray<ushort>(n);
        GetGlyphs(text, length, encoding, glyphs);
        GetGlyphPositions(glyphs, offsets, origin);
    }

    public void GetGlyphPositions(ReadOnlySpan<ushort> glyphs, Span<Point> positions, Point origin = default)
    {
        if (glyphs.Length != positions.Length)
            throw new ArgumentException("The length of glyphs must be the same as the length of positions.",
                nameof(positions));

        fixed (ushort* gp = glyphs)
        fixed (Point* pp = positions)
        {
            SkiaApi.sk_font_get_pos(Handle, gp, glyphs.Length, pp, &origin);
        }
    }

    public void GetGlyphWidths(ReadOnlySpan<ushort> glyphs, Span<float> widths, Span<Rect> bounds,
        SKPaint? paint = null)
    {
        fixed (ushort* gp = glyphs)
        fixed (float* wp = widths)
        fixed (Rect* bp = bounds)
        {
            var w = widths.Length > 0 ? wp : null;
            var b = bounds.Length > 0 ? bp : null;
            SkiaApi.sk_font_get_widths_bounds(Handle, gp, glyphs.Length, w, b, paint?.Handle ?? IntPtr.Zero);
        }
    }

    public float[] GetGlyphWidths(ReadOnlySpan<ushort> glyphs)
    {
        var res = new float[glyphs.Length];
        fixed (float* widthsPtr = res)
        fixed (ushort* glyphsPtr = glyphs)
        {
            SkiaApi.sk_font_get_widths_bounds(Handle, glyphsPtr, glyphs.Length, widthsPtr, null, IntPtr.Zero);
        }

        return res;
    }

    public Rect[] GetBounds(ReadOnlySpan<ushort> glyphs)
    {
        var bounds = new Rect[glyphs.Length];
        fixed (Rect* boundsPtr = bounds)
        fixed (ushort* glyphsPtr = glyphs)
        {
            SkiaApi.sk_font_get_widths_bounds(Handle, glyphsPtr, glyphs.Length, null, boundsPtr, IntPtr.Zero);
        }

        return bounds;
    }

    private static bool ValidateTextArgs(void* text, int length, SKTextEncoding encoding)
    {
        if (length == 0)
            return false;

        if (text == null)
            throw new ArgumentNullException(nameof(text));

        return true;
    }
}