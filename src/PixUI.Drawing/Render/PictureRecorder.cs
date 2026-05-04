namespace PixUI;

public interface IPictureRecorder : IDisposable
{
    ICanvas BeginRecording(Rect cullRect);

    IPicture EndRecording();
}

public static class PictureRecorder
{
    public static IPictureRecorder Create() => Render.Backend.MakePictureRecorder();
}