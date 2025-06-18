using System;
using System.Threading.Tasks;
using PixUI.Platform;

namespace PixUI;

public sealed class IconPainter : IDisposable
{
    public IconPainter(Action onFontLoaded)
    {
        _onFontLoaded = onFontLoaded;
    }

    private readonly Action _onFontLoaded;
    private Font? _cachedFont;
    private ushort _cachedGlyphId;
    private bool _loading;

    public void Paint(Canvas canvas, float size, in Color color, in IconData data,
        float offsetX = 0, float offsetY = 0)
    {
        if (_cachedFont == null)
        {
            var typeface = FontCollection.Instance.TryMatchFamilyFromAsset(data.Asset.FontFamily);
            if (typeface == null)
            {
                if (!_loading)
                {
                    _loading = true;
                    FontCollection.Instance.FontChanged += _OnFontChanged;
                    StartLoadFontFromAsset(data.Asset.AssemblyName, data.Asset.AssetPath, data.Asset.FontFamily);
                }

                return;
            }

            _cachedFont = new Font(typeface, size);
            _cachedGlyphId = _cachedFont.GetGlyphId(data.CodePoint);
        }

        var paint = PixUI.Paint.Shared(color);
        canvas.DrawGlyph(_cachedGlyphId, offsetX, size + offsetY, 0, 0, _cachedFont!, paint);
    }

    private static bool StartLoadFontFromAsset(string asmName, string assetPath, string familyName)
    {
        if (FontCollection.Instance.HasLoading(familyName))
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
                FontCollection.Instance.RegisterTypeface(data, familyName, true);
                data.Dispose();
            });
        });

        return true;
    }

    private void _OnFontChanged()
    {
        FontCollection.Instance.FontChanged -= _OnFontChanged;
        _onFontLoaded();
    }

    public void Reset()
    {
        _cachedFont?.Dispose();
        _cachedFont = null;
        _cachedGlyphId = 0;
        _loading = false;
    }

    public void Dispose()
    {
        _cachedFont?.Dispose();
    }
}