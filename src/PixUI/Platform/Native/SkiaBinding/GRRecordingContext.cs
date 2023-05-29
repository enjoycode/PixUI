#if !__WEB__
using System;

namespace PixUI
{
    public unsafe class GRRecordingContext : SKObject, ISKReferenceCounted
    {
        internal GRRecordingContext(IntPtr h, bool owns) : base(h, owns) { }

        public GRBackend Backend => SkiaApi.gr_recording_context_get_backend(Handle).FromNative();

        public int GetMaxSurfaceSampleCount(ColorType colorType) =>
            SkiaApi.gr_recording_context_get_max_surface_sample_count_for_color_type(Handle,
                colorType.ToNative());

        internal static GRRecordingContext? GetObject(IntPtr handle, bool owns = true,
            bool unrefExisting = true) =>
            GetOrAddObject(handle, owns, unrefExisting, (h, o) => new GRRecordingContext(h, o));
    }
}
#endif