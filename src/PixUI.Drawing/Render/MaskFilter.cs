namespace PixUI;

public interface IMaskFilter : IDisposable { }

public static class MaskFilter
{
    private const float BlurSigmaScale = 0.57735f;

    public static float ConvertRadiusToSigma(float radius) =>
        radius > 0 ? BlurSigmaScale * radius + 0.5f : 0.0f;

    public static float ConvertSigmaToRadius(float sigma) =>
        sigma > 0.5f ? (sigma - 0.5f) / BlurSigmaScale : 0.0f;

    public static IMaskFilter? CreateBlur(BlurStyle blurStyle, float sigma)
        => Render.Provider.MaskFilterCreateBlur(blurStyle, sigma);
}