using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;

namespace PixUI.Platform.Blazor;

internal sealed class BlazorFileDialog : IPlatformFileDialog
{
    public async Task<Stream?> OpenFileAsync(OpenFileOptions options)
    {
        //注意:JSRuntime.InvokeAsync<必须为JSStreamReference, 不可以为IJSStreamReference>
        var jsStreamRef = await BlazorApplication.JSRuntime.InvokeAsync<JSStreamReference?>("PixUI.OpenFile");
        if (jsStreamRef == null)
            return null;

        try
        {
            var stream = await ((IJSStreamReference)jsStreamRef)
                .OpenReadStreamAsync(10 * 1024 * 1024 /*TODO: max size*/);
            return stream;
        }
        finally
        {
            await jsStreamRef.DisposeAsync();
        }
    }

    public async Task SaveFileAsync(SaveFileOptions options)
    {
        var streamRef = new DotNetStreamReference(options.FileStream, true);
        await BlazorApplication.JSRuntime.InvokeVoidAsync("PixUI.SaveFile", options.FileName, streamRef);
    }
}