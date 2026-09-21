using System;
using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

public unsafe partial class SkiaApi
{
	#region gr_context.h

	// void gr_backendrendertarget_delete(gr_backendrendertarget_t* rendertarget)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_backendrendertarget_delete (gr_backendrendertarget_t rendertarget);
		
	// gr_backend_t gr_backendrendertarget_get_backend(const gr_backendrendertarget_t* rendertarget)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern GRBackendNative gr_backendrendertarget_get_backend (gr_backendrendertarget_t rendertarget);

	// bool gr_backendrendertarget_get_gl_framebufferinfo(const gr_backendrendertarget_t* rendertarget, gr_gl_framebufferinfo_t* glInfo)
	// [DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	// [return: MarshalAs (UnmanagedType.I1)]
	// internal static extern bool gr_backendrendertarget_get_gl_framebufferinfo (gr_backendrendertarget_t rendertarget, GRGlFramebufferInfo* glInfo);

	// int gr_backendrendertarget_get_height(const gr_backendrendertarget_t* rendertarget)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 gr_backendrendertarget_get_height (gr_backendrendertarget_t rendertarget);
		
	// int gr_backendrendertarget_get_samples(const gr_backendrendertarget_t* rendertarget)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 gr_backendrendertarget_get_samples (gr_backendrendertarget_t rendertarget);

	// int gr_backendrendertarget_get_stencils(const gr_backendrendertarget_t* rendertarget)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 gr_backendrendertarget_get_stencils (gr_backendrendertarget_t rendertarget);

	// int gr_backendrendertarget_get_width(const gr_backendrendertarget_t* rendertarget)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 gr_backendrendertarget_get_width (gr_backendrendertarget_t rendertarget);

	// bool gr_backendrendertarget_is_valid(const gr_backendrendertarget_t* rendertarget)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool gr_backendrendertarget_is_valid (gr_backendrendertarget_t rendertarget);

	// gr_backendrendertarget_t* gr_backendrendertarget_new_gl(int width, int height, int samples, int stencils, const gr_gl_framebufferinfo_t* glInfo)
	// [DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	// internal static extern gr_backendrendertarget_t gr_backendrendertarget_new_gl (Int32 width, Int32 height, Int32 samples, Int32 stencils, GRGlFramebufferInfo* glInfo);

	// gr_backendrendertarget_t* gr_backendrendertarget_new_metal(int width, int height, int samples, const gr_mtl_textureinfo_t* mtlInfo)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_backendrendertarget_t gr_backendrendertarget_new_metal (Int32 width, Int32 height, Int32 samples, GRMtlTextureInfoNative* mtlInfo);

	// gr_backendrendertarget_t* gr_backendrendertarget_new_vulkan(int width, int height, int samples, const gr_vk_imageinfo_t* vkImageInfo)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_backendrendertarget_t gr_backendrendertarget_new_vulkan (Int32 width, Int32 height, Int32 samples, GRVkImageInfo* vkImageInfo);

	[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_backendrendertarget_t gr_backendrendertarget_new_direct3d(Int32 width, Int32 height, IntPtr buffer);

	// void gr_backendtexture_delete(gr_backendtexture_t* texture)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_backendtexture_delete (gr_backendtexture_t texture);

	// gr_backend_t gr_backendtexture_get_backend(const gr_backendtexture_t* texture)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern GRBackendNative gr_backendtexture_get_backend (gr_backendtexture_t texture);

	// bool gr_backendtexture_get_gl_textureinfo(const gr_backendtexture_t* texture, gr_gl_textureinfo_t* glInfo)
	// [DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	// [return: MarshalAs (UnmanagedType.I1)]
	// internal static extern bool gr_backendtexture_get_gl_textureinfo (gr_backendtexture_t texture, GRGlTextureInfo* glInfo);

	// int gr_backendtexture_get_height(const gr_backendtexture_t* texture)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 gr_backendtexture_get_height (gr_backendtexture_t texture);

	// int gr_backendtexture_get_width(const gr_backendtexture_t* texture)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 gr_backendtexture_get_width (gr_backendtexture_t texture);

	// bool gr_backendtexture_has_mipmaps(const gr_backendtexture_t* texture)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool gr_backendtexture_has_mipmaps (gr_backendtexture_t texture);

	// bool gr_backendtexture_is_valid(const gr_backendtexture_t* texture)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool gr_backendtexture_is_valid (gr_backendtexture_t texture);

	// gr_backendtexture_t* gr_backendtexture_new_gl(int width, int height, bool mipmapped, const gr_gl_textureinfo_t* glInfo)
	// [DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	// internal static extern gr_backendtexture_t gr_backendtexture_new_gl (Int32 width, Int32 height, [MarshalAs (UnmanagedType.I1)] bool mipmapped, GRGlTextureInfo* glInfo);

	// gr_backendtexture_t* gr_backendtexture_new_metal(int width, int height, bool mipmapped, const gr_mtl_textureinfo_t* mtlInfo)
	// [DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	// internal static extern gr_backendtexture_t gr_backendtexture_new_metal (Int32 width, Int32 height, [MarshalAs (UnmanagedType.I1)] bool mipmapped, GRMtlTextureInfoNative* mtlInfo);

	// gr_backendtexture_t* gr_backendtexture_new_vulkan(int width, int height, const gr_vk_imageinfo_t* vkInfo)
	// [DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	// internal static extern gr_backendtexture_t gr_backendtexture_new_vulkan (Int32 width, Int32 height, GRVkImageInfo* vkInfo);

	// void gr_direct_context_abandon_context(gr_direct_context_t* context)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_abandon_context (gr_direct_context_t context);

	// void gr_direct_context_dump_memory_statistics(const gr_direct_context_t* context, sk_tracememorydump_t* dump)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_dump_memory_statistics (gr_direct_context_t context, sk_tracememorydump_t dump);

	// void gr_direct_context_flush(gr_direct_context_t* context)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_flush (gr_direct_context_t context);

	// void gr_direct_context_flush_and_submit(gr_direct_context_t* context, bool syncCpu)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_flush_and_submit (gr_direct_context_t context, [MarshalAs (UnmanagedType.I1)] bool syncCpu);

	// void gr_direct_context_free_gpu_resources(gr_direct_context_t* context)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_free_gpu_resources (gr_direct_context_t context);

	// size_t gr_direct_context_get_resource_cache_limit(gr_direct_context_t* context)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr gr_direct_context_get_resource_cache_limit (gr_direct_context_t context);

	// void gr_direct_context_get_resource_cache_usage(gr_direct_context_t* context, int* maxResources, size_t* maxResourceBytes)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_get_resource_cache_usage (gr_direct_context_t context, Int32* maxResources, /* size_t */ IntPtr* maxResourceBytes);

	// bool gr_direct_context_is_abandoned(gr_direct_context_t* context)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool gr_direct_context_is_abandoned (gr_direct_context_t context);

	// gr_direct_context_t* gr_direct_context_make_gl(const gr_glinterface_t* glInterface)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_direct_context_t gr_direct_context_make_gl (gr_glinterface_t glInterface);

	// gr_direct_context_t* gr_direct_context_make_gl_with_options(const gr_glinterface_t* glInterface, const gr_context_options_t* options)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_direct_context_t gr_direct_context_make_gl_with_options (gr_glinterface_t glInterface, GRContextOptionsNative* options);

	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern IntPtr gr_direct_context_make_gl_onscreen_surface(gr_direct_context_t grContext, int width, int height);
		
	// gr_direct_context_t* gr_direct_context_make_metal(void* device, void* queue)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_direct_context_t gr_direct_context_make_metal (void* device, void* queue);

	// gr_direct_context_t* gr_direct_context_make_metal_with_options(void* device, void* queue, const gr_context_options_t* options)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_direct_context_t gr_direct_context_make_metal_with_options (void* device, void* queue, GRContextOptionsNative* options);

	[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_direct_context_t gr_direct_context_make_direct3d(void* backendContext);

	// gr_direct_context_t* gr_direct_context_make_vulkan(const gr_vk_backendcontext_t vkBackendContext)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_direct_context_t gr_direct_context_make_vulkan (GRVkBackendContextNative vkBackendContext);

	// gr_direct_context_t* gr_direct_context_make_vulkan_with_options(const gr_vk_backendcontext_t vkBackendContext, const gr_context_options_t* options)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_direct_context_t gr_direct_context_make_vulkan_with_options (GRVkBackendContextNative vkBackendContext, GRContextOptionsNative* options);

	// void gr_direct_context_perform_deferred_cleanup(gr_direct_context_t* context, long long ms)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_perform_deferred_cleanup (gr_direct_context_t context, Int64 ms);

	// void gr_direct_context_purge_unlocked_resources(gr_direct_context_t* context, bool scratchResourcesOnly)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_purge_unlocked_resources (gr_direct_context_t context, [MarshalAs (UnmanagedType.I1)] bool scratchResourcesOnly);

	// void gr_direct_context_purge_unlocked_resources_bytes(gr_direct_context_t* context, size_t bytesToPurge, bool preferScratchResources)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_purge_unlocked_resources_bytes (gr_direct_context_t context, /* size_t */ IntPtr bytesToPurge, [MarshalAs (UnmanagedType.I1)] bool preferScratchResources);

	// void gr_direct_context_release_resources_and_abandon_context(gr_direct_context_t* context)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_release_resources_and_abandon_context (gr_direct_context_t context);

	// void gr_direct_context_reset_context(gr_direct_context_t* context, uint32_t state)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_reset_context (gr_direct_context_t context, UInt32 state);

	// void gr_direct_context_set_resource_cache_limit(gr_direct_context_t* context, size_t maxResourceBytes)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_direct_context_set_resource_cache_limit (gr_direct_context_t context, /* size_t */ IntPtr maxResourceBytes);

	// bool gr_direct_context_submit(gr_direct_context_t* context, bool syncCpu)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool gr_direct_context_submit (gr_direct_context_t context, [MarshalAs (UnmanagedType.I1)] bool syncCpu);

	// const gr_glinterface_t* gr_glinterface_assemble_gl_interface(void* ctx, gr_gl_get_proc get)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_glinterface_t gr_glinterface_assemble_gl_interface (void* ctx, GRGlGetProcProxyDelegate get);

	// const gr_glinterface_t* gr_glinterface_assemble_gles_interface(void* ctx, gr_gl_get_proc get)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_glinterface_t gr_glinterface_assemble_gles_interface (void* ctx, GRGlGetProcProxyDelegate get);

	// const gr_glinterface_t* gr_glinterface_assemble_interface(void* ctx, gr_gl_get_proc get)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_glinterface_t gr_glinterface_assemble_interface (void* ctx, GRGlGetProcProxyDelegate get);

	// const gr_glinterface_t* gr_glinterface_assemble_webgl_interface(void* ctx, gr_gl_get_proc get)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_glinterface_t gr_glinterface_assemble_webgl_interface (void* ctx, GRGlGetProcProxyDelegate get);

	// const gr_glinterface_t* gr_glinterface_create_native_interface()
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_glinterface_t gr_glinterface_create_native_interface ();

	// bool gr_glinterface_has_extension(const gr_glinterface_t* glInterface, const char* extension)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool gr_glinterface_has_extension (gr_glinterface_t glInterface, [MarshalAs (UnmanagedType.LPStr)] String extension);

	// void gr_glinterface_unref(const gr_glinterface_t* glInterface)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_glinterface_unref (gr_glinterface_t glInterface);

	// bool gr_glinterface_validate(const gr_glinterface_t* glInterface)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool gr_glinterface_validate (gr_glinterface_t glInterface);

	// gr_backend_t gr_recording_context_get_backend(gr_recording_context_t* context)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern GRBackendNative gr_recording_context_get_backend (gr_recording_context_t context);

	// int gr_recording_context_get_max_surface_sample_count_for_color_type(gr_recording_context_t* context, sk_colortype_t colorType)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 gr_recording_context_get_max_surface_sample_count_for_color_type (gr_recording_context_t context, SKColorTypeNative colorType);

	// void gr_recording_context_unref(gr_recording_context_t* context)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_recording_context_unref (gr_recording_context_t context);

	// void gr_vk_extensions_delete(gr_vk_extensions_t* extensions)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_vk_extensions_delete (gr_vk_extensions_t extensions);

	// bool gr_vk_extensions_has_extension(gr_vk_extensions_t* extensions, const char* ext, uint32_t minVersion)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool gr_vk_extensions_has_extension (gr_vk_extensions_t extensions, [MarshalAs (UnmanagedType.LPStr)] String ext, UInt32 minVersion);

	// void gr_vk_extensions_init(gr_vk_extensions_t* extensions, gr_vk_get_proc getProc, void* userData, vk_instance_t* instance, vk_physical_device_t* physDev, uint32_t instanceExtensionCount, const char** instanceExtensions, uint32_t deviceExtensionCount, const char** deviceExtensions)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void gr_vk_extensions_init (gr_vk_extensions_t extensions, GRVkGetProcProxyDelegate getProc, void* userData, vk_instance_t instance, vk_physical_device_t physDev, UInt32 instanceExtensionCount, [MarshalAs (UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] String[] instanceExtensions, UInt32 deviceExtensionCount, [MarshalAs (UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] String[] deviceExtensions);

	// gr_vk_extensions_t* gr_vk_extensions_new()
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_vk_extensions_t gr_vk_extensions_new ();

	[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr gr_d3d_new_backend_context();

	[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr gr_d3d_new_swapchain(IntPtr hwnd, IntPtr d3dbackendCtx, uint width, uint height);

	[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
	public static extern int gr_d3d_swapchain_get_current_buffer_index(IntPtr swapchain);

	[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr gr_d3d_swapchain_get_buffer(IntPtr swapchain, int index);

	[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
	public static extern void gr_d3d_swapchain_release_buffers(IntPtr swapchain, int count);

	[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
	public static extern void gr_d3d_swapchain_resize_buffers(IntPtr swapchain, uint width, uint height);

	[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
	public static extern void gr_d3d_swapbuffer(IntPtr d3dbackendCtx, IntPtr grCtx, IntPtr surface, IntPtr swapchain);

	#endregion

	#region sk_bitmap.h

	// void sk_bitmap_destructor(sk_bitmap_t* cbitmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_destructor (sk_bitmap_t cbitmap);

	// void sk_bitmap_erase(sk_bitmap_t* cbitmap, sk_color_t color)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_erase (sk_bitmap_t cbitmap, UInt32 color);

	// void sk_bitmap_erase_rect(sk_bitmap_t* cbitmap, sk_color_t color, sk_irect_t* rect)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_erase_rect (sk_bitmap_t cbitmap, UInt32 color, RectI* rect);

	// bool sk_bitmap_extract_alpha(sk_bitmap_t* cbitmap, sk_bitmap_t* dst, const sk_paint_t* paint, sk_ipoint_t* offset)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_extract_alpha (sk_bitmap_t cbitmap, sk_bitmap_t dst, sk_paint_t paint, PointI* offset);

	// bool sk_bitmap_extract_subset(sk_bitmap_t* cbitmap, sk_bitmap_t* dst, sk_irect_t* subset)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_extract_subset (sk_bitmap_t cbitmap, sk_bitmap_t dst, RectI* subset);

	// void* sk_bitmap_get_addr(sk_bitmap_t* cbitmap, int x, int y)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void* sk_bitmap_get_addr (sk_bitmap_t cbitmap, Int32 x, Int32 y);

	// uint16_t* sk_bitmap_get_addr_16(sk_bitmap_t* cbitmap, int x, int y)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt16* sk_bitmap_get_addr_16 (sk_bitmap_t cbitmap, Int32 x, Int32 y);

	// uint32_t* sk_bitmap_get_addr_32(sk_bitmap_t* cbitmap, int x, int y)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32* sk_bitmap_get_addr_32 (sk_bitmap_t cbitmap, Int32 x, Int32 y);

	// uint8_t* sk_bitmap_get_addr_8(sk_bitmap_t* cbitmap, int x, int y)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Byte* sk_bitmap_get_addr_8 (sk_bitmap_t cbitmap, Int32 x, Int32 y);

	// size_t sk_bitmap_get_byte_count(sk_bitmap_t* cbitmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_bitmap_get_byte_count (sk_bitmap_t cbitmap);

	// void sk_bitmap_get_info(sk_bitmap_t* cbitmap, sk_imageinfo_t* info)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_get_info (sk_bitmap_t cbitmap, SKImageInfoNative* info);

	// sk_color_t sk_bitmap_get_pixel_color(sk_bitmap_t* cbitmap, int x, int y)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_bitmap_get_pixel_color (sk_bitmap_t cbitmap, Int32 x, Int32 y);

	// void sk_bitmap_get_pixel_colors(sk_bitmap_t* cbitmap, sk_color_t* colors)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_get_pixel_colors (sk_bitmap_t cbitmap, UInt32* colors);
		
	// void* sk_bitmap_get_pixels(sk_bitmap_t* cbitmap, size_t* length)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void* sk_bitmap_get_pixels (sk_bitmap_t cbitmap, /* size_t */ IntPtr* length);
		
	// size_t sk_bitmap_get_row_bytes(sk_bitmap_t* cbitmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_bitmap_get_row_bytes (sk_bitmap_t cbitmap);
		
	// bool sk_bitmap_install_pixels(sk_bitmap_t* cbitmap, const sk_imageinfo_t* cinfo, void* pixels, size_t rowBytes, const sk_bitmap_release_proc releaseProc, void* context)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_install_pixels (sk_bitmap_t cbitmap, SKImageInfoNative* cinfo, void* pixels, /* size_t */ IntPtr rowBytes, SKBitmapReleaseProxyDelegate releaseProc, void* context);
		
	// bool sk_bitmap_install_pixels_with_pixmap(sk_bitmap_t* cbitmap, const sk_pixmap_t* cpixmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_install_pixels_with_pixmap (sk_bitmap_t cbitmap, sk_pixmap_t cpixmap);
		
	// bool sk_bitmap_is_immutable(sk_bitmap_t* cbitmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_is_immutable (sk_bitmap_t cbitmap);
		
	// bool sk_bitmap_is_null(sk_bitmap_t* cbitmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_is_null (sk_bitmap_t cbitmap);
		
	// sk_shader_t* sk_bitmap_make_shader(sk_bitmap_t* cbitmap, sk_shader_tilemode_t tmx, sk_shader_tilemode_t tmy, const sk_matrix_t* cmatrix)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_bitmap_make_shader (sk_bitmap_t cbitmap, TileMode tmx, TileMode tmy, Matrix3* cmatrix);
		
	// sk_bitmap_t* sk_bitmap_new()
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_bitmap_t sk_bitmap_new ();
		
	// void sk_bitmap_notify_pixels_changed(sk_bitmap_t* cbitmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_notify_pixels_changed (sk_bitmap_t cbitmap);
		
	// bool sk_bitmap_peek_pixels(sk_bitmap_t* cbitmap, sk_pixmap_t* cpixmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_peek_pixels (sk_bitmap_t cbitmap, sk_pixmap_t cpixmap);
		
	// bool sk_bitmap_ready_to_draw(sk_bitmap_t* cbitmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_ready_to_draw (sk_bitmap_t cbitmap);
		
	// void sk_bitmap_reset(sk_bitmap_t* cbitmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_reset (sk_bitmap_t cbitmap);
		
	// void sk_bitmap_set_immutable(sk_bitmap_t* cbitmap)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_set_immutable (sk_bitmap_t cbitmap);
		
	// void sk_bitmap_set_pixels(sk_bitmap_t* cbitmap, void* pixels)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_set_pixels (sk_bitmap_t cbitmap, void* pixels);
		
	// void sk_bitmap_swap(sk_bitmap_t* cbitmap, sk_bitmap_t* cother)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_bitmap_swap (sk_bitmap_t cbitmap, sk_bitmap_t cother);
		
	// bool sk_bitmap_try_alloc_pixels(sk_bitmap_t* cbitmap, const sk_imageinfo_t* requestedInfo, size_t rowBytes)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_try_alloc_pixels (sk_bitmap_t cbitmap, SKImageInfoNative* requestedInfo, /* size_t */ IntPtr rowBytes);
		
	// bool sk_bitmap_try_alloc_pixels_with_flags(sk_bitmap_t* cbitmap, const sk_imageinfo_t* requestedInfo, uint32_t flags)
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_bitmap_try_alloc_pixels_with_flags (sk_bitmap_t cbitmap, SKImageInfoNative* requestedInfo, UInt32 flags);
		
	#endregion

	#region sk_codec.h

	// void sk_codec_destroy(sk_codec_t* codec)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_codec_destroy (sk_codec_t codec);
		

	// sk_encoded_image_format_t sk_codec_get_encoded_format(sk_codec_t* codec)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern EncodedImageFormat sk_codec_get_encoded_format (sk_codec_t codec);
		

	// int sk_codec_get_frame_count(sk_codec_t* codec)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_codec_get_frame_count (sk_codec_t codec);
		

	// void sk_codec_get_frame_info(sk_codec_t* codec, sk_codec_frameinfo_t* frameInfo)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_codec_get_frame_info (sk_codec_t codec, SKCodecFrameInfo* frameInfo);
		

	// bool sk_codec_get_frame_info_for_index(sk_codec_t* codec, int index, sk_codec_frameinfo_t* frameInfo)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_codec_get_frame_info_for_index (sk_codec_t codec, Int32 index, SKCodecFrameInfo* frameInfo);
		

	// void sk_codec_get_info(sk_codec_t* codec, sk_imageinfo_t* info)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_codec_get_info (sk_codec_t codec, SKImageInfoNative* info);
		

	// sk_encodedorigin_t sk_codec_get_origin(sk_codec_t* codec)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKEncodedOrigin sk_codec_get_origin (sk_codec_t codec);
		

	// sk_codec_result_t sk_codec_get_pixels(sk_codec_t* codec, const sk_imageinfo_t* info, void* pixels, size_t rowBytes, const sk_codec_options_t* options)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKCodecResult sk_codec_get_pixels (sk_codec_t codec, SKImageInfoNative* info, void* pixels, /* size_t */ IntPtr rowBytes, SKCodecOptionsInternal* options);
		

	// int sk_codec_get_repetition_count(sk_codec_t* codec)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_codec_get_repetition_count (sk_codec_t codec);
		

	// void sk_codec_get_scaled_dimensions(sk_codec_t* codec, float desiredScale, sk_isize_t* dimensions)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_codec_get_scaled_dimensions (sk_codec_t codec, Single desiredScale, SizeI* dimensions);
		

	// sk_codec_scanline_order_t sk_codec_get_scanline_order(sk_codec_t* codec)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKCodecScanlineOrder sk_codec_get_scanline_order (sk_codec_t codec);
		

	// int sk_codec_get_scanlines(sk_codec_t* codec, void* dst, int countLines, size_t rowBytes)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_codec_get_scanlines (sk_codec_t codec, void* dst, Int32 countLines, /* size_t */ IntPtr rowBytes);
		

	// bool sk_codec_get_valid_subset(sk_codec_t* codec, sk_irect_t* desiredSubset)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_codec_get_valid_subset (sk_codec_t codec, RectI* desiredSubset);
		

	// sk_codec_result_t sk_codec_incremental_decode(sk_codec_t* codec, int* rowsDecoded)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKCodecResult sk_codec_incremental_decode (sk_codec_t codec, Int32* rowsDecoded);
		

	// size_t sk_codec_min_buffered_bytes_needed()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_codec_min_buffered_bytes_needed ();
		

	// sk_codec_t* sk_codec_new_from_data(sk_data_t* data)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_codec_t sk_codec_new_from_data (sk_data_t data);
		

	// sk_codec_t* sk_codec_new_from_stream(sk_stream_t* stream, sk_codec_result_t* result)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_codec_t sk_codec_new_from_stream (sk_stream_t stream, SKCodecResult* result);
		

	// int sk_codec_next_scanline(sk_codec_t* codec)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_codec_next_scanline (sk_codec_t codec);
		

	// int sk_codec_output_scanline(sk_codec_t* codec, int inputScanline)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_codec_output_scanline (sk_codec_t codec, Int32 inputScanline);
		

	// bool sk_codec_skip_scanlines(sk_codec_t* codec, int countLines)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_codec_skip_scanlines (sk_codec_t codec, Int32 countLines);
		

	// sk_codec_result_t sk_codec_start_incremental_decode(sk_codec_t* codec, const sk_imageinfo_t* info, void* pixels, size_t rowBytes, const sk_codec_options_t* options)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKCodecResult sk_codec_start_incremental_decode (sk_codec_t codec, SKImageInfoNative* info, void* pixels, /* size_t */ IntPtr rowBytes, SKCodecOptionsInternal* options);
		

	// sk_codec_result_t sk_codec_start_scanline_decode(sk_codec_t* codec, const sk_imageinfo_t* info, const sk_codec_options_t* options)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKCodecResult sk_codec_start_scanline_decode (sk_codec_t codec, SKImageInfoNative* info, SKCodecOptionsInternal* options);
		

	#endregion

	#region sk_colorfilter.h

	// sk_colorfilter_t* sk_colorfilter_new_color_matrix(const float[20] array = 20)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorfilter_t sk_colorfilter_new_color_matrix (Single* array);
		

	// sk_colorfilter_t* sk_colorfilter_new_compose(sk_colorfilter_t* outer, sk_colorfilter_t* inner)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorfilter_t sk_colorfilter_new_compose (sk_colorfilter_t outer, sk_colorfilter_t inner);
		

	// sk_colorfilter_t* sk_colorfilter_new_high_contrast(const sk_highcontrastconfig_t* config)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorfilter_t sk_colorfilter_new_high_contrast (SKHighContrastConfig* config);
		

	// sk_colorfilter_t* sk_colorfilter_new_lighting(sk_color_t mul, sk_color_t add)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorfilter_t sk_colorfilter_new_lighting (UInt32 mul, UInt32 add);
		

	// sk_colorfilter_t* sk_colorfilter_new_luma_color()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorfilter_t sk_colorfilter_new_luma_color ();
		

	// sk_colorfilter_t* sk_colorfilter_new_mode(sk_color_t c, sk_blendmode_t mode)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorfilter_t sk_colorfilter_new_mode (UInt32 c, BlendMode mode);
		

	// sk_colorfilter_t* sk_colorfilter_new_table(const uint8_t[256] table = 256)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorfilter_t sk_colorfilter_new_table (Byte* table);
		

	// sk_colorfilter_t* sk_colorfilter_new_table_argb(const uint8_t[256] tableA = 256, const uint8_t[256] tableR = 256, const uint8_t[256] tableG = 256, const uint8_t[256] tableB = 256)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorfilter_t sk_colorfilter_new_table_argb (Byte* tableA, Byte* tableR, Byte* tableG, Byte* tableB);
		

	// void sk_colorfilter_unref(sk_colorfilter_t* filter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorfilter_unref (sk_colorfilter_t filter);
		

	#endregion

	#region sk_colorspace.h

	// void sk_color4f_from_color(sk_color_t color, sk_color4f_t* color4f)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_color4f_from_color (UInt32 color, SKColorF* color4f);
		

	// sk_color_t sk_color4f_to_color(const sk_color4f_t* color4f)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_color4f_to_color (SKColorF* color4f);
		

	// bool sk_colorspace_equals(const sk_colorspace_t* src, const sk_colorspace_t* dst)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_equals (sk_colorspace_t src, sk_colorspace_t dst);
		

	// bool sk_colorspace_gamma_close_to_srgb(const sk_colorspace_t* colorspace)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_gamma_close_to_srgb (sk_colorspace_t colorspace);
		

	// bool sk_colorspace_gamma_is_linear(const sk_colorspace_t* colorspace)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_gamma_is_linear (sk_colorspace_t colorspace);
		

	// void sk_colorspace_icc_profile_delete(sk_colorspace_icc_profile_t* profile)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_icc_profile_delete (sk_colorspace_icc_profile_t profile);
		

	// const uint8_t* sk_colorspace_icc_profile_get_buffer(const sk_colorspace_icc_profile_t* profile, uint32_t* size)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Byte* sk_colorspace_icc_profile_get_buffer (sk_colorspace_icc_profile_t profile, UInt32* size);
		

	// bool sk_colorspace_icc_profile_get_to_xyzd50(const sk_colorspace_icc_profile_t* profile, sk_colorspace_xyz_t* toXYZD50)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_icc_profile_get_to_xyzd50 (sk_colorspace_icc_profile_t profile, SKColorSpaceXyz* toXYZD50);
		

	// sk_colorspace_icc_profile_t* sk_colorspace_icc_profile_new()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorspace_icc_profile_t sk_colorspace_icc_profile_new ();
		

	// bool sk_colorspace_icc_profile_parse(const void* buffer, size_t length, sk_colorspace_icc_profile_t* profile)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_icc_profile_parse (void* buffer, /* size_t */ IntPtr length, sk_colorspace_icc_profile_t profile);
		

	// bool sk_colorspace_is_numerical_transfer_fn(const sk_colorspace_t* colorspace, sk_colorspace_transfer_fn_t* transferFn)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_is_numerical_transfer_fn (sk_colorspace_t colorspace, SKColorSpaceTransferFn* transferFn);
		

	// bool sk_colorspace_is_srgb(const sk_colorspace_t* colorspace)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_is_srgb (sk_colorspace_t colorspace);
		

	// sk_colorspace_t* sk_colorspace_make_linear_gamma(const sk_colorspace_t* colorspace)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorspace_t sk_colorspace_make_linear_gamma (sk_colorspace_t colorspace);
		

	// sk_colorspace_t* sk_colorspace_make_srgb_gamma(const sk_colorspace_t* colorspace)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorspace_t sk_colorspace_make_srgb_gamma (sk_colorspace_t colorspace);
		

	// sk_colorspace_t* sk_colorspace_new_icc(const sk_colorspace_icc_profile_t* profile)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorspace_t sk_colorspace_new_icc (sk_colorspace_icc_profile_t profile);
		

	// sk_colorspace_t* sk_colorspace_new_rgb(const sk_colorspace_transfer_fn_t* transferFn, const sk_colorspace_xyz_t* toXYZD50)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorspace_t sk_colorspace_new_rgb (SKColorSpaceTransferFn* transferFn, SKColorSpaceXyz* toXYZD50);
		

	// sk_colorspace_t* sk_colorspace_new_srgb()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorspace_t sk_colorspace_new_srgb ();
		

	// sk_colorspace_t* sk_colorspace_new_srgb_linear()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorspace_t sk_colorspace_new_srgb_linear ();
		

	// bool sk_colorspace_primaries_to_xyzd50(const sk_colorspace_primaries_t* primaries, sk_colorspace_xyz_t* toXYZD50)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_primaries_to_xyzd50 (SKColorSpacePrimaries* primaries, SKColorSpaceXyz* toXYZD50);
		

	// void sk_colorspace_ref(sk_colorspace_t* colorspace)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_ref (sk_colorspace_t colorspace);
		

	// void sk_colorspace_to_profile(const sk_colorspace_t* colorspace, sk_colorspace_icc_profile_t* profile)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_to_profile (sk_colorspace_t colorspace, sk_colorspace_icc_profile_t profile);
		

	// bool sk_colorspace_to_xyzd50(const sk_colorspace_t* colorspace, sk_colorspace_xyz_t* toXYZD50)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_to_xyzd50 (sk_colorspace_t colorspace, SKColorSpaceXyz* toXYZD50);
		

	// float sk_colorspace_transfer_fn_eval(const sk_colorspace_transfer_fn_t* transferFn, float x)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Single sk_colorspace_transfer_fn_eval (SKColorSpaceTransferFn* transferFn, Single x);
		

	// bool sk_colorspace_transfer_fn_invert(const sk_colorspace_transfer_fn_t* src, sk_colorspace_transfer_fn_t* dst)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_transfer_fn_invert (SKColorSpaceTransferFn* src, SKColorSpaceTransferFn* dst);
		

	// void sk_colorspace_transfer_fn_named_2dot2(sk_colorspace_transfer_fn_t* transferFn)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_transfer_fn_named_2dot2 (SKColorSpaceTransferFn* transferFn);
		

	// void sk_colorspace_transfer_fn_named_hlg(sk_colorspace_transfer_fn_t* transferFn)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_transfer_fn_named_hlg (SKColorSpaceTransferFn* transferFn);
		

	// void sk_colorspace_transfer_fn_named_linear(sk_colorspace_transfer_fn_t* transferFn)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_transfer_fn_named_linear (SKColorSpaceTransferFn* transferFn);
		

	// void sk_colorspace_transfer_fn_named_pq(sk_colorspace_transfer_fn_t* transferFn)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_transfer_fn_named_pq (SKColorSpaceTransferFn* transferFn);
		

	// void sk_colorspace_transfer_fn_named_rec2020(sk_colorspace_transfer_fn_t* transferFn)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_transfer_fn_named_rec2020 (SKColorSpaceTransferFn* transferFn);
		

	// void sk_colorspace_transfer_fn_named_srgb(sk_colorspace_transfer_fn_t* transferFn)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_transfer_fn_named_srgb (SKColorSpaceTransferFn* transferFn);
		

	// void sk_colorspace_unref(sk_colorspace_t* colorspace)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_unref (sk_colorspace_t colorspace);
		

	// void sk_colorspace_xyz_concat(const sk_colorspace_xyz_t* a, const sk_colorspace_xyz_t* b, sk_colorspace_xyz_t* result)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_xyz_concat (SKColorSpaceXyz* a, SKColorSpaceXyz* b, SKColorSpaceXyz* result);
		

	// bool sk_colorspace_xyz_invert(const sk_colorspace_xyz_t* src, sk_colorspace_xyz_t* dst)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_colorspace_xyz_invert (SKColorSpaceXyz* src, SKColorSpaceXyz* dst);
		

	// void sk_colorspace_xyz_named_adobe_rgb(sk_colorspace_xyz_t* xyz)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_xyz_named_adobe_rgb (SKColorSpaceXyz* xyz);
		

	// void sk_colorspace_xyz_named_display_p3(sk_colorspace_xyz_t* xyz)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_xyz_named_display_p3 (SKColorSpaceXyz* xyz);
		

	// void sk_colorspace_xyz_named_rec2020(sk_colorspace_xyz_t* xyz)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_xyz_named_rec2020 (SKColorSpaceXyz* xyz);
		

	// void sk_colorspace_xyz_named_srgb(sk_colorspace_xyz_t* xyz)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_xyz_named_srgb (SKColorSpaceXyz* xyz);
		

	// void sk_colorspace_xyz_named_xyz(sk_colorspace_xyz_t* xyz)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colorspace_xyz_named_xyz (SKColorSpaceXyz* xyz);
		

	#endregion

	#region sk_colortable.h

	// int sk_colortable_count(const sk_colortable_t* ctable)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_colortable_count (sk_colortable_t ctable);
		

	// sk_colortable_t* sk_colortable_new(const sk_pmcolor_t* colors, int count)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colortable_t sk_colortable_new (UInt32* colors, Int32 count);
		

	// void sk_colortable_read_colors(const sk_colortable_t* ctable, sk_pmcolor_t** colors)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colortable_read_colors (sk_colortable_t ctable, UInt32** colors);
		

	// void sk_colortable_unref(sk_colortable_t* ctable)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_colortable_unref (sk_colortable_t ctable);
		

	#endregion

	#region sk_document.h

	// void sk_document_abort(sk_document_t* document)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_document_abort (sk_document_t document);
		

	// sk_canvas_t* sk_document_begin_page(sk_document_t* document, float width, float height, const sk_rect_t* content)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_canvas_t sk_document_begin_page (sk_document_t document, Single width, Single height, Rect* content);
		

	// void sk_document_close(sk_document_t* document)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_document_close (sk_document_t document);
		

	// sk_document_t* sk_document_create_pdf_from_stream(sk_wstream_t* stream)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_document_t sk_document_create_pdf_from_stream (sk_wstream_t stream);
		

	// sk_document_t* sk_document_create_pdf_from_stream_with_metadata(sk_wstream_t* stream, const sk_document_pdf_metadata_t* metadata)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_document_t sk_document_create_pdf_from_stream_with_metadata (sk_wstream_t stream, SKDocumentPdfMetadataInternal* metadata);
		

	// sk_document_t* sk_document_create_xps_from_stream(sk_wstream_t* stream, float dpi)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_document_t sk_document_create_xps_from_stream (sk_wstream_t stream, Single dpi);
		

	// void sk_document_end_page(sk_document_t* document)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_document_end_page (sk_document_t document);
		

	// void sk_document_unref(sk_document_t* document)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_document_unref (sk_document_t document);
		

	#endregion

	#region sk_drawable.h

	// void sk_drawable_draw(sk_drawable_t*, sk_canvas_t*, const sk_matrix_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_drawable_draw (sk_drawable_t param0, sk_canvas_t param1, Matrix3* param2);
		

	// void sk_drawable_get_bounds(sk_drawable_t*, sk_rect_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_drawable_get_bounds (sk_drawable_t param0, Rect* param1);
		

	// uint32_t sk_drawable_get_generation_id(sk_drawable_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_drawable_get_generation_id (sk_drawable_t param0);
		

	// sk_picture_t* sk_drawable_new_picture_snapshot(sk_drawable_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_picture_t sk_drawable_new_picture_snapshot (sk_drawable_t param0);
		

	// void sk_drawable_notify_drawing_changed(sk_drawable_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_drawable_notify_drawing_changed (sk_drawable_t param0);
		

	// void sk_drawable_unref(sk_drawable_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_drawable_unref (sk_drawable_t param0);
		

	#endregion

	#region sk_general.h

	// sk_colortype_t sk_colortype_get_default_8888()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKColorTypeNative sk_colortype_get_default_8888 ();
		

	// int sk_nvrefcnt_get_ref_count(const sk_nvrefcnt_t* refcnt)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_nvrefcnt_get_ref_count (sk_nvrefcnt_t refcnt);
		

	// void sk_nvrefcnt_safe_ref(sk_nvrefcnt_t* refcnt)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_nvrefcnt_safe_ref (sk_nvrefcnt_t refcnt);
		

	// void sk_nvrefcnt_safe_unref(sk_nvrefcnt_t* refcnt)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_nvrefcnt_safe_unref (sk_nvrefcnt_t refcnt);
		

	// bool sk_nvrefcnt_unique(const sk_nvrefcnt_t* refcnt)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_nvrefcnt_unique (sk_nvrefcnt_t refcnt);
		

	// int sk_refcnt_get_ref_count(const sk_refcnt_t* refcnt)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_refcnt_get_ref_count (sk_refcnt_t refcnt);
		

	// void sk_refcnt_safe_ref(sk_refcnt_t* refcnt)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_refcnt_safe_ref (sk_refcnt_t refcnt);
		

	// void sk_refcnt_safe_unref(sk_refcnt_t* refcnt)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_refcnt_safe_unref (sk_refcnt_t refcnt);
		

	// bool sk_refcnt_unique(const sk_refcnt_t* refcnt)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_refcnt_unique (sk_refcnt_t refcnt);
		

	// int sk_version_get_increment()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_version_get_increment ();
		

	// int sk_version_get_milestone()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_version_get_milestone ();
		

	// const char* sk_version_get_string()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* char */ void* sk_version_get_string ();
		

	#endregion

	#region sk_graphics.h

	// void sk_graphics_dump_memory_statistics(sk_tracememorydump_t* dump)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_graphics_dump_memory_statistics (sk_tracememorydump_t dump);
		

	// int sk_graphics_get_font_cache_count_limit()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_graphics_get_font_cache_count_limit ();
		

	// int sk_graphics_get_font_cache_count_used()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_graphics_get_font_cache_count_used ();
		

	// size_t sk_graphics_get_font_cache_limit()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_graphics_get_font_cache_limit ();
		

	// int sk_graphics_get_font_cache_point_size_limit()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_graphics_get_font_cache_point_size_limit ();
		

	// size_t sk_graphics_get_font_cache_used()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_graphics_get_font_cache_used ();
		

	// size_t sk_graphics_get_resource_cache_single_allocation_byte_limit()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_graphics_get_resource_cache_single_allocation_byte_limit ();
		

	// size_t sk_graphics_get_resource_cache_total_byte_limit()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_graphics_get_resource_cache_total_byte_limit ();
		

	// size_t sk_graphics_get_resource_cache_total_bytes_used()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_graphics_get_resource_cache_total_bytes_used ();
		

	// void sk_graphics_init()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_graphics_init ();
		

	// void sk_graphics_purge_all_caches()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_graphics_purge_all_caches ();
		

	// void sk_graphics_purge_font_cache()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_graphics_purge_font_cache ();
		

	// void sk_graphics_purge_resource_cache()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_graphics_purge_resource_cache ();
		

	// int sk_graphics_set_font_cache_count_limit(int count)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_graphics_set_font_cache_count_limit (Int32 count);
		

	// size_t sk_graphics_set_font_cache_limit(size_t bytes)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_graphics_set_font_cache_limit (/* size_t */ IntPtr bytes);
		

	// int sk_graphics_set_font_cache_point_size_limit(int maxPointSize)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_graphics_set_font_cache_point_size_limit (Int32 maxPointSize);
		

	// size_t sk_graphics_set_resource_cache_single_allocation_byte_limit(size_t newLimit)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_graphics_set_resource_cache_single_allocation_byte_limit (/* size_t */ IntPtr newLimit);
		

	// size_t sk_graphics_set_resource_cache_total_byte_limit(size_t newLimit)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_graphics_set_resource_cache_total_byte_limit (/* size_t */ IntPtr newLimit);
		

	#endregion

	#region sk_imagefilter.h

	// void sk_imagefilter_croprect_destructor(sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_imagefilter_croprect_destructor (sk_imagefilter_croprect_t cropRect);
		

	// uint32_t sk_imagefilter_croprect_get_flags(sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_imagefilter_croprect_get_flags (sk_imagefilter_croprect_t cropRect);
		

	// void sk_imagefilter_croprect_get_rect(sk_imagefilter_croprect_t* cropRect, sk_rect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_imagefilter_croprect_get_rect (sk_imagefilter_croprect_t cropRect, Rect* rect);
		

	// sk_imagefilter_croprect_t* sk_imagefilter_croprect_new()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_croprect_t sk_imagefilter_croprect_new ();
		

	// sk_imagefilter_croprect_t* sk_imagefilter_croprect_new_with_rect(const sk_rect_t* rect, uint32_t flags)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_croprect_t sk_imagefilter_croprect_new_with_rect (Rect* rect, UInt32 flags);
		

	// sk_imagefilter_t* sk_imagefilter_new_arithmetic(float k1, float k2, float k3, float k4, bool enforcePMColor, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_arithmetic (Single k1, Single k2, Single k3, Single k4, [MarshalAs (UnmanagedType.I1)] bool enforcePMColor, sk_imagefilter_t background, sk_imagefilter_t foreground, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_blur(float sigmaX, float sigmaY, sk_shader_tilemode_t tileMode, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_blur (Single sigmaX, Single sigmaY, TileMode tileMode, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_color_filter(sk_colorfilter_t* cf, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_color_filter (sk_colorfilter_t cf, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_compose(sk_imagefilter_t* outer, sk_imagefilter_t* inner)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_compose (sk_imagefilter_t outer, sk_imagefilter_t inner);
		

	// sk_imagefilter_t* sk_imagefilter_new_dilate(float radiusX, float radiusY, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_dilate (Single radiusX, Single radiusY, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_displacement_map_effect(sk_color_channel_t xChannelSelector, sk_color_channel_t yChannelSelector, float scale, sk_imagefilter_t* displacement, sk_imagefilter_t* color, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_displacement_map_effect (ColorChannel xChannelSelector, ColorChannel yChannelSelector, Single scale, sk_imagefilter_t displacement, sk_imagefilter_t color, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_distant_lit_diffuse(const sk_point3_t* direction, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_distant_lit_diffuse (Point3* direction, UInt32 lightColor, Single surfaceScale, Single kd, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_distant_lit_specular(const sk_point3_t* direction, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_distant_lit_specular (Point3* direction, UInt32 lightColor, Single surfaceScale, Single ks, Single shininess, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_drop_shadow(float dx, float dy, float sigmaX, float sigmaY, sk_color_t color, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_drop_shadow (Single dx, Single dy, Single sigmaX, Single sigmaY, UInt32 color, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_drop_shadow_only(float dx, float dy, float sigmaX, float sigmaY, sk_color_t color, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_drop_shadow_only (Single dx, Single dy, Single sigmaX, Single sigmaY, UInt32 color, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_erode(float radiusX, float radiusY, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_erode (Single radiusX, Single radiusY, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_image_source(sk_image_t* image, const sk_rect_t* srcRect, const sk_rect_t* dstRect, sk_filter_quality_t filterQuality)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_image_source (sk_image_t image, Rect* srcRect, Rect* dstRect, SKFilterQuality filterQuality);
		

	// sk_imagefilter_t* sk_imagefilter_new_image_source_default(sk_image_t* image)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_image_source_default (sk_image_t image);
		

	// sk_imagefilter_t* sk_imagefilter_new_magnifier(const sk_rect_t* src, float inset, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_magnifier (Rect* src, Single inset, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_matrix(const sk_matrix_t* matrix, sk_filter_quality_t quality, sk_imagefilter_t* input)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_matrix (Matrix3* matrix, SKFilterQuality quality, sk_imagefilter_t input);
		

	// sk_imagefilter_t* sk_imagefilter_new_matrix_convolution(const sk_isize_t* kernelSize, const float[-1] kernel, float gain, float bias, const sk_ipoint_t* kernelOffset, sk_shader_tilemode_t tileMode, bool convolveAlpha, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_matrix_convolution (SizeI* kernelSize, Single* kernel, Single gain, Single bias, PointI* kernelOffset, TileMode tileMode, [MarshalAs (UnmanagedType.I1)] bool convolveAlpha, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_merge(sk_imagefilter_t*[-1] filters, int count, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_merge (sk_imagefilter_t* filters, Int32 count, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_offset(float dx, float dy, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_offset (Single dx, Single dy, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_paint(const sk_paint_t* paint, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_paint (sk_paint_t paint, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_picture(sk_picture_t* picture)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_picture (sk_picture_t picture);
		

	// sk_imagefilter_t* sk_imagefilter_new_picture_with_croprect(sk_picture_t* picture, const sk_rect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_picture_with_croprect (sk_picture_t picture, Rect* cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_point_lit_diffuse(const sk_point3_t* location, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_point_lit_diffuse (Point3* location, UInt32 lightColor, Single surfaceScale, Single kd, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_point_lit_specular(const sk_point3_t* location, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_point_lit_specular (Point3* location, UInt32 lightColor, Single surfaceScale, Single ks, Single shininess, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_spot_lit_diffuse(const sk_point3_t* location, const sk_point3_t* target, float specularExponent, float cutoffAngle, sk_color_t lightColor, float surfaceScale, float kd, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_spot_lit_diffuse (Point3* location, Point3* target, Single specularExponent, Single cutoffAngle, UInt32 lightColor, Single surfaceScale, Single kd, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_spot_lit_specular(const sk_point3_t* location, const sk_point3_t* target, float specularExponent, float cutoffAngle, sk_color_t lightColor, float surfaceScale, float ks, float shininess, sk_imagefilter_t* input, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_spot_lit_specular (Point3* location, Point3* target, Single specularExponent, Single cutoffAngle, UInt32 lightColor, Single surfaceScale, Single ks, Single shininess, sk_imagefilter_t input, sk_imagefilter_croprect_t cropRect);
		

	// sk_imagefilter_t* sk_imagefilter_new_tile(const sk_rect_t* src, const sk_rect_t* dst, sk_imagefilter_t* input)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_tile (Rect* src, Rect* dst, sk_imagefilter_t input);
		

	// sk_imagefilter_t* sk_imagefilter_new_xfermode(sk_blendmode_t mode, sk_imagefilter_t* background, sk_imagefilter_t* foreground, const sk_imagefilter_croprect_t* cropRect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_imagefilter_t sk_imagefilter_new_xfermode (BlendMode mode, sk_imagefilter_t background, sk_imagefilter_t foreground, sk_imagefilter_croprect_t cropRect);
		

	// void sk_imagefilter_unref(sk_imagefilter_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_imagefilter_unref (sk_imagefilter_t param0);
		

	#endregion

	#region sk_mask.h

	// uint8_t* sk_mask_alloc_image(size_t bytes)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Byte* sk_mask_alloc_image (/* size_t */ IntPtr bytes);
		

	// size_t sk_mask_compute_image_size(sk_mask_t* cmask)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_mask_compute_image_size (SKMask* cmask);
		

	// size_t sk_mask_compute_total_image_size(sk_mask_t* cmask)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_mask_compute_total_image_size (SKMask* cmask);
		

	// void sk_mask_free_image(void* image)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_mask_free_image (void* image);
		

	// void* sk_mask_get_addr(sk_mask_t* cmask, int x, int y)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void* sk_mask_get_addr (SKMask* cmask, Int32 x, Int32 y);
		

	// uint8_t* sk_mask_get_addr_1(sk_mask_t* cmask, int x, int y)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Byte* sk_mask_get_addr_1 (SKMask* cmask, Int32 x, Int32 y);
		

	// uint32_t* sk_mask_get_addr_32(sk_mask_t* cmask, int x, int y)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32* sk_mask_get_addr_32 (SKMask* cmask, Int32 x, Int32 y);
		

	// uint8_t* sk_mask_get_addr_8(sk_mask_t* cmask, int x, int y)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Byte* sk_mask_get_addr_8 (SKMask* cmask, Int32 x, Int32 y);
		

	// uint16_t* sk_mask_get_addr_lcd_16(sk_mask_t* cmask, int x, int y)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt16* sk_mask_get_addr_lcd_16 (SKMask* cmask, Int32 x, Int32 y);
		

	// bool sk_mask_is_empty(sk_mask_t* cmask)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_mask_is_empty (SKMask* cmask);
		

	#endregion

	#region sk_maskfilter.h

	// sk_maskfilter_t* sk_maskfilter_new_blur(sk_blurstyle_t, float sigma)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_maskfilter_t sk_maskfilter_new_blur (BlurStyle param0, Single sigma);
		

	// sk_maskfilter_t* sk_maskfilter_new_blur_with_flags(sk_blurstyle_t, float sigma, bool respectCTM)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_maskfilter_t sk_maskfilter_new_blur_with_flags (BlurStyle param0, Single sigma, [MarshalAs (UnmanagedType.I1)] bool respectCTM);
		

	// sk_maskfilter_t* sk_maskfilter_new_clip(uint8_t min, uint8_t max)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_maskfilter_t sk_maskfilter_new_clip (Byte min, Byte max);
		

	// sk_maskfilter_t* sk_maskfilter_new_gamma(float gamma)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_maskfilter_t sk_maskfilter_new_gamma (Single gamma);
		

	// sk_maskfilter_t* sk_maskfilter_new_shader(sk_shader_t* cshader)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_maskfilter_t sk_maskfilter_new_shader (sk_shader_t cshader);
		

	// sk_maskfilter_t* sk_maskfilter_new_table(const uint8_t[256] table = 256)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_maskfilter_t sk_maskfilter_new_table (Byte* table);
		

	// void sk_maskfilter_ref(sk_maskfilter_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_maskfilter_ref (sk_maskfilter_t param0);
		

	// void sk_maskfilter_unref(sk_maskfilter_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_maskfilter_unref (sk_maskfilter_t param0);
		

	#endregion

	#region sk_matrix.h

	// void sk_matrix_concat(sk_matrix_t* result, sk_matrix_t* first, sk_matrix_t* second)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_matrix_concat (Matrix3* result, Matrix3* first, Matrix3* second);
		

	// void sk_matrix_map_points(sk_matrix_t* matrix, sk_point_t* dst, sk_point_t* src, int count)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_matrix_map_points (Matrix3* matrix, Point* dst, Point* src, Int32 count);
		

	// float sk_matrix_map_radius(sk_matrix_t* matrix, float radius)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Single sk_matrix_map_radius (Matrix3* matrix, Single radius);
		

	// void sk_matrix_map_rect(sk_matrix_t* matrix, sk_rect_t* dest, sk_rect_t* source)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_matrix_map_rect (Matrix3* matrix, Rect* dest, Rect* source);
		

	// void sk_matrix_map_vector(sk_matrix_t* matrix, float x, float y, sk_point_t* result)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_matrix_map_vector (Matrix3* matrix, Single x, Single y, Point* result);
		

	// void sk_matrix_map_vectors(sk_matrix_t* matrix, sk_point_t* dst, sk_point_t* src, int count)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_matrix_map_vectors (Matrix3* matrix, Point* dst, Point* src, Int32 count);
		

	// void sk_matrix_map_xy(sk_matrix_t* matrix, float x, float y, sk_point_t* result)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_matrix_map_xy (Matrix3* matrix, Single x, Single y, Point* result);
		

	// void sk_matrix_post_concat(sk_matrix_t* result, sk_matrix_t* matrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_matrix_post_concat (Matrix3* result, Matrix3* matrix);
		

	// void sk_matrix_pre_concat(sk_matrix_t* result, sk_matrix_t* matrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_matrix_pre_concat (Matrix3* result, Matrix3* matrix);
		

	// bool sk_matrix_try_invert(sk_matrix_t* matrix, sk_matrix_t* result)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_matrix_try_invert (Matrix3* matrix, Matrix3* result);
		

	#endregion

	#region sk_patheffect.h

	// sk_path_effect_t* sk_path_effect_create_1d_path(const sk_path_t* path, float advance, float phase, sk_path_effect_1d_style_t style)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_path_effect_t sk_path_effect_create_1d_path (sk_path_t path, Single advance, Single phase, Path1DPathEffectStyle style);
		

	// sk_path_effect_t* sk_path_effect_create_2d_line(float width, const sk_matrix_t* matrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_path_effect_t sk_path_effect_create_2d_line (Single width, Matrix3* matrix);
		

	// sk_path_effect_t* sk_path_effect_create_2d_path(const sk_matrix_t* matrix, const sk_path_t* path)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_path_effect_t sk_path_effect_create_2d_path (Matrix3* matrix, sk_path_t path);
		

	// sk_path_effect_t* sk_path_effect_create_compose(sk_path_effect_t* outer, sk_path_effect_t* inner)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_path_effect_t sk_path_effect_create_compose (sk_path_effect_t outer, sk_path_effect_t inner);
		

	// sk_path_effect_t* sk_path_effect_create_corner(float radius)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_path_effect_t sk_path_effect_create_corner (Single radius);
		

	// sk_path_effect_t* sk_path_effect_create_dash(const float[-1] intervals, int count, float phase)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_path_effect_t sk_path_effect_create_dash (Single* intervals, Int32 count, Single phase);
		

	// sk_path_effect_t* sk_path_effect_create_discrete(float segLength, float deviation, uint32_t seedAssist)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_path_effect_t sk_path_effect_create_discrete (Single segLength, Single deviation, UInt32 seedAssist);
		

	// sk_path_effect_t* sk_path_effect_create_sum(sk_path_effect_t* first, sk_path_effect_t* second)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_path_effect_t sk_path_effect_create_sum (sk_path_effect_t first, sk_path_effect_t second);
		

	// sk_path_effect_t* sk_path_effect_create_trim(float start, float stop, sk_path_effect_trim_mode_t mode)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_path_effect_t sk_path_effect_create_trim (Single start, Single stop, SKTrimPathEffectMode mode);
		

	// void sk_path_effect_unref(sk_path_effect_t* t)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_path_effect_unref (sk_path_effect_t t);
		

	#endregion

	#region sk_picture.h

	// sk_picture_t* sk_picture_deserialize_from_data(sk_data_t* data)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_picture_t sk_picture_deserialize_from_data (sk_data_t data);
		

	// sk_picture_t* sk_picture_deserialize_from_memory(void* buffer, size_t length)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_picture_t sk_picture_deserialize_from_memory (void* buffer, /* size_t */ IntPtr length);
		

	// sk_picture_t* sk_picture_deserialize_from_stream(sk_stream_t* stream)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_picture_t sk_picture_deserialize_from_stream (sk_stream_t stream);
		

	// void sk_picture_get_cull_rect(sk_picture_t*, sk_rect_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_picture_get_cull_rect (sk_picture_t param0, Rect* param1);
		

	// sk_canvas_t* sk_picture_get_recording_canvas(sk_picture_recorder_t* crec)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_canvas_t sk_picture_get_recording_canvas (sk_picture_recorder_t crec);
		

	// uint32_t sk_picture_get_unique_id(sk_picture_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_picture_get_unique_id (sk_picture_t param0);
		

	// sk_shader_t* sk_picture_make_shader(sk_picture_t* src, sk_shader_tilemode_t tmx, sk_shader_tilemode_t tmy, const sk_matrix_t* localMatrix, const sk_rect_t* tile)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_picture_make_shader (sk_picture_t src, TileMode tmx, TileMode tmy, Matrix3* localMatrix, Rect* tile);
		

	// sk_canvas_t* sk_picture_recorder_begin_recording(sk_picture_recorder_t*, const sk_rect_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_canvas_t sk_picture_recorder_begin_recording (sk_picture_recorder_t param0, Rect* param1);
		

	// void sk_picture_recorder_delete(sk_picture_recorder_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_picture_recorder_delete (sk_picture_recorder_t param0);
		

	// sk_picture_t* sk_picture_recorder_end_recording(sk_picture_recorder_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_picture_t sk_picture_recorder_end_recording (sk_picture_recorder_t param0);
		

	// sk_drawable_t* sk_picture_recorder_end_recording_as_drawable(sk_picture_recorder_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_drawable_t sk_picture_recorder_end_recording_as_drawable (sk_picture_recorder_t param0);
		

	// sk_picture_recorder_t* sk_picture_recorder_new()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_picture_recorder_t sk_picture_recorder_new ();
		

	// void sk_picture_ref(sk_picture_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_picture_ref (sk_picture_t param0);
		

	// sk_data_t* sk_picture_serialize_to_data(const sk_picture_t* picture)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_data_t sk_picture_serialize_to_data (sk_picture_t picture);
		

	// void sk_picture_serialize_to_stream(const sk_picture_t* picture, sk_wstream_t* stream)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_picture_serialize_to_stream (sk_picture_t picture, sk_wstream_t stream);
		

	// void sk_picture_unref(sk_picture_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_picture_unref (sk_picture_t param0);
		

	#endregion

	#region sk_pixmap.h

	// void sk_color_get_bit_shift(int* a, int* r, int* g, int* b)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_color_get_bit_shift (Int32* a, Int32* r, Int32* g, Int32* b);
		

	// sk_pmcolor_t sk_color_premultiply(const sk_color_t color)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_color_premultiply (UInt32 color);
		

	// void sk_color_premultiply_array(const sk_color_t* colors, int size, sk_pmcolor_t* pmcolors)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_color_premultiply_array (UInt32* colors, Int32 size, UInt32* pmcolors);
		

	// sk_color_t sk_color_unpremultiply(const sk_pmcolor_t pmcolor)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_color_unpremultiply (UInt32 pmcolor);
		

	// void sk_color_unpremultiply_array(const sk_pmcolor_t* pmcolors, int size, sk_color_t* colors)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_color_unpremultiply_array (UInt32* pmcolors, Int32 size, UInt32* colors);
		

	// bool sk_jpegencoder_encode(sk_wstream_t* dst, const sk_pixmap_t* src, const sk_jpegencoder_options_t* options)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_jpegencoder_encode (sk_wstream_t dst, sk_pixmap_t src, SKJpegEncoderOptions* options);
		

	// void sk_pixmap_destructor(sk_pixmap_t* cpixmap)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_pixmap_destructor (sk_pixmap_t cpixmap);
		

	// bool sk_pixmap_erase_color(const sk_pixmap_t* cpixmap, sk_color_t color, const sk_irect_t* subset)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_pixmap_erase_color (sk_pixmap_t cpixmap, UInt32 color, RectI* subset);
		

	// bool sk_pixmap_erase_color4f(const sk_pixmap_t* cpixmap, const sk_color4f_t* color, sk_colorspace_t* colorspace, const sk_irect_t* subset)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_pixmap_erase_color4f (sk_pixmap_t cpixmap, SKColorF* color, sk_colorspace_t colorspace, RectI* subset);
		

	// bool sk_pixmap_extract_subset(const sk_pixmap_t* cpixmap, sk_pixmap_t* result, const sk_irect_t* subset)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_pixmap_extract_subset (sk_pixmap_t cpixmap, sk_pixmap_t result, RectI* subset);
		

	// void sk_pixmap_get_info(const sk_pixmap_t* cpixmap, sk_imageinfo_t* cinfo)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_pixmap_get_info (sk_pixmap_t cpixmap, SKImageInfoNative* cinfo);
		

	// sk_color_t sk_pixmap_get_pixel_color(const sk_pixmap_t* cpixmap, int x, int y)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_pixmap_get_pixel_color (sk_pixmap_t cpixmap, Int32 x, Int32 y);
		

	// const void* sk_pixmap_get_pixels(const sk_pixmap_t* cpixmap)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void* sk_pixmap_get_pixels (sk_pixmap_t cpixmap);
		

	// const void* sk_pixmap_get_pixels_with_xy(const sk_pixmap_t* cpixmap, int x, int y)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void* sk_pixmap_get_pixels_with_xy (sk_pixmap_t cpixmap, Int32 x, Int32 y);
		

	// size_t sk_pixmap_get_row_bytes(const sk_pixmap_t* cpixmap)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_pixmap_get_row_bytes (sk_pixmap_t cpixmap);
		

	// void* sk_pixmap_get_writable_addr(const sk_pixmap_t* cpixmap)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void* sk_pixmap_get_writable_addr (sk_pixmap_t cpixmap);
		

	// sk_pixmap_t* sk_pixmap_new()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_pixmap_t sk_pixmap_new ();
		

	// sk_pixmap_t* sk_pixmap_new_with_params(const sk_imageinfo_t* cinfo, const void* addr, size_t rowBytes)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_pixmap_t sk_pixmap_new_with_params (SKImageInfoNative* cinfo, void* addr, /* size_t */ IntPtr rowBytes);
		

	// bool sk_pixmap_read_pixels(const sk_pixmap_t* cpixmap, const sk_imageinfo_t* dstInfo, void* dstPixels, size_t dstRowBytes, int srcX, int srcY)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_pixmap_read_pixels (sk_pixmap_t cpixmap, SKImageInfoNative* dstInfo, void* dstPixels, /* size_t */ IntPtr dstRowBytes, Int32 srcX, Int32 srcY);
		

	// void sk_pixmap_reset(sk_pixmap_t* cpixmap)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_pixmap_reset (sk_pixmap_t cpixmap);
		

	// void sk_pixmap_reset_with_params(sk_pixmap_t* cpixmap, const sk_imageinfo_t* cinfo, const void* addr, size_t rowBytes)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_pixmap_reset_with_params (sk_pixmap_t cpixmap, SKImageInfoNative* cinfo, void* addr, /* size_t */ IntPtr rowBytes);
		

	// bool sk_pixmap_scale_pixels(const sk_pixmap_t* cpixmap, const sk_pixmap_t* dst, sk_filter_quality_t quality)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_pixmap_scale_pixels (sk_pixmap_t cpixmap, sk_pixmap_t dst, SKFilterQuality quality);
		

	// bool sk_pngencoder_encode(sk_wstream_t* dst, const sk_pixmap_t* src, const sk_pngencoder_options_t* options)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_pngencoder_encode (sk_wstream_t dst, sk_pixmap_t src, SKPngEncoderOptions* options);
		

	// void sk_swizzle_swap_rb(uint32_t* dest, const uint32_t* src, int count)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_swizzle_swap_rb (UInt32* dest, UInt32* src, Int32 count);
		

	// bool sk_webpencoder_encode(sk_wstream_t* dst, const sk_pixmap_t* src, const sk_webpencoder_options_t* options)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_webpencoder_encode (sk_wstream_t dst, sk_pixmap_t src, SKWebpEncoderOptions* options);
		

	#endregion

	#region sk_region.h

	// void sk_region_cliperator_delete(sk_region_cliperator_t* iter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_cliperator_delete (sk_region_cliperator_t iter);
		

	// bool sk_region_cliperator_done(sk_region_cliperator_t* iter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_cliperator_done (sk_region_cliperator_t iter);
		

	// sk_region_cliperator_t* sk_region_cliperator_new(const sk_region_t* region, const sk_irect_t* clip)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_region_cliperator_t sk_region_cliperator_new (sk_region_t region, RectI* clip);
		

	// void sk_region_cliperator_next(sk_region_cliperator_t* iter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_cliperator_next (sk_region_cliperator_t iter);
		

	// void sk_region_cliperator_rect(const sk_region_cliperator_t* iter, sk_irect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_cliperator_rect (sk_region_cliperator_t iter, RectI* rect);
		

	// bool sk_region_contains(const sk_region_t* r, const sk_region_t* region)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_contains (sk_region_t r, sk_region_t region);
		

	// bool sk_region_contains_point(const sk_region_t* r, int x, int y)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_contains_point (sk_region_t r, Int32 x, Int32 y);
		

	// bool sk_region_contains_rect(const sk_region_t* r, const sk_irect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_contains_rect (sk_region_t r, RectI* rect);
		

	// void sk_region_delete(sk_region_t* r)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_delete (sk_region_t r);
		

	// bool sk_region_get_boundary_path(const sk_region_t* r, sk_path_t* path)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_get_boundary_path (sk_region_t r, sk_path_t path);
		

	// void sk_region_get_bounds(const sk_region_t* r, sk_irect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_get_bounds (sk_region_t r, RectI* rect);
		

	// bool sk_region_intersects(const sk_region_t* r, const sk_region_t* src)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_intersects (sk_region_t r, sk_region_t src);
		

	// bool sk_region_intersects_rect(const sk_region_t* r, const sk_irect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_intersects_rect (sk_region_t r, RectI* rect);
		

	// bool sk_region_is_complex(const sk_region_t* r)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_is_complex (sk_region_t r);
		

	// bool sk_region_is_empty(const sk_region_t* r)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_is_empty (sk_region_t r);
		

	// bool sk_region_is_rect(const sk_region_t* r)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_is_rect (sk_region_t r);
		

	// void sk_region_iterator_delete(sk_region_iterator_t* iter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_iterator_delete (sk_region_iterator_t iter);
		

	// bool sk_region_iterator_done(const sk_region_iterator_t* iter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_iterator_done (sk_region_iterator_t iter);
		

	// sk_region_iterator_t* sk_region_iterator_new(const sk_region_t* region)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_region_iterator_t sk_region_iterator_new (sk_region_t region);
		

	// void sk_region_iterator_next(sk_region_iterator_t* iter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_iterator_next (sk_region_iterator_t iter);
		

	// void sk_region_iterator_rect(const sk_region_iterator_t* iter, sk_irect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_iterator_rect (sk_region_iterator_t iter, RectI* rect);
		

	// bool sk_region_iterator_rewind(sk_region_iterator_t* iter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_iterator_rewind (sk_region_iterator_t iter);
		

	// sk_region_t* sk_region_new()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_region_t sk_region_new ();
		

	// bool sk_region_op(sk_region_t* r, const sk_region_t* region, sk_region_op_t op)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_op (sk_region_t r, sk_region_t region, SKRegionOperation op);
		

	// bool sk_region_op_rect(sk_region_t* r, const sk_irect_t* rect, sk_region_op_t op)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_op_rect (sk_region_t r, RectI* rect, SKRegionOperation op);
		

	// bool sk_region_quick_contains(const sk_region_t* r, const sk_irect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_quick_contains (sk_region_t r, RectI* rect);
		

	// bool sk_region_quick_reject(const sk_region_t* r, const sk_region_t* region)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_quick_reject (sk_region_t r, sk_region_t region);
		

	// bool sk_region_quick_reject_rect(const sk_region_t* r, const sk_irect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_quick_reject_rect (sk_region_t r, RectI* rect);
		

	// bool sk_region_set_empty(sk_region_t* r)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_set_empty (sk_region_t r);
		

	// bool sk_region_set_path(sk_region_t* r, const sk_path_t* t, const sk_region_t* clip)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_set_path (sk_region_t r, sk_path_t t, sk_region_t clip);
		

	// bool sk_region_set_rect(sk_region_t* r, const sk_irect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_set_rect (sk_region_t r, RectI* rect);
		

	// bool sk_region_set_rects(sk_region_t* r, const sk_irect_t* rects, int count)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_set_rects (sk_region_t r, RectI* rects, Int32 count);
		

	// bool sk_region_set_region(sk_region_t* r, const sk_region_t* region)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_set_region (sk_region_t r, sk_region_t region);
		

	// void sk_region_spanerator_delete(sk_region_spanerator_t* iter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_spanerator_delete (sk_region_spanerator_t iter);
		

	// sk_region_spanerator_t* sk_region_spanerator_new(const sk_region_t* region, int y, int left, int right)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_region_spanerator_t sk_region_spanerator_new (sk_region_t region, Int32 y, Int32 left, Int32 right);
		

	// bool sk_region_spanerator_next(sk_region_spanerator_t* iter, int* left, int* right)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_region_spanerator_next (sk_region_spanerator_t iter, Int32* left, Int32* right);
		

	// void sk_region_translate(sk_region_t* r, int x, int y)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_region_translate (sk_region_t r, Int32 x, Int32 y);
		

	#endregion

	#region sk_rrect.h

	// bool sk_rrect_contains(const sk_rrect_t* rrect, const sk_rect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_rrect_contains (sk_rrect_t rrect, Rect* rect);
		

	// void sk_rrect_delete(const sk_rrect_t* rrect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_delete (sk_rrect_t rrect);
		

	// float sk_rrect_get_height(const sk_rrect_t* rrect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Single sk_rrect_get_height (sk_rrect_t rrect);
		

	// void sk_rrect_get_radii(const sk_rrect_t* rrect, sk_rrect_corner_t corner, sk_vector_t* radii)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_get_radii (sk_rrect_t rrect, SKRoundRectCorner corner, Point* radii);
		

	// void sk_rrect_get_rect(const sk_rrect_t* rrect, sk_rect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_get_rect (sk_rrect_t rrect, Rect* rect);
		

	// sk_rrect_type_t sk_rrect_get_type(const sk_rrect_t* rrect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKRoundRectType sk_rrect_get_type (sk_rrect_t rrect);
		

	// float sk_rrect_get_width(const sk_rrect_t* rrect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Single sk_rrect_get_width (sk_rrect_t rrect);
		

	// void sk_rrect_inset(sk_rrect_t* rrect, float dx, float dy)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_inset (sk_rrect_t rrect, Single dx, Single dy);
		

	// bool sk_rrect_is_valid(const sk_rrect_t* rrect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_rrect_is_valid (sk_rrect_t rrect);
		

	// sk_rrect_t* sk_rrect_new()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_rrect_t sk_rrect_new ();
		

	// sk_rrect_t* sk_rrect_new_copy(const sk_rrect_t* rrect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_rrect_t sk_rrect_new_copy (sk_rrect_t rrect);
		

	// void sk_rrect_offset(sk_rrect_t* rrect, float dx, float dy)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_offset (sk_rrect_t rrect, Single dx, Single dy);
		

	// void sk_rrect_outset(sk_rrect_t* rrect, float dx, float dy)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_outset (sk_rrect_t rrect, Single dx, Single dy);
		

	// void sk_rrect_set_empty(sk_rrect_t* rrect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_set_empty (sk_rrect_t rrect);
		

	// void sk_rrect_set_nine_patch(sk_rrect_t* rrect, const sk_rect_t* rect, float leftRad, float topRad, float rightRad, float bottomRad)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_set_nine_patch (sk_rrect_t rrect, Rect* rect, Single leftRad, Single topRad, Single rightRad, Single bottomRad);
		

	// void sk_rrect_set_oval(sk_rrect_t* rrect, const sk_rect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_set_oval (sk_rrect_t rrect, Rect* rect);
		

	// void sk_rrect_set_rect(sk_rrect_t* rrect, const sk_rect_t* rect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_set_rect (sk_rrect_t rrect, Rect* rect);
		

	// void sk_rrect_set_rect_radii(sk_rrect_t* rrect, const sk_rect_t* rect, const sk_vector_t* radii)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_set_rect_radii (sk_rrect_t rrect, Rect* rect, Point* radii);
		

	// void sk_rrect_set_rect_xy(sk_rrect_t* rrect, const sk_rect_t* rect, float xRad, float yRad)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_rrect_set_rect_xy (sk_rrect_t rrect, Rect* rect, Single xRad, Single yRad);
		

	// bool sk_rrect_transform(sk_rrect_t* rrect, const sk_matrix_t* matrix, sk_rrect_t* dest)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_rrect_transform (sk_rrect_t rrect, Matrix3* matrix, sk_rrect_t dest);
		

	#endregion

	#region sk_runtimeeffect.h

	// void sk_runtimeeffect_get_child_name(const sk_runtimeeffect_t* effect, int index, sk_string_t* name)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_runtimeeffect_get_child_name (sk_runtimeeffect_t effect, Int32 index, sk_string_t name);
		

	// size_t sk_runtimeeffect_get_children_count(const sk_runtimeeffect_t* effect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_runtimeeffect_get_children_count (sk_runtimeeffect_t effect);
		

	// const sk_runtimeeffect_uniform_t* sk_runtimeeffect_get_uniform_from_index(const sk_runtimeeffect_t* effect, int index)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_runtimeeffect_uniform_t sk_runtimeeffect_get_uniform_from_index (sk_runtimeeffect_t effect, Int32 index);
		

	// const sk_runtimeeffect_uniform_t* sk_runtimeeffect_get_uniform_from_name(const sk_runtimeeffect_t* effect, const char* name, size_t len)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_runtimeeffect_uniform_t sk_runtimeeffect_get_uniform_from_name (sk_runtimeeffect_t effect, /* char */ void* name, /* size_t */ IntPtr len);
		

	// void sk_runtimeeffect_get_uniform_name(const sk_runtimeeffect_t* effect, int index, sk_string_t* name)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_runtimeeffect_get_uniform_name (sk_runtimeeffect_t effect, Int32 index, sk_string_t name);
		

	// size_t sk_runtimeeffect_get_uniform_size(const sk_runtimeeffect_t* effect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_runtimeeffect_get_uniform_size (sk_runtimeeffect_t effect);
		

	// size_t sk_runtimeeffect_get_uniforms_count(const sk_runtimeeffect_t* effect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_runtimeeffect_get_uniforms_count (sk_runtimeeffect_t effect);
		

	// sk_runtimeeffect_t* sk_runtimeeffect_make(sk_string_t* sksl, sk_string_t* error)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_runtimeeffect_t sk_runtimeeffect_make (sk_string_t sksl, sk_string_t error);
		

	// sk_colorfilter_t* sk_runtimeeffect_make_color_filter(sk_runtimeeffect_t* effect, sk_data_t* uniforms, sk_colorfilter_t** children, size_t childCount)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_colorfilter_t sk_runtimeeffect_make_color_filter (sk_runtimeeffect_t effect, sk_data_t uniforms, sk_colorfilter_t* children, /* size_t */ IntPtr childCount);
		

	// sk_shader_t* sk_runtimeeffect_make_shader(sk_runtimeeffect_t* effect, sk_data_t* uniforms, sk_shader_t** children, size_t childCount, const sk_matrix_t* localMatrix, bool isOpaque)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_runtimeeffect_make_shader (sk_runtimeeffect_t effect, sk_data_t uniforms, sk_shader_t* children, /* size_t */ IntPtr childCount, Matrix3* localMatrix, [MarshalAs (UnmanagedType.I1)] bool isOpaque);
		

	// size_t sk_runtimeeffect_uniform_get_offset(const sk_runtimeeffect_uniform_t* variable)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_runtimeeffect_uniform_get_offset (sk_runtimeeffect_uniform_t variable);
		

	// size_t sk_runtimeeffect_uniform_get_size_in_bytes(const sk_runtimeeffect_uniform_t* variable)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_runtimeeffect_uniform_get_size_in_bytes (sk_runtimeeffect_uniform_t variable);
		

	// void sk_runtimeeffect_unref(sk_runtimeeffect_t* effect)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_runtimeeffect_unref (sk_runtimeeffect_t effect);
		

	#endregion

	#region sk_shader.h

	// sk_shader_t* sk_shader_new_blend(sk_blendmode_t mode, const sk_shader_t* dst, const sk_shader_t* src)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_blend (BlendMode mode, sk_shader_t dst, sk_shader_t src);
		

	// sk_shader_t* sk_shader_new_color(sk_color_t color)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_color (UInt32 color);
		

	// sk_shader_t* sk_shader_new_color4f(const sk_color4f_t* color, const sk_colorspace_t* colorspace)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_color4f (SKColorF* color, sk_colorspace_t colorspace);
		

	// sk_shader_t* sk_shader_new_empty()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_empty ();
		

	// sk_shader_t* sk_shader_new_lerp(float t, const sk_shader_t* dst, const sk_shader_t* src)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_lerp (Single t, sk_shader_t dst, sk_shader_t src);
		

	// sk_shader_t* sk_shader_new_linear_gradient(const sk_point_t[2] points = 2, const sk_color_t[-1] colors, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_linear_gradient (Point* points, UInt32* colors, Single* colorPos, Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);
		

	// sk_shader_t* sk_shader_new_linear_gradient_color4f(const sk_point_t[2] points = 2, const sk_color4f_t* colors, const sk_colorspace_t* colorspace, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_linear_gradient_color4f (Point* points, SKColorF* colors, sk_colorspace_t colorspace, Single* colorPos, Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);
		

	// sk_shader_t* sk_shader_new_perlin_noise_improved_noise(float baseFrequencyX, float baseFrequencyY, int numOctaves, float z)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_perlin_noise_improved_noise (Single baseFrequencyX, Single baseFrequencyY, Int32 numOctaves, Single z);
		

	// sk_shader_t* sk_shader_new_radial_gradient(const sk_point_t* center, float radius, const sk_color_t[-1] colors, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_radial_gradient (Point* center, Single radius, UInt32* colors, Single* colorPos, Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);
		

	// sk_shader_t* sk_shader_new_radial_gradient_color4f(const sk_point_t* center, float radius, const sk_color4f_t* colors, const sk_colorspace_t* colorspace, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_radial_gradient_color4f (Point* center, Single radius, SKColorF* colors, sk_colorspace_t colorspace, Single* colorPos, Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);
		

	// sk_shader_t* sk_shader_new_sweep_gradient(const sk_point_t* center, const sk_color_t[-1] colors, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, float startAngle, float endAngle, const sk_matrix_t* localMatrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_sweep_gradient (Point* center, UInt32* colors, Single* colorPos, Int32 colorCount, TileMode tileMode, Single startAngle, Single endAngle, Matrix3* localMatrix);
		

	// sk_shader_t* sk_shader_new_sweep_gradient_color4f(const sk_point_t* center, const sk_color4f_t* colors, const sk_colorspace_t* colorspace, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, float startAngle, float endAngle, const sk_matrix_t* localMatrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_sweep_gradient_color4f (Point* center, SKColorF* colors, sk_colorspace_t colorspace, Single* colorPos, Int32 colorCount, TileMode tileMode, Single startAngle, Single endAngle, Matrix3* localMatrix);
		

	// sk_shader_t* sk_shader_new_two_point_conical_gradient(const sk_point_t* start, float startRadius, const sk_point_t* end, float endRadius, const sk_color_t[-1] colors, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_two_point_conical_gradient (Point* start, Single startRadius, Point* end, Single endRadius, UInt32* colors, Single* colorPos, Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);
		

	// sk_shader_t* sk_shader_new_two_point_conical_gradient_color4f(const sk_point_t* start, float startRadius, const sk_point_t* end, float endRadius, const sk_color4f_t* colors, const sk_colorspace_t* colorspace, const float[-1] colorPos, int colorCount, sk_shader_tilemode_t tileMode, const sk_matrix_t* localMatrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_new_two_point_conical_gradient_color4f (Point* start, Single startRadius, Point* end, Single endRadius, SKColorF* colors, sk_colorspace_t colorspace, Single* colorPos, Int32 colorCount, TileMode tileMode, Matrix3* localMatrix);
		

	// void sk_shader_ref(sk_shader_t* shader)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_shader_ref (sk_shader_t shader);
		

	// void sk_shader_unref(sk_shader_t* shader)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_shader_unref (sk_shader_t shader);
		

	// sk_shader_t* sk_shader_with_color_filter(const sk_shader_t* shader, const sk_colorfilter_t* filter)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_with_color_filter (sk_shader_t shader, sk_colorfilter_t filter);
		

	// sk_shader_t* sk_shader_with_local_matrix(const sk_shader_t* shader, const sk_matrix_t* localMatrix)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_shader_t sk_shader_with_local_matrix (sk_shader_t shader, Matrix3* localMatrix);
		

	#endregion

	#region sk_string.h

	// void sk_string_destructor(const sk_string_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_string_destructor (sk_string_t param0);
		

	// const char* sk_string_get_c_str(const sk_string_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* char */ void* sk_string_get_c_str (sk_string_t param0);
		

	// size_t sk_string_get_size(const sk_string_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern /* size_t */ IntPtr sk_string_get_size (sk_string_t param0);
		

	// sk_string_t* sk_string_new_empty()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_string_t sk_string_new_empty ();
		

	// sk_string_t* sk_string_new_with_copy(const char* src, size_t length)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_string_t sk_string_new_with_copy (/* char */ void* src, /* size_t */ IntPtr length);
		

	#endregion

	#region sk_surface.h

	// void sk_surface_draw(sk_surface_t* surface, sk_canvas_t* canvas, float x, float y, const sk_paint_t* paint)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_surface_draw (sk_surface_t surface, sk_canvas_t canvas, Single x, Single y, sk_paint_t paint);

	// sk_canvas_t* sk_surface_get_canvas(sk_surface_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_canvas_t sk_surface_get_canvas (sk_surface_t param0);
		

	// const sk_surfaceprops_t* sk_surface_get_props(sk_surface_t* surface)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_surfaceprops_t sk_surface_get_props (sk_surface_t surface);
		

	// gr_recording_context_t* sk_surface_get_recording_context(sk_surface_t* surface)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern gr_recording_context_t sk_surface_get_recording_context (sk_surface_t surface);
		

	// sk_surface_t* sk_surface_new_backend_render_target(gr_recording_context_t* context, const gr_backendrendertarget_t* target, gr_surfaceorigin_t origin, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_surface_t sk_surface_new_backend_render_target (gr_recording_context_t context, gr_backendrendertarget_t target, SurfaceOrigin origin, SKColorTypeNative colorType, sk_colorspace_t colorspace, sk_surfaceprops_t props);
		

	// sk_surface_t* sk_surface_new_backend_texture(gr_recording_context_t* context, const gr_backendtexture_t* texture, gr_surfaceorigin_t origin, int samples, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_surface_t sk_surface_new_backend_texture (gr_recording_context_t context, gr_backendtexture_t texture, SurfaceOrigin origin, Int32 samples, SKColorTypeNative colorType, sk_colorspace_t colorspace, sk_surfaceprops_t props);
		

	// sk_image_t* sk_surface_new_image_snapshot(sk_surface_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_image_t sk_surface_new_image_snapshot (sk_surface_t param0);
		

	// sk_image_t* sk_surface_new_image_snapshot_with_crop(sk_surface_t* surface, const sk_irect_t* bounds)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_image_t sk_surface_new_image_snapshot_with_crop (sk_surface_t surface, RectI* bounds);
		

	// sk_surface_t* sk_surface_new_metal_layer(gr_recording_context_t* context, const void* layer, gr_surfaceorigin_t origin, int sampleCount, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props, const void** drawable)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_surface_t sk_surface_new_metal_layer (gr_recording_context_t context, void* layer, SurfaceOrigin origin, Int32 sampleCount, SKColorTypeNative colorType, sk_colorspace_t colorspace, sk_surfaceprops_t props, void** drawable);
		

	// sk_surface_t* sk_surface_new_metal_view(gr_recording_context_t* context, const void* mtkView, gr_surfaceorigin_t origin, int sampleCount, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_surface_t sk_surface_new_metal_view (gr_recording_context_t context, void* mtkView, SurfaceOrigin origin, Int32 sampleCount, SKColorTypeNative colorType, sk_colorspace_t colorspace, sk_surfaceprops_t props);
		

	// sk_surface_t* sk_surface_new_raster(const sk_imageinfo_t*, size_t rowBytes, const sk_surfaceprops_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_surface_t sk_surface_new_raster (SKImageInfoNative* param0, /* size_t */ IntPtr rowBytes, sk_surfaceprops_t param2);
		

	// sk_surface_t* sk_surface_new_raster_direct(const sk_imageinfo_t*, void* pixels, size_t rowBytes, const sk_surface_raster_release_proc releaseProc, void* context, const sk_surfaceprops_t* props)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_surface_t sk_surface_new_raster_direct (SKImageInfoNative* param0, void* pixels, /* size_t */ IntPtr rowBytes, SKSurfaceRasterReleaseProxyDelegate releaseProc, void* context, sk_surfaceprops_t props);
		

	// sk_surface_t* sk_surface_new_render_target(gr_recording_context_t* context, bool budgeted, const sk_imageinfo_t* cinfo, int sampleCount, gr_surfaceorigin_t origin, const sk_surfaceprops_t* props, bool shouldCreateWithMips)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_surface_t sk_surface_new_render_target (gr_recording_context_t context, [MarshalAs (UnmanagedType.I1)] bool budgeted, SKImageInfoNative* cinfo, Int32 sampleCount, SurfaceOrigin origin, sk_surfaceprops_t props, [MarshalAs (UnmanagedType.I1)] bool shouldCreateWithMips);
		

	// bool sk_surface_peek_pixels(sk_surface_t* surface, sk_pixmap_t* pixmap)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_surface_peek_pixels (sk_surface_t surface, sk_pixmap_t pixmap);
		

	// bool sk_surface_read_pixels(sk_surface_t* surface, sk_imageinfo_t* dstInfo, void* dstPixels, size_t dstRowBytes, int srcX, int srcY)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs (UnmanagedType.I1)]
	internal static extern bool sk_surface_read_pixels (sk_surface_t surface, SKImageInfoNative* dstInfo, void* dstPixels, /* size_t */ IntPtr dstRowBytes, Int32 srcX, Int32 srcY);
		

	// void sk_surface_unref(sk_surface_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_surface_unref (sk_surface_t param0);
		

	// void sk_surfaceprops_delete(sk_surfaceprops_t* props)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_surfaceprops_delete (sk_surfaceprops_t props);
		

	// uint32_t sk_surfaceprops_get_flags(sk_surfaceprops_t* props)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_surfaceprops_get_flags (sk_surfaceprops_t props);
		

	// sk_pixelgeometry_t sk_surfaceprops_get_pixel_geometry(sk_surfaceprops_t* props)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKPixelGeometry sk_surfaceprops_get_pixel_geometry (sk_surfaceprops_t props);
		

	// sk_surfaceprops_t* sk_surfaceprops_new(uint32_t flags, sk_pixelgeometry_t geometry)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_surfaceprops_t sk_surfaceprops_new (UInt32 flags, SKPixelGeometry geometry);
		

	#endregion

	#region sk_svg.h

	// sk_canvas_t* sk_svgcanvas_create_with_stream(const sk_rect_t* bounds, sk_wstream_t* stream)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_canvas_t sk_svgcanvas_create_with_stream (Rect* bounds, sk_wstream_t stream);
		

	// sk_canvas_t* sk_svgcanvas_create_with_writer(const sk_rect_t* bounds, sk_xmlwriter_t* writer)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_canvas_t sk_svgcanvas_create_with_writer (Rect* bounds, sk_xmlwriter_t writer);
		

	#endregion

	#region sk_textblob.h

	// void sk_textblob_builder_alloc_run(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float x, float y, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_builder_alloc_run (sk_textblob_builder_t builder, sk_font_t font, Int32 count, Single x, Single y, Rect* bounds, SKRunBufferInternal* runbuffer);
		

	// void sk_textblob_builder_alloc_run_pos(sk_textblob_builder_t* builder, const sk_font_t* font, int count, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_builder_alloc_run_pos (sk_textblob_builder_t builder, sk_font_t font, Int32 count, Rect* bounds, SKRunBufferInternal* runbuffer);
		

	// void sk_textblob_builder_alloc_run_pos_h(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float y, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_builder_alloc_run_pos_h (sk_textblob_builder_t builder, sk_font_t font, Int32 count, Single y, Rect* bounds, SKRunBufferInternal* runbuffer);
		

	// void sk_textblob_builder_alloc_run_rsxform(sk_textblob_builder_t* builder, const sk_font_t* font, int count, sk_textblob_builder_runbuffer_t* runbuffer)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_builder_alloc_run_rsxform (sk_textblob_builder_t builder, sk_font_t font, Int32 count, SKRunBufferInternal* runbuffer);
		

	// void sk_textblob_builder_alloc_run_text(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float x, float y, int textByteCount, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_builder_alloc_run_text (sk_textblob_builder_t builder, sk_font_t font, Int32 count, Single x, Single y, Int32 textByteCount, Rect* bounds, SKRunBufferInternal* runbuffer);
		

	// void sk_textblob_builder_alloc_run_text_pos(sk_textblob_builder_t* builder, const sk_font_t* font, int count, int textByteCount, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_builder_alloc_run_text_pos (sk_textblob_builder_t builder, sk_font_t font, Int32 count, Int32 textByteCount, Rect* bounds, SKRunBufferInternal* runbuffer);
		

	// void sk_textblob_builder_alloc_run_text_pos_h(sk_textblob_builder_t* builder, const sk_font_t* font, int count, float y, int textByteCount, const sk_rect_t* bounds, sk_textblob_builder_runbuffer_t* runbuffer)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_builder_alloc_run_text_pos_h (sk_textblob_builder_t builder, sk_font_t font, Int32 count, Single y, Int32 textByteCount, Rect* bounds, SKRunBufferInternal* runbuffer);
		

	// void sk_textblob_builder_delete(sk_textblob_builder_t* builder)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_builder_delete (sk_textblob_builder_t builder);
		

	// sk_textblob_t* sk_textblob_builder_make(sk_textblob_builder_t* builder)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_textblob_t sk_textblob_builder_make (sk_textblob_builder_t builder);
		

	// sk_textblob_builder_t* sk_textblob_builder_new()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_textblob_builder_t sk_textblob_builder_new ();
		

	// void sk_textblob_get_bounds(const sk_textblob_t* blob, sk_rect_t* bounds)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_get_bounds (sk_textblob_t blob, Rect* bounds);
		

	// int sk_textblob_get_intercepts(const sk_textblob_t* blob, const float[2] bounds = 2, float[-1] intervals, const sk_paint_t* paint)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern Int32 sk_textblob_get_intercepts (sk_textblob_t blob, Single* bounds, Single* intervals, sk_paint_t paint);
		

	// uint32_t sk_textblob_get_unique_id(const sk_textblob_t* blob)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern UInt32 sk_textblob_get_unique_id (sk_textblob_t blob);
		

	// void sk_textblob_ref(const sk_textblob_t* blob)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_ref (sk_textblob_t blob);
		

	// void sk_textblob_unref(const sk_textblob_t* blob)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_textblob_unref (sk_textblob_t blob);
		

	#endregion

	#region sk_vertices.h

	// sk_vertices_t* sk_vertices_make_copy(sk_vertices_vertex_mode_t vmode, int vertexCount, const sk_point_t* positions, const sk_point_t* texs, const sk_color_t* colors, int indexCount, const uint16_t* indices)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_vertices_t sk_vertices_make_copy (SKVertexMode vmode, Int32 vertexCount, Point* positions, Point* texs, UInt32* colors, Int32 indexCount, UInt16* indices);
		

	// void sk_vertices_ref(sk_vertices_t* cvertices)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_vertices_ref (sk_vertices_t cvertices);
		

	// void sk_vertices_unref(sk_vertices_t* cvertices)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_vertices_unref (sk_vertices_t cvertices);
		

	#endregion

	#region sk_xml.h

	// void sk_xmlstreamwriter_delete(sk_xmlstreamwriter_t* writer)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_xmlstreamwriter_delete (sk_xmlstreamwriter_t writer);
		

	// sk_xmlstreamwriter_t* sk_xmlstreamwriter_new(sk_wstream_t* stream)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_xmlstreamwriter_t sk_xmlstreamwriter_new (sk_wstream_t stream);
		

	#endregion

	#region sk_compatpaint.h

	// sk_compatpaint_t* sk_compatpaint_clone(const sk_compatpaint_t* paint)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_compatpaint_t sk_compatpaint_clone (sk_compatpaint_t paint);
		

	// void sk_compatpaint_delete(sk_compatpaint_t* paint)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_compatpaint_delete (sk_compatpaint_t paint);
		

	// sk_font_t* sk_compatpaint_get_font(sk_compatpaint_t* paint)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_font_t sk_compatpaint_get_font (sk_compatpaint_t paint);
		

	// sk_text_align_t sk_compatpaint_get_text_align(const sk_compatpaint_t* paint)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern TextAlign sk_compatpaint_get_text_align (sk_compatpaint_t paint);
		

	// sk_text_encoding_t sk_compatpaint_get_text_encoding(const sk_compatpaint_t* paint)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern SKTextEncoding sk_compatpaint_get_text_encoding (sk_compatpaint_t paint);
		

	// sk_font_t* sk_compatpaint_make_font(sk_compatpaint_t* paint)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_font_t sk_compatpaint_make_font (sk_compatpaint_t paint);
		

	// sk_compatpaint_t* sk_compatpaint_new()
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_compatpaint_t sk_compatpaint_new ();
		

	// sk_compatpaint_t* sk_compatpaint_new_with_font(const sk_font_t* font)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_compatpaint_t sk_compatpaint_new_with_font (sk_font_t font);
		

	// void sk_compatpaint_reset(sk_compatpaint_t* paint)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_compatpaint_reset (sk_compatpaint_t paint);
		

	// void sk_compatpaint_set_text_align(sk_compatpaint_t* paint, sk_text_align_t align)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_compatpaint_set_text_align (sk_compatpaint_t paint, TextAlign align);
		

	// void sk_compatpaint_set_text_encoding(sk_compatpaint_t* paint, sk_text_encoding_t encoding)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_compatpaint_set_text_encoding (sk_compatpaint_t paint, SKTextEncoding encoding);
		

	#endregion

	#region sk_manageddrawable.h

	// sk_manageddrawable_t* sk_manageddrawable_new(void* context)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_manageddrawable_t sk_manageddrawable_new (void* context);
		

	// void sk_manageddrawable_set_procs(sk_manageddrawable_procs_t procs)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_manageddrawable_set_procs (SKManagedDrawableDelegates procs);
		

	// void sk_manageddrawable_unref(sk_manageddrawable_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_manageddrawable_unref (sk_manageddrawable_t param0);
		

	#endregion

	#region sk_managedtracememorydump.h

	// void sk_managedtracememorydump_delete(sk_managedtracememorydump_t*)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_managedtracememorydump_delete (sk_managedtracememorydump_t param0);
		

	// sk_managedtracememorydump_t* sk_managedtracememorydump_new(bool detailed, bool dumpWrapped, void* context)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern sk_managedtracememorydump_t sk_managedtracememorydump_new ([MarshalAs (UnmanagedType.I1)] bool detailed, [MarshalAs (UnmanagedType.I1)] bool dumpWrapped, void* context);
		

	// void sk_managedtracememorydump_set_procs(sk_managedtracememorydump_procs_t procs)
		
	[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
	internal static extern void sk_managedtracememorydump_set_procs (SKManagedTraceMemoryDumpDelegates procs);
		

	#endregion

}