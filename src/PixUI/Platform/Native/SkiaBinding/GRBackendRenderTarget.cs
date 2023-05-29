#if !__WEB__
using System;

namespace PixUI
{
    public unsafe class GRBackendRenderTarget : SKObject, ISKSkipObjectRegistration
    {
        private GRBackendRenderTarget(IntPtr handle, bool owns) : base(handle, owns) { }

        public static GRBackendRenderTarget CreateVulkan(int width, int height, int sampleCount, GRVkImageInfo vkImageInfo)
        {
            var handle = SkiaApi.gr_backendrendertarget_new_vulkan(width, height, sampleCount, &vkImageInfo);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Unable to create a new GRBackendRenderTarget instance.");

            return new GRBackendRenderTarget(handle, true);
        }

        public static GRBackendRenderTarget CreateMetal(int width, int height, int sampleCount, GRMtlTextureInfoNative mtlInfo)
        {
            var handle = SkiaApi.gr_backendrendertarget_new_metal(width, height, sampleCount, &mtlInfo);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Unable to create a new GRBackendRenderTarget instance.");
            return new GRBackendRenderTarget(handle, true);
        }

        public static GRBackendRenderTarget CreateDirect3D(int width, int height, IntPtr buffer)
        {
            var handle = SkiaApi.gr_backendrendertarget_new_direct3d(width, height, buffer);
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Unable to create a new GRBackendRenderTarget instance.");

            return new GRBackendRenderTarget(handle, true);
        }

        protected override void DisposeNative() =>
            SkiaApi.gr_backendrendertarget_delete(Handle);

        public bool IsValid => SkiaApi.gr_backendrendertarget_is_valid(Handle);
        public int Width => SkiaApi.gr_backendrendertarget_get_width(Handle);
        public int Height => SkiaApi.gr_backendrendertarget_get_height(Handle);
        public int SampleCount => SkiaApi.gr_backendrendertarget_get_samples(Handle);
        public int StencilBits => SkiaApi.gr_backendrendertarget_get_stencils(Handle);

        public GRBackend Backend => SkiaApi.gr_backendrendertarget_get_backend(Handle).FromNative();
        // public SKSizeI Size => new SKSizeI (Width, Height);
        // public SKRectI Rect => new SKRectI (0, 0, Width, Height);

        public GRGlFramebufferInfo GetGlFramebufferInfo() =>
            GetGlFramebufferInfo(out var info) ? info : default;

        public bool GetGlFramebufferInfo(out GRGlFramebufferInfo glInfo)
        {
            fixed (GRGlFramebufferInfo* g = &glInfo)
            {
                return SkiaApi.gr_backendrendertarget_get_gl_framebufferinfo(Handle, g);
            }
        }
    }
}
#endif