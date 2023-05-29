#if !__WEB__
using System;
using System.Runtime.InteropServices;

namespace PixUI
{
	#region Class declarations

	using gr_backendrendertarget_t = IntPtr;
	using gr_backendtexture_t = IntPtr;
	using gr_direct_context_t = IntPtr;
	using gr_glinterface_t = IntPtr;
	using gr_recording_context_t = IntPtr;
	using gr_vk_extensions_t = IntPtr;
	using gr_vk_memory_allocator_t = IntPtr;
	using gr_vkinterface_t = IntPtr;
	using sk_bitmap_t = IntPtr;
	using sk_canvas_t = IntPtr;
	using sk_codec_t = IntPtr;
	using sk_colorfilter_t = IntPtr;
	using sk_colorspace_icc_profile_t = IntPtr;
	using sk_colorspace_t = IntPtr;
	using sk_colortable_t = IntPtr;
	using sk_compatpaint_t = IntPtr;
	using sk_data_t = IntPtr;
	using sk_document_t = IntPtr;
	using sk_drawable_t = IntPtr;
	using sk_font_t = IntPtr;
	using sk_fontmgr_t = IntPtr;
	using sk_fontstyleset_t = IntPtr;
	using sk_image_t = IntPtr;
	using sk_imagefilter_croprect_t = IntPtr;
	using sk_imagefilter_t = IntPtr;
	using sk_manageddrawable_t = IntPtr;
	using sk_managedtracememorydump_t = IntPtr;
	using sk_maskfilter_t = IntPtr;
	using sk_matrix4_t = IntPtr;
	using sk_nodraw_canvas_t = IntPtr;
	using sk_nvrefcnt_t = IntPtr;
	using sk_nway_canvas_t = IntPtr;
	using sk_opbuilder_t = IntPtr;
	using sk_overdraw_canvas_t = IntPtr;
	using sk_paint_t = IntPtr;
	using sk_path_effect_t = IntPtr;
	using sk_path_iterator_t = IntPtr;
	using sk_path_rawiterator_t = IntPtr;
	using sk_path_t = IntPtr;
	using sk_pathmeasure_t = IntPtr;
	using sk_picture_recorder_t = IntPtr;
	using sk_picture_t = IntPtr;
	using sk_pixelref_factory_t = IntPtr;
	using sk_pixmap_t = IntPtr;
	using sk_refcnt_t = IntPtr;
	using sk_region_cliperator_t = IntPtr;
	using sk_region_iterator_t = IntPtr;
	using sk_region_spanerator_t = IntPtr;
	using sk_region_t = IntPtr;
	using sk_rrect_t = IntPtr;
	using sk_runtimeeffect_t = IntPtr;
	using sk_runtimeeffect_uniform_t = IntPtr;
	using sk_shader_t = IntPtr;
	using sk_stream_asset_t = IntPtr;
	using sk_stream_filestream_t = IntPtr;
	using sk_stream_managedstream_t = IntPtr;
	using sk_stream_memorystream_t = IntPtr;
	using sk_stream_streamrewindable_t = IntPtr;
	using sk_stream_t = IntPtr;
	using sk_string_t = IntPtr;
	using sk_surface_t = IntPtr;
	using sk_surfaceprops_t = IntPtr;
	using sk_svgcanvas_t = IntPtr;
	using sk_textblob_builder_t = IntPtr;
	using sk_textblob_t = IntPtr;
	using sk_tracememorydump_t = IntPtr;
	using sk_typeface_t = IntPtr;
	using sk_vertices_t = IntPtr;
	using sk_wstream_dynamicmemorystream_t = IntPtr;
	using sk_wstream_filestream_t = IntPtr;
	using sk_wstream_managedstream_t = IntPtr;
	using sk_wstream_t = IntPtr;
	using sk_xmlstreamwriter_t = IntPtr;
	using sk_xmlwriter_t = IntPtr;
	using vk_device_t = IntPtr;
	using vk_instance_t = IntPtr;
	using vk_physical_device_features_2_t = IntPtr;
	using vk_physical_device_features_t = IntPtr;
	using vk_physical_device_t = IntPtr;
	using vk_queue_t = IntPtr;

	#endregion

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
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool gr_backendrendertarget_get_gl_framebufferinfo (gr_backendrendertarget_t rendertarget, GRGlFramebufferInfo* glInfo);

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
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern gr_backendrendertarget_t gr_backendrendertarget_new_gl (Int32 width, Int32 height, Int32 samples, Int32 stencils, GRGlFramebufferInfo* glInfo);

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
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool gr_backendtexture_get_gl_textureinfo (gr_backendtexture_t texture, GRGlTextureInfo* glInfo);

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
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern gr_backendtexture_t gr_backendtexture_new_gl (Int32 width, Int32 height, [MarshalAs (UnmanagedType.I1)] bool mipmapped, GRGlTextureInfo* glInfo);

		// gr_backendtexture_t* gr_backendtexture_new_metal(int width, int height, bool mipmapped, const gr_mtl_textureinfo_t* mtlInfo)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern gr_backendtexture_t gr_backendtexture_new_metal (Int32 width, Int32 height, [MarshalAs (UnmanagedType.I1)] bool mipmapped, GRMtlTextureInfoNative* mtlInfo);

		// gr_backendtexture_t* gr_backendtexture_new_vulkan(int width, int height, const gr_vk_imageinfo_t* vkInfo)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern gr_backendtexture_t gr_backendtexture_new_vulkan (Int32 width, Int32 height, GRVkImageInfo* vkInfo);

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
		
		// bool sk_bitmap_install_mask_pixels(sk_bitmap_t* cbitmap, const sk_mask_t* cmask)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_bitmap_install_mask_pixels (sk_bitmap_t cbitmap, SKMask* cmask);
		
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

		#region sk_canvas.h

		// void sk_canvas_clear(sk_canvas_t*, sk_color_t)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_clear (sk_canvas_t param0, UInt32 param1);

		// void sk_canvas_clear_color4f(sk_canvas_t*, sk_color4f_t)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_clear_color4f (sk_canvas_t param0, SKColorF param1);
		
		// void sk_canvas_clip_path_with_operation(sk_canvas_t* t, const sk_path_t* crect, sk_clipop_t op, bool doAA)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_clip_path_with_operation (sk_canvas_t t, sk_path_t crect, ClipOp op, [MarshalAs (UnmanagedType.I1)] bool doAA);
		
		// void sk_canvas_clip_rect_with_operation(sk_canvas_t* t, const sk_rect_t* crect, sk_clipop_t op, bool doAA)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_clip_rect_with_operation (sk_canvas_t t, Rect* crect, ClipOp op, [MarshalAs (UnmanagedType.I1)] bool doAA);
		

		// void sk_canvas_clip_region(sk_canvas_t* canvas, const sk_region_t* region, sk_clipop_t op)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_clip_region (sk_canvas_t canvas, sk_region_t region, ClipOp op);
		

		// void sk_canvas_clip_rrect_with_operation(sk_canvas_t* t, const sk_rrect_t* crect, sk_clipop_t op, bool doAA)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_clip_rrect_with_operation (sk_canvas_t t, sk_rrect_t crect, ClipOp op, [MarshalAs (UnmanagedType.I1)] bool doAA);
		

		// void sk_canvas_concat(sk_canvas_t*, const sk_matrix_t*)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_concat (sk_canvas_t param0, Matrix4* param1);

		// void sk_canvas_destroy(sk_canvas_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_destroy (sk_canvas_t param0);
		

		// void sk_canvas_discard(sk_canvas_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_discard (sk_canvas_t param0);
		

		// void sk_canvas_draw_annotation(sk_canvas_t* t, const sk_rect_t* rect, const char* key, sk_data_t* value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_annotation (sk_canvas_t t, Rect* rect, /* char */ void* key, sk_data_t value);
		

		// void sk_canvas_draw_arc(sk_canvas_t* ccanvas, const sk_rect_t* oval, float startAngle, float sweepAngle, bool useCenter, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_arc (sk_canvas_t ccanvas, Rect* oval, Single startAngle, Single sweepAngle, [MarshalAs (UnmanagedType.I1)] bool useCenter, sk_paint_t paint);
		

		// void sk_canvas_draw_atlas(sk_canvas_t* ccanvas, const sk_image_t* atlas, const sk_rsxform_t* xform, const sk_rect_t* tex, const sk_color_t* colors, int count, sk_blendmode_t mode, const sk_rect_t* cullRect, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_atlas (sk_canvas_t ccanvas, sk_image_t atlas, SKRotationScaleMatrix* xform, Rect* tex, UInt32* colors, Int32 count, BlendMode mode, Rect* cullRect, sk_paint_t paint);
		

		// void sk_canvas_draw_circle(sk_canvas_t*, float cx, float cy, float rad, const sk_paint_t*)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_circle (sk_canvas_t param0, Single cx, Single cy, Single rad, sk_paint_t param4);

		// void sk_canvas_draw_color(sk_canvas_t* ccanvas, sk_color_t color, sk_blendmode_t mode)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_color (sk_canvas_t ccanvas, UInt32 color, BlendMode mode);
		

		// void sk_canvas_draw_color4f(sk_canvas_t* ccanvas, sk_color4f_t color, sk_blendmode_t mode)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_color4f (sk_canvas_t ccanvas, SKColorF color, BlendMode mode);
		

		// void sk_canvas_draw_drawable(sk_canvas_t*, sk_drawable_t*, const sk_matrix_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_drawable (sk_canvas_t param0, sk_drawable_t param1, Matrix3* param2);
		

		// void sk_canvas_draw_drrect(sk_canvas_t* ccanvas, const sk_rrect_t* outer, const sk_rrect_t* inner, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_drrect (sk_canvas_t ccanvas, sk_rrect_t outer, sk_rrect_t inner, sk_paint_t paint);
		

		// void sk_canvas_draw_image(sk_canvas_t*, const sk_image_t*, float x, float y, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_image (sk_canvas_t param0, sk_image_t param1, Single x, Single y, sk_paint_t param4);
		

		// void sk_canvas_draw_image_lattice(sk_canvas_t* t, const sk_image_t* image, const sk_lattice_t* lattice, const sk_rect_t* dst, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_image_lattice (sk_canvas_t t, sk_image_t image, SKLatticeInternal* lattice, Rect* dst, sk_paint_t paint);
		

		// void sk_canvas_draw_image_nine(sk_canvas_t* t, const sk_image_t* image, const sk_irect_t* center, const sk_rect_t* dst, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_image_nine (sk_canvas_t t, sk_image_t image, RectI* center, Rect* dst, sk_paint_t paint);
		

		// void sk_canvas_draw_image_rect(sk_canvas_t*, const sk_image_t*, const sk_rect_t* src, const sk_rect_t* dst, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_image_rect (sk_canvas_t param0, sk_image_t param1, Rect* src, Rect* dst, sk_paint_t param4);
		

		// void sk_canvas_draw_line(sk_canvas_t* ccanvas, float x0, float y0, float x1, float y1, sk_paint_t* cpaint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_line (sk_canvas_t ccanvas, Single x0, Single y0, Single x1, Single y1, sk_paint_t cpaint);
		

		// void sk_canvas_draw_link_destination_annotation(sk_canvas_t* t, const sk_rect_t* rect, sk_data_t* value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_link_destination_annotation (sk_canvas_t t, Rect* rect, sk_data_t value);
		

		// void sk_canvas_draw_named_destination_annotation(sk_canvas_t* t, const sk_point_t* point, sk_data_t* value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_named_destination_annotation (sk_canvas_t t, Point* point, sk_data_t value);
		

		// void sk_canvas_draw_oval(sk_canvas_t*, const sk_rect_t*, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_oval (sk_canvas_t param0, Rect* param1, sk_paint_t param2);
		

		// void sk_canvas_draw_paint(sk_canvas_t*, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_paint (sk_canvas_t param0, sk_paint_t param1);
		

		// void sk_canvas_draw_patch(sk_canvas_t* ccanvas, const sk_point_t* cubics, const sk_color_t* colors, const sk_point_t* texCoords, sk_blendmode_t mode, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_patch (sk_canvas_t ccanvas, Point* cubics, UInt32* colors, Point* texCoords, BlendMode mode, sk_paint_t paint);
		

		// void sk_canvas_draw_path(sk_canvas_t*, const sk_path_t*, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_path (sk_canvas_t param0, sk_path_t param1, sk_paint_t param2);
		

		// void sk_canvas_draw_picture(sk_canvas_t*, const sk_picture_t*, const sk_matrix_t*, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_picture (sk_canvas_t param0, sk_picture_t param1, Matrix3* param2, sk_paint_t param3);
		

		// void sk_canvas_draw_point(sk_canvas_t*, float, float, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_point (sk_canvas_t param0, Single param1, Single param2, sk_paint_t param3);
		

		// void sk_canvas_draw_points(sk_canvas_t*, sk_point_mode_t, size_t, const sk_point_t[-1], const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_points (sk_canvas_t param0, SKPointMode param1, /* size_t */ IntPtr param2, Point* param3, sk_paint_t param4);
		

		// void sk_canvas_draw_rect(sk_canvas_t*, const sk_rect_t*, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_rect (sk_canvas_t param0, Rect* param1, sk_paint_t param2);
		

		// void sk_canvas_draw_region(sk_canvas_t*, const sk_region_t*, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_region (sk_canvas_t param0, sk_region_t param1, sk_paint_t param2);
		

		// void sk_canvas_draw_round_rect(sk_canvas_t*, const sk_rect_t*, float rx, float ry, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_round_rect (sk_canvas_t param0, Rect* param1, Single rx, Single ry, sk_paint_t param4);
		

		// void sk_canvas_draw_rrect(sk_canvas_t*, const sk_rrect_t*, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_rrect (sk_canvas_t param0, sk_rrect_t param1, sk_paint_t param2);
		

		// void sk_canvas_draw_simple_text(sk_canvas_t* ccanvas, const void* text, size_t byte_length, sk_text_encoding_t encoding, float x, float y, const sk_font_t* cfont, const sk_paint_t* cpaint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_simple_text (sk_canvas_t ccanvas, void* text, /* size_t */ IntPtr byte_length, SKTextEncoding encoding, Single x, Single y, sk_font_t cfont, sk_paint_t cpaint);
		

		// void sk_canvas_draw_text_blob(sk_canvas_t*, sk_textblob_t* text, float x, float y, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_text_blob (sk_canvas_t param0, sk_textblob_t text, Single x, Single y, sk_paint_t paint);
		

		// void sk_canvas_draw_url_annotation(sk_canvas_t* t, const sk_rect_t* rect, sk_data_t* value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_url_annotation (sk_canvas_t t, Rect* rect, sk_data_t value);
		

		// void sk_canvas_draw_vertices(sk_canvas_t* ccanvas, const sk_vertices_t* vertices, sk_blendmode_t mode, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_draw_vertices (sk_canvas_t ccanvas, sk_vertices_t vertices, BlendMode mode, sk_paint_t paint);

		// bool sk_canvas_get_device_clip_bounds(sk_canvas_t* t, sk_irect_t* cbounds)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_canvas_get_device_clip_bounds (sk_canvas_t t, RectI* cbounds);
		

		// bool sk_canvas_get_local_clip_bounds(sk_canvas_t* t, sk_rect_t* cbounds)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_canvas_get_local_clip_bounds (sk_canvas_t t, Rect* cbounds);
		

		// int sk_canvas_get_save_count(sk_canvas_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_canvas_get_save_count (sk_canvas_t param0);
		

		// void sk_canvas_get_total_matrix(sk_canvas_t* ccanvas, sk_matrix_t* matrix)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_get_total_matrix (sk_canvas_t ccanvas, Matrix3* matrix);
		

		// bool sk_canvas_is_clip_empty(sk_canvas_t* ccanvas)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_canvas_is_clip_empty (sk_canvas_t ccanvas);
		

		// bool sk_canvas_is_clip_rect(sk_canvas_t* ccanvas)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_canvas_is_clip_rect (sk_canvas_t ccanvas);
		

		// sk_canvas_t* sk_canvas_new_from_bitmap(const sk_bitmap_t* bitmap)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_canvas_t sk_canvas_new_from_bitmap (sk_bitmap_t bitmap);
		

		// bool sk_canvas_quick_reject(sk_canvas_t*, const sk_rect_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_canvas_quick_reject (sk_canvas_t param0, Rect* param1);
		

		// void sk_canvas_reset_matrix(sk_canvas_t* ccanvas)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_reset_matrix (sk_canvas_t ccanvas);
		

		// void sk_canvas_restore(sk_canvas_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_restore (sk_canvas_t param0);
		

		// void sk_canvas_restore_to_count(sk_canvas_t*, int saveCount)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_restore_to_count (sk_canvas_t param0, Int32 saveCount);
		

		// void sk_canvas_rotate_degrees(sk_canvas_t*, float degrees)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_rotate_degrees (sk_canvas_t param0, Single degrees);
		

		// void sk_canvas_rotate_radians(sk_canvas_t*, float radians)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_rotate_radians (sk_canvas_t param0, Single radians);
		

		// int sk_canvas_save(sk_canvas_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_canvas_save (sk_canvas_t param0);
		

		// int sk_canvas_save_layer(sk_canvas_t*, const sk_rect_t*, const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_canvas_save_layer (sk_canvas_t param0, Rect* param1, sk_paint_t param2);
		

		// void sk_canvas_scale(sk_canvas_t*, float sx, float sy)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_scale (sk_canvas_t param0, Single sx, Single sy);
		

		// void sk_canvas_set_matrix(sk_canvas_t* ccanvas, const sk_matrix_t* matrix)
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_set_matrix (sk_canvas_t ccanvas, Matrix4* matrix);

		// void sk_canvas_skew(sk_canvas_t*, float sx, float sy)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_skew (sk_canvas_t param0, Single sx, Single sy);
		

		// void sk_canvas_translate(sk_canvas_t*, float dx, float dy)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_canvas_translate (sk_canvas_t param0, Single dx, Single dy);
		

		// void sk_nodraw_canvas_destroy(sk_nodraw_canvas_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_nodraw_canvas_destroy (sk_nodraw_canvas_t param0);
		

		// sk_nodraw_canvas_t* sk_nodraw_canvas_new(int width, int height)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_nodraw_canvas_t sk_nodraw_canvas_new (Int32 width, Int32 height);
		

		// void sk_nway_canvas_add_canvas(sk_nway_canvas_t*, sk_canvas_t* canvas)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_nway_canvas_add_canvas (sk_nway_canvas_t param0, sk_canvas_t canvas);
		

		// void sk_nway_canvas_destroy(sk_nway_canvas_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_nway_canvas_destroy (sk_nway_canvas_t param0);
		

		// sk_nway_canvas_t* sk_nway_canvas_new(int width, int height)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_nway_canvas_t sk_nway_canvas_new (Int32 width, Int32 height);
		

		// void sk_nway_canvas_remove_all(sk_nway_canvas_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_nway_canvas_remove_all (sk_nway_canvas_t param0);
		

		// void sk_nway_canvas_remove_canvas(sk_nway_canvas_t*, sk_canvas_t* canvas)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_nway_canvas_remove_canvas (sk_nway_canvas_t param0, sk_canvas_t canvas);
		

		// void sk_overdraw_canvas_destroy(sk_overdraw_canvas_t* canvas)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_overdraw_canvas_destroy (sk_overdraw_canvas_t canvas);
		

		// sk_overdraw_canvas_t* sk_overdraw_canvas_new(sk_canvas_t* canvas)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_overdraw_canvas_t sk_overdraw_canvas_new (sk_canvas_t canvas);
		

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

		#region sk_data.h

		// const uint8_t* sk_data_get_bytes(const sk_data_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Byte* sk_data_get_bytes (sk_data_t param0);
		

		// const void* sk_data_get_data(const sk_data_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void* sk_data_get_data (sk_data_t param0);
		

		// size_t sk_data_get_size(const sk_data_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_data_get_size (sk_data_t param0);
		

		// sk_data_t* sk_data_new_empty()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_data_new_empty ();
		

		// sk_data_t* sk_data_new_from_file(const char* path)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_data_new_from_file (/* char */ void* path);
		

		// sk_data_t* sk_data_new_from_stream(sk_stream_t* stream, size_t length)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_data_new_from_stream (sk_stream_t stream, /* size_t */ IntPtr length);
		

		// sk_data_t* sk_data_new_subset(const sk_data_t* src, size_t offset, size_t length)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_data_new_subset (sk_data_t src, /* size_t */ IntPtr offset, /* size_t */ IntPtr length);
		

		// sk_data_t* sk_data_new_uninitialized(size_t size)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_data_new_uninitialized (/* size_t */ IntPtr size);
		

		// sk_data_t* sk_data_new_with_copy(const void* src, size_t length)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_data_new_with_copy (void* src, /* size_t */ IntPtr length);
		

		// sk_data_t* sk_data_new_with_proc(const void* ptr, size_t length, sk_data_release_proc proc, void* ctx)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_data_new_with_proc (void* ptr, /* size_t */ IntPtr length, SKDataReleaseProxyDelegate proc, void* ctx);
		

		// void sk_data_ref(const sk_data_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_data_ref (sk_data_t param0);
		

		// void sk_data_unref(const sk_data_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_data_unref (sk_data_t param0);
		

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

		#region sk_font.h

		// size_t sk_font_break_text(const sk_font_t* font, const void* text, size_t byteLength, sk_text_encoding_t encoding, float maxWidth, float* measuredWidth, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_font_break_text (sk_font_t font, void* text, /* size_t */ IntPtr byteLength, SKTextEncoding encoding, Single maxWidth, Single* measuredWidth, sk_paint_t paint);
		

		// void sk_font_delete(sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_delete (sk_font_t font);
		

		// sk_font_edging_t sk_font_get_edging(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern SKFontEdging sk_font_get_edging (sk_font_t font);
		

		// sk_font_hinting_t sk_font_get_hinting(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern SKFontHinting sk_font_get_hinting (sk_font_t font);
		

		// float sk_font_get_metrics(const sk_font_t* font, sk_fontmetrics_t* metrics)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_font_get_metrics (sk_font_t font, FontMetrics* metrics);
		

		// bool sk_font_get_path(const sk_font_t* font, uint16_t glyph, sk_path_t* path)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_font_get_path (sk_font_t font, UInt16 glyph, sk_path_t path);
		

		// void sk_font_get_paths(const sk_font_t* font, uint16_t[-1] glyphs, int count, const sk_glyph_path_proc glyphPathProc, void* context)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_get_paths (sk_font_t font, UInt16* glyphs, Int32 count, SKGlyphPathProxyDelegate glyphPathProc, void* context);
		

		// void sk_font_get_pos(const sk_font_t* font, const uint16_t[-1] glyphs, int count, sk_point_t[-1] pos, sk_point_t* origin)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_get_pos (sk_font_t font, UInt16* glyphs, Int32 count, Point* pos, Point* origin);
		

		// float sk_font_get_scale_x(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_font_get_scale_x (sk_font_t font);
		

		// float sk_font_get_size(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_font_get_size (sk_font_t font);
		

		// float sk_font_get_skew_x(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_font_get_skew_x (sk_font_t font);
		

		// sk_typeface_t* sk_font_get_typeface(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_font_get_typeface (sk_font_t font);
		

		// void sk_font_get_widths_bounds(const sk_font_t* font, const uint16_t[-1] glyphs, int count, float[-1] widths, sk_rect_t[-1] bounds, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_get_widths_bounds (sk_font_t font, UInt16* glyphs, Int32 count, Single* widths, Rect* bounds, sk_paint_t paint);
		

		// void sk_font_get_xpos(const sk_font_t* font, const uint16_t[-1] glyphs, int count, float[-1] xpos, float origin)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_get_xpos (sk_font_t font, UInt16* glyphs, Int32 count, Single* xpos, Single origin);
		

		// bool sk_font_is_baseline_snap(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_font_is_baseline_snap (sk_font_t font);
		

		// bool sk_font_is_embedded_bitmaps(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_font_is_embedded_bitmaps (sk_font_t font);
		

		// bool sk_font_is_embolden(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_font_is_embolden (sk_font_t font);
		

		// bool sk_font_is_force_auto_hinting(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_font_is_force_auto_hinting (sk_font_t font);
		

		// bool sk_font_is_linear_metrics(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_font_is_linear_metrics (sk_font_t font);
		

		// bool sk_font_is_subpixel(const sk_font_t* font)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_font_is_subpixel (sk_font_t font);
		

		// float sk_font_measure_text(const sk_font_t* font, const void* text, size_t byteLength, sk_text_encoding_t encoding, sk_rect_t* bounds, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_font_measure_text (sk_font_t font, void* text, /* size_t */ IntPtr byteLength, SKTextEncoding encoding, Rect* bounds, sk_paint_t paint);
		

		// void sk_font_measure_text_no_return(const sk_font_t* font, const void* text, size_t byteLength, sk_text_encoding_t encoding, sk_rect_t* bounds, const sk_paint_t* paint, float* measuredWidth)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_measure_text_no_return (sk_font_t font, void* text, /* size_t */ IntPtr byteLength, SKTextEncoding encoding, Rect* bounds, sk_paint_t paint, Single* measuredWidth);
		

		// sk_font_t* sk_font_new()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_font_t sk_font_new ();
		

		// sk_font_t* sk_font_new_with_values(sk_typeface_t* typeface, float size, float scaleX, float skewX)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_font_t sk_font_new_with_values (sk_typeface_t typeface, Single size, Single scaleX, Single skewX);
		

		// void sk_font_set_baseline_snap(sk_font_t* font, bool value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_baseline_snap (sk_font_t font, [MarshalAs (UnmanagedType.I1)] bool value);
		

		// void sk_font_set_edging(sk_font_t* font, sk_font_edging_t value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_edging (sk_font_t font, SKFontEdging value);
		

		// void sk_font_set_embedded_bitmaps(sk_font_t* font, bool value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_embedded_bitmaps (sk_font_t font, [MarshalAs (UnmanagedType.I1)] bool value);
		

		// void sk_font_set_embolden(sk_font_t* font, bool value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_embolden (sk_font_t font, [MarshalAs (UnmanagedType.I1)] bool value);
		

		// void sk_font_set_force_auto_hinting(sk_font_t* font, bool value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_force_auto_hinting (sk_font_t font, [MarshalAs (UnmanagedType.I1)] bool value);
		

		// void sk_font_set_hinting(sk_font_t* font, sk_font_hinting_t value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_hinting (sk_font_t font, SKFontHinting value);
		

		// void sk_font_set_linear_metrics(sk_font_t* font, bool value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_linear_metrics (sk_font_t font, [MarshalAs (UnmanagedType.I1)] bool value);
		

		// void sk_font_set_scale_x(sk_font_t* font, float value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_scale_x (sk_font_t font, Single value);
		

		// void sk_font_set_size(sk_font_t* font, float value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_size (sk_font_t font, Single value);
		

		// void sk_font_set_skew_x(sk_font_t* font, float value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_skew_x (sk_font_t font, Single value);
		

		// void sk_font_set_subpixel(sk_font_t* font, bool value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_subpixel (sk_font_t font, [MarshalAs (UnmanagedType.I1)] bool value);
		

		// void sk_font_set_typeface(sk_font_t* font, sk_typeface_t* value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_set_typeface (sk_font_t font, sk_typeface_t value);
		

		// int sk_font_text_to_glyphs(const sk_font_t* font, const void* text, size_t byteLength, sk_text_encoding_t encoding, uint16_t[-1] glyphs, int maxGlyphCount)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_font_text_to_glyphs (sk_font_t font, void* text, /* size_t */ IntPtr byteLength, SKTextEncoding encoding, UInt16* glyphs, Int32 maxGlyphCount);
		

		// uint16_t sk_font_unichar_to_glyph(const sk_font_t* font, int32_t uni)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern UInt16 sk_font_unichar_to_glyph (sk_font_t font, Int32 uni);
		

		// void sk_font_unichars_to_glyphs(const sk_font_t* font, const int32_t[-1] uni, int count, uint16_t[-1] glyphs)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_font_unichars_to_glyphs (sk_font_t font, Int32* uni, Int32 count, UInt16* glyphs);
		

		// void sk_text_utils_get_path(const void* text, size_t length, sk_text_encoding_t encoding, float x, float y, const sk_font_t* font, sk_path_t* path)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_text_utils_get_path (void* text, /* size_t */ IntPtr length, SKTextEncoding encoding, Single x, Single y, sk_font_t font, sk_path_t path);
		

		// void sk_text_utils_get_pos_path(const void* text, size_t length, sk_text_encoding_t encoding, const sk_point_t[-1] pos, const sk_font_t* font, sk_path_t* path)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_text_utils_get_pos_path (void* text, /* size_t */ IntPtr length, SKTextEncoding encoding, Point* pos, sk_font_t font, sk_path_t path);
		

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

		#region sk_image.h

		// sk_data_t* sk_image_encode(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_image_encode (sk_image_t param0);
		

		// sk_data_t* sk_image_encode_specific(const sk_image_t* cimage, sk_encoded_image_format_t encoder, int quality)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_image_encode_specific (sk_image_t cimage, EncodedImageFormat encoder, Int32 quality);
		

		// sk_alphatype_t sk_image_get_alpha_type(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern AlphaType sk_image_get_alpha_type (sk_image_t param0);
		

		// sk_colortype_t sk_image_get_color_type(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern SKColorTypeNative sk_image_get_color_type (sk_image_t param0);
		

		// sk_colorspace_t* sk_image_get_colorspace(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_colorspace_t sk_image_get_colorspace (sk_image_t param0);
		

		// int sk_image_get_height(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_image_get_height (sk_image_t param0);
		

		// uint32_t sk_image_get_unique_id(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern UInt32 sk_image_get_unique_id (sk_image_t param0);
		

		// int sk_image_get_width(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_image_get_width (sk_image_t param0);
		

		// bool sk_image_is_alpha_only(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_image_is_alpha_only (sk_image_t param0);
		

		// bool sk_image_is_lazy_generated(const sk_image_t* image)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_image_is_lazy_generated (sk_image_t image);
		

		// bool sk_image_is_texture_backed(const sk_image_t* image)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_image_is_texture_backed (sk_image_t image);
		

		// bool sk_image_is_valid(const sk_image_t* image, gr_recording_context_t* context)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_image_is_valid (sk_image_t image, gr_recording_context_t context);
		

		// sk_image_t* sk_image_make_non_texture_image(const sk_image_t* cimage)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_make_non_texture_image (sk_image_t cimage);
		

		// sk_image_t* sk_image_make_raster_image(const sk_image_t* cimage)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_make_raster_image (sk_image_t cimage);
		

		// sk_shader_t* sk_image_make_shader(const sk_image_t*, sk_shader_tilemode_t tileX, sk_shader_tilemode_t tileY, const sk_matrix_t* localMatrix)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_shader_t sk_image_make_shader (sk_image_t param0, TileMode tileX, TileMode tileY, Matrix3* localMatrix);
		

		// sk_image_t* sk_image_make_subset(const sk_image_t* cimage, const sk_irect_t* subset)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_make_subset (sk_image_t cimage, RectI* subset);
		

		// sk_image_t* sk_image_make_texture_image(const sk_image_t* cimage, gr_direct_context_t* context, bool mipmapped)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_make_texture_image (sk_image_t cimage, gr_direct_context_t context, [MarshalAs (UnmanagedType.I1)] bool mipmapped);
		

		// sk_image_t* sk_image_make_with_filter(const sk_image_t* cimage, gr_recording_context_t* context, const sk_imagefilter_t* filter, const sk_irect_t* subset, const sk_irect_t* clipBounds, sk_irect_t* outSubset, sk_ipoint_t* outOffset)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_make_with_filter (sk_image_t cimage, gr_recording_context_t context, sk_imagefilter_t filter, RectI* subset, RectI* clipBounds, RectI* outSubset, PointI* outOffset);
		

		// sk_image_t* sk_image_make_with_filter_legacy(const sk_image_t* cimage, const sk_imagefilter_t* filter, const sk_irect_t* subset, const sk_irect_t* clipBounds, sk_irect_t* outSubset, sk_ipoint_t* outOffset)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_make_with_filter_legacy (sk_image_t cimage, sk_imagefilter_t filter, RectI* subset, RectI* clipBounds, RectI* outSubset, PointI* outOffset);
		

		// sk_image_t* sk_image_new_from_adopted_texture(gr_recording_context_t* context, const gr_backendtexture_t* texture, gr_surfaceorigin_t origin, sk_colortype_t colorType, sk_alphatype_t alpha, sk_colorspace_t* colorSpace)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_new_from_adopted_texture (gr_recording_context_t context, gr_backendtexture_t texture, GRSurfaceOrigin origin, SKColorTypeNative colorType, AlphaType alpha, sk_colorspace_t colorSpace);
		

		// sk_image_t* sk_image_new_from_bitmap(const sk_bitmap_t* cbitmap)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_new_from_bitmap (sk_bitmap_t cbitmap);
		

		// sk_image_t* sk_image_new_from_encoded(sk_data_t* encoded)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_new_from_encoded (sk_data_t encoded);
		

		// sk_image_t* sk_image_new_from_picture(sk_picture_t* picture, const sk_isize_t* dimensions, const sk_matrix_t* matrix, const sk_paint_t* paint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_new_from_picture (sk_picture_t picture, SizeI* dimensions, Matrix3* matrix, sk_paint_t paint);
		

		// sk_image_t* sk_image_new_from_texture(gr_recording_context_t* context, const gr_backendtexture_t* texture, gr_surfaceorigin_t origin, sk_colortype_t colorType, sk_alphatype_t alpha, sk_colorspace_t* colorSpace, sk_image_texture_release_proc releaseProc, void* releaseContext)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_new_from_texture (gr_recording_context_t context, gr_backendtexture_t texture, GRSurfaceOrigin origin, SKColorTypeNative colorType, AlphaType alpha, sk_colorspace_t colorSpace, SKImageTextureReleaseProxyDelegate releaseProc, void* releaseContext);
		

		// sk_image_t* sk_image_new_raster(const sk_pixmap_t* pixmap, sk_image_raster_release_proc releaseProc, void* context)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_new_raster (sk_pixmap_t pixmap, SKImageRasterReleaseProxyDelegate releaseProc, void* context);
		

		// sk_image_t* sk_image_new_raster_copy(const sk_imageinfo_t*, const void* pixels, size_t rowBytes)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_new_raster_copy (SKImageInfoNative* param0, void* pixels, /* size_t */ IntPtr rowBytes);
		

		// sk_image_t* sk_image_new_raster_copy_with_pixmap(const sk_pixmap_t* pixmap)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_new_raster_copy_with_pixmap (sk_pixmap_t pixmap);
		

		// sk_image_t* sk_image_new_raster_data(const sk_imageinfo_t* cinfo, sk_data_t* pixels, size_t rowBytes)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_image_new_raster_data (SKImageInfoNative* cinfo, sk_data_t pixels, /* size_t */ IntPtr rowBytes);
		

		// bool sk_image_peek_pixels(const sk_image_t* image, sk_pixmap_t* pixmap)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_image_peek_pixels (sk_image_t image, sk_pixmap_t pixmap);
		

		// bool sk_image_read_pixels(const sk_image_t* image, const sk_imageinfo_t* dstInfo, void* dstPixels, size_t dstRowBytes, int srcX, int srcY, sk_image_caching_hint_t cachingHint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_image_read_pixels (sk_image_t image, SKImageInfoNative* dstInfo, void* dstPixels, /* size_t */ IntPtr dstRowBytes, Int32 srcX, Int32 srcY, SKImageCachingHint cachingHint);
		

		// bool sk_image_read_pixels_into_pixmap(const sk_image_t* image, const sk_pixmap_t* dst, int srcX, int srcY, sk_image_caching_hint_t cachingHint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_image_read_pixels_into_pixmap (sk_image_t image, sk_pixmap_t dst, Int32 srcX, Int32 srcY, SKImageCachingHint cachingHint);
		

		// void sk_image_ref(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_image_ref (sk_image_t param0);
		

		// sk_data_t* sk_image_ref_encoded(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_image_ref_encoded (sk_image_t param0);
		

		// bool sk_image_scale_pixels(const sk_image_t* image, const sk_pixmap_t* dst, sk_filter_quality_t quality, sk_image_caching_hint_t cachingHint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_image_scale_pixels (sk_image_t image, sk_pixmap_t dst, SKFilterQuality quality, SKImageCachingHint cachingHint);
		

		// void sk_image_unref(const sk_image_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_image_unref (sk_image_t param0);
		

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
		

		// sk_imagefilter_t* sk_imagefilter_new_alpha_threshold(const sk_region_t* region, float innerThreshold, float outerThreshold, sk_imagefilter_t* input)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_imagefilter_t sk_imagefilter_new_alpha_threshold (sk_region_t region, Single innerThreshold, Single outerThreshold, sk_imagefilter_t input);
		

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

		#region sk_paint.h

		// sk_paint_t* sk_paint_clone(sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_paint_t sk_paint_clone (sk_paint_t param0);
		

		// void sk_paint_delete(sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_delete (sk_paint_t param0);
		

		// sk_blendmode_t sk_paint_get_blendmode(sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern BlendMode sk_paint_get_blendmode (sk_paint_t param0);
		

		// sk_color_t sk_paint_get_color(const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern UInt32 sk_paint_get_color (sk_paint_t param0);
		

		// void sk_paint_get_color4f(const sk_paint_t* paint, sk_color4f_t* color)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_get_color4f (sk_paint_t paint, SKColorF* color);
		

		// sk_colorfilter_t* sk_paint_get_colorfilter(sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_colorfilter_t sk_paint_get_colorfilter (sk_paint_t param0);
		

		// // bool sk_paint_get_fill_path(const sk_paint_t*, const sk_path_t* src, sk_path_t* dst, const sk_rect_t* cullRect, float resScale)
		//
		// [DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		// [return: MarshalAs (UnmanagedType.I1)]
		// internal static extern bool sk_paint_get_fill_path (sk_paint_t param0, sk_path_t src, sk_path_t dst, Rect* cullRect, Single resScale);
		

		// sk_filter_quality_t sk_paint_get_filter_quality(sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern SKFilterQuality sk_paint_get_filter_quality (sk_paint_t param0);
		

		// sk_imagefilter_t* sk_paint_get_imagefilter(sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_imagefilter_t sk_paint_get_imagefilter (sk_paint_t param0);
		

		// sk_maskfilter_t* sk_paint_get_maskfilter(sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_maskfilter_t sk_paint_get_maskfilter (sk_paint_t param0);
		

		// sk_path_effect_t* sk_paint_get_path_effect(sk_paint_t* cpaint)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_path_effect_t sk_paint_get_path_effect (sk_paint_t cpaint);
		

		// sk_shader_t* sk_paint_get_shader(sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_shader_t sk_paint_get_shader (sk_paint_t param0);
		

		// sk_stroke_cap_t sk_paint_get_stroke_cap(const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern StrokeCap sk_paint_get_stroke_cap (sk_paint_t param0);
		

		// sk_stroke_join_t sk_paint_get_stroke_join(const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern StrokeJoin sk_paint_get_stroke_join (sk_paint_t param0);
		

		// float sk_paint_get_stroke_miter(const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_paint_get_stroke_miter (sk_paint_t param0);
		

		// float sk_paint_get_stroke_width(const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_paint_get_stroke_width (sk_paint_t param0);
		

		// sk_paint_style_t sk_paint_get_style(const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern PaintStyle sk_paint_get_style (sk_paint_t param0);
		

		// bool sk_paint_is_antialias(const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_paint_is_antialias (sk_paint_t param0);
		

		// bool sk_paint_is_dither(const sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_paint_is_dither (sk_paint_t param0);
		

		// sk_paint_t* sk_paint_new()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_paint_t sk_paint_new ();
		

		// void sk_paint_reset(sk_paint_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_reset (sk_paint_t param0);
		

		// void sk_paint_set_antialias(sk_paint_t*, bool)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_antialias (sk_paint_t param0, [MarshalAs (UnmanagedType.I1)] bool param1);
		

		// void sk_paint_set_blendmode(sk_paint_t*, sk_blendmode_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_blendmode (sk_paint_t param0, BlendMode param1);
		

		// void sk_paint_set_color(sk_paint_t*, sk_color_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_color (sk_paint_t param0, UInt32 param1);
		

		// void sk_paint_set_color4f(sk_paint_t* paint, sk_color4f_t* color, sk_colorspace_t* colorspace)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_color4f (sk_paint_t paint, SKColorF* color, sk_colorspace_t colorspace);
		

		// void sk_paint_set_colorfilter(sk_paint_t*, sk_colorfilter_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_colorfilter (sk_paint_t param0, sk_colorfilter_t param1);
		

		// void sk_paint_set_dither(sk_paint_t*, bool)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_dither (sk_paint_t param0, [MarshalAs (UnmanagedType.I1)] bool param1);
		

		// void sk_paint_set_filter_quality(sk_paint_t*, sk_filter_quality_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_filter_quality (sk_paint_t param0, SKFilterQuality param1);
		

		// void sk_paint_set_imagefilter(sk_paint_t*, sk_imagefilter_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_imagefilter (sk_paint_t param0, sk_imagefilter_t param1);
		

		// void sk_paint_set_maskfilter(sk_paint_t*, sk_maskfilter_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_maskfilter (sk_paint_t param0, sk_maskfilter_t param1);
		

		// void sk_paint_set_path_effect(sk_paint_t* cpaint, sk_path_effect_t* effect)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_path_effect (sk_paint_t cpaint, sk_path_effect_t effect);
		

		// void sk_paint_set_shader(sk_paint_t*, sk_shader_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_shader (sk_paint_t param0, sk_shader_t param1);
		

		// void sk_paint_set_stroke_cap(sk_paint_t*, sk_stroke_cap_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_stroke_cap (sk_paint_t param0, StrokeCap param1);
		

		// void sk_paint_set_stroke_join(sk_paint_t*, sk_stroke_join_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_stroke_join (sk_paint_t param0, StrokeJoin param1);
		

		// void sk_paint_set_stroke_miter(sk_paint_t*, float miter)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_stroke_miter (sk_paint_t param0, Single miter);
		

		// void sk_paint_set_stroke_width(sk_paint_t*, float width)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_stroke_width (sk_paint_t param0, Single width);
		

		// void sk_paint_set_style(sk_paint_t*, sk_paint_style_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_paint_set_style (sk_paint_t param0, PaintStyle param1);
		

		#endregion

		#region sk_path.h

		// void sk_opbuilder_add(sk_opbuilder_t* builder, const sk_path_t* path, sk_pathop_t op)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_opbuilder_add (sk_opbuilder_t builder, sk_path_t path, PathOp op);
		

		// void sk_opbuilder_destroy(sk_opbuilder_t* builder)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_opbuilder_destroy (sk_opbuilder_t builder);
		

		// sk_opbuilder_t* sk_opbuilder_new()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_opbuilder_t sk_opbuilder_new ();
		

		// bool sk_opbuilder_resolve(sk_opbuilder_t* builder, sk_path_t* result)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_opbuilder_resolve (sk_opbuilder_t builder, sk_path_t result);
		

		// void sk_path_add_arc(sk_path_t* cpath, const sk_rect_t* crect, float startAngle, float sweepAngle)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_arc (sk_path_t cpath, Rect* crect, Single startAngle, Single sweepAngle);
		

		// void sk_path_add_circle(sk_path_t*, float x, float y, float radius, sk_path_direction_t dir)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_circle (sk_path_t param0, Single x, Single y, Single radius, SKPathDirection dir);
		

		// void sk_path_add_oval(sk_path_t*, const sk_rect_t*, sk_path_direction_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_oval (sk_path_t param0, Rect* param1, SKPathDirection param2);
		

		// void sk_path_add_path(sk_path_t* cpath, sk_path_t* other, sk_path_add_mode_t add_mode)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_path (sk_path_t cpath, sk_path_t other, SKPathAddMode add_mode);
		

		// void sk_path_add_path_matrix(sk_path_t* cpath, sk_path_t* other, sk_matrix_t* matrix, sk_path_add_mode_t add_mode)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_path_matrix (sk_path_t cpath, sk_path_t other, Matrix3* matrix, SKPathAddMode add_mode);
		

		// void sk_path_add_path_offset(sk_path_t* cpath, sk_path_t* other, float dx, float dy, sk_path_add_mode_t add_mode)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_path_offset (sk_path_t cpath, sk_path_t other, Single dx, Single dy, SKPathAddMode add_mode);
		

		// void sk_path_add_path_reverse(sk_path_t* cpath, sk_path_t* other)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_path_reverse (sk_path_t cpath, sk_path_t other);
		

		// void sk_path_add_poly(sk_path_t* cpath, const sk_point_t* points, int count, bool close)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_poly (sk_path_t cpath, Point* points, Int32 count, [MarshalAs (UnmanagedType.I1)] bool close);
		

		// void sk_path_add_rect(sk_path_t*, const sk_rect_t*, sk_path_direction_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_rect (sk_path_t param0, Rect* param1, SKPathDirection param2);
		

		// void sk_path_add_rect_start(sk_path_t* cpath, const sk_rect_t* crect, sk_path_direction_t cdir, uint32_t startIndex)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_rect_start (sk_path_t cpath, Rect* crect, SKPathDirection cdir, UInt32 startIndex);
		

		// void sk_path_add_rounded_rect(sk_path_t*, const sk_rect_t*, float, float, sk_path_direction_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_rounded_rect (sk_path_t param0, Rect* param1, Single param2, Single param3, SKPathDirection param4);
		

		// void sk_path_add_rrect(sk_path_t*, const sk_rrect_t*, sk_path_direction_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_rrect (sk_path_t param0, sk_rrect_t param1, SKPathDirection param2);
		

		// void sk_path_add_rrect_start(sk_path_t*, const sk_rrect_t*, sk_path_direction_t, uint32_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_add_rrect_start (sk_path_t param0, sk_rrect_t param1, SKPathDirection param2, UInt32 param3);
		

		// void sk_path_arc_to(sk_path_t*, float rx, float ry, float xAxisRotate, sk_path_arc_size_t largeArc, sk_path_direction_t sweep, float x, float y)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_arc_to (sk_path_t param0, Single rx, Single ry, Single xAxisRotate, SKPathArcSize largeArc, SKPathDirection sweep, Single x, Single y);
		

		// void sk_path_arc_to_with_oval(sk_path_t*, const sk_rect_t* oval, float startAngle, float sweepAngle, bool forceMoveTo)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_arc_to_with_oval (sk_path_t param0, Rect* oval, Single startAngle, Single sweepAngle, [MarshalAs (UnmanagedType.I1)] bool forceMoveTo);
		

		// void sk_path_arc_to_with_points(sk_path_t*, float x1, float y1, float x2, float y2, float radius)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_arc_to_with_points (sk_path_t param0, Single x1, Single y1, Single x2, Single y2, Single radius);
		

		// sk_path_t* sk_path_clone(const sk_path_t* cpath)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_path_t sk_path_clone (sk_path_t cpath);
		

		// void sk_path_close(sk_path_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_close (sk_path_t param0);
		

		// void sk_path_compute_tight_bounds(const sk_path_t*, sk_rect_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_compute_tight_bounds (sk_path_t param0, Rect* param1);
		

		// void sk_path_conic_to(sk_path_t*, float x0, float y0, float x1, float y1, float w)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_conic_to (sk_path_t param0, Single x0, Single y0, Single x1, Single y1, Single w);
		

		// bool sk_path_contains(const sk_path_t* cpath, float x, float y)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_path_contains (sk_path_t cpath, Single x, Single y);
		

		// int sk_path_convert_conic_to_quads(const sk_point_t* p0, const sk_point_t* p1, const sk_point_t* p2, float w, sk_point_t* pts, int pow2)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_path_convert_conic_to_quads (Point* p0, Point* p1, Point* p2, Single w, Point* pts, Int32 pow2);
		

		// int sk_path_count_points(const sk_path_t* cpath)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_path_count_points (sk_path_t cpath);
		

		// int sk_path_count_verbs(const sk_path_t* cpath)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_path_count_verbs (sk_path_t cpath);
		

		// sk_path_iterator_t* sk_path_create_iter(sk_path_t* cpath, int forceClose)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_path_iterator_t sk_path_create_iter (sk_path_t cpath, Int32 forceClose);
		

		// sk_path_rawiterator_t* sk_path_create_rawiter(sk_path_t* cpath)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_path_rawiterator_t sk_path_create_rawiter (sk_path_t cpath);
		

		// void sk_path_cubic_to(sk_path_t*, float x0, float y0, float x1, float y1, float x2, float y2)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_cubic_to (sk_path_t param0, Single x0, Single y0, Single x1, Single y1, Single x2, Single y2);
		

		// void sk_path_delete(sk_path_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_delete (sk_path_t param0);
		

		// void sk_path_get_bounds(const sk_path_t*, sk_rect_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_get_bounds (sk_path_t param0, Rect* param1);
		

		// sk_path_filltype_t sk_path_get_filltype(sk_path_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern SKPathFillType sk_path_get_filltype (sk_path_t param0);
		

		// bool sk_path_get_last_point(const sk_path_t* cpath, sk_point_t* point)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_path_get_last_point (sk_path_t cpath, Point* point);
		

		// void sk_path_get_point(const sk_path_t* cpath, int index, sk_point_t* point)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_get_point (sk_path_t cpath, Int32 index, Point* point);
		

		// int sk_path_get_points(const sk_path_t* cpath, sk_point_t* points, int max)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_path_get_points (sk_path_t cpath, Point* points, Int32 max);
		

		// uint32_t sk_path_get_segment_masks(sk_path_t* cpath)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern UInt32 sk_path_get_segment_masks (sk_path_t cpath);
		

		// bool sk_path_is_convex(const sk_path_t* cpath)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_path_is_convex (sk_path_t cpath);

		[DllImport(SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_path_is_last_contour_closed(sk_path_t cpath);

		// bool sk_path_is_line(sk_path_t* cpath, sk_point_t[2] line = 2)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_path_is_line (sk_path_t cpath, Point* line);
		

		// bool sk_path_is_oval(sk_path_t* cpath, sk_rect_t* bounds)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_path_is_oval (sk_path_t cpath, Rect* bounds);
		

		// bool sk_path_is_rect(sk_path_t* cpath, sk_rect_t* rect, bool* isClosed, sk_path_direction_t* direction)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_path_is_rect (sk_path_t cpath, Rect* rect, Byte* isClosed, SKPathDirection* direction);
		

		// bool sk_path_is_rrect(sk_path_t* cpath, sk_rrect_t* bounds)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_path_is_rrect (sk_path_t cpath, sk_rrect_t bounds);
		

		// float sk_path_iter_conic_weight(sk_path_iterator_t* iterator)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_path_iter_conic_weight (sk_path_iterator_t iterator);
		

		// void sk_path_iter_destroy(sk_path_iterator_t* iterator)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_iter_destroy (sk_path_iterator_t iterator);
		

		// int sk_path_iter_is_close_line(sk_path_iterator_t* iterator)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_path_iter_is_close_line (sk_path_iterator_t iterator);
		

		// int sk_path_iter_is_closed_contour(sk_path_iterator_t* iterator)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_path_iter_is_closed_contour (sk_path_iterator_t iterator);
		

		// sk_path_verb_t sk_path_iter_next(sk_path_iterator_t* iterator, sk_point_t[4] points = 4)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern SKPathVerb sk_path_iter_next (sk_path_iterator_t iterator, Point* points);
		

		// void sk_path_line_to(sk_path_t*, float x, float y)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_line_to (sk_path_t param0, Single x, Single y);
		

		// void sk_path_move_to(sk_path_t*, float x, float y)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_move_to (sk_path_t param0, Single x, Single y);
		

		// sk_path_t* sk_path_new()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_path_t sk_path_new ();
		

		// bool sk_path_parse_svg_string(sk_path_t* cpath, const char* str)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_path_parse_svg_string (sk_path_t cpath, [MarshalAs (UnmanagedType.LPStr)] String str);
		

		// void sk_path_quad_to(sk_path_t*, float x0, float y0, float x1, float y1)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_quad_to (sk_path_t param0, Single x0, Single y0, Single x1, Single y1);
		

		// void sk_path_rarc_to(sk_path_t*, float rx, float ry, float xAxisRotate, sk_path_arc_size_t largeArc, sk_path_direction_t sweep, float x, float y)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_rarc_to (sk_path_t param0, Single rx, Single ry, Single xAxisRotate, SKPathArcSize largeArc, SKPathDirection sweep, Single x, Single y);
		

		// float sk_path_rawiter_conic_weight(sk_path_rawiterator_t* iterator)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_path_rawiter_conic_weight (sk_path_rawiterator_t iterator);
		

		// void sk_path_rawiter_destroy(sk_path_rawiterator_t* iterator)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_rawiter_destroy (sk_path_rawiterator_t iterator);
		

		// sk_path_verb_t sk_path_rawiter_next(sk_path_rawiterator_t* iterator, sk_point_t[4] points = 4)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern SKPathVerb sk_path_rawiter_next (sk_path_rawiterator_t iterator, Point* points);
		

		// sk_path_verb_t sk_path_rawiter_peek(sk_path_rawiterator_t* iterator)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern SKPathVerb sk_path_rawiter_peek (sk_path_rawiterator_t iterator);
		

		// void sk_path_rconic_to(sk_path_t*, float dx0, float dy0, float dx1, float dy1, float w)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_rconic_to (sk_path_t param0, Single dx0, Single dy0, Single dx1, Single dy1, Single w);
		

		// void sk_path_rcubic_to(sk_path_t*, float dx0, float dy0, float dx1, float dy1, float dx2, float dy2)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_rcubic_to (sk_path_t param0, Single dx0, Single dy0, Single dx1, Single dy1, Single dx2, Single dy2);
		

		// void sk_path_reset(sk_path_t* cpath)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_reset (sk_path_t cpath);
		

		// void sk_path_rewind(sk_path_t* cpath)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_rewind (sk_path_t cpath);
		

		// void sk_path_rline_to(sk_path_t*, float dx, float yd)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_rline_to (sk_path_t param0, Single dx, Single yd);
		

		// void sk_path_rmove_to(sk_path_t*, float dx, float dy)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_rmove_to (sk_path_t param0, Single dx, Single dy);
		

		// void sk_path_rquad_to(sk_path_t*, float dx0, float dy0, float dx1, float dy1)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_rquad_to (sk_path_t param0, Single dx0, Single dy0, Single dx1, Single dy1);
		

		// void sk_path_set_filltype(sk_path_t*, sk_path_filltype_t)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_set_filltype (sk_path_t param0, SKPathFillType param1);
		

		// void sk_path_to_svg_string(const sk_path_t* cpath, sk_string_t* str)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_to_svg_string (sk_path_t cpath, sk_string_t str);
		

		// void sk_path_transform(sk_path_t* cpath, const sk_matrix_t* cmatrix)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_transform (sk_path_t cpath, Matrix3* cmatrix);
		

		// void sk_path_transform_to_dest(const sk_path_t* cpath, const sk_matrix_t* cmatrix, sk_path_t* destination)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_path_transform_to_dest (sk_path_t cpath, Matrix3* cmatrix, sk_path_t destination);
		

		// void sk_pathmeasure_destroy(sk_pathmeasure_t* pathMeasure)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_pathmeasure_destroy (sk_pathmeasure_t pathMeasure);
		

		// float sk_pathmeasure_get_length(sk_pathmeasure_t* pathMeasure)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Single sk_pathmeasure_get_length (sk_pathmeasure_t pathMeasure);
		

		// bool sk_pathmeasure_get_matrix(sk_pathmeasure_t* pathMeasure, float distance, sk_matrix_t* matrix, sk_pathmeasure_matrixflags_t flags)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pathmeasure_get_matrix (sk_pathmeasure_t pathMeasure, Single distance, Matrix3* matrix, SKPathMeasureMatrixFlags flags);
		

		// bool sk_pathmeasure_get_pos_tan(sk_pathmeasure_t* pathMeasure, float distance, sk_point_t* position, sk_vector_t* tangent)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pathmeasure_get_pos_tan (sk_pathmeasure_t pathMeasure, Single distance, Point* position, Point* tangent);
		

		// bool sk_pathmeasure_get_segment(sk_pathmeasure_t* pathMeasure, float start, float stop, sk_path_t* dst, bool startWithMoveTo)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pathmeasure_get_segment (sk_pathmeasure_t pathMeasure, Single start, Single stop, sk_path_t dst, [MarshalAs (UnmanagedType.I1)] bool startWithMoveTo);
		

		// bool sk_pathmeasure_is_closed(sk_pathmeasure_t* pathMeasure)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pathmeasure_is_closed (sk_pathmeasure_t pathMeasure);
		

		// sk_pathmeasure_t* sk_pathmeasure_new()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_pathmeasure_t sk_pathmeasure_new ();
		

		// sk_pathmeasure_t* sk_pathmeasure_new_with_path(const sk_path_t* path, bool forceClosed, float resScale)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_pathmeasure_t sk_pathmeasure_new_with_path (sk_path_t path, [MarshalAs (UnmanagedType.I1)] bool forceClosed, Single resScale);
		

		// bool sk_pathmeasure_next_contour(sk_pathmeasure_t* pathMeasure)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pathmeasure_next_contour (sk_pathmeasure_t pathMeasure);
		

		// void sk_pathmeasure_set_path(sk_pathmeasure_t* pathMeasure, const sk_path_t* path, bool forceClosed)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_pathmeasure_set_path (sk_pathmeasure_t pathMeasure, sk_path_t path, [MarshalAs (UnmanagedType.I1)] bool forceClosed);
		

		// bool sk_pathop_as_winding(const sk_path_t* path, sk_path_t* result)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pathop_as_winding (sk_path_t path, sk_path_t result);
		

		// bool sk_pathop_op(const sk_path_t* one, const sk_path_t* two, sk_pathop_t op, sk_path_t* result)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pathop_op (sk_path_t one, sk_path_t two, PathOp op, sk_path_t result);
		

		// bool sk_pathop_simplify(const sk_path_t* path, sk_path_t* result)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pathop_simplify (sk_path_t path, sk_path_t result);
		

		// bool sk_pathop_tight_bounds(const sk_path_t* path, sk_rect_t* result)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pathop_tight_bounds (sk_path_t path, Rect* result);
		

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
		

		// bool sk_pixmap_encode_image(sk_wstream_t* dst, const sk_pixmap_t* src, sk_encoded_image_format_t encoder, int quality)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_pixmap_encode_image (sk_wstream_t dst, sk_pixmap_t src, EncodedImageFormat encoder, Int32 quality);
		

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
		

		// sk_shader_t* sk_shader_new_perlin_noise_fractal_noise(float baseFrequencyX, float baseFrequencyY, int numOctaves, float seed, const sk_isize_t* tileSize)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_shader_t sk_shader_new_perlin_noise_fractal_noise (Single baseFrequencyX, Single baseFrequencyY, Int32 numOctaves, Single seed, SizeI* tileSize);
		

		// sk_shader_t* sk_shader_new_perlin_noise_improved_noise(float baseFrequencyX, float baseFrequencyY, int numOctaves, float z)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_shader_t sk_shader_new_perlin_noise_improved_noise (Single baseFrequencyX, Single baseFrequencyY, Int32 numOctaves, Single z);
		

		// sk_shader_t* sk_shader_new_perlin_noise_turbulence(float baseFrequencyX, float baseFrequencyY, int numOctaves, float seed, const sk_isize_t* tileSize)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_shader_t sk_shader_new_perlin_noise_turbulence (Single baseFrequencyX, Single baseFrequencyY, Int32 numOctaves, Single seed, SizeI* tileSize);
		

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

		#region sk_stream.h

		// void sk_dynamicmemorywstream_copy_to(sk_wstream_dynamicmemorystream_t* cstream, void* data)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_dynamicmemorywstream_copy_to (sk_wstream_dynamicmemorystream_t cstream, void* data);
		

		// void sk_dynamicmemorywstream_destroy(sk_wstream_dynamicmemorystream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_dynamicmemorywstream_destroy (sk_wstream_dynamicmemorystream_t cstream);
		

		// sk_data_t* sk_dynamicmemorywstream_detach_as_data(sk_wstream_dynamicmemorystream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_dynamicmemorywstream_detach_as_data (sk_wstream_dynamicmemorystream_t cstream);
		

		// sk_stream_asset_t* sk_dynamicmemorywstream_detach_as_stream(sk_wstream_dynamicmemorystream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_asset_t sk_dynamicmemorywstream_detach_as_stream (sk_wstream_dynamicmemorystream_t cstream);
		

		// sk_wstream_dynamicmemorystream_t* sk_dynamicmemorywstream_new()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_wstream_dynamicmemorystream_t sk_dynamicmemorywstream_new ();
		

		// bool sk_dynamicmemorywstream_write_to_stream(sk_wstream_dynamicmemorystream_t* cstream, sk_wstream_t* dst)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_dynamicmemorywstream_write_to_stream (sk_wstream_dynamicmemorystream_t cstream, sk_wstream_t dst);
		

		// void sk_filestream_destroy(sk_stream_filestream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_filestream_destroy (sk_stream_filestream_t cstream);
		

		// bool sk_filestream_is_valid(sk_stream_filestream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_filestream_is_valid (sk_stream_filestream_t cstream);
		

		// sk_stream_filestream_t* sk_filestream_new(const char* path)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_filestream_t sk_filestream_new (/* char */ void* path);
		

		// void sk_filewstream_destroy(sk_wstream_filestream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_filewstream_destroy (sk_wstream_filestream_t cstream);
		

		// bool sk_filewstream_is_valid(sk_wstream_filestream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_filewstream_is_valid (sk_wstream_filestream_t cstream);
		

		// sk_wstream_filestream_t* sk_filewstream_new(const char* path)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_wstream_filestream_t sk_filewstream_new (/* char */ void* path);
		

		// void sk_memorystream_destroy(sk_stream_memorystream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_memorystream_destroy (sk_stream_memorystream_t cstream);
		

		// sk_stream_memorystream_t* sk_memorystream_new()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_memorystream_t sk_memorystream_new ();
		

		// sk_stream_memorystream_t* sk_memorystream_new_with_data(const void* data, size_t length, bool copyData)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_memorystream_t sk_memorystream_new_with_data (void* data, /* size_t */ IntPtr length, [MarshalAs (UnmanagedType.I1)] bool copyData);
		

		// sk_stream_memorystream_t* sk_memorystream_new_with_length(size_t length)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_memorystream_t sk_memorystream_new_with_length (/* size_t */ IntPtr length);
		

		// sk_stream_memorystream_t* sk_memorystream_new_with_skdata(sk_data_t* data)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_memorystream_t sk_memorystream_new_with_skdata (sk_data_t data);
		

		// void sk_memorystream_set_memory(sk_stream_memorystream_t* cmemorystream, const void* data, size_t length, bool copyData)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_memorystream_set_memory (sk_stream_memorystream_t cmemorystream, void* data, /* size_t */ IntPtr length, [MarshalAs (UnmanagedType.I1)] bool copyData);
		

		// void sk_stream_asset_destroy(sk_stream_asset_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_stream_asset_destroy (sk_stream_asset_t cstream);
		

		// void sk_stream_destroy(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_stream_destroy (sk_stream_t cstream);
		

		// sk_stream_t* sk_stream_duplicate(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_t sk_stream_duplicate (sk_stream_t cstream);
		

		// sk_stream_t* sk_stream_fork(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_t sk_stream_fork (sk_stream_t cstream);
		

		// size_t sk_stream_get_length(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_stream_get_length (sk_stream_t cstream);
		

		// const void* sk_stream_get_memory_base(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void* sk_stream_get_memory_base (sk_stream_t cstream);
		

		// size_t sk_stream_get_position(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_stream_get_position (sk_stream_t cstream);
		

		// bool sk_stream_has_length(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_has_length (sk_stream_t cstream);
		

		// bool sk_stream_has_position(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_has_position (sk_stream_t cstream);
		

		// bool sk_stream_is_at_end(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_is_at_end (sk_stream_t cstream);
		

		// bool sk_stream_move(sk_stream_t* cstream, int offset)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_move (sk_stream_t cstream, Int32 offset);
		

		// size_t sk_stream_peek(sk_stream_t* cstream, void* buffer, size_t size)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_stream_peek (sk_stream_t cstream, void* buffer, /* size_t */ IntPtr size);
		

		// size_t sk_stream_read(sk_stream_t* cstream, void* buffer, size_t size)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_stream_read (sk_stream_t cstream, void* buffer, /* size_t */ IntPtr size);
		

		// bool sk_stream_read_bool(sk_stream_t* cstream, bool* buffer)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_read_bool (sk_stream_t cstream, Byte* buffer);
		

		// bool sk_stream_read_s16(sk_stream_t* cstream, int16_t* buffer)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_read_s16 (sk_stream_t cstream, Int16* buffer);
		

		// bool sk_stream_read_s32(sk_stream_t* cstream, int32_t* buffer)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_read_s32 (sk_stream_t cstream, Int32* buffer);
		

		// bool sk_stream_read_s8(sk_stream_t* cstream, int8_t* buffer)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_read_s8 (sk_stream_t cstream, SByte* buffer);
		

		// bool sk_stream_read_u16(sk_stream_t* cstream, uint16_t* buffer)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_read_u16 (sk_stream_t cstream, UInt16* buffer);
		

		// bool sk_stream_read_u32(sk_stream_t* cstream, uint32_t* buffer)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_read_u32 (sk_stream_t cstream, UInt32* buffer);
		

		// bool sk_stream_read_u8(sk_stream_t* cstream, uint8_t* buffer)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_read_u8 (sk_stream_t cstream, Byte* buffer);
		

		// bool sk_stream_rewind(sk_stream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_rewind (sk_stream_t cstream);
		

		// bool sk_stream_seek(sk_stream_t* cstream, size_t position)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_stream_seek (sk_stream_t cstream, /* size_t */ IntPtr position);
		

		// size_t sk_stream_skip(sk_stream_t* cstream, size_t size)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_stream_skip (sk_stream_t cstream, /* size_t */ IntPtr size);
		

		// size_t sk_wstream_bytes_written(sk_wstream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_wstream_bytes_written (sk_wstream_t cstream);
		

		// void sk_wstream_flush(sk_wstream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_wstream_flush (sk_wstream_t cstream);
		

		// int sk_wstream_get_size_of_packed_uint(size_t value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_wstream_get_size_of_packed_uint (/* size_t */ IntPtr value);
		

		// bool sk_wstream_newline(sk_wstream_t* cstream)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_newline (sk_wstream_t cstream);
		

		// bool sk_wstream_write(sk_wstream_t* cstream, const void* buffer, size_t size)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write (sk_wstream_t cstream, void* buffer, /* size_t */ IntPtr size);
		

		// bool sk_wstream_write_16(sk_wstream_t* cstream, uint16_t value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_16 (sk_wstream_t cstream, UInt16 value);
		

		// bool sk_wstream_write_32(sk_wstream_t* cstream, uint32_t value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_32 (sk_wstream_t cstream, UInt32 value);
		

		// bool sk_wstream_write_8(sk_wstream_t* cstream, uint8_t value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_8 (sk_wstream_t cstream, Byte value);
		

		// bool sk_wstream_write_bigdec_as_text(sk_wstream_t* cstream, int64_t value, int minDigits)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_bigdec_as_text (sk_wstream_t cstream, Int64 value, Int32 minDigits);
		

		// bool sk_wstream_write_bool(sk_wstream_t* cstream, bool value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_bool (sk_wstream_t cstream, [MarshalAs (UnmanagedType.I1)] bool value);
		

		// bool sk_wstream_write_dec_as_text(sk_wstream_t* cstream, int32_t value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_dec_as_text (sk_wstream_t cstream, Int32 value);
		

		// bool sk_wstream_write_hex_as_text(sk_wstream_t* cstream, uint32_t value, int minDigits)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_hex_as_text (sk_wstream_t cstream, UInt32 value, Int32 minDigits);
		

		// bool sk_wstream_write_packed_uint(sk_wstream_t* cstream, size_t value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_packed_uint (sk_wstream_t cstream, /* size_t */ IntPtr value);
		

		// bool sk_wstream_write_scalar(sk_wstream_t* cstream, float value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_scalar (sk_wstream_t cstream, Single value);
		

		// bool sk_wstream_write_scalar_as_text(sk_wstream_t* cstream, float value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_scalar_as_text (sk_wstream_t cstream, Single value);
		

		// bool sk_wstream_write_stream(sk_wstream_t* cstream, sk_stream_t* input, size_t length)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_stream (sk_wstream_t cstream, sk_stream_t input, /* size_t */ IntPtr length);
		

		// bool sk_wstream_write_text(sk_wstream_t* cstream, const char* value)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_wstream_write_text (sk_wstream_t cstream, [MarshalAs (UnmanagedType.LPStr)] String value);
		

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
		

		// void sk_surface_flush(sk_surface_t* surface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_surface_flush (sk_surface_t surface);
		

		// void sk_surface_flush_and_submit(sk_surface_t* surface, bool syncCpu)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_surface_flush_and_submit (sk_surface_t surface, [MarshalAs (UnmanagedType.I1)] bool syncCpu);
		

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
		internal static extern sk_surface_t sk_surface_new_backend_render_target (gr_recording_context_t context, gr_backendrendertarget_t target, GRSurfaceOrigin origin, SKColorTypeNative colorType, sk_colorspace_t colorspace, sk_surfaceprops_t props);
		

		// sk_surface_t* sk_surface_new_backend_texture(gr_recording_context_t* context, const gr_backendtexture_t* texture, gr_surfaceorigin_t origin, int samples, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_surface_t sk_surface_new_backend_texture (gr_recording_context_t context, gr_backendtexture_t texture, GRSurfaceOrigin origin, Int32 samples, SKColorTypeNative colorType, sk_colorspace_t colorspace, sk_surfaceprops_t props);
		

		// sk_image_t* sk_surface_new_image_snapshot(sk_surface_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_surface_new_image_snapshot (sk_surface_t param0);
		

		// sk_image_t* sk_surface_new_image_snapshot_with_crop(sk_surface_t* surface, const sk_irect_t* bounds)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_image_t sk_surface_new_image_snapshot_with_crop (sk_surface_t surface, RectI* bounds);
		

		// sk_surface_t* sk_surface_new_metal_layer(gr_recording_context_t* context, const void* layer, gr_surfaceorigin_t origin, int sampleCount, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props, const void** drawable)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_surface_t sk_surface_new_metal_layer (gr_recording_context_t context, void* layer, GRSurfaceOrigin origin, Int32 sampleCount, SKColorTypeNative colorType, sk_colorspace_t colorspace, sk_surfaceprops_t props, void** drawable);
		

		// sk_surface_t* sk_surface_new_metal_view(gr_recording_context_t* context, const void* mtkView, gr_surfaceorigin_t origin, int sampleCount, sk_colortype_t colorType, sk_colorspace_t* colorspace, const sk_surfaceprops_t* props)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_surface_t sk_surface_new_metal_view (gr_recording_context_t context, void* mtkView, GRSurfaceOrigin origin, Int32 sampleCount, SKColorTypeNative colorType, sk_colorspace_t colorspace, sk_surfaceprops_t props);
		

		// sk_surface_t* sk_surface_new_null(int width, int height)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_surface_t sk_surface_new_null (Int32 width, Int32 height);
		

		// sk_surface_t* sk_surface_new_raster(const sk_imageinfo_t*, size_t rowBytes, const sk_surfaceprops_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_surface_t sk_surface_new_raster (SKImageInfoNative* param0, /* size_t */ IntPtr rowBytes, sk_surfaceprops_t param2);
		

		// sk_surface_t* sk_surface_new_raster_direct(const sk_imageinfo_t*, void* pixels, size_t rowBytes, const sk_surface_raster_release_proc releaseProc, void* context, const sk_surfaceprops_t* props)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_surface_t sk_surface_new_raster_direct (SKImageInfoNative* param0, void* pixels, /* size_t */ IntPtr rowBytes, SKSurfaceRasterReleaseProxyDelegate releaseProc, void* context, sk_surfaceprops_t props);
		

		// sk_surface_t* sk_surface_new_render_target(gr_recording_context_t* context, bool budgeted, const sk_imageinfo_t* cinfo, int sampleCount, gr_surfaceorigin_t origin, const sk_surfaceprops_t* props, bool shouldCreateWithMips)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_surface_t sk_surface_new_render_target (gr_recording_context_t context, [MarshalAs (UnmanagedType.I1)] bool budgeted, SKImageInfoNative* cinfo, Int32 sampleCount, GRSurfaceOrigin origin, sk_surfaceprops_t props, [MarshalAs (UnmanagedType.I1)] bool shouldCreateWithMips);
		

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

		#region sk_typeface.h

		// int sk_fontmgr_count_families(sk_fontmgr_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_fontmgr_count_families (sk_fontmgr_t param0);
		

		// sk_fontmgr_t* sk_fontmgr_create_default()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_fontmgr_t sk_fontmgr_create_default ();
		

		// sk_typeface_t* sk_fontmgr_create_from_data(sk_fontmgr_t*, sk_data_t* data, int index)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_fontmgr_create_from_data (sk_fontmgr_t param0, sk_data_t data, Int32 index);
		

		// sk_typeface_t* sk_fontmgr_create_from_file(sk_fontmgr_t*, const char* path, int index)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_fontmgr_create_from_file (sk_fontmgr_t param0, /* char */ void* path, Int32 index);
		

		// sk_typeface_t* sk_fontmgr_create_from_stream(sk_fontmgr_t*, sk_stream_asset_t* stream, int index)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_fontmgr_create_from_stream (sk_fontmgr_t param0, sk_stream_asset_t stream, Int32 index);
		

		// sk_fontstyleset_t* sk_fontmgr_create_styleset(sk_fontmgr_t*, int index)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_fontstyleset_t sk_fontmgr_create_styleset (sk_fontmgr_t param0, Int32 index);
		

		// void sk_fontmgr_get_family_name(sk_fontmgr_t*, int index, sk_string_t* familyName)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_fontmgr_get_family_name (sk_fontmgr_t param0, Int32 index, sk_string_t familyName);
		

		// sk_typeface_t* sk_fontmgr_match_face_style(sk_fontmgr_t*, const sk_typeface_t* face, sk_fontstyle_t* style)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_fontmgr_match_face_style (sk_fontmgr_t param0, sk_typeface_t face, SKFontStyle* style);
		

		// sk_fontstyleset_t* sk_fontmgr_match_family(sk_fontmgr_t*, const char* familyName)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_fontstyleset_t sk_fontmgr_match_family (sk_fontmgr_t param0, [MarshalAs (UnmanagedType.LPStr)] String familyName);
		

		// sk_typeface_t* sk_fontmgr_match_family_style(sk_fontmgr_t*, const char* familyName, sk_fontstyle_t* style)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_fontmgr_match_family_style (sk_fontmgr_t param0, [MarshalAs (UnmanagedType.LPStr)] String familyName, SKFontStyle* style);
		

		// sk_typeface_t* sk_fontmgr_match_family_style_character(sk_fontmgr_t*, const char* familyName, sk_fontstyle_t* style, const char** bcp47, int bcp47Count, int32_t character)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_fontmgr_match_family_style_character (sk_fontmgr_t param0, [MarshalAs (UnmanagedType.LPStr)] String familyName, SKFontStyle* style, [MarshalAs (UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] String[] bcp47, Int32 bcp47Count, Int32 character);
		

		// sk_fontmgr_t* sk_fontmgr_ref_default()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_fontmgr_t sk_fontmgr_ref_default ();
		

		// void sk_fontmgr_unref(sk_fontmgr_t*)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_fontmgr_unref (sk_fontmgr_t param0);
		
		// sk_fontstyleset_t* sk_fontstyleset_create_empty()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_fontstyleset_t sk_fontstyleset_create_empty ();
		

		// sk_typeface_t* sk_fontstyleset_create_typeface(sk_fontstyleset_t* fss, int index)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_fontstyleset_create_typeface (sk_fontstyleset_t fss, Int32 index);
		

		// int sk_fontstyleset_get_count(sk_fontstyleset_t* fss)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_fontstyleset_get_count (sk_fontstyleset_t fss);
		

		// void sk_fontstyleset_get_style(sk_fontstyleset_t* fss, int index, sk_fontstyle_t* fs, sk_string_t* style)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_fontstyleset_get_style (sk_fontstyleset_t fss, Int32 index, SKFontStyle* fs, sk_string_t style);
		

		// sk_typeface_t* sk_fontstyleset_match_style(sk_fontstyleset_t* fss, sk_fontstyle_t* style)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_fontstyleset_match_style (sk_fontstyleset_t fss, SKFontStyle* style);
		

		// void sk_fontstyleset_unref(sk_fontstyleset_t* fss)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_fontstyleset_unref (sk_fontstyleset_t fss);
		

		// sk_data_t* sk_typeface_copy_table_data(const sk_typeface_t* typeface, sk_font_table_tag_t tag)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_data_t sk_typeface_copy_table_data (sk_typeface_t typeface, UInt32 tag);
		

		// int sk_typeface_count_glyphs(const sk_typeface_t* typeface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_typeface_count_glyphs (sk_typeface_t typeface);
		

		// int sk_typeface_count_tables(const sk_typeface_t* typeface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_typeface_count_tables (sk_typeface_t typeface);
		

		// sk_typeface_t* sk_typeface_create_default()
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_typeface_create_default ();
		

		// sk_typeface_t* sk_typeface_create_from_data(sk_data_t* data, int index)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_typeface_create_from_data (sk_data_t data, Int32 index);
		

		// sk_typeface_t* sk_typeface_create_from_file(const char* path, int index)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_typeface_create_from_file (/* char */ void* path, Int32 index);
		

		// sk_typeface_t* sk_typeface_create_from_name(const char* familyName, const sk_fontstyle_t* style)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_typeface_create_from_name ([MarshalAs (UnmanagedType.LPStr)] String familyName, SKFontStyle* style);
		

		// sk_typeface_t* sk_typeface_create_from_stream(sk_stream_asset_t* stream, int index)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_typeface_t sk_typeface_create_from_stream (sk_stream_asset_t stream, Int32 index);
		

		// sk_string_t* sk_typeface_get_family_name(const sk_typeface_t* typeface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_string_t sk_typeface_get_family_name (sk_typeface_t typeface);
		

		// sk_font_style_slant_t sk_typeface_get_font_slant(const sk_typeface_t* typeface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern FontSlant sk_typeface_get_font_slant (sk_typeface_t typeface);
		

		// int sk_typeface_get_font_weight(const sk_typeface_t* typeface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_typeface_get_font_weight (sk_typeface_t typeface);
		

		// int sk_typeface_get_font_width(const sk_typeface_t* typeface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_typeface_get_font_width (sk_typeface_t typeface);
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern int sk_typeface_get_fontstyle (sk_typeface_t typeface);
		
		// bool sk_typeface_get_kerning_pair_adjustments(const sk_typeface_t* typeface, const uint16_t[-1] glyphs, int count, int32_t[-1] adjustments)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_typeface_get_kerning_pair_adjustments (sk_typeface_t typeface, UInt16* glyphs, Int32 count, Int32* adjustments);
		

		// size_t sk_typeface_get_table_data(const sk_typeface_t* typeface, sk_font_table_tag_t tag, size_t offset, size_t length, void* data)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_typeface_get_table_data (sk_typeface_t typeface, UInt32 tag, /* size_t */ IntPtr offset, /* size_t */ IntPtr length, void* data);
		

		// size_t sk_typeface_get_table_size(const sk_typeface_t* typeface, sk_font_table_tag_t tag)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern /* size_t */ IntPtr sk_typeface_get_table_size (sk_typeface_t typeface, UInt32 tag);
		

		// int sk_typeface_get_table_tags(const sk_typeface_t* typeface, sk_font_table_tag_t[-1] tags)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_typeface_get_table_tags (sk_typeface_t typeface, UInt32* tags);
		

		// int sk_typeface_get_units_per_em(const sk_typeface_t* typeface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern Int32 sk_typeface_get_units_per_em (sk_typeface_t typeface);
		

		// bool sk_typeface_is_fixed_pitch(const sk_typeface_t* typeface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs (UnmanagedType.I1)]
		internal static extern bool sk_typeface_is_fixed_pitch (sk_typeface_t typeface);
		

		// sk_stream_asset_t* sk_typeface_open_stream(const sk_typeface_t* typeface, int* ttcIndex)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_asset_t sk_typeface_open_stream (sk_typeface_t typeface, Int32* ttcIndex);
		

		// // sk_typeface_t* sk_typeface_ref_default()
		//
		// [DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		// internal static extern sk_typeface_t sk_typeface_ref_default ();
		//

		// uint16_t sk_typeface_unichar_to_glyph(const sk_typeface_t* typeface, const int32_t unichar)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern UInt16 sk_typeface_unichar_to_glyph (sk_typeface_t typeface, Int32 unichar);
		

		// void sk_typeface_unichars_to_glyphs(const sk_typeface_t* typeface, const int32_t[-1] unichars, int count, uint16_t[-1] glyphs)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_typeface_unichars_to_glyphs (sk_typeface_t typeface, Int32* unichars, Int32 count, UInt16* glyphs);
		

		// void sk_typeface_unref(sk_typeface_t* typeface)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_typeface_unref (sk_typeface_t typeface);
		

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

		#region sk_managedstream.h

		// void sk_managedstream_destroy(sk_stream_managedstream_t* s)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_managedstream_destroy (sk_stream_managedstream_t s);
		

		// sk_stream_managedstream_t* sk_managedstream_new(void* context)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_stream_managedstream_t sk_managedstream_new (void* context);
		

		// void sk_managedstream_set_procs(sk_managedstream_procs_t procs)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_managedstream_set_procs (SKManagedStreamDelegates procs);
		

		// void sk_managedwstream_destroy(sk_wstream_managedstream_t* s)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_managedwstream_destroy (sk_wstream_managedstream_t s);
		

		// sk_wstream_managedstream_t* sk_managedwstream_new(void* context)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern sk_wstream_managedstream_t sk_managedwstream_new (void* context);
		

		// void sk_managedwstream_set_procs(sk_managedwstream_procs_t procs)
		
		[DllImport (SKIA, CallingConvention = CallingConvention.Cdecl)]
		internal static extern void sk_managedwstream_set_procs (SKManagedWStreamDelegates procs);
		

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

}
#endif
