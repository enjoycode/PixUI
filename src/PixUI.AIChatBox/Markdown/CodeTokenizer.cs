using TextMateSharp.Grammars;
using TextMateSharp.Registry;

namespace PixUI.AIChatBox;

internal static class CodeTokenizer
{
    static CodeTokenizer()
    {
        Registry = new Registry(new RegistryOptions(ThemeName.LightPlus));

        Theme = Registry.GetTheme();
        var guiColors = Theme.GetGuiColorDictionary();
        EditorForeground = Color.Parse(guiColors["editor.foreground"]);
        EditorBackground = Color.Parse(guiColors["editor.background"]);
    }

    private static readonly Registry Registry;
    private static readonly TextMateSharp.Themes.Theme Theme;

    public static Color EditorForeground { get; private set; }
    public static Color EditorBackground { get; private set; }

    public static Color GetColor(int colorId)
    {
        if (colorId < 0 || colorId == 1) return EditorForeground;
        if (colorId == 2) return EditorBackground;
        return Color.Parse(Theme.GetColor(colorId));
    }

    public static IGrammar? GetGramma(string? language) =>
        language switch
        {
            "csharp" or "c#" => Registry.LoadGrammar("source.cs"),
            "json" => Registry.LoadGrammar("source.json"),
            _ => null
        };
}