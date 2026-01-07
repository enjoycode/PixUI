using System.Collections.Generic;
using PixUI.Platform.Mac;
using UniformTypeIdentifiers;

namespace PixUI.Demo.Mac;

internal static class MainClass
{
    private static void Main(string[] args)
    {
        MacApplication.Run(new DemoRoute());
        // MacApplication.Run(new DemoPage());
    }
}

internal class DemoPage : View
{
    public DemoPage()
    {
        Child = new Center()
        {
            Child = new Button("Click") { OnTap = _ => OnTap() }
        };
    }

    private void OnTap()
    {
        var dialog = NSOpenPanel.OpenPanel;
        dialog.Title = "请选择文件";
        dialog.CanChooseDirectories = false;
        dialog.AllowsMultipleSelection = false;
        // dialog.AllowedFileTypes = ["png"];
        dialog.AllowedContentTypes = [UTTypes.Png, UTTypes.Url];
        //不要使用同步方法dialog.RunModal()
        dialog.Begin(result =>
        {
            Console.WriteLine(
                $"对话框关闭: {result} {UIApplication.Current.UIThread.ManagedThreadId} {Environment.CurrentManagedThreadId}");
        });
        Console.WriteLine("Done");
    }
}