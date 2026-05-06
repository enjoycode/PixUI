namespace PixUI;

public interface IGRContext : IDisposable
{
    void AbandonContext(bool releaseResources = false);
    void Flush(bool submit, bool synchronous = false);

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