#if !__WEB__
using System;

namespace PixUI
{
    public enum GRBackend
    {
        Metal = 0,
        OpenGL = 1,
        Vulkan = 2,
        Dawn = 3,
        Direct3D = 4,
    }

    public static partial class SkiaExtensions
    {
        internal static GRBackendNative ToNative(this GRBackend backend) =>
            backend switch
            {
                GRBackend.Metal => GRBackendNative.Metal,
                GRBackend.OpenGL => GRBackendNative.OpenGL,
                GRBackend.Vulkan => GRBackendNative.Vulkan,
                GRBackend.Dawn => GRBackendNative.Dawn,
                GRBackend.Direct3D => GRBackendNative.Direct3D,
                _ => throw new ArgumentOutOfRangeException(nameof(backend)),
            };

        internal static GRBackend FromNative(this GRBackendNative backend) =>
            backend switch
            {
                GRBackendNative.Metal => GRBackend.Metal,
                GRBackendNative.OpenGL => GRBackend.OpenGL,
                GRBackendNative.Vulkan => GRBackend.Vulkan,
                GRBackendNative.Dawn => GRBackend.Dawn,
                GRBackendNative.Direct3D => GRBackend.Direct3D,
                _ => throw new ArgumentOutOfRangeException(nameof(backend)),
            };

        internal static SKColorTypeNative ToNative(this ColorType colorType) =>
            colorType switch
            {
                // ColorType.Unknown => SKColorTypeNative.Unknown,
                ColorType.Alpha8 => SKColorTypeNative.Alpha8,
                ColorType.Rgb565 => SKColorTypeNative.Rgb565,
                // ColorType.Argb4444 => SKColorTypeNative.Argb4444,
                ColorType.Rgba8888 => SKColorTypeNative.Rgba8888,
                // ColorType.Rgb888x => SKColorTypeNative.Rgb888x,
                ColorType.Bgra8888 => SKColorTypeNative.Bgra8888,
                ColorType.Rgba1010102 => SKColorTypeNative.Rgba1010102,
                ColorType.Rgb101010x => SKColorTypeNative.Rgb101010x,
                ColorType.Gray8 => SKColorTypeNative.Gray8,
                // ColorType.RgbaF16Clamped => SKColorTypeNative.RgbaF16Norm,
                ColorType.RgbaF16 => SKColorTypeNative.RgbaF16,
                ColorType.RgbaF32 => SKColorTypeNative.RgbaF32,
                // ColorType.Rg88 => SKColorTypeNative.R8g8Unorm,
                // ColorType.AlphaF16 => SKColorTypeNative.A16Float,
                // ColorType.RgF16 => SKColorTypeNative.R16g16Float,
                // ColorType.Alpha16 => SKColorTypeNative.A16Unorm,
                // ColorType.Rg1616 => SKColorTypeNative.R16g16Unorm,
                // ColorType.Rgba16161616 => SKColorTypeNative.R16g16b16a16Unorm,
                // ColorType.Bgra1010102 => SKColorTypeNative.Bgra1010102,
                // ColorType.Bgr101010x => SKColorTypeNative.Bgr101010x,
                _ => throw new ArgumentOutOfRangeException(nameof(colorType)),
            };

        internal static ColorType FromNative(this SKColorTypeNative colorType) =>
            colorType switch
            {
                // SKColorTypeNative.Unknown => ColorType.Unknown,
                SKColorTypeNative.Alpha8 => ColorType.Alpha8,
                SKColorTypeNative.Rgb565 => ColorType.Rgb565,
                // SKColorTypeNative.Argb4444 => ColorType.Argb4444,
                SKColorTypeNative.Rgba8888 => ColorType.Rgba8888,
                // SKColorTypeNative.Rgb888x => ColorType.Rgb888x,
                SKColorTypeNative.Bgra8888 => ColorType.Bgra8888,
                SKColorTypeNative.Rgba1010102 => ColorType.Rgba1010102,
                SKColorTypeNative.Rgb101010x => ColorType.Rgb101010x,
                SKColorTypeNative.Gray8 => ColorType.Gray8,
                // SKColorTypeNative.RgbaF16Norm => ColorType.RgbaF16Clamped,
                SKColorTypeNative.RgbaF16 => ColorType.RgbaF16,
                SKColorTypeNative.RgbaF32 => ColorType.RgbaF32,
                // SKColorTypeNative.R8g8Unorm => ColorType.Rg88,
                // SKColorTypeNative.A16Float => ColorType.AlphaF16,
                // SKColorTypeNative.R16g16Float => ColorType.RgF16,
                // SKColorTypeNative.A16Unorm => ColorType.Alpha16,
                // SKColorTypeNative.R16g16Unorm => ColorType.Rg1616,
                // SKColorTypeNative.R16g16b16a16Unorm => ColorType.Rgba16161616,
                // SKColorTypeNative.Bgra1010102 => ColorType.Bgra1010102,
                // SKColorTypeNative.Bgr101010x => ColorType.Bgr101010x,
                _ => throw new ArgumentOutOfRangeException(nameof(colorType)),
            };
    }
}
#endif