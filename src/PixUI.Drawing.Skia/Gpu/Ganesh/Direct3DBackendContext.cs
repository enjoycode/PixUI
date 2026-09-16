namespace PixUI.Drawing.Skia;

public sealed class Direct3DBackendContext : IDirect3DBackendContext
{
    internal Direct3DBackendContext(IntPtr handle)
    {
        Handle = handle;
    }

    internal readonly IntPtr Handle;

    public void Dispose()
    {
        //TODO:
    }
}