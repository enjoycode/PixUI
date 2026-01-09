using System.IO;
using System.Threading.Tasks;

namespace PixUI.Platform;

public static class FileDialog
{
    public static Task<Stream?> OpenFileAsync(OpenFileOptions options) =>
        UIApplication.Current.FileDialogProvider.OpenFileAsync(options);

    public static Task SaveFileAsync(SaveFileOptions options) =>
        UIApplication.Current.FileDialogProvider.SaveFileAsync(options);
}

public interface IPlatformFileDialog
{
    Task<Stream?> OpenFileAsync(OpenFileOptions options);

    Task SaveFileAsync(SaveFileOptions options);
}

public readonly struct OpenFileOptions
{
    public OpenFileOptions() { }

    public string Title { get; init; } = string.Empty;
}

public readonly struct SaveFileOptions
{
    public SaveFileOptions() { }

    public string Title { get; init; } = string.Empty;

    public string FileName { get; init; } = string.Empty;

    public Stream FileStream { get; init; } = Stream.Null;
}