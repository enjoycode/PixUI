using AppKit;
using CoreAnimation;
using Foundation;
using Metal;

namespace PixUI.Platform.Mac;

public sealed class MacMetalWindowContext : MetalWindowContext
{
    private readonly NSView _mainView;

    public MacMetalWindowContext(NSView mainView, NativeWindow nativeWindow, DisplayParams displayParams) 
        : base(nativeWindow, displayParams)
    {
        _mainView = mainView;

        // any config code here (particularly for msaa)?
        InitializeContext();
    }

    public override void Resize(int width, int height)
    {
        var backingScaleFactor = NativeWindow.ScaleFactor;

        var backingSize = _mainView.Bounds.Size;
        backingSize.Width *= backingScaleFactor;
        backingSize.Height *= backingScaleFactor;

        MetalLayer!.DrawableSize = backingSize;
        MetalLayer.ContentsScale = backingScaleFactor;

        Width = (int)backingSize.Width;
        Height = (int)backingSize.Height;

        //Reset cached offscreen canvas
        OffscreenCanvas?.Dispose();
        OffscreenCanvas?.Surface.Dispose();
        OffscreenCanvas = null;
    }

    protected override bool OnInitializeContext()
    {
        MetalLayer = new CAMetalLayer();
        MetalLayer.Device = Device;
        MetalLayer.PixelFormat = MTLPixelFormat.BGRA8Unorm;

        // resize ignores the passed values and uses the fMainView directly.
        Resize(0, 0);

        var useVsync = !DisplayParams.DisableVsync;
        //注意: MetalLayer.FramebufferOnly = false, 否则BlendModel不正确
        //参考https://groups.google.com/g/skia-discuss/c/FTO0NSR_09Q/m/3peivvP4AgAJ?pli=1
        MetalLayer.FramebufferOnly = false;
        MetalLayer.DisplaySyncEnabled = useVsync; // TODO: need solution for 10.12 or lower
        MetalLayer.LayoutManager = CAConstraintLayoutManager.LayoutManager;
        MetalLayer.AutoresizingMask = CAAutoresizingMask.HeightSizable | CAAutoresizingMask.WidthSizable;
        MetalLayer.ContentsGravity = CALayer.GravityTopLeft;
        MetalLayer.MagnificationFilter = CALayer.FilterNearest;
        MetalLayer.MaximumDrawableCount = 2; //2 or 3, default is 3
        MetalLayer.ColorSpace = _mainView.Window.ColorSpace.ColorSpace;

        _mainView.Layer = MetalLayer;
        _mainView.WantsLayer = true;

        return true;
    }

    protected override void OnDestroyContext()
    {
        _mainView.Layer = null;
        _mainView.WantsLayer = false;
    }
}