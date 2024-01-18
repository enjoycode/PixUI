using AppKit;
using Foundation;

namespace PixUI.Platform.Mac;

public sealed class AppDelegate : NSApplicationDelegate
{
    private int _done = 0;

    internal bool IsDone => System.Threading.Volatile.Read(ref _done) == 1;

    public override void DidFinishLaunching(NSNotification notification)
    {
        //Stops the main event loop
        NSApplication.SharedApplication.Stop(NSApplication.SharedApplication);
    }

    public override void WillTerminate(NSNotification notification)
    {
        // Insert code here to tear down your application
    }

    public override NSApplicationTerminateReply ApplicationShouldTerminate(NSApplication sender)
    {
        System.Threading.Interlocked.Exchange(ref _done, 1);
        return NSApplicationTerminateReply.Now; //暂直接退出
    }

    public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
    {
        return true;
    }

    public override bool SupportsSecureRestorableState(NSApplication application) => true;
}