#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.Image")]
    public sealed class Image : IDisposable
    {
        private Image() { }

        [TSTemplate("CanvasKit.MakeImageFromEncoded({1})")]
        public static Image? FromEncodedData(byte[] data) => null;

        [TSRename("width")] public int Width { get; } = 0;

        [TSRename("height")] public int Height { get; } = 0;

        [TSTemplate("{0}.getImageInfo().alphaType")]
        public AlphaType AlphaType { get; } = AlphaType.Opaque;

        [TSRename("delete")]
        public void Dispose() { }
    }
}

#endif