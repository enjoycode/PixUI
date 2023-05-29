#if !__WEB__
using System;

namespace PixUI
{
    internal partial struct SKImageInfoNative
    {
        public static void UpdateNative(ref ImageInfo managed, ref SKImageInfoNative native)
        {
            native.colorspace = managed.ColorSpace?.Handle ?? IntPtr.Zero;
            native.width = managed.Width;
            native.height = managed.Height;
            native.colorType = managed.ColorType.ToNative();
            native.alphaType = managed.AlphaType;
        }

        public static SKImageInfoNative FromManaged(ref ImageInfo managed) =>
            new SKImageInfoNative
            {
                colorspace = managed.ColorSpace?.Handle ?? IntPtr.Zero,
                width = managed.Width,
                height = managed.Height,
                colorType = managed.ColorType.ToNative(),
                alphaType = managed.AlphaType,
            };

        public static ImageInfo ToManaged(ref SKImageInfoNative native) =>
            new ImageInfo
            {
                ColorSpace = ColorSpace.GetObject(native.colorspace),
                Width = native.width,
                Height = native.height,
                ColorType = native.colorType.FromNative(),
                AlphaType = native.alphaType,
            };
    }

    public unsafe struct ImageInfo : IEquatable<ImageInfo>
    {
        // public static readonly ImageInfo Empty;
        private static readonly ColorType PlatformColorType;
        // public static readonly int PlatformColorAlphaShift;
        // public static readonly int PlatformColorRedShift;
        // public static readonly int PlatformColorGreenShift;
        // public static readonly int PlatformColorBlueShift;

        static ImageInfo()
        {
            PlatformColorType = PixUI.ColorType.Bgra8888; //SkiaApi.sk_colortype_get_default_8888().FromNative();

            //fixed (int* a = &PlatformColorAlphaShift)
            //fixed (int* r = &PlatformColorRedShift)
            //fixed (int* g = &PlatformColorGreenShift)
            //fixed (int* b = &PlatformColorBlueShift)
            //{
            //    SkiaApi.sk_color_get_bit_shift(a, r, g, b);
            //}
        }

        public ImageInfo() { }

        public int Width { get; set; } = 0;

        public int Height { get; set; } = 0;

        public ColorType ColorType { get; set; } = PlatformColorType;

        public AlphaType AlphaType { get; set; } = AlphaType.Premul;

        public ColorSpace? ColorSpace { get; set; } = null;

        public readonly int BytesPerPixel => ColorType.GetBytesPerPixel();

        // public readonly int BitsPerPixel => BytesPerPixel * 8;
        
        public readonly int BytesSize => Width * Height * BytesPerPixel;
        
        // public readonly long BytesSize64 => (long)Width * (long)Height * (long)BytesPerPixel;
        //
        // public readonly int RowBytes => Width * BytesPerPixel;
        //
        // public readonly long RowBytes64 => (long)Width * (long)BytesPerPixel;
        //
        // public readonly bool IsEmpty => Width <= 0 || Height <= 0;
        //
        // public readonly bool IsOpaque => AlphaType == AlphaType.Opaque;

        // public readonly SKSizeI Size => new SKSizeI (Width, Height);
        //
        // public readonly SKRectI Rect => SKRectI.Create (Width, Height);

        public readonly bool Equals(ImageInfo obj) =>
            ColorSpace == obj.ColorSpace &&
            Width == obj.Width &&
            Height == obj.Height &&
            ColorType == obj.ColorType &&
            AlphaType == obj.AlphaType;

        public readonly override bool Equals(object obj) => obj is ImageInfo f && Equals(f);

        public static bool operator ==(ImageInfo left, ImageInfo right) => left.Equals(right);

        public static bool operator !=(ImageInfo left, ImageInfo right) => !left.Equals(right);

        public readonly override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(ColorSpace);
            hash.Add(Width);
            hash.Add(Height);
            hash.Add(ColorType);
            hash.Add(AlphaType);
            return hash.ToHashCode();
        }
    }
}

#endif