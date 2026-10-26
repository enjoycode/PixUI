using System.IO;
using PixUI.Platform;

public class DemoFileDialog : View
{
    public DemoFileDialog()
    {
        Child = new Center
        {
            Child = new Row()
            {
                Spacing = 10,
                Children =
                [
                    new Button("Open File") { OnTap = OnOpenFile },
                    new Button("Save File") { OnTap = OnSaveFile },
                ]
            }
        };
    }
    
    private async void OnOpenFile(PointerEvent e)
    {
        var files = await FileDialog.OpenFileAsync(new OpenFileOptions() { Title = "打开文件" });
        foreach (var file in files)
        {
            Console.WriteLine($"打开文件: {file.FileName} {file.FileStream.Length}");
            await file.FileStream.DisposeAsync();
        }
    }

    private async void OnSaveFile(PointerEvent e)
    {
        var ms = new MemoryStream();
        var textWriter = new StreamWriter(ms);
        await textWriter.WriteLineAsync("Hello World!");
        await textWriter.FlushAsync();
        ms.Position = 0;

        await FileDialog.SaveFileAsync(new SaveFileOptions()
        {
            FileName = "HelloWorld.txt",
            FileStream = ms,
        });
    }
}