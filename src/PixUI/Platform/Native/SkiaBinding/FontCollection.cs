#if !__WEB__
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using PixUI.Platform;

namespace PixUI
{
    public sealed class FontCollection
    {
        public const string DefaultFamilyName = "MiSans";

        public static readonly FontCollection Instance = new();

        private readonly IntPtr _fontCollectionHandle;
        private readonly IntPtr _assetFontMgrHandle;

        private readonly HashSet<string> _loading = new();
        private readonly Dictionary<string, Typeface> _loaded = new();

        internal IntPtr Handle => _fontCollectionHandle;

        public event Action? FontChanged;

        private FontCollection()
        {
            _assetFontMgrHandle = SkiaApi.sk_typeface_font_provider_new();
            var isWasm = RuntimeInformation.ProcessArchitecture.ToString() == "Wasm";
            _fontCollectionHandle = SkiaApi.sk_font_collection_new(_assetFontMgrHandle, isWasm);
        }

        public void RegisterTypefaceToAsset(SKData data, string fontFamily, bool raiseEvent = true)
        {
            var typeface = Typeface.FromData(data);
            if (typeface == null)
            {
                Console.WriteLine("Can't create Typeface from data");
                return;
            }

            SkiaApi.sk_typeface_font_provider_register_typeface(_assetFontMgrHandle, typeface.Handle);
#if DEBUG
            Console.WriteLine($"FontCollection.RegisterTypefaceToAsset: {typeface.FamilyName}");
#endif

            _loaded.Add(fontFamily, typeface);
            if (raiseEvent)
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
        public Typeface? TryMatchFamilyFromAsset(string familyName)
        {
            return _loaded.TryGetValue(familyName, out var typeface) ? typeface : null;
        }

        public bool StartLoadFontFromAsset(string asmName, string assetPath, string familyName)
        {
            if (!_loading.Add(familyName))
                return false;

            Task.Run(() =>
            {
                var stream = AssetLoader.LoadAsStream(asmName, assetPath);
                if (stream == null) return;

                var data = SKData.Create(stream);
                stream.Dispose();
                UIApplication.Current.BeginInvoke(() =>
                {
                    RegisterTypefaceToAsset(data, familyName);
                    data.Dispose();
                });
            });

            return true;
        }
    }
}
#endif