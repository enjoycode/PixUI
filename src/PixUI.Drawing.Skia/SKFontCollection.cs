namespace PixUI.Drawing.Skia;

public sealed class SKFontCollection : IFontCollection
{
    private readonly IntPtr _fontCollectionHandle;
    private readonly IntPtr _assetFontMgr;

    private readonly HashSet<string> _loading = new();
    private readonly Dictionary<string, SKTypeface> _loaded = new();

    internal IntPtr Handle => _fontCollectionHandle;

    public event Action? FontChanged;

    internal SKFontCollection()
    {
        _assetFontMgr = SkiaApi.sk_typeface_font_provider_new();
        _fontCollectionHandle = SkiaApi.sk_font_collection_new(_assetFontMgr, OperatingSystem.IsBrowser());
    }

    public bool HasAny => _loading.Count > 0;

    public void RegisterTypeface(Stream stream, string fontFAmily, bool isAsset)
    {
        using var data = SKData.Create(stream);
        if (data == null)
            throw new Exception("Can't create SKData from stream");
        RegisterTypeface(data, fontFAmily, isAsset);
    }

    /// <summary>
    /// 加载并注册字体
    /// </summary>
    public void RegisterTypeface(SKData data, string fontFamily, bool isAsset)
    {
        SKTypeface? typeface;
        if (OperatingSystem.IsBrowser())
        {
            typeface = SKTypeface.FromData(data);
        }
        else
        {
            var defaultFontMgr = SkiaApi.sk_font_collection_get_fallback_manager(Handle);
            typeface = SKTypeface.GetObject(SkiaApi.sk_fontmgr_create_from_data(defaultFontMgr, data.Handle, 0));
        }

        if (typeface == null)
        {
            Console.WriteLine($"Can't create Typeface[{fontFamily}] from data");
            return;
        }

        SkiaApi.sk_typeface_font_provider_register_typeface(_assetFontMgr, typeface.Handle);
        Console.WriteLine($"FontCollection.RegisterTypeface: {typeface.FamilyName}");

        _loaded[fontFamily] = typeface;
        if (isAsset)
            FontChanged?.Invoke();
    }

    public unsafe ITypeface? FindTypeface(string familyName, bool bold, bool italic)
    {
        fixed (char* namePtr = familyName)
        {
            var typefaceHandler = SkiaApi.sk_font_collection_find_typeface(_fontCollectionHandle,
                new IntPtr(namePtr), familyName.Length * 2, bold, italic);
            return typefaceHandler == IntPtr.Zero
                ? null
                : SKTypeface.GetDisposeProtectedObject(typefaceHandler);
        }
    }

    public ITypeface? DefaultFallback(int unicode, string? familyName, bool bold, bool italic)
    {
        var defaultFontMgr = SkiaApi.sk_font_collection_get_fallback_manager(Handle);
        var fontManager = new SKFontManager(defaultFontMgr, false);
        var fontStyle = SKFontStyle.Make(bold, italic);
        var typeface = fontManager.MatchCharacter(familyName, fontStyle.Weight, fontStyle.Width,
            fontStyle.Slant, null, unicode);
        if (typeface == null && !string.IsNullOrEmpty(familyName))
            typeface = fontManager.MatchCharacter(null, fontStyle.Weight, fontStyle.Width,
                fontStyle.Slant, null, unicode);
        return typeface;
    }

    /// <summary>
    /// 仅从资源中匹配，目前仅用于加载Icon及Emoji字体
    /// </summary>
    public ITypeface? TryMatchFamilyFromAsset(string familyName) => _loaded.GetValueOrDefault(familyName);

    public bool HasLoading(string familyName) => !_loading.Add(familyName);
}