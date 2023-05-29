using System.Threading.Tasks;

namespace PixUI;

public static class Clipboard
{
    private static IPlatformClipboard _platformClipboard = null!;

    public static void Init(IPlatformClipboard platformClipboard)
    {
        _platformClipboard = platformClipboard;
    }

    public static ValueTask WriteText(string text) => _platformClipboard.WriteText(text);
    public static ValueTask<string?> ReadText() => _platformClipboard.ReadText();
}

public interface IPlatformClipboard
{
    ValueTask WriteText(string text);
    ValueTask<string?> ReadText();
}