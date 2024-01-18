#if !__WEB__
using System;

namespace PixUI;

[Flags]
public enum GRGlBackendState : UInt32
{
    None = 0,
    RenderTarget = 1 << 0,
    TextureBinding = 1 << 1,
    View = 1 << 2, // scissor and viewport
    Blend = 1 << 3,
    MSAAEnable = 1 << 4,
    Vertex = 1 << 5,
    Stencil = 1 << 6,
    PixelStore = 1 << 7,
    Program = 1 << 8,
    FixedFunction = 1 << 9,
    Misc = 1 << 10,
    PathRendering = 1 << 11,
    All = 0xffff
}

[Flags]
public enum GRBackendState : UInt32
{
    None = 0,
    All = 0xffffffff,
}
#endif