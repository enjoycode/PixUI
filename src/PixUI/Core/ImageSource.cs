using System;
using System.Net.Http;

namespace PixUI;

/// <summary>
/// 图像来源
/// </summary>
public sealed class ImageSource
{
    private ImageSource() { }

    /// <summary>
    /// 是否正在加载中
    /// </summary>
    public bool Loading { get; private set; } = true;

    /// <summary>
    /// 加载完的图像，如果错误返回null
    /// </summary>
    public Image? Image { get; private set; }

    /// <summary>
    /// 是否已加载，仅用于异步加载后回调
    /// </summary>
    public Action? OnLoaded { get; set; }

    public static ImageSource FromEncodedData(byte[] data)
    {
        var imgSrc = new ImageSource
        {
            Loading = false,
            Image = Image.FromEncodedData(data)
        };
        return imgSrc;
    }

    public static ImageSource FromNetwork(string url)
    {
        var imgSrc = new ImageSource();
        LoadImageFromUrl(imgSrc, url);
        return imgSrc;
    }

    private static async void LoadImageFromUrl(ImageSource imageSource, string url)
    {
        try
        {
            using var httpClient = new HttpClient();
            await using var response = await httpClient.GetStreamAsync(url);
            imageSource.Image = Image.FromEncodedData(response);
            imageSource.Loading = false;
            imageSource.OnLoaded?.Invoke();
        }
        catch (Exception)
        {
            imageSource.Loading = false;
            imageSource.OnLoaded?.Invoke();
        }
    }
}