namespace PixUI;

public static class Render
{
    public static IRender Provider { get; private set; } = null!;

    public static void Init(IRender provider)
    {
        Provider = provider;
    }
}