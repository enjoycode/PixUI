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

    public Task<Stream?> SaveFileAsync(SaveFileOptions options)
    {
        throw new NotImplementedException();
    }
}