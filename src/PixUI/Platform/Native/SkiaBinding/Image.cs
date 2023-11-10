#if !__WEB__
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PixUI
{
    public sealed class Image : SKObject, ISKReferenceCounted
    {
        private Image(IntPtr x, bool owns) : base(x, owns) { }

        public int Width => SkiaApi.sk_image_get_width(Handle);

        public int Height => SkiaApi.sk_image_get_height(Handle);

        public uint UniqueId => SkiaApi.sk_image_get_unique_id(Handle);

        public AlphaType AlphaType => SkiaApi.sk_image_get_alpha_type(Handle);

        public bool IsTextureBacked => SkiaApi.sk_image_is_texture_backed(Handle);
        public bool IsLazyGenerated => SkiaApi.sk_image_is_lazy_generated(Handle);

        internal static Image? GetObject(IntPtr handle) =>
            GetOrAddObject(handle, (h, o) => new Image(h, o));

        public static Image? FromEncodedData(SKData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var handle = SkiaApi.sk_image_new_from_encoded(data.Handle);
            return GetObject(handle);
        }

        public static Image? FromEncodedData(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length == 0)
                throw new ArgumentException("The data buffer was empty.");

            using var skdata = SKData.CreateCopy(data);
            return FromEncodedData(skdata)!;
        }

        public static Image? FromEncodedData(Stream data)
        {
            using var skdata = SKData.Create(data);
            return skdata == null ? null : FromEncodedData(skdata);
        }

        public static unsafe Image FromPicture(Picture picture, SizeI dimensions) =>
            FromPicture(picture, dimensions, null, null);

        private static unsafe Image FromPicture(Picture picture, SizeI dimensions, Matrix3* matrix, Paint? paint)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var p = paint?.Handle ?? IntPtr.Zero;
            return GetObject(SkiaApi.sk_image_new_from_picture(picture.Handle, &dimensions, matrix, p))!;
        }

        public SKData Encode(EncodedImageFormat format, int quality)
        {
            return SKData.GetObject(SkiaApi.sk_image_encode_specific(Handle, format, quality));
        }

        #region ====ToShader====

        public Shader? ToShader() => ToShader(TileMode.Clamp, TileMode.Clamp);

        public unsafe Shader? ToShader(TileMode tileX, TileMode tileY) =>
            Shader.GetObject(SkiaApi.sk_image_make_shader(Handle, tileX, tileY, null));

        #endregion
    }
}
#endif