using PixUI;
using PixUI.Demo;
using PixUI.Drawing.Skia;
using PixUI.Platform.Win;

Render.Init(new SkiaRender());
//WinApplication.Run(new DemoRoute());
WinApplication.Run(new Center()
{
    Child = new Container()
    {
        Width = 100,
        Height = 100,
        FillColor = Colors.Red,
    }
});