using System.Runtime.InteropServices.JavaScript;

namespace PixUI.Platform.Blazor;

internal partial class WebGPU
{
    [JSImport("globalThis.PixUI.WebGPU.requestAdapter")]
    internal static partial Task<JSObject> RequestAdapter();

    [JSImport("globalThis.PixUI.WebGPU.requestDevice")]
    internal static partial Task<JSObject> RequestDevice(JSObject adapter);

    [JSImport("globalThis.PixUI.WebGPU.deviceQueue")]
    internal static partial JSObject GetDeviceQueue(JSObject device);

    [JSImport("globalThis.PixUI.WebGPU.registerDevice")]
    internal static partial int RegisterDevice(JSObject device, int parent);

    [JSImport("globalThis.PixUI.WebGPU.registerQueue")]
    internal static partial int RegisterQueue(JSObject queue, int parent);

    [JSImport("globalThis.PixUI.WebGPU.registerTexture")]
    internal static partial int RegisterTexture(JSObject texture);

    [JSImport("globalThis.PixUI.WebGPU.releaseTexture")]
    internal static partial void ReleaseTexture(int textureId);

    [JSImport("globalThis.PixUI.WebGPU.createInstance")]
    internal static partial int CreateInstance();

    [JSImport("globalThis.PixUI.WebGPU.createTexture")]
    internal static partial JSObject CreateTexture(JSObject device, int width, int height);

    [JSImport("globalThis.PixUI.WebGPU.createBuffer")]
    internal static partial JSObject CreateBuffer(JSObject device, int size);

    [JSImport("globalThis.PixUI.WebGPU.createCommandEncoder")]
    internal static partial JSObject CreateCommandEncoder(JSObject device);

    [JSImport("globalThis.PixUI.WebGPU.copyTextureToBuffer")]
    internal static partial void CopyTextureToBuffer(JSObject encoder, JSObject texture, JSObject buffer,
        int bytesPerRow, int width, int height);

    [JSImport("globalThis.PixUI.WebGPU.submitEncoder")]
    internal static partial void SubmitEncoder(JSObject device, JSObject encoder);

    [JSImport("globalThis.PixUI.WebGPU.mapBufferRead")]
    internal static partial Task MapBufferReadAsync(JSObject buffer);

    [JSImport("globalThis.PixUI.WebGPU.getMappedBase64")]
    internal static partial string GetMappedBase64(JSObject buffer, int bytesPerRow, int width, int height);
}