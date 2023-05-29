#if !__WEB__
using System;

namespace PixUI
{
    public sealed unsafe class ColorFilter : SKObject, ISKReferenceCounted
    {
        private ColorFilter(IntPtr handle, bool owns) : base (handle, owns) { }
        
        internal static ColorFilter? GetObject (IntPtr handle) =>
            GetOrAddObject (handle, (h, o) => new ColorFilter (h, o))!;
        
        //TODO: others to fit CanvasKit
        
        public static ColorFilter? CreateBlendMode(Color c, BlendMode mode) 
            => GetObject (SkiaApi.sk_colorfilter_new_mode((uint)c, mode));
        
        public static ColorFilter? CreateCompose(ColorFilter outer, ColorFilter inner)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            return GetObject (SkiaApi.sk_colorfilter_new_compose(outer.Handle, inner.Handle));
        }
        
        public static ColorFilter? CreateLumaColor() => GetObject (SkiaApi.sk_colorfilter_new_luma_color());
        
        public static ColorFilter? CreateColorMatrix(float[] matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            if (matrix.Length != 20)
                throw new ArgumentException("Matrix must have a length of 20.", nameof(matrix));
            fixed (float* m = matrix) {
                return GetObject (SkiaApi.sk_colorfilter_new_color_matrix (m));
            }
        }
    }
}

#endif