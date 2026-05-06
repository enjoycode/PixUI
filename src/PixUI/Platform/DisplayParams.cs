namespace PixUI.Platform;

public struct DisplayParams
{
    public readonly ColorType ColorType;
    public readonly IColorSpace? ColorSpace;
    public readonly int MSAASampleCount;
    public readonly ISurfaceProperties SurfaceProps;
    public readonly bool DisableVsync;
    public readonly bool DelayDrawableAcquisition;
    public readonly bool EnableBinaryArchive;
    public GRContextOptions GrContextOptions;
}