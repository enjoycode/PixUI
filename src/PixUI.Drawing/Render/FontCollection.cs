namespace PixUI;

public interface IFontCollection
{
    event Action? FontChanged;

    bool HasAny { get; }

    ITypeface? TryMatchFamilyFromAsset(string familyName);

    bool HasLoading(string familyName);

    void RegisterTypeface(Stream stream, string fontFAmily, bool isAsset);

    ITypeface? FindTypeface(string familyName, bool bold, bool italic);

    ITypeface? DefaultFallback(int unicode, string? familyName, bool bold, bool italic);
}

public static class FontCollection
{
    public static readonly string DefaultFamilyName;

    static FontCollection()
    {
        if (OperatingSystem.IsBrowser())
            DefaultFamilyName = "MiSans";
        else if (OperatingSystem.IsMacOS())
            DefaultFamilyName = "Helvetica Neue";
        else if (OperatingSystem.IsWindows())
            DefaultFamilyName = "Microsoft YaHei UI";
        else
            DefaultFamilyName = "sans-serif";
    }

    public static event Action? FontChanged
    {
        add => Render.Backend.FontCollection.FontChanged += value;
        remove => Render.Backend.FontCollection.FontChanged -= value;
    }

    public static bool HasAny => Render.Backend.FontCollection.HasAny;

    public static bool HasLoading(string familyName) => Render.Backend.FontCollection.HasLoading(familyName);

    public static ITypeface? TryMatchFamilyFromAsset(string familyName) =>
        Render.Backend.FontCollection.TryMatchFamilyFromAsset(familyName);

    public static void RegisterTypeface(Stream stream, string fontFamily, bool isAsset) =>
        Render.Backend.FontCollection.RegisterTypeface(stream, fontFamily, isAsset);

    public static ITypeface? FindTypeface(string familyName, bool bold, bool italic) =>
        Render.Backend.FontCollection.FindTypeface(familyName, bold, italic);

    public static ITypeface? DefaultFallback(int unicode, string? familyName, bool bold, bool italic) =>
        Render.Backend.FontCollection.DefaultFallback(unicode, familyName, bold, italic);
}