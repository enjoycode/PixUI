using PixUI.Drawing.Skia;
using PixUI.Platform.Mac;

namespace PixUI.Demo.Mac;

internal static class MainClass
{
    private static void Main(string[] args)
    {
        Render.Init(new SkiaRender());
        MacApplication.Run(new DemoRoute());
    }
}