using System.Runtime.Versioning;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using PixUI.Demo;
using PixUI.Demo.Mac;
using PixUI.Drawing.Skia;

namespace PixUI.Platform.Blazor;

[SupportedOSPlatform("browser")]
public static class Program
{
    public static async Task Main(string[] args)
    {
        Render.Init(new SkiaRender());
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        var host = builder.Build();
        BlazorApplication.JSRuntime = host.Services.GetRequiredService<IJSRuntime>();
        BlazorApplication.HttpClient = host.Services.GetService<HttpClient>()!;

        //调用js获取启动参数
        var jsRuntime = ((IJSInProcessRuntime)BlazorApplication.JSRuntime);
#if SK_GRAPHITE
        const bool useGraphite = true;
#else
        const bool useGraphite = false;
#endif
        var runInfo = await jsRuntime.InvokeAsync<RunInfo>("PixUI.BeforeRunApp", useGraphite);
        await Run(runInfo);
        jsRuntime.InvokeVoid("PixUI.BindEvents");

        await host.RunAsync();
    }

    private static async Task Run(RunInfo runInfo)
    {
        //初始化默认字体
        var fontUrl = "/fonts/MiSans-Regular.woff2";
        // var fontUrl = "/fonts/NotoMono-Regular.ttf";
        await using var fontDataStream = await BlazorApplication.HttpClient.GetStreamAsync(fontUrl);
        //因fontDataStream不支持同步复制(DotNet10)，所以先复制至MemoryStream
        using var ms = new MemoryStream();
        await fontDataStream.CopyToAsync(ms);
        ms.Position = 0;
        FontCollection.RegisterTypeface(ms, FontCollection.DefaultFamilyName, false);

        //开始执行Blazor应用
        BlazorApplication.Run(() => new DemoDataGrid(), runInfo);
        //BlazorApplication.Run(() => new TestWidget() { Width = 100, Height = 100 }, runInfo);
    }

    private class TestWidget : Widget, IMouseRegion
    {
        public TestWidget()
        {
            MouseRegion.HoverChanged += OnHoverChanged;
        }

        private bool _isHovered;

        public MouseRegion MouseRegion { get; } = new MouseRegion();

        private void OnHoverChanged(bool isHovered)
        {
            _isHovered = isHovered;
            Repaint();
        }

        public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
        {
            var color = _isHovered ? Colors.Red : Colors.Green;
            var paint = Paint.Shared(color, PaintStyle.Stroke);
            for (int i = 0; i < 100; i++)
            {
                canvas.DrawLine(i, i, 100, i, paint);

                using var ph = TextPainter.BuildParagraph("中国", 100, 12, Colors.Blue);
                canvas.DrawParagraph(ph, 10, 10);
            }
        }
    }
}