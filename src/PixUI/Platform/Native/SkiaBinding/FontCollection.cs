#if !__WEB__
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using PixUI.Platform;

namespace PixUI;

public sealed class FontCollection
{
    public static readonly string DefaultFamilyName;

    public static readonly FontCollection Instance = new();

    private readonly IntPtr _fontCollectionHandle;
    private readonly IntPtr _assetFontMgrHandle;

    private readonly HashSet<string> _loading = new();
    private readonly Dictionary<string, Typeface> _loaded = new();

    internal IntPtr Handle => _fontCollectionHandle;

    public event Action? FontChanged;

    static FontCollection()
    {
        if (OperatingSystem.IsBrowser() /*RuntimeInformation.ProcessArchitecture == Architecture.Wasm*/)
            DefaultFamilyName = "MiSans";
        else if (OperatingSystem.IsMacOS() /*RuntimeInformation.IsOSPlatform(OSPlatform.OSX)*/)
            DefaultFamilyName = "Helvetica Neue";
        else if (OperatingSystem.IsWindows() /*RuntimeInformation.IsOSPlatform(OSPlatform.Windows)*/)
            DefaultFamilyName = "Microsoft YaHei SC";
        else
            DefaultFamilyName = "sans-serif";
    }

    private FontCollection()
    {
        _assetFontMgrHandle = SkiaApi.sk_typeface_font_provider_new();
        _fontCollectionHandle = SkiaApi.sk_font_collection_new(_assetFontMgrHandle, OperatingSystem.IsBrowser());
    }
    
    public bool HasAny => _loading.Count > 0;

    /// <summary>
    /// 加载并注册字体
    /// </summary>
    public void RegisterTypeface(SKData data, string fontFamily, bool isAsset)
    {
        Typeface? typeface = null;
        if (OperatingSystem.IsBrowser())
        {
            typeface = Typeface.GetObject(SkiaApi.sk_typeface_make_from_data(data.Handle));
        }
        else
        {
            var defaultFontMgr = SkiaApi.sk_font_collection_get_fallback_manager(Handle);
            typeface = Typeface.GetObject(SkiaApi.sk_fontmgr_create_from_data(defaultFontMgr, data.Handle, 0));
        }

        if (typeface == null)
        {
            Log.Error($"Can't create Typeface[{fontFamily}] from data");
            return;
        }

        SkiaApi.sk_typeface_font_provider_register_typeface(_assetFontMgrHandle, typeface.Handle);
        Log.Debug($"FontCollection.RegisterTypeface: {typeface.FamilyName}");

        _loaded[fontFamily] = typeface;
        if (isAsset)
            FontChanged?.Invoke();
    }

    public unsafe Typeface FindTypeface(string familyName, bool bold, bool italic)
    {
        fixed (char* namePtr = familyName)
        {
            var typefaceHandler = SkiaApi.sk_font_collection_find_typeface(_fontCollectionHandle,
                new IntPtr(namePtr), familyName.Length * 2, bold, italic);
            return Typeface.GetObject(typefaceHandler)!; //Typeface.PreventPublicDisposal()
        }
    }

    public unsafe Typeface? DefaultFallback(int unicode, string? familyName, bool bold, bool italic)
    {
        var defaultFontMgr = SkiaApi.sk_font_collection_get_fallback_manager(Handle);
        var fontStyle = SKFontStyle.Make(bold, italic);
        var typeface = SkiaApi.sk_fontmgr_match_family_style_character(defaultFontMgr,
            familyName, &fontStyle, null, 0, unicode);
        if (typeface == IntPtr.Zero && familyName != null)
            typeface = SkiaApi.sk_fontmgr_match_family_style_character(Handle, null, &fontStyle, null, 0, unicode);
        if (typeface == IntPtr.Zero) return null; //TODO:考虑返回默认的
        return new Typeface(typeface, false);
    }

    /// <summary>
    /// 仅从资源中匹配，目前仅用于加载Icon及Emoji字体
    /// </summary>
    public Typeface? TryMatchFamilyFromAsset(string familyName) => _loaded.GetValueOrDefault(familyName);

    public bool StartLoadFontFromAsset(string asmName, string assetPath, string familyName)
    {
        if (!_loading.Add(familyName))
            return false;

        Task.Run(() =>
        {
            using var stream = AssetLoader.LoadAsStream(asmName, assetPath);
            if (stream == null) return;

            var data = SKData.Create(stream);
            if (data == null)
                throw new Exception("Can't create SKData from stream");

            UIApplication.Current.BeginInvoke(() =>
            {
                RegisterTypeface(data, familyName, true);
                data.Dispose();
            });
        });

        return true;
    }
}
#endif