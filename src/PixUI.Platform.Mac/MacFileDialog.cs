namespace PixUI.Platform.Mac;

internal sealed class MacFileDialog : IPlatformFileDialog
{
    public Task<OpenFileResult[]> OpenFileAsync(OpenFileOptions options)
    {
        var taskCompletionSource = new TaskCompletionSource<OpenFileResult[]>();
        var dialog = NSOpenPanel.OpenPanel;
        dialog.Title = options.Title;
        dialog.CanChooseDirectories = false;
        dialog.AllowsMultipleSelection = options.AllowMultiple;
        // dialog.AllowedFileTypes = ["png"];
        // dialog.AllowedContentTypes = [UTTypes.Png, UTTypes.Url];
        dialog.Begin(result =>
        {
            if (result == 1)
            {
                var results = new OpenFileResult[dialog.Urls.Length];
                for (var i = 0; i < dialog.Urls.Length; i++)
                {
                    results[i] = new OpenFileResult()
                    {
                        FileName = System.IO.Path.GetFileName(dialog.Urls[i].Path!),
                        FileStream = File.OpenRead(dialog.Urls[i].Path!),
                    };
                }

                taskCompletionSource.SetResult(results);
            }
            else
            {
                taskCompletionSource.SetResult([]);
            }
        });

        return taskCompletionSource.Task;
    }

    public Task SaveFileAsync(SaveFileOptions options)
    {
        throw new NotImplementedException();
    }
}