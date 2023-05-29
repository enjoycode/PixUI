#if __WEB__
using System;

namespace PixUI
{
    [TSType("PixUI.RRect")]
    public sealed class RRect : IDisposable
    {
        public static RRect FromRectAndCorner(Rect rect,
            Radius? topLeft = null, Radius? topRight = null,
            Radius? bottomLeft = null, Radius? bottomRight = null) =>
            new RRect();

        public static RRect FromRectAndRadius(Rect rect, float radiusX, float radiusY)
            => new RRect();

        public static RRect FromCopy(RRect from) => new RRect();

        private RRect() {}

        public void Deflate(float dx, float dy) { }

        public void Inflate(float dx, float dy) { }

        public void Shift(float dx, float dy) { }

        [TSIgnoreMethodInvoke]
        public void Dispose() { }
    }
}
#endif