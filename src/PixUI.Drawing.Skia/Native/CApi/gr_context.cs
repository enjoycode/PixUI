using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void gr_backendrendertarget_delete(gr_backendrendertarget_t* rendertarget)
    [LibraryImport(SKIA)]
    internal static partial void gr_backendrendertarget_delete(gr_backendrendertarget_t rendertarget);

    // gr_backend_t gr_backendrendertarget_get_backend(const gr_backendrendertarget_t* rendertarget)
    [LibraryImport(SKIA)]
    internal static partial GRBackendNative gr_backendrendertarget_get_backend(gr_backendrendertarget_t rendertarget);

    // bool gr_backendrendertarget_get_gl_framebufferinfo(const gr_backendrendertarget_t* rendertarget, gr_gl_framebufferinfo_t* glInfo)
    // [LibraryImport(SKIA)
    // [return: MarshalAs (UnmanagedType.I1)]
    // internal static partial bool gr_backendrendertarget_get_gl_framebufferinfo (gr_backendrendertarget_t rendertarget, GRGlFramebufferInfo* glInfo);

    // int gr_backendrendertarget_get_height(const gr_backendrendertarget_t* rendertarget)
    [LibraryImport(SKIA)]
    internal static partial Int32 gr_backendrendertarget_get_height(gr_backendrendertarget_t rendertarget);

    // int gr_backendrendertarget_get_samples(const gr_backendrendertarget_t* rendertarget)
    [LibraryImport(SKIA)]
    internal static partial Int32 gr_backendrendertarget_get_samples(gr_backendrendertarget_t rendertarget);

    // int gr_backendrendertarget_get_stencils(const gr_backendrendertarget_t* rendertarget)
    [LibraryImport(SKIA)]
    internal static partial Int32 gr_backendrendertarget_get_stencils(gr_backendrendertarget_t rendertarget);

    // int gr_backendrendertarget_get_width(const gr_backendrendertarget_t* rendertarget)
    [LibraryImport(SKIA)]
    internal static partial Int32 gr_backendrendertarget_get_width(gr_backendrendertarget_t rendertarget);

    // bool gr_backendrendertarget_is_valid(const gr_backendrendertarget_t* rendertarget)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool gr_backendrendertarget_is_valid(gr_backendrendertarget_t rendertarget);

    // gr_backendrendertarget_t* gr_backendrendertarget_new_gl(int width, int height, int samples, int stencils, const gr_gl_framebufferinfo_t* glInfo)
    // [LibraryImport(SKIA)
    // internal static partial gr_backendrendertarget_t gr_backendrendertarget_new_gl (Int32 width, Int32 height, Int32 samples, Int32 stencils, GRGlFramebufferInfo* glInfo);

    // gr_backendrendertarget_t* gr_backendrendertarget_new_metal(int width, int height, int samples, const gr_mtl_textureinfo_t* mtlInfo)
    [LibraryImport(SKIA)]
    internal static partial gr_backendrendertarget_t gr_backendrendertarget_new_metal(Int32 width, Int32 height,
        GRMtlTextureInfoNative* mtlInfo);

    // gr_backendrendertarget_t* gr_backendrendertarget_new_vulkan(int width, int height, int samples, const gr_vk_imageinfo_t* vkImageInfo)
    [LibraryImport(SKIA)]
    internal static partial gr_backendrendertarget_t gr_backendrendertarget_new_vulkan(Int32 width, Int32 height,
        GRVkImageInfo* vkImageInfo);

    [LibraryImport(SKIA)]
    internal static partial gr_backendrendertarget_t gr_backendrendertarget_new_direct3d_buffer(Int32 width, Int32 height,
        IntPtr buffer);

    // void gr_backendtexture_delete(gr_backendtexture_t* texture)
    [LibraryImport(SKIA)]
    internal static partial void gr_backendtexture_delete(gr_backendtexture_t texture);

    // gr_backend_t gr_backendtexture_get_backend(const gr_backendtexture_t* texture)
    [LibraryImport(SKIA)]
    internal static partial GRBackendNative gr_backendtexture_get_backend(gr_backendtexture_t texture);

    // bool gr_backendtexture_get_gl_textureinfo(const gr_backendtexture_t* texture, gr_gl_textureinfo_t* glInfo)
    // [LibraryImport(SKIA)
    // [return: MarshalAs (UnmanagedType.I1)]
    // internal static partial bool gr_backendtexture_get_gl_textureinfo (gr_backendtexture_t texture, GRGlTextureInfo* glInfo);

    // int gr_backendtexture_get_height(const gr_backendtexture_t* texture)
    [LibraryImport(SKIA)]
    internal static partial Int32 gr_backendtexture_get_height(gr_backendtexture_t texture);

    // int gr_backendtexture_get_width(const gr_backendtexture_t* texture)
    [LibraryImport(SKIA)]
    internal static partial Int32 gr_backendtexture_get_width(gr_backendtexture_t texture);

    // bool gr_backendtexture_has_mipmaps(const gr_backendtexture_t* texture)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool gr_backendtexture_has_mipmaps(gr_backendtexture_t texture);

    // bool gr_backendtexture_is_valid(const gr_backendtexture_t* texture)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool gr_backendtexture_is_valid(gr_backendtexture_t texture);

    // gr_backendtexture_t* gr_backendtexture_new_gl(int width, int height, bool mipmapped, const gr_gl_textureinfo_t* glInfo)
    // [LibraryImport(SKIA)
    // internal static partial gr_backendtexture_t gr_backendtexture_new_gl (Int32 width, Int32 height, [MarshalAs (UnmanagedType.I1)] bool mipmapped, GRGlTextureInfo* glInfo);

    // gr_backendtexture_t* gr_backendtexture_new_metal(int width, int height, bool mipmapped, const gr_mtl_textureinfo_t* mtlInfo)
    // [LibraryImport(SKIA)
    // internal static partial gr_backendtexture_t gr_backendtexture_new_metal (Int32 width, Int32 height, [MarshalAs (UnmanagedType.I1)] bool mipmapped, GRMtlTextureInfoNative* mtlInfo);

    // gr_backendtexture_t* gr_backendtexture_new_vulkan(int width, int height, const gr_vk_imageinfo_t* vkInfo)
    // [LibraryImport(SKIA)
    // internal static partial gr_backendtexture_t gr_backendtexture_new_vulkan (Int32 width, Int32 height, GRVkImageInfo* vkInfo);

    // void gr_direct_context_abandon_context(gr_direct_context_t* context)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_abandon_context(gr_direct_context_t context);

    // void gr_direct_context_dump_memory_statistics(const gr_direct_context_t* context, sk_tracememorydump_t* dump)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_dump_memory_statistics(gr_direct_context_t context,
        sk_tracememorydump_t dump);

    // void gr_direct_context_flush(gr_direct_context_t* context)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_flush(gr_direct_context_t context);

    // void gr_direct_context_flush_and_submit(gr_direct_context_t* context, bool syncCpu)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_flush_and_submit(gr_direct_context_t context,
        [MarshalAs(UnmanagedType.I1)] bool syncCpu);

    // void gr_direct_context_free_gpu_resources(gr_direct_context_t* context)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_free_gpu_resources(gr_direct_context_t context);

    // size_t gr_direct_context_get_resource_cache_limit(gr_direct_context_t* context)
    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr gr_direct_context_get_resource_cache_limit(gr_direct_context_t context);

    // void gr_direct_context_get_resource_cache_usage(gr_direct_context_t* context, int* maxResources, size_t* maxResourceBytes)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_get_resource_cache_usage(gr_direct_context_t context,
        Int32* maxResources, /* size_t */ IntPtr* maxResourceBytes);

    // bool gr_direct_context_is_abandoned(gr_direct_context_t* context)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool gr_direct_context_is_abandoned(gr_direct_context_t context);

    // gr_direct_context_t* gr_direct_context_make_gl(const gr_glinterface_t* glInterface)
    [LibraryImport(SKIA)]
    internal static partial gr_direct_context_t gr_direct_context_make_gl(gr_glinterface_t glInterface);

    // gr_direct_context_t* gr_direct_context_make_gl_with_options(const gr_glinterface_t* glInterface, const gr_context_options_t* options)
    [LibraryImport(SKIA)]
    internal static partial gr_direct_context_t gr_direct_context_make_gl_with_options(gr_glinterface_t glInterface,
        GRContextOptionsNative* options);

    [LibraryImport(SKIA)]
    internal static partial IntPtr gr_direct_context_make_gl_onscreen_surface(gr_direct_context_t grContext, int width,
        int height);

    // gr_direct_context_t* gr_direct_context_make_metal(void* device, void* queue)
    [LibraryImport(SKIA)]
    internal static partial gr_direct_context_t gr_direct_context_make_metal(void* device, void* queue);

    // gr_direct_context_t* gr_direct_context_make_metal_with_options(void* device, void* queue, const gr_context_options_t* options)
    [LibraryImport(SKIA)]
    internal static partial gr_direct_context_t gr_direct_context_make_metal_with_options(void* device, void* queue,
        GRContextOptionsNative* options);

    [LibraryImport(SKIA)]
    internal static partial gr_direct_context_t gr_direct_context_make_direct3d(void* backendContext);

    // gr_direct_context_t* gr_direct_context_make_vulkan(const gr_vk_backendcontext_t vkBackendContext)
    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern gr_direct_context_t gr_direct_context_make_vulkan(GRVkBackendContextNative vkBackendContext);

    // gr_direct_context_t* gr_direct_context_make_vulkan_with_options(const gr_vk_backendcontext_t vkBackendContext, const gr_context_options_t* options)
    [DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
    internal static extern gr_direct_context_t gr_direct_context_make_vulkan_with_options(
        GRVkBackendContextNative vkBackendContext, GRContextOptionsNative* options);

    // void gr_direct_context_perform_deferred_cleanup(gr_direct_context_t* context, long long ms)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_perform_deferred_cleanup(gr_direct_context_t context, Int64 ms);

    // void gr_direct_context_purge_unlocked_resources(gr_direct_context_t* context, bool scratchResourcesOnly)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_purge_unlocked_resources(gr_direct_context_t context,
        [MarshalAs(UnmanagedType.I1)] bool scratchResourcesOnly);

    // void gr_direct_context_purge_unlocked_resources_bytes(gr_direct_context_t* context, size_t bytesToPurge, bool preferScratchResources)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_purge_unlocked_resources_bytes(
        gr_direct_context_t context, /* size_t */ IntPtr bytesToPurge,
        [MarshalAs(UnmanagedType.I1)] bool preferScratchResources);

    // void gr_direct_context_release_resources_and_abandon_context(gr_direct_context_t* context)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_release_resources_and_abandon_context(gr_direct_context_t context);

    // void gr_direct_context_reset_context(gr_direct_context_t* context, uint32_t state)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_reset_context(gr_direct_context_t context, UInt32 state);

    // void gr_direct_context_set_resource_cache_limit(gr_direct_context_t* context, size_t maxResourceBytes)
    [LibraryImport(SKIA)]
    internal static partial void gr_direct_context_set_resource_cache_limit(gr_direct_context_t context, /* size_t */
        IntPtr maxResourceBytes);

    // bool gr_direct_context_submit(gr_direct_context_t* context, bool syncCpu)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool gr_direct_context_submit(gr_direct_context_t context,
        [MarshalAs(UnmanagedType.I1)] bool syncCpu);

    // const gr_glinterface_t* gr_glinterface_assemble_gl_interface(void* ctx, gr_gl_get_proc get)
    [LibraryImport(SKIA)]
    internal static partial gr_glinterface_t gr_glinterface_assemble_gl_interface(void* ctx,
        GRGlGetProcProxyDelegate get);

    // const gr_glinterface_t* gr_glinterface_assemble_gles_interface(void* ctx, gr_gl_get_proc get)
    [LibraryImport(SKIA)]
    internal static partial gr_glinterface_t gr_glinterface_assemble_gles_interface(void* ctx,
        GRGlGetProcProxyDelegate get);

    // const gr_glinterface_t* gr_glinterface_assemble_interface(void* ctx, gr_gl_get_proc get)
    [LibraryImport(SKIA)]
    internal static partial gr_glinterface_t gr_glinterface_assemble_interface(void* ctx, GRGlGetProcProxyDelegate get);

    // const gr_glinterface_t* gr_glinterface_assemble_webgl_interface(void* ctx, gr_gl_get_proc get)
    [LibraryImport(SKIA)]
    internal static partial gr_glinterface_t gr_glinterface_assemble_webgl_interface(void* ctx,
        GRGlGetProcProxyDelegate get);

    // const gr_glinterface_t* gr_glinterface_create_native_interface()
    [LibraryImport(SKIA)]
    internal static partial gr_glinterface_t gr_glinterface_create_native_interface();

    // bool gr_glinterface_has_extension(const gr_glinterface_t* glInterface, const char* extension)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool gr_glinterface_has_extension(gr_glinterface_t glInterface,
        [MarshalAs(UnmanagedType.LPStr)] String extension);

    // void gr_glinterface_unref(const gr_glinterface_t* glInterface)
    [LibraryImport(SKIA)]
    internal static partial void gr_glinterface_unref(gr_glinterface_t glInterface);

    // bool gr_glinterface_validate(const gr_glinterface_t* glInterface)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool gr_glinterface_validate(gr_glinterface_t glInterface);

    // gr_backend_t gr_recording_context_get_backend(gr_recording_context_t* context)
    [LibraryImport(SKIA)]
    internal static partial GRBackendNative gr_recording_context_get_backend(gr_recording_context_t context);

    // int gr_recording_context_get_max_surface_sample_count_for_color_type(gr_recording_context_t* context, sk_colortype_t colorType)
    [LibraryImport(SKIA)]
    internal static partial Int32 gr_recording_context_get_max_surface_sample_count_for_color_type(
        gr_recording_context_t context, SKColorTypeNative colorType);

    // void gr_recording_context_unref(gr_recording_context_t* context)
    [LibraryImport(SKIA)]
    internal static partial void gr_recording_context_unref(gr_recording_context_t context);

    // void gr_vk_extensions_delete(gr_vk_extensions_t* extensions)
    [LibraryImport(SKIA)]
    internal static partial void gr_vk_extensions_delete(gr_vk_extensions_t extensions);

    // bool gr_vk_extensions_has_extension(gr_vk_extensions_t* extensions, const char* ext, uint32_t minVersion)
    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool gr_vk_extensions_has_extension(gr_vk_extensions_t extensions,
        [MarshalAs(UnmanagedType.LPStr)] String ext, UInt32 minVersion);

    // void gr_vk_extensions_init(gr_vk_extensions_t* extensions, gr_vk_get_proc getProc, void* userData, vk_instance_t* instance, vk_physical_device_t* physDev, uint32_t instanceExtensionCount, const char** instanceExtensions, uint32_t deviceExtensionCount, const char** deviceExtensions)
    [LibraryImport(SKIA)]
    internal static partial void gr_vk_extensions_init(gr_vk_extensions_t extensions, GRVkGetProcProxyDelegate getProc,
        void* userData, vk_instance_t instance, vk_physical_device_t physDev, UInt32 instanceExtensionCount,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
        String[] instanceExtensions,
        UInt32 deviceExtensionCount,
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
        String[] deviceExtensions);

    // gr_vk_extensions_t* gr_vk_extensions_new()
    [LibraryImport(SKIA)]
    internal static partial gr_vk_extensions_t gr_vk_extensions_new();

    [LibraryImport(SKIA)]
    public static partial IntPtr gr_d3d_new_backend_context();

    [LibraryImport(SKIA)]
    public static partial IntPtr gr_d3d_new_swapchain(IntPtr hwnd, IntPtr d3dbackendCtx, uint width, uint height);

    [LibraryImport(SKIA)]
    public static partial int gr_d3d_swapchain_get_current_buffer_index(IntPtr swapchain);

    [LibraryImport(SKIA)]
    public static partial IntPtr gr_d3d_swapchain_get_buffer(IntPtr swapchain, int index);

    [LibraryImport(SKIA)]
    public static partial void gr_d3d_swapchain_release_buffers(IntPtr swapchain, int count);

    [LibraryImport(SKIA)]
    public static partial void gr_d3d_swapchain_resize_buffers(IntPtr swapchain, uint width, uint height);

    [LibraryImport(SKIA)]
    public static partial void gr_d3d_swapbuffer(IntPtr d3dbackendCtx, IntPtr grCtx, IntPtr surface, IntPtr swapchain);
}