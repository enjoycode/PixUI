using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using PixUI.Demo;
using PixUI.Drawing.Skia;

namespace PixUI.Platform.Blazor;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // Render.Init(new SkiaRender());
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        var host = builder.Build();
        BlazorApplication.JSRuntime = host.Services.GetRequiredService<IJSRuntime>();
        BlazorApplication.HttpClient = host.Services.GetService<HttpClient>()!;

        //调用js获取启动参数
        var jsRuntime = ((IJSInProcessRuntime)BlazorApplication.JSRuntime);
        var runInfo = jsRuntime.Invoke<RunInfo>("PixUI.BeforeRunApp");
        await Run(runInfo.Width, runInfo.Height, runInfo.PixelRatio, runInfo.RoutePath,
            runInfo.IsMacOS);
        jsRuntime.InvokeVoid("PixUI.BindEvents");

        await host.RunAsync();
    }

    private static async Task Run(int width, int height, float ratio, string? routePath, bool isMacOS)
    {
        //初始化默认字体
        // var fontUrl = "/fonts/MiSans-Regular.woff2";
        // // var fontUrl = "/fonts/NotoMono-Regular.ttf";
        // await using var fontDataStream = await BlazorApplication.HttpClient.GetStreamAsync(fontUrl);
        // //因fontDataStream不支持同步复制(DotNet10)，所以先复制至MemoryStream
        // using var ms = new MemoryStream();
        // await fontDataStream.CopyToAsync(ms);
        // ms.Position = 0;
        // FontCollection.RegisterTypeface(ms, FontCollection.DefaultFamilyName, false);

        //await TestWebGpu();
        await TestCanvas();

        //开始执行Blazor应用
        //BlazorApplication.Run(() => new DemoRoute(), width, height, ratio, routePath, isMacOS);
    }

    private static async Task TestCanvas()
    {
        var adapter = await WebGPU.RequestAdapter()
                      ?? throw new InvalidOperationException(
                          "navigator.gpu.requestAdapter returned null — WebGPU is unavailable in this browser.");
        var device = await WebGPU.RequestDevice(adapter)
                     ?? throw new InvalidOperationException(
                         "adapter.requestDevice returned null.");

        var instanceId = WebGPU.CreateInstance();
        if (instanceId == 0)
            throw new InvalidOperationException(
                "Module._wgpuCreateInstance not exported — cannot obtain a real WGPUInstance.");

        var queue = WebGPU.GetDeviceQueue(device);
        var queueId = WebGPU.RegisterQueue(queue, instanceId);
        var deviceId = WebGPU.RegisterDevice(device, instanceId);

        var backendContext = new SKGraphiteDawnBackendContext
        {
            WgpuInstance = (IntPtr)instanceId,
            WgpuDevice = (IntPtr)deviceId,
            WgpuQueue = (IntPtr)queueId,
        };
        var context = SKGraphiteContext.CreateDawn(backendContext)
                      ?? throw new InvalidOperationException("SKGraphiteContext.CreateDawn returned null.");
        var recorder = context.CreateRecorder()
                       ?? throw new InvalidOperationException("SKGraphiteContext.CreateRecorder returned null.");

        //var texture = WebGPU.CreateTexture(offscreenDevice, 400, 300); //offscreen texture
        var texture = WebGPU.GetCanvasTexture(device); //onscreen texture
        var textureId = WebGPU.RegisterTexture(texture);

        using var backendTex = SKGraphiteBackendTexture.CreateDawn((IntPtr)textureId)
                               ?? throw new InvalidOperationException(
                                   "SKGraphiteBackendTexture.CreateDawn returned null.");
        using var surface = SKSurface.Create(recorder, backendTex, ColorType.Rgba8888)
                            ?? throw new InvalidOperationException("SKSurface.Create returned null on Graphite/Dawn.");
        Console.WriteLine(backendTex.Dimensions);

        //Draw to surface.Canvas
        using var paint = new SKPaint();
        paint.Color = Colors.Red;
        surface.Canvas.Clear(Colors.White);
        //surface.Canvas.DrawLine(10, 10, 100, 100, Paint.Shared(Colors.Red, PaintStyle.Stroke));
        surface.Canvas.DrawRect(Rect.FromLTWH(10, 10, 100, 100), paint);

        using (var recording = recorder.Snap() ?? throw new InvalidOperationException("Recorder.Snap() returned null."))
        {
            if (context.InsertRecording(recording) != SKGraphiteInsertStatus.Success)
                throw new InvalidOperationException("InsertRecording did not report Success.");
        }

        context.Submit(new SKGraphiteSubmitInfo { Sync = false });
    }

    public struct RunInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float PixelRatio { get; set; }
        public string? RoutePath { get; set; }
        public bool IsMacOS { get; set; }
    }
}