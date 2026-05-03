namespace PixUI;

public interface ITypeface : IDisposable
{
    IFont MakeFont(float size);
}