using System;

namespace PixUI;

public sealed class ImageBox : Widget
{
    private readonly State<ImageSource> _imgSrc;

    public ImageBox(State<ImageSource> imgSrc /*TODO: LoadingBuilder, ErrorBuilder*/)
    {
        _imgSrc = Bind(imgSrc, BindingOptions.AffectsLayout);
    }

    public override bool IsOpaque
    {
        get
        {
            //根据图像是否透明来返回
            if (_imgSrc.Value.Loading && _imgSrc.Value.Image == null) return false;
            return _imgSrc.Value.Image!.AlphaType == AlphaType.Opaque;
        }
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        //TODO:根据是否指定大小及加载状态及加载的图像大小来计算
        SetSize(width, height);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (_imgSrc.Value.Loading)
        {
            //TODO: paint loading widget
            return;
        }

        var img = _imgSrc.Value.Image;
        if (img == null)
        {
            //TODO: paint error widget
            return;
        }

        //TODO:暂简单绘制，应根据大小及fit
        // @ts-ignore //TODO:尝试忽略web端canvaskit的paint参数
        canvas.DrawImage(img, Rect.FromLTWH(0, 0, img.Width, img.Height),
            Rect.FromLTWH(0, 0, W, H));
    }
}