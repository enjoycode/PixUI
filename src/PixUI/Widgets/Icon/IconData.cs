namespace PixUI;

public readonly struct IconData
{
    public readonly int CodePoint;
    public readonly string FontFamily;
    public readonly string AssemblyName;
    public readonly string AssetPath;

    public IconData(int codePoint, string fontFamily, string assemblyName, string assetPath)
    {
        CodePoint = codePoint;
        FontFamily = fontFamily;
        AssemblyName = assemblyName;
        AssetPath = assetPath;
    }

    public IconData Clone() => new IconData(CodePoint, FontFamily, AssemblyName, AssetPath);
}