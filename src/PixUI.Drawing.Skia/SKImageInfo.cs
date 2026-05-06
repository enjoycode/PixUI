namespace PixUI.Drawing.Skia;

internal partial struct SKImageInfoNative
{
    // public static void UpdateNative(ref ImageInfo managed, ref SKImageInfoNative native)
    // {
    //     native.colorspace = managed.ColorSpace?.Handle ?? IntPtr.Zero;
    //     native.width = managed.Width;
    //     native.height = managed.Height;
    //     native.colorType = managed.ColorType.ToNative();
    //     native.alphaType = managed.AlphaType;
    // }

    public static SKImageInfoNative FromManaged(ref ImageInfo managed) => new SKImageInfoNative
    {
        //TODO: colorspace = (managed.ColorSpace as ColorSpace)?.Handle ?? IntPtr.Zero,
        colorspace = IntPtr.Zero,
        width = managed.Width,
        height = managed.Height,
        colorType = managed.ColorType.ToNative(),
        alphaType = managed.AlphaType,
    };

    // public static ImageInfo ToManaged(ref SKImageInfoNative native) => new ImageInfo
    // {
    //     ColorSpace = ColorSpace.GetObject(native.colorspace),
    //     Width = native.width,
    //     Height = native.height,
    //     ColorType = native.colorType.FromNative(),
    //     AlphaType = native.alphaType,
    // };
}