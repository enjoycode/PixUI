using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;

namespace PixUI.Platform.Blazor;

internal sealed class BlazorFileDialog : IPlatformFileDialog
{
    public async Task<OpenFileResult[]> OpenFileAsync(OpenFileOptions options)
    {
        //注意:JSRuntime.InvokeAsync<必须为JSStreamReference, 不可以为IJSStreamReference>
        var jsOpenFileResults = await BlazorApplication.JSRuntime.InvokeAsync<JSOpenFileResult[]>(
            "PixUI.OpenFile", options.AllowMultiple, Array.Empty<string>() /*TODO: accept*/);
        if (jsOpenFileResults.Length == 0)
            return [];

        try
        {
            var results = new OpenFileResult[jsOpenFileResults.Length];
            for (var i = 0; i < jsOpenFileResults.Length; i++)
            {
                results[i] = new OpenFileResult()
                {
                    FileName = jsOpenFileResults[i].FileName,
                    // FileSize = jsOpenFileResults[i].FileSize,
                    FileStream = await ((IJSStreamReference)jsOpenFileResults[i].FileStream)
                        .OpenReadStreamAsync(options.MaxFileSize)
                };
            }

            return results;
        }
        finally
        {
            for (var i = 0; i < jsOpenFileResults.Length; i++)
            {
                await jsOpenFileResults[i].FileStream.DisposeAsync();
            }
        }
    }

    public async Task SaveFileAsync(SaveFileOptions options)
    {
        var streamRef = new DotNetStreamReference(options.FileStream, true);
        await BlazorApplication.JSRuntime.InvokeVoidAsync("PixUI.SaveFile", options.FileName, streamRef);
    }
}

public struct JSOpenFileResult
{
    public string FileName { get; set; }
    public int FileSize { get; set; }
    public JSStreamReference FileStream { get; set; }
}