using System.IO;
using System.Threading.Tasks;

namespace PixUI.Platform;

public static class FileDialog
{
    public static Task<Stream> OpenFileAsync(OpenFileOptions options) =>
        UIApplication.Current.FileDialogProvider.OpenFileAsync(options);

    public static Task<Stream> SaveFileAsync(SaveFileOptions options) =>
        UIApplication.Current.FileDialogProvider.SaveFileAsync(options);
}

public interface IPlatformFileDialog
{
    Task<Stream> OpenFileAsync(OpenFileOptions options);

    Task<Stream> SaveFileAsync(SaveFileOptions options);
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
}