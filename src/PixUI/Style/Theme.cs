namespace PixUI;

public static class Theme
{
    public static bool IsDark { get; private set; } = false;

    public static float DefaultFontSize { get; private set; } = 15;

    public static Color FocusedColor { get; private set; } = 0xFF2196F3;

    public static float FocusedBorderWidth { get; private set; } = 2.0f;

    public static Color CaretColor { get; private set; } = 0xFF2196F3;

    public static Color AccentColor { get; private set; } = 0xFFF7821B;

    /// <summary>
    /// 禁用的组件的背景色
    /// </summary>
    public static Color DisabledBgColor { get; private set; } = 0xFFF5F7FA;

    public static void ApplyTheme(ITheme theme)
    {
        //TODO:
    }

    private static readonly Dictionary<Type, Style> Styles = new();

    public static T GetStyle<T>() where T : Style, new()
    {
        var type = typeof(T);
        if (Styles.TryGetValue(type, out var style))
            return (T)style;

        var res = new T { IsDefault = true };
        Styles[type] = res;
        return res;
    }
}

public interface ITheme
{
    bool IsDark { get; }

    Dictionary<Type, Style> Styles { get; }
}