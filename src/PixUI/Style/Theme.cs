using System.Runtime.CompilerServices;

namespace PixUI;

public static class Theme
{
    public static bool IsDark { get; private set; } = false;

    public static Color TextColor { get; private set; } = Colors.Black;

    public static float FontSize { get; private set; } = 15;

    public static Color FocusedColor { get; private set; } = 0xFF2196F3;

    public static float FocusedBorderWidth { get; private set; } = 2.0f;

    public static Color CaretColor { get; private set; } = 0xFF2196F3;

    public static Color AccentColor { get; private set; } = 0xFFF7821B;

    /// <summary>
    /// 禁用的组件的背景色
    /// </summary>
    public static Color DisabledBgColor { get; private set; } = 0xFFF5F7FA;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetDefaultStyle<T>() where T : Style, IStyle<T>, new()
    {
        T.Default ??= new T() { IsDefault = true };
        return T.Default;
    }

    public static void ApplyTheme(ITheme theme)
    {
        theme.OnApply();
        //TODO: relayout all
    }
}

public interface ITheme
{
    void OnApply();
}

// public class LightTheme : ITheme
// {
//     public void ApplyTheme()
//     {
//         Theme.FontSize = 13f;
//         ButtonStyle.Default = new ButtonStyle()
//         {
//             IsDefault = true,
//             FontSize = 15f,
//         };
//     }
// }