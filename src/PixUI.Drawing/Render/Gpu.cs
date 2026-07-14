namespace PixUI;

public interface IGRContext : IDisposable
{
    void AbandonContext(bool releaseResources = false);
    void Flush(bool submit, bool synchronous = false);
    void PurgeResources();
}

public interface IDirect3DBackendContext : IDisposable { }

public interface IDirect3DSwapChain : IDisposable
{
    int CurrentBufferIndex { get; }
    void ReleaseBuffers(int count);
    void ResizeBuffers(uint width, uint height);
    void SwapBuffer(IGRContext context, IDirect3DBackendContext backend, ISurface surface);
}

public enum SurfaceOrigin
{
    TopLeft = 0,
    BottomLeft = 1,
}

public sealed class GRContextOptions //TODO: change to struct
{
    public bool AvoidStencilBuffers { get; init; } = false;

    public int RuntimeProgramCacheSize { get; init; } = 256;

    public int GlyphCacheTextureMaximumBytes { get; init; } = 2048 * 1024 * 4;

    public bool AllowPathMaskCaching { get; init; } = true;

    public bool DoManualMipmapping { get; init; } = false;

    public int BufferMapThreshold { get; init; } = -1;
}