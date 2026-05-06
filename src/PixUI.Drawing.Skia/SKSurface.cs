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

    #endregion

    public IImage Snapshot() =>
        SKImage.GetObject(SkiaApi.sk_surface_new_image_snapshot(Handle))!;

    public IImage Snapshot(RectI bounds) =>
        SKImage.GetObject(SkiaApi.sk_surface_new_image_snapshot_with_crop(Handle, &bounds))!;

    public void Draw(ICanvas canvas, float x, float y, IPaint? paint)
        => SkiaApi.sk_surface_draw(Handle, ((SKCanvas)canvas).Handle, x, y, (paint as SKPaint)?.Handle ?? IntPtr.Zero);
}