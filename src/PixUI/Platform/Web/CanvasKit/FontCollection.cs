#if __WEB__
using System;

namespace PixUI
{
    [TSType("PixUI.FontCollection")]
    public sealed class FontCollection
    {
        public static readonly FontCollection Instance = new();
        
        public event Action? FontChanged;

        public Typeface? TryMatchFamilyFromAsset(string familyName) => null;

        public bool StartLoadFontFromAsset(string asmName, string assetPath, string familyName)
            => false;
    }
}
#endif