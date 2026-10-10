namespace PixUI.Drawing.Skia;

public sealed class Direct3DSwapChain : IDirect3DSwapChain
{
    internal readonly IntPtr Handle;

    internal Direct3DSwapChain(IntPtr handle) => Handle = handle;

    public int CurrentBufferIndex => SkiaApi.gr_d3d_swapchain_get_current_buffer_index(Handle);

    public void ReleaseBuffer(int index) => SkiaApi.gr_d3d_swapchain_release_buffer(Handle, index);

    public void ResizeBuffers(uint width, uint height) =>
        SkiaApi.gr_d3d_swapchain_resize_buffers(Handle, width, height);

    public void SwapBuffer(IGRContext context, IDirect3DBackendContext backend, ISurface surface,
        IntPtr fence, ulong fenceValue)
        => SkiaApi.gr_d3d_swapbuffer(((Direct3DBackendContext)backend).Handle, ((GRContext)context).Handle,
            ((SKSurface)surface).Handle, Handle, fence, fenceValue);

    public void Dispose()
    {
        //TODO:
    }
}