namespace PixUI;

public interface IFontCollection
{
    event Action? FontChanged;

    ITypeface? TryMatchFamilyFromAsset(string familyName);

    bool HasLoading(string familyName);

    void RegisterTypeface(Stream stream, string fontFAmily, bool isAsset);
}