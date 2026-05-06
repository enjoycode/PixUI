namespace PixUI;

public interface ITypeface : IDisposable
{
    string FamilyName { get; }

    IFont MakeFont(float size);
}