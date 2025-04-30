using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using PixUI.Demo;

namespace PixUI.Platform.Blazor;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        var host = builder.Build();
        BlazorApplication.JSRuntime = host.Services.GetRequiredService<IJSRuntime>();
        BlazorApplication.HttpClient = host.Services.GetService<HttpClient>()!;

        //调用js获取启动参数
        var jsRuntime = ((IJSInProcessRuntime)BlazorApplication.JSRuntime);
        var runInfo = jsRuntime.Invoke<RunInfo>("PixUI.BeforeRunApp");
        await Run(runInfo.GLHandle, runInfo.Width, runInfo.Height, runInfo.PixelRatio, runInfo.RoutePath,
            runInfo.IsMacOS);
        jsRuntime.InvokeVoid("PixUI.BindEvents");

        await host.RunAsync();
    }

    private static async Task Run(int glHandle, int width, int height, float ratio, string? routePath, bool isMacOS)
    {
        //初始化默认字体
        await using var fontDataStream =
            await BlazorApplication.HttpClient.GetStreamAsync("/fonts/MiSans-Regular.woff2");
        using var fontData = SKData.Create(fontDataStream);
        FontCollection.Instance.RegisterTypeface(fontData!, FontCollection.DefaultFamilyName, false);

        BlazorApplication.Run(() => new DemoRoute(), glHandle, width, height, ratio, routePath, isMacOS);
    }

    public struct RunInfo
    {
        public int GLHandle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float PixelRatio { get; set; }
        public string? RoutePath { get; set; }
        public bool IsMacOS { get; set; }
    }
}