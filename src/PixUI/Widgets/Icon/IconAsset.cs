using System.Diagnostics;

namespace PixUI;

public sealed class IconAsset
{
    public IconAsset(string fontFamily, string assemblyName, string assetPath)
    {
        FontFamily = fontFamily;
        AssemblyName = assemblyName;
        AssetPath = assetPath;

        using var stream = AssetLoader.LoadAsStream(assemblyName, assetPath);
        Debug.Assert(stream != null);
        Typeface = FontCollection.RegisterTypeface(stream, fontFamily, true)!;
    }

    public readonly string FontFamily;
    public readonly string AssemblyName;
    public readonly string AssetPath;
    public ITypeface Typeface { get; }
}