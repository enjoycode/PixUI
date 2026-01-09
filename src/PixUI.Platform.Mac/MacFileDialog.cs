namespace PixUI.Platform.Mac;

internal sealed class MacFileDialog : IPlatformFileDialog
{
    public Task<Stream?> OpenFileAsync(OpenFileOptions options)
    {
        var taskCompletionSource = new TaskCompletionSource<Stream?>();
        var dialog = NSOpenPanel.OpenPanel;
        dialog.Title = options.Title;
        dialog.CanChooseDirectories = false;
        dialog.AllowsMultipleSelection = false;
        // dialog.AllowedFileTypes = ["png"];
        // dialog.AllowedContentTypes = [UTTypes.Png, UTTypes.Url];
        dialog.Begin(result =>
        {
            if (result == 1)
            {
                var filePath = dialog.Url.Path!;
                taskCompletionSource.SetResult(File.OpenRead(filePath));
            }
            else
            {
                taskCompletionSource.SetResult(null);
            }
        });

        return taskCompletionSource.Task;
    }

    public Task SaveFileAsync(SaveFileOptions options)
    {
        throw new NotImplementedException();
    }
}