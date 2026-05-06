namespace PixUI.Drawing.Skia;

public sealed unsafe class SKPictureRecorder : SKObject, ISKSkipObjectRegistration, IPictureRecorder
{
    internal SKPictureRecorder(IntPtr handle, bool owns) : base(handle, owns) { }

    public SKPictureRecorder() : this(SkiaApi.sk_picture_recorder_new(), true)
    {
        if (Handle == IntPtr.Zero)
            throw new InvalidOperationException("Unable to create a new PictureRecorder instance.");
    }

    protected override void DisposeNative() => SkiaApi.sk_picture_recorder_delete(Handle);

    public ICanvas BeginRecording(Rect cullRect) =>
        OwnedBy(SKCanvas.GetObject(SkiaApi.sk_picture_recorder_begin_recording(Handle, &cullRect), false)!, this);

    public IPicture EndRecording() => SKPicture.GetObject(SkiaApi.sk_picture_recorder_end_recording(Handle))!;

    public SKCanvas RecordingCanvas =>
        OwnedBy(SKCanvas.GetObject(SkiaApi.sk_picture_get_recording_canvas(Handle), false)!, this);
}