#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.ImageInfo")]
    public struct ImageInfo
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public ColorType ColorType { get; set; }

        public AlphaType AlphaType { get; set; }

        public ColorSpace? ColorSpace { get; set; }
    }
}

#endif