using System.Runtime.InteropServices;

namespace PixUI.Platform.Win;

internal sealed class WinFileDialog : IPlatformFileDialog
{
    private const int ErrorCancelled = unchecked((int)0x800704C7);

    public Task<OpenFileResult[]> OpenFileAsync(OpenFileOptions options)
    {
        var dlg = WinApi.CreateComInstance<IFileOpenDialog>(
            new(ComIds.CID_FileOpenDialog), new(ComIds.IID_IFileOpenDialog));
        if (!string.IsNullOrEmpty(options.Title))
            ThrowIfFailed(dlg.SetTitle(options.Title));
        //SetFileTypes(dlg, ("Text files", "*.txt;*.md"), ("All files", "*.*"));
        ThrowIfFailed(dlg.GetOptions(out var dlgOptions));
        dlgOptions = dlgOptions | FOS.FOS_FORCEFILESYSTEM | FOS.FOS_FILEMUSTEXIST | FOS.FOS_PATHMUSTEXIST;
        if (options.AllowMultiple)
            dlgOptions |= FOS.FOS_ALLOWMULTISELECT;
        ThrowIfFailed(dlg.SetOptions(dlgOptions));

        //show dialog
        var res = dlg.Show(IntPtr.Zero); //TODO: should use current window as owner
        if (res == ErrorCancelled)
            return Task.FromResult(Array.Empty<OpenFileResult>());
        ThrowIfFailed(res);

        //get result
        if (options.AllowMultiple)
        {
            ThrowIfFailed(dlg.GetResults(out var items));
            ThrowIfFailed(items.GetCount(out var count));

            var results = new OpenFileResult[count];
            for (uint index = 0; index < count; index++)
            {
                ThrowIfFailed(items.GetItemAt(index, out var item));
                var path = GetSelectedPath(item);
                results[index] = new OpenFileResult()
                    { FileName = System.IO.Path.GetFileName(path), FileStream = File.OpenRead(path) };
            }

            return Task.FromResult(results);
        }
        else
        {
            ThrowIfFailed(dlg.GetResult(out var item));
            var path = GetSelectedPath(item);
            var results = new[]
            {
                new OpenFileResult()
                    { FileName = System.IO.Path.GetFileName(path), FileStream = File.OpenRead(path) }
            };
            return Task.FromResult(results);
        }
    }

    public async Task SaveFileAsync(SaveFileOptions options)
    {
        var dlg = WinApi.CreateComInstance<IFileSaveDialog>(
            new(ComIds.CID_FileSaveDialog), new(ComIds.IID_IFileSaveDialog));
        if (!string.IsNullOrEmpty(options.Title))
            ThrowIfFailed(dlg.SetTitle(options.Title));

        ThrowIfFailed(dlg.GetOptions(out var dlgOptions));
        ThrowIfFailed(dlg.SetOptions(dlgOptions | FOS.FOS_FORCEFILESYSTEM |
                                     FOS.FOS_OVERWRITEPROMPT | FOS.FOS_PATHMUSTEXIST));
        ThrowIfFailed(dlg.SetFileName(options.FileName));
        //ThrowIfFailed(dlg.SetDefaultExtension("txt"));

        //show dialog
        var res = dlg.Show(IntPtr.Zero);
        if (res == ErrorCancelled)
            return;
        ThrowIfFailed(res);
        ThrowIfFailed(dlg.GetResult(out var item));
        var path = GetSelectedPath(item);

        //save file
        await using var fs = File.OpenWrite(path);
        await options.FileStream.CopyToAsync(fs);
    }

    private static void ThrowIfFailed(int result) => Marshal.ThrowExceptionForHR(result);

    private static void SetFileTypes(IFileDialog dialog, params (string Name, string Pattern)[] fileTypes)
    {
        if (fileTypes.Length == 0)
        {
            throw new ArgumentException("At least one file type filter is required.", nameof(fileTypes));
        }

        var filters = new COMDLG_FILTERSPEC[fileTypes.Length];
        try
        {
            for (int i = 0; i < fileTypes.Length; i++)
            {
                filters[i].pszName = Marshal.StringToCoTaskMemUni(fileTypes[i].Name);
                filters[i].pszSpec = Marshal.StringToCoTaskMemUni(fileTypes[i].Pattern);
            }

            ThrowIfFailed(dialog.SetFileTypes((uint)filters.Length, in filters[0]));
        }
        finally
        {
            foreach (COMDLG_FILTERSPEC filter in filters)
            {
                WinApi.CoTaskMemFree(filter.pszName);
                WinApi.CoTaskMemFree(filter.pszSpec);
            }
        }
    }

    private static string GetSelectedPath(IShellItem item)
    {
        ThrowIfFailed(item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out var pathPointer));
        try
        {
            return Marshal.PtrToStringUni(pathPointer)!;
        }
        finally
        {
            WinApi.CoTaskMemFree(pathPointer);
        }
    }
}