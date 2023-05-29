using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using PixUI.Demo;

namespace PixUI.Platform.Blazor;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        var host = builder.Build();
        BlazorApplication.JSRuntime = host.Services.GetRequiredService<IJSRuntime>();
        BlazorApplication.HttpClient = host.Services.GetService<HttpClient>()!;
        
        await host.RunAsync();
    }

    [JSInvokable]
    public static async Task Run(int glHandle, int width, int height, float ratio, string? routePath)
    {
        //初始化默认字体
        await using var fontDataStream =
            await BlazorApplication.HttpClient.GetStreamAsync("/fonts/MiSans-Regular.woff2");
        using var fontData = SKData.Create(fontDataStream);
        FontCollection.Instance.RegisterTypefaceToAsset(fontData!, FontCollection.DefaultFamilyName, false);
        
        BlazorApplication.Run(() => new DemoRoute(), glHandle, width, height, ratio, routePath);
    }
}