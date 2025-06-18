using System;

#if !__WEB__
namespace PixUI;

public sealed unsafe class PictureRecorder : SKObject, ISKSkipObjectRegistration
{
    internal PictureRecorder(IntPtr handle, bool owns) : base(handle, owns) { }

    public PictureRecorder() : this(SkiaApi.sk_picture_recorder_new(), true)
    {
        if (Handle == IntPtr.Zero)
            throw new InvalidOperationException("Unable to create a new PictureRecorder instance.");
    }

    protected override void DisposeNative() => SkiaApi.sk_picture_recorder_delete(Handle);

    public Canvas BeginRecording(Rect cullRect) =>
        OwnedBy(Canvas.GetObject(SkiaApi.sk_picture_recorder_begin_recording(Handle, &cullRect), false), this);

    public Picture EndRecording() => Picture.GetObject(SkiaApi.sk_picture_recorder_end_recording(Handle))!;

    public Canvas RecordingCanvas =>
        OwnedBy(Canvas.GetObject(SkiaApi.sk_picture_get_recording_canvas(Handle), false), this);
}

#endif