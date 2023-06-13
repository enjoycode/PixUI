using Microsoft.JSInterop;

namespace PixUI.Platform.Blazor;

public sealed class BlazorClipboard : IPlatformClipboard
{
    public ValueTask WriteText(string text)
    {
        return BlazorApplication.JSRuntime.InvokeVoidAsync("ClipboardWriteText", text);
    }

    public ValueTask<string?> ReadText()
    {
        return BlazorApplication.JSRuntime.InvokeAsync<string?>("ClipboardReadText");
    }
}