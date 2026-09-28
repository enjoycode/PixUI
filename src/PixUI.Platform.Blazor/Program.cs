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
        WebApplication.JSRuntime = host.Services.GetRequiredService<IJSRuntime>();
        WebApplication.HttpClient = host.Services.GetService<HttpClient>()!;

        //调用js获取启动参数
        var jsRuntime = ((IJSInProcessRuntime)WebApplication.JSRuntime);
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
        const string fontUrl = "/fonts/MiSans-Regular.woff2";
        await using var fontDataStream = await WebApplication.HttpClient.GetStreamAsync(fontUrl);
        //因fontDataStream不支持同步复制(DotNet10)，所以先复制至MemoryStream
        using var ms = new MemoryStream();
        await fontDataStream.CopyToAsync(ms);
        ms.Position = 0;
        FontCollection.RegisterTypeface(ms, FontCollection.DefaultFamilyName, false);

        //开始执行Blazor应用
        WebApplication.Run(() => new DemoRoute(), runInfo);
    }
}