#if !__WEB__
using System;

namespace PixUI;

public sealed class GRGlInterface : SKObject, ISKReferenceCounted, ISKSkipObjectRegistration
{
    private GRGlInterface(IntPtr h, bool owns) : base(h, owns) { }

    private static GRGlInterface? GetObject(IntPtr handle) =>
        handle == IntPtr.Zero ? null : new GRGlInterface(handle, true);

    public static GRGlInterface? Create() => GetObject(SkiaApi.gr_glinterface_create_native_interface());
}
#endif