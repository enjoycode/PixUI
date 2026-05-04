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
        if (OperatingSystem.IsBrowser() /*RuntimeInformation.ProcessArchitecture == Architecture.Wasm*/)
            DefaultFamilyName = "MiSans";
        else if (OperatingSystem.IsMacOS() /*RuntimeInformation.IsOSPlatform(OSPlatform.OSX)*/)
            DefaultFamilyName = "Helvetica Neue";
        else if (OperatingSystem.IsWindows() /*RuntimeInformation.IsOSPlatform(OSPlatform.Windows)*/)
            DefaultFamilyName = "Microsoft YaHei SC";
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

    public static void RegisterTypeface(Stream stream, string fontFAmily, bool isAsset) =>
        Render.Backend.FontCollection.RegisterTypeface(stream, fontFAmily, isAsset);

    public static ITypeface? FindTypeface(string familyName, bool bold, bool italic) =>
        Render.Backend.FontCollection.FindTypeface(familyName, bold, italic);

    public static ITypeface? DefaultFallback(int unicode, string? familyName, bool bold, bool italic) =>
        Render.Backend.FontCollection.DefaultFallback(unicode, familyName, bold, italic);
}