using System.Runtime.Versioning;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using PixUI.Demo;
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
        var runInfo = await jsRuntime.InvokeAsync<RunInfo>("PixUI.BeforeRunApp");
        await Run(runInfo.Width, runInfo.Height, runInfo.PixelRatio, runInfo.RoutePath,
            runInfo.IsMacOS);
        jsRuntime.InvokeVoid("PixUI.BindEvents");

        await host.RunAsync();
    }

    private static async Task Run(int width, int height, float ratio, string? routePath, bool isMacOS)
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
        BlazorApplication.Run(() => new DemoRoute(), width, height, ratio, routePath, isMacOS);
        // BlazorApplication.Run(() => new Center()
        //         .WithChild(new Card()
        //             .WithChild(new Container() { Width = 200, Height = 200 })),
        //     width, height, ratio, routePath, isMacOS);
    }

    // private static async Task TestCanvas()
    // {
    //     var adapter = await WebGPU.RequestAdapter()
    //                   ?? throw new InvalidOperationException(
    //                       "navigator.gpu.requestAdapter returned null — WebGPU is unavailable in this browser.");
    //     var device = await WebGPU.RequestDevice(adapter)
    //                  ?? throw new InvalidOperationException(
    //                      "adapter.requestDevice returned null.");
    //
    //     var instanceId = WebGPU.CreateInstance();
    //     if (instanceId == 0)
    //         throw new InvalidOperationException(
    //             "Module._wgpuCreateInstance not exported — cannot obtain a real WGPUInstance.");
    //
    //     var queue = WebGPU.GetDeviceQueue(device);
    //     var queueId = WebGPU.RegisterQueue(queue, instanceId);
    //     var deviceId = WebGPU.RegisterDevice(device, instanceId);
    //
    //     var backendContext = new SKGraphiteDawnBackendContext
    //     {
    //         WgpuInstance = instanceId,
    //         WgpuDevice = deviceId,
    //         WgpuQueue = queueId,
    //     };
    //     var context = SKGraphiteContext.CreateDawn(backendContext)
    //                   ?? throw new InvalidOperationException("SKGraphiteContext.CreateDawn returned null.");
    //     var onscreenRecorder = context.CreateRecorder() ?? throw new InvalidOperationException(
    //         "SKGraphiteContext.CreateRecorder returned null.");
    //     var offscreenRecorder = context.CreateRecorder() ?? throw new InvalidOperationException(
    //         "SKGraphiteContext.CreateRecorder returned null.");
    //
    //     //create onscreen surface
    //     var onscreenTexture = WebGPU.GetCanvasTexture(device);
    //     var onscreenTextureId = WebGPU.RegisterTexture(onscreenTexture);
    //
    //     using var onscreenBackend = SKGraphiteBackendTexture.CreateDawn(onscreenTextureId) ??
    //                                 throw new InvalidOperationException(
    //                                     "SKGraphiteBackendTexture.CreateDawn returned null.");
    //     using var onscreenSurface = SKSurface.Create(onscreenRecorder, onscreenBackend, ColorType.Rgba8888)
    //                                 ?? throw new InvalidOperationException(
    //                                     "SKSurface.Create returned null on Graphite/Dawn.");
    //
    //     //create offscreen surface
    //     var offscreenTexture =
    //         WebGPU.CreateTexture(device, onscreenBackend.Dimensions.Width, onscreenBackend.Dimensions.Height);
    //     var offscreenTextureId = WebGPU.RegisterTexture(offscreenTexture);
    //     using var offscreenBackend = SKGraphiteBackendTexture.CreateDawn(offscreenTextureId)
    //                                  ?? throw new InvalidOperationException(
    //                                      "SKGraphiteBackendTexture.CreateDawn returned null.");
    //     using var offscreenSurface = SKSurface.Create(offscreenRecorder, offscreenBackend, ColorType.Rgba8888)
    //                                  ?? throw new InvalidOperationException(
    //                                      "SKSurface.Create returned null on Graphite/Dawn.");
    //
    //     //Draw something
    //     using var paint = new SKPaint();
    //     paint.Color = Colors.Red;
    //     paint.IsAntialias = true;
    //     offscreenSurface.Canvas.Clear(Colors.White);
    //     offscreenSurface.Canvas.DrawRect(Rect.FromLTWH(10, 10, 100, 100), paint);
    //     paint.Color = Colors.Green;
    //     paint.Style = PaintStyle.Stroke;
    //     offscreenSurface.Canvas.DrawLine(10, 10, 110, 110, paint);
    //     FlushSurface(context, offscreenRecorder);
    //     offscreenSurface.Draw(onscreenSurface.Canvas, 0, 0, null);
    //
    //     //flush onscreen surface
    //     FlushSurface(context, onscreenRecorder);
    // }

    // private static void FlushSurface(SKGraphiteContext context, SKGraphiteRecorder recorder)
    // {
    //     using (var recording = recorder.Snap() ??
    //                            throw new InvalidOperationException("Recorder.Snap() returned null."))
    //     {
    //         if (context.InsertRecording(recording) != SKGraphiteInsertStatus.Success)
    //             throw new InvalidOperationException("InsertRecording did not report Success.");
    //     }
    //
    //     context.Submit(new SKGraphiteSubmitInfo { Sync = false });
    // }

    public struct RunInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float PixelRatio { get; set; }
        public string? RoutePath { get; set; }
        public bool IsMacOS { get; set; }
    }
}