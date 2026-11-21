namespace PixUI;

public readonly struct IconData : IEquatable<IconData>
{
    public readonly int CodePoint;
    public readonly IconAsset Asset;

    public IconData(int codePoint, IconAsset asset)
    {
        CodePoint = codePoint;
        Asset = asset;
    }

#if __WEB__
    public IconData Clone() => new IconData(CodePoint, Asset);
#endif

    public bool Equals(IconData other) => CodePoint == other.CodePoint && ReferenceEquals(Asset, other.Asset);

    public override bool Equals(object? obj) => obj is IconData other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(CodePoint, Asset);
}