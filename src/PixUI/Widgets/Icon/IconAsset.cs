namespace PixUI;

public sealed class IconAsset
{
    public IconAsset(string fontFamily, string assemblyName, string assetPath)
    {
        FontFamily = fontFamily;
        AssemblyName = assemblyName;
        AssetPath = assetPath;
    }

    public readonly string FontFamily;
    public readonly string AssemblyName;
    public readonly string AssetPath;
}