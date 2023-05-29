#if !__WEB__
using System.Runtime.InteropServices;

namespace PixUI
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Shadow
    {
        public readonly Color Color;
        public readonly Point Offset;
        public readonly double BlurRadius;
    }
}
#endif