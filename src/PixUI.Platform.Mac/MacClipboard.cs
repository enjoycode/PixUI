using System.Threading.Tasks;
using AppKit;

namespace PixUI.Platform.Mac;

internal sealed class MacClipboard : IPlatformClipboard
{
    private const string StringDataType = "public.utf8-plain-text";

    public ValueTask WriteText(string text)
    {
        var pasteboard = NSPasteboard.GeneralPasteboard;
        pasteboard.ClearContents(); // Empty the current contents
        pasteboard.SetStringForType(text, StringDataType);
        return new ValueTask();
    }

    public ValueTask<string?> ReadText()
    {
        var pasteboard = NSPasteboard.GeneralPasteboard;
        var text = pasteboard.GetStringForType(StringDataType);
        return new ValueTask<string?>(text);
    }
}