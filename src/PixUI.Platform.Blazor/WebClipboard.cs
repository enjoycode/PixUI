using System.Runtime.Versioning;

namespace PixUI.Platform.Blazor;

[SupportedOSPlatform("browser")]
public sealed class WebClipboard : IPlatformClipboard
{
    public ValueTask WriteText(string text) => new(WebBrowser.ClipboardWriteText(text));

    public ValueTask<string?> ReadText() => new(WebBrowser.ClipboardReadText());
}