namespace PixUI;

public readonly struct IconData
{
    public readonly int CodePoint;
    public readonly IconAsset Asset;

    public IconData(int codePoint, IconAsset asset)
    {
        CodePoint = codePoint;
        Asset = asset;
    }

#if __WEB__    
    public IconData Clone() => new IconData(CodePoint, FontFamily, AssemblyName, AssetPath);
#endif
}