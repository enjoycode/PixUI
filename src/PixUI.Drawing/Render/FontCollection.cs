namespace PixUI;

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
        add => Render.Provider.FontCollection.FontChanged += value;
        remove => Render.Provider.FontCollection.FontChanged -= value;
    }

    public static bool HasLoading(string familyName) => Render.Provider.FontCollection.HasLoading(familyName);

    public static ITypeface? TryMatchFamilyFromAsset(string familyName) =>
        Render.Provider.FontCollection.TryMatchFamilyFromAsset(familyName);

    public static void RegisterTypeface(Stream stream, string fontFAmily, bool isAsset) =>
        Render.Provider.FontCollection.RegisterTypeface(stream, fontFAmily, isAsset);
}