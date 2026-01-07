using System.Threading.Tasks;

namespace PixUI;

public static class Clipboard
{
    public static ValueTask WriteText(string text) => UIApplication.Current.ClipboardProvider.WriteText(text);
    public static ValueTask<string?> ReadText() => UIApplication.Current.ClipboardProvider.ReadText();
}

public interface IPlatformClipboard
{
    ValueTask WriteText(string text);
    ValueTask<string?> ReadText();
}