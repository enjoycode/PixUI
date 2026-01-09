using System.IO;
using System.Threading.Tasks;

namespace PixUI.Platform;

public static class FileDialog
{
    public static Task<OpenFileResult[]> OpenFileAsync(OpenFileOptions options) =>
        UIApplication.Current.FileDialogProvider.OpenFileAsync(options);

    public static Task SaveFileAsync(SaveFileOptions options) =>
        UIApplication.Current.FileDialogProvider.SaveFileAsync(options);
}

public interface IPlatformFileDialog
{
    Task<OpenFileResult[]> OpenFileAsync(OpenFileOptions options);

    Task SaveFileAsync(SaveFileOptions options);
}

public readonly struct OpenFileOptions
{
    public OpenFileOptions() { }

    public string Title { get; init; } = string.Empty;

    public bool AllowMultiple { get; init; } = false;

    public int MaxFileSize { get; init; } = 2 * 1024 * 1024;
}

public readonly struct OpenFileResult
{
    public OpenFileResult() { }

    public string FileName { get; init; } = string.Empty;

    /// <summary>
    /// 待读取文件的输出流
    /// </summary>
    public Stream FileStream { get; init; } = Stream.Null;
}

public readonly struct SaveFileOptions
{
    public SaveFileOptions() { }

    public string Title { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;

    /// <summary>
    /// 待写入文件的输入流
    /// </summary>
    public Stream FileStream { get; init; } = Stream.Null;
}