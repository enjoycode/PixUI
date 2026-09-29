using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class SkiaApi
{
    // void sk_codec_destroy(sk_codec_t* codec)

    [LibraryImport(SKIA)]
    internal static partial void sk_codec_destroy(sk_codec_t codec);


    // sk_encoded_image_format_t sk_codec_get_encoded_format(sk_codec_t* codec)

    [LibraryImport(SKIA)]
    internal static partial EncodedImageFormat sk_codec_get_encoded_format(sk_codec_t codec);


    // int sk_codec_get_frame_count(sk_codec_t* codec)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_codec_get_frame_count(sk_codec_t codec);


    // void sk_codec_get_frame_info(sk_codec_t* codec, sk_codec_frameinfo_t* frameInfo)

    [LibraryImport(SKIA)]
    internal static partial void sk_codec_get_frame_info(sk_codec_t codec, SKCodecFrameInfo* frameInfo);


    // bool sk_codec_get_frame_info_for_index(sk_codec_t* codec, int index, sk_codec_frameinfo_t* frameInfo)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_codec_get_frame_info_for_index(sk_codec_t codec, Int32 index,
        SKCodecFrameInfo* frameInfo);


    // void sk_codec_get_info(sk_codec_t* codec, sk_imageinfo_t* info)

    [LibraryImport(SKIA)]
    internal static partial void sk_codec_get_info(sk_codec_t codec, SKImageInfoNative* info);


    // sk_encodedorigin_t sk_codec_get_origin(sk_codec_t* codec)

    [LibraryImport(SKIA)]
    internal static partial SKEncodedOrigin sk_codec_get_origin(sk_codec_t codec);


    // sk_codec_result_t sk_codec_get_pixels(sk_codec_t* codec, const sk_imageinfo_t* info, void* pixels, size_t rowBytes, const sk_codec_options_t* options)

    [LibraryImport(SKIA)]
    internal static partial SKCodecResult sk_codec_get_pixels(sk_codec_t codec, SKImageInfoNative* info,
        void* pixels, /* size_t */ IntPtr rowBytes, SKCodecOptionsInternal* options);


    // int sk_codec_get_repetition_count(sk_codec_t* codec)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_codec_get_repetition_count(sk_codec_t codec);


    // void sk_codec_get_scaled_dimensions(sk_codec_t* codec, float desiredScale, sk_isize_t* dimensions)

    [LibraryImport(SKIA)]
    internal static partial void
        sk_codec_get_scaled_dimensions(sk_codec_t codec, Single desiredScale, SizeI* dimensions);


    // sk_codec_scanline_order_t sk_codec_get_scanline_order(sk_codec_t* codec)

    [LibraryImport(SKIA)]
    internal static partial SKCodecScanlineOrder sk_codec_get_scanline_order(sk_codec_t codec);


    // int sk_codec_get_scanlines(sk_codec_t* codec, void* dst, int countLines, size_t rowBytes)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_codec_get_scanlines(sk_codec_t codec, void* dst, Int32 countLines, /* size_t */
        IntPtr rowBytes);


    // bool sk_codec_get_valid_subset(sk_codec_t* codec, sk_irect_t* desiredSubset)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_codec_get_valid_subset(sk_codec_t codec, RectI* desiredSubset);


    // sk_codec_result_t sk_codec_incremental_decode(sk_codec_t* codec, int* rowsDecoded)

    [LibraryImport(SKIA)]
    internal static partial SKCodecResult sk_codec_incremental_decode(sk_codec_t codec, Int32* rowsDecoded);


    // size_t sk_codec_min_buffered_bytes_needed()

    [LibraryImport(SKIA)]
    internal static partial /* size_t */ IntPtr sk_codec_min_buffered_bytes_needed();


    // sk_codec_t* sk_codec_new_from_data(sk_data_t* data)

    [LibraryImport(SKIA)]
    internal static partial sk_codec_t sk_codec_new_from_data(sk_data_t data);


    // sk_codec_t* sk_codec_new_from_stream(sk_stream_t* stream, sk_codec_result_t* result)

    [LibraryImport(SKIA)]
    internal static partial sk_codec_t sk_codec_new_from_stream(sk_stream_t stream, SKCodecResult* result);


    // int sk_codec_next_scanline(sk_codec_t* codec)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_codec_next_scanline(sk_codec_t codec);


    // int sk_codec_output_scanline(sk_codec_t* codec, int inputScanline)

    [LibraryImport(SKIA)]
    internal static partial Int32 sk_codec_output_scanline(sk_codec_t codec, Int32 inputScanline);


    // bool sk_codec_skip_scanlines(sk_codec_t* codec, int countLines)

    [LibraryImport(SKIA)]
    [return: MarshalAs(UnmanagedType.I1)]
    internal static partial bool sk_codec_skip_scanlines(sk_codec_t codec, Int32 countLines);


    // sk_codec_result_t sk_codec_start_incremental_decode(sk_codec_t* codec, const sk_imageinfo_t* info, void* pixels, size_t rowBytes, const sk_codec_options_t* options)

    [LibraryImport(SKIA)]
    internal static partial SKCodecResult sk_codec_start_incremental_decode(sk_codec_t codec, SKImageInfoNative* info,
        void* pixels, /* size_t */ IntPtr rowBytes, SKCodecOptionsInternal* options);


    // sk_codec_result_t sk_codec_start_scanline_decode(sk_codec_t* codec, const sk_imageinfo_t* info, const sk_codec_options_t* options)

    [LibraryImport(SKIA)]
    internal static partial SKCodecResult sk_codec_start_scanline_decode(sk_codec_t codec, SKImageInfoNative* info,
        SKCodecOptionsInternal* options);
}