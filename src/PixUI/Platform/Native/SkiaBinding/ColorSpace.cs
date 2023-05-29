#if !__WEB__
using System;

namespace PixUI
{
    public unsafe class ColorSpace : SKObject, ISKNonVirtualReferenceCounted
    {
        public static readonly ColorSpace SRGB;
        // private static readonly ColorSpace srgbLinear;

        static ColorSpace()
        {
            SRGB = new SKColorSpaceStatic(SkiaApi.sk_colorspace_new_srgb());
            // srgbLinear = new SKColorSpaceStatic(SkiaApi.sk_colorspace_new_srgb_linear());
        }

        internal static void EnsureStaticInstanceAreInitialized()
        {
            // IMPORTANT: do not remove to ensure that the static instances
            //            are initialized before any access is made to them
        }

        internal ColorSpace(IntPtr handle, bool owns) : base(handle, owns) { }

        void ISKNonVirtualReferenceCounted.ReferenceNative() =>
            SkiaApi.sk_colorspace_ref(Handle);

        void ISKNonVirtualReferenceCounted.UnreferenceNative() =>
            SkiaApi.sk_colorspace_unref(Handle);

        // properties

        // public bool GammaIsCloseToSrgb => SkiaApi.sk_colorspace_gamma_close_to_srgb(Handle);
        //
        // public bool GammaIsLinear => SkiaApi.sk_colorspace_gamma_is_linear(Handle);
        //
        // public bool IsSrgb => SkiaApi.sk_colorspace_is_srgb(Handle);
        //
        // public bool IsNumericalTransferFunction => GetNumericalTransferFunction(out _);

        public static bool Equal(ColorSpace left, ColorSpace right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return SkiaApi.sk_colorspace_equals(left.Handle, right.Handle);
        }

        // CreateIcc

        // public static ColorSpace? CreateIcc(IntPtr input, long length) =>
        //     CreateIcc(SKColorSpaceIccProfile.Create(input, length));
        //
        // public static ColorSpace? CreateIcc(byte[] input, long length)
        // {
        //     if (input == null)
        //         throw new ArgumentNullException(nameof(input));
        //
        //     fixed (byte* i = input)
        //     {
        //         return CreateIcc(SKColorSpaceIccProfile.Create((IntPtr)i, length));
        //     }
        // }
        //
        // public static ColorSpace? CreateIcc(byte[] input) =>
        //     CreateIcc(input.AsSpan());
        //
        // public static ColorSpace? CreateIcc(ReadOnlySpan<byte> input) =>
        //     CreateIcc(SKColorSpaceIccProfile.Create(input));
        //
        // public static ColorSpace? CreateIcc(SKData input) =>
        //     CreateIcc(SKColorSpaceIccProfile.Create(input));
        //
        // public static ColorSpace? CreateIcc(SKColorSpaceIccProfile? profile)
        // {
        //     if (profile == null)
        //         throw new ArgumentNullException(nameof(profile));
        //
        //     return Referenced(GetObject(SkiaApi.sk_colorspace_new_icc(profile.Handle)), profile);
        // }

        // CreateRgb

        // public static ColorSpace CreateRgb(SKColorSpaceTransferFn transferFn,
        //     SKColorSpaceXyz toXyzD50) =>
        //     GetObject(SkiaApi.sk_colorspace_new_rgb(&transferFn, &toXyzD50));
        //
        // // GetNumericalTransferFunction
        //
        // public SKColorSpaceTransferFn GetNumericalTransferFunction() =>
        //     GetNumericalTransferFunction(out var fn) ? fn : SKColorSpaceTransferFn.Empty;
        //
        // public bool GetNumericalTransferFunction(out SKColorSpaceTransferFn fn)
        // {
        //     fixed (SKColorSpaceTransferFn* f = &fn)
        //     {
        //         return SkiaApi.sk_colorspace_is_numerical_transfer_fn(Handle, f);
        //     }
        // }

        // ToProfile

        // public SKColorSpaceIccProfile ToProfile()
        // {
        //     var profile = new SKColorSpaceIccProfile();
        //     SkiaApi.sk_colorspace_to_profile(Handle, profile.Handle);
        //     return profile;
        // }

        // ToColorSpaceXyz

        // public bool ToColorSpaceXyz(out SKColorSpaceXyz toXyzD50)
        // {
        //     fixed (SKColorSpaceXyz* xyz = &toXyzD50)
        //     {
        //         return SkiaApi.sk_colorspace_to_xyzd50(Handle, xyz);
        //     }
        // }

        // public SKColorSpaceXyz ToColorSpaceXyz() =>
        //     ToColorSpaceXyz(out var toXYZ) ? toXYZ : SKColorSpaceXyz.Empty;

        // To*Gamma

        // public ColorSpace ToLinearGamma() =>
        //     GetObject(SkiaApi.sk_colorspace_make_linear_gamma(Handle));
        //
        // public ColorSpace ToSrgbGamma() =>
        //     GetObject(SkiaApi.sk_colorspace_make_srgb_gamma(Handle));

        internal static ColorSpace GetObject(IntPtr handle, bool owns = true,
            bool unrefExisting = true) =>
            GetOrAddObject(handle, owns, unrefExisting, (h, o) => new ColorSpace(h, o));

        private sealed class SKColorSpaceStatic : ColorSpace
        {
            internal SKColorSpaceStatic(IntPtr x) : base(x, false) { }

            protected override void Dispose(bool disposing) { }
        }
    }
}
#endif