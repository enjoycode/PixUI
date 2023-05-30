using System.Threading.Tasks;
using AppKit;

namespace PixUI.Platform.Mac;

internal sealed class MacClipboard : IPlatformClipboard
{
    public ValueTask WriteText(string text)
    {
        var pasteboard = NSPasteboard.GeneralPasteboard;
        pasteboard.ClearContents(); // Empty the current contents
        pasteboard.SetStringForType(text, "string");
        return new ValueTask();
    }

    public ValueTask<string?> ReadText()
    {
        var pasteboard = NSPasteboard.GeneralPasteboard;
        var text = pasteboard.GetStringForType("string");
        return new ValueTask<string?>(text);
    }
}