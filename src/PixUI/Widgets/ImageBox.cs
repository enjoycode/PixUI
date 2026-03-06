using System;

namespace PixUI;

public sealed class ImageBox : Widget
{
    /*TODO: LoadingBuilder, ErrorBuilder*/

    private readonly State<ImageSource> _imgSrc = null!;

    public required State<ImageSource> ImageSource
    {
        get => _imgSrc;
        init
        {
            _imgSrc = value;
            _imgSrc.AddListener(OnImageSourceChanged);
            _imgSrc.Value.OnLoaded = OnImageLoaded;
        }
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

    private void OnImageSourceChanged(State imgSrc)
    {
        _imgSrc.Value.OnLoaded = OnImageLoaded;
        RelayoutOnStateChanged(imgSrc);
    }

    private void OnImageLoaded() => RelayoutOnStateChanged(_imgSrc);

    public override void Layout(float availableWidth, float availableHeight)
    {
        var availableSize = CacheAndGetMaxSize(availableWidth, availableHeight);
        if (Width != null)
            availableSize.Width = Math.Min(availableWidth, Width.Value);
        if (Height != null)
            availableSize.Height = Math.Min(availableHeight, Height.Value);

        //TODO:根据加载状态及加载的图像大小来计算
        SetSize(availableSize.Width, availableSize.Height);
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
        canvas.DrawImage(img, Rect.FromLTWH(0, 0, img.Width, img.Height),
            Rect.FromLTWH(0, 0, W, H));
    }
}