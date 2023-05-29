#if __WEB__
namespace PixUI
{
    [TSType("CanvasKit.ImageFilter")]
    public class ImageFilter
    {
        //TODO:

        [TSTemplate("CanvasKit.ImageFilter.MakeBlur({1},{2},{3},{4})")]
        public static ImageFilter? CreateBlur(float sigmaX, float sigmaY, TileMode tileMode,
            ImageFilter? input) => throw new System.Exception();

        [TSTemplate("CanvasKit.ImageFilter.MakeDropShadow({1},{2},{3},{4},{5},{6})")]
        public static ImageFilter? CreateDropShadow(float dx, float dy, float sigmaX, float sigmaY, Color color,
            ImageFilter? input) => throw new System.Exception();
        
        [TSRename("delete")]
        public void Dispose() {}
    }
}
#endif