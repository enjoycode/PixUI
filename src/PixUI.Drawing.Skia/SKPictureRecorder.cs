#if !__WEB__
namespace PixUI;

public sealed unsafe class SKPictureRecorder : SKObject, ISKSkipObjectRegistration
{
    internal SKPictureRecorder(IntPtr handle, bool owns) : base(handle, owns) { }

    public SKPictureRecorder() : this(SkiaApi.sk_picture_recorder_new(), true)
    {
        if (Handle == IntPtr.Zero)
            throw new InvalidOperationException("Unable to create a new PictureRecorder instance.");
    }

    protected override void DisposeNative() => SkiaApi.sk_picture_recorder_delete(Handle);

    public SKCanvas BeginRecording(Rect cullRect) =>
        OwnedBy(SKCanvas.GetObject(SkiaApi.sk_picture_recorder_begin_recording(Handle, &cullRect), false), this);

    public SKPicture EndRecording() => SKPicture.GetObject(SkiaApi.sk_picture_recorder_end_recording(Handle))!;

    public SKCanvas RecordingCanvas =>
        OwnedBy(SKCanvas.GetObject(SkiaApi.sk_picture_get_recording_canvas(Handle), false), this);
}

#endif