using System;

namespace PixUI;

/// <summary>
/// 图像来源
/// </summary>
public sealed class ImageSource
{
    /// <summary>
    /// 是否正在加载中
    /// </summary>
    public bool Loading { get; private set; } = true;

    /// <summary>
    /// 加载完的图像，如果错误返回null
    /// </summary>
    public Image? Image { get; private set; }

    private ImageSource() { }

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
        throw new NotImplementedException();
    }
}