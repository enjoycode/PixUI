#nullable disable

namespace PixUI.Drawing.Skia;

public unsafe class SKSurface : SKObject, ISKReferenceCounted, ISKSkipObjectRegistration, ISurface
{
    private SKSurface(IntPtr h, bool owns) : base(h, owns) { }

    internal static SKSurface? GetObject(IntPtr handle) =>
        handle == IntPtr.Zero ? null : new SKSurface(handle, true);

    public ICanvas Canvas =>
        OwnedBy(SKCanvas.GetObject(this, SkiaApi.sk_surface_get_canvas(Handle), false, unrefExisting: false)!, this);

    #region ====Static Create====

    // RASTER DIRECT surface
    public static SKSurface Create(ImageInfo info, int rowBytes = 0, SKSurfaceProperties? props = null)
    {
        var cinfo = SKImageInfoNative.FromManaged(ref info);
        return GetObject(SkiaApi.sk_surface_new_raster(&cinfo, (IntPtr)rowBytes, props?.Handle ?? IntPtr.Zero))!;
    }

    public static SKSurface Create(ImageInfo info, IntPtr pixels, int rowBytes)
    {
        var cinfo = SKImageInfoNative.FromManaged(ref info);
        return GetObject(SkiaApi.sk_surface_new_raster_direct(&cinfo, (void*)pixels, (IntPtr)rowBytes,
            null, null, IntPtr.Zero))!;
    }

    // Graphite-backed render target

    /// <summary>Creates a Graphite-backed surface for the specified recorder.</summary>
    /// <param name="recorder">The recorder that the surface is created for.</param>
    /// <param name="info">The image info describing the size and format of the surface.</param>
    /// <returns>A new <see cref="T:SkiaSharp.SKSurface" />, or <see langword="null" /> if it could not be created.</returns>
    /// <remarks />
    public static SKSurface Create(SKGraphiteRecorder recorder, ImageInfo info) =>
        Create(recorder, info, mipmapped: false, props: null);

    /// <summary>Creates a Graphite-backed surface for the specified recorder, optionally with mipmaps.</summary>
    /// <param name="recorder">The recorder that the surface is created for.</param>
    /// <param name="info">The image info describing the size and format of the surface.</param>
    /// <param name="mipmapped"><see langword="true" /> to allocate the surface with mipmaps; otherwise, <see langword="false" />.</param>
    /// <returns>A new <see cref="T:SkiaSharp.SKSurface" />, or <see langword="null" /> if it could not be created.</returns>
    /// <remarks />
    public static SKSurface Create(SKGraphiteRecorder recorder, ImageInfo info, bool mipmapped) =>
        Create(recorder, info, mipmapped, props: null);

    /// <summary>Creates a Graphite-backed surface for the specified recorder, using the specified surface properties.</summary>
    /// <param name="recorder">The recorder that the surface is created for.</param>
    /// <param name="info">The image info describing the size and format of the surface.</param>
    /// <param name="props">The surface properties to use.</param>
    /// <returns>A new <see cref="T:SkiaSharp.SKSurface" />, or <see langword="null" /> if it could not be created.</returns>
    /// <remarks />
    public static SKSurface Create(SKGraphiteRecorder recorder, ImageInfo info, SKSurfaceProperties props) =>
        Create(recorder, info, mipmapped: false, props);

    /// <summary>Creates a Graphite-backed surface for the specified recorder, optionally with mipmaps and using the specified surface properties.</summary>
    /// <param name="recorder">The recorder that the surface is created for.</param>
    /// <param name="info">The image info describing the size and format of the surface.</param>
    /// <param name="mipmapped"><see langword="true" /> to allocate the surface with mipmaps; otherwise, <see langword="false" />.</param>
    /// <param name="props">The surface properties to use.</param>
    /// <returns>A new <see cref="T:SkiaSharp.SKSurface" />, or <see langword="null" /> if it could not be created.</returns>
    /// <remarks />
    public static SKSurface Create(SKGraphiteRecorder recorder, ImageInfo info, bool mipmapped,
        SKSurfaceProperties props)
    {
        if (recorder == null)
            throw new ArgumentNullException(nameof(recorder));

        var cinfo = SKImageInfoNative.FromManaged(ref info);
        return GetObject(SkiaApi.sk_graphite_surface_make_render_target(recorder.Handle, &cinfo, mipmapped,
            props?.Handle ?? IntPtr.Zero));
    }

    // Graphite-backed surface wrapping a caller-allocated GPU texture

    /// <summary>Creates a Graphite-backed surface that renders into an existing backend texture.</summary>
    /// <param name="recorder">The recorder that the surface is created for.</param>
    /// <param name="backendTexture">The backend texture to render into.</param>
    /// <param name="colorType">One of the enumeration values that specifies the color type of the texture.</param>
    /// <returns>A new <see cref="T:SkiaSharp.SKSurface" />, or <see langword="null" /> if it could not be created.</returns>
    /// <remarks />
    public static SKSurface Create(SKGraphiteRecorder recorder, SKGraphiteBackendTexture backendTexture,
        ColorType colorType) =>
        Create(recorder, backendTexture, colorType, colorSpace: null, props: null);

    /// <summary>Creates a Graphite-backed surface that renders into an existing backend texture, using the specified color space.</summary>
    /// <param name="recorder">The recorder that the surface is created for.</param>
    /// <param name="backendTexture">The backend texture to render into.</param>
    /// <param name="colorType">One of the enumeration values that specifies the color type of the texture.</param>
    /// <param name="colorSpace">The color space of the texture, or <see langword="null" /> to use no color space.</param>
    /// <returns>A new <see cref="T:SkiaSharp.SKSurface" />, or <see langword="null" /> if it could not be created.</returns>
    /// <remarks />
    public static SKSurface Create(SKGraphiteRecorder recorder, SKGraphiteBackendTexture backendTexture,
        ColorType colorType, SKColorSpace colorSpace) =>
        Create(recorder, backendTexture, colorType, colorSpace, props: null);

    /// <summary>Creates a Graphite-backed surface that renders into an existing backend texture, using the specified color space and surface properties.</summary>
    /// <param name="recorder">The recorder that the surface is created for.</param>
    /// <param name="backendTexture">The backend texture to render into.</param>
    /// <param name="colorType">One of the enumeration values that specifies the color type of the texture.</param>
    /// <param name="colorSpace">The color space of the texture, or <see langword="null" /> to use no color space.</param>
    /// <param name="props">The surface properties to use.</param>
    /// <returns>A new <see cref="T:SkiaSharp.SKSurface" />, or <see langword="null" /> if it could not be created.</returns>
    /// <remarks />
    public static SKSurface Create(SKGraphiteRecorder recorder, SKGraphiteBackendTexture backendTexture,
        ColorType colorType, SKColorSpace colorSpace, SKSurfaceProperties props) =>
        Create(recorder, backendTexture, colorType, colorSpace, props, releaseProc: null);

    /// <summary>Creates a Graphite-backed surface that renders into an existing backend texture, invoking a callback when Skia no longer needs the texture.</summary>
    /// <param name="recorder">The recorder that the surface is created for.</param>
    /// <param name="backendTexture">The backend texture to render into.</param>
    /// <param name="colorType">One of the enumeration values that specifies the color type of the texture.</param>
    /// <param name="colorSpace">The color space of the texture, or <see langword="null" /> to use no color space.</param>
    /// <param name="props">The surface properties to use.</param>
    /// <param name="releaseProc">The callback invoked when Skia is finished using the texture, or <see langword="null" /> for none.</param>
    /// <returns>A new <see cref="T:SkiaSharp.SKSurface" />, or <see langword="null" /> if it could not be created.</returns>
    /// <remarks />
    public static SKSurface Create(SKGraphiteRecorder recorder, SKGraphiteBackendTexture backendTexture,
        ColorType colorType,
        SKColorSpace colorSpace, SKSurfaceProperties props, SKGraphiteReleaseDelegate releaseProc)
    {
        if (recorder == null)
            throw new ArgumentNullException(nameof(recorder));
        if (backendTexture == null)
            throw new ArgumentNullException(nameof(backendTexture));

        DelegateProxies.Create(releaseProc, out _, out var ctx);
        var proxy = releaseProc != null ? DelegateProxies.SKGraphiteReleaseProxy : null;

        return GetObject(SkiaApi.sk_graphite_surface_wrap_backend_texture(
            recorder.Handle,
            backendTexture.Handle,
            colorType.ToNative(),
            colorSpace?.Handle ?? IntPtr.Zero,
            props?.Handle ?? IntPtr.Zero,
            proxy,
            (void*)ctx));
    }

#if SK_GANESH
    // ----GPU BACKEND RENDER TARGET surface----

    public static SKSurface? Create(GRRecordingContext context, GRBackendRenderTarget renderTarget,
        SurfaceOrigin origin, ColorType colorType,
        SKColorSpace? colorspace, SKSurfaceProperties? props)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        if (renderTarget == null)
            throw new ArgumentNullException(nameof(renderTarget));

        return GetObject(SkiaApi.sk_surface_new_backend_render_target(context.Handle, renderTarget.Handle,
            origin, colorType.ToNative(), colorspace?.Handle ?? IntPtr.Zero, props?.Handle ?? IntPtr.Zero));
    }
    
    public static SKSurface? CreateGLOnScreen(GRContext grContext, int width, int height)
    {
        var surfacePtr = SkiaApi.gr_direct_context_make_gl_onscreen_surface(grContext.Handle, width, height);
        return GetObject(surfacePtr);
    }

    // ----GPU NEW surface----
    public static SKSurface? Create(GRRecordingContext context, bool budgeted, ImageInfo info) =>
        Create(context, budgeted, info, 0, SurfaceOrigin.TopLeft, null, false);

    public static SKSurface? Create(GRRecordingContext context, bool budgeted, ImageInfo info,
        int sampleCount, SurfaceOrigin origin, ISurfaceProperties? props, bool shouldCreateWithMips)
    {
        var cinfo = SKImageInfoNative.FromManaged(ref info);
        return GetObject(SkiaApi.sk_surface_new_render_target(context.Handle, budgeted, &cinfo, sampleCount, origin,
            (props as SKSurfaceProperties)?.Handle ?? IntPtr.Zero, shouldCreateWithMips));
    }
#endif //SK_GANESH

    #endregion


    public IImage Snapshot() =>
        SKImage.GetObject(SkiaApi.sk_surface_new_image_snapshot(Handle))!;

    public IImage Snapshot(RectI bounds) =>
        SKImage.GetObject(SkiaApi.sk_surface_new_image_snapshot_with_crop(Handle, &bounds))!;

    public void Draw(ICanvas canvas, float x, float y, IPaint paint)
        => SkiaApi.sk_surface_draw(Handle, ((SKCanvas)canvas).Handle, x, y, (paint as SKPaint)?.Handle ?? IntPtr.Zero);
}