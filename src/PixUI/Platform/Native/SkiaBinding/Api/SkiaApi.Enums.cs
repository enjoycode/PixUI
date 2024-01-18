#if !__WEB__
using System;

namespace PixUI;

// gr_backend_t
internal enum GRBackendNative
{
    // OPENGL_GR_BACKEND = 0
    OpenGL = 0,

    // VULKAN_GR_BACKEND = 1
    Vulkan = 1,

    // METAL_GR_BACKEND = 2
    Metal = 2,

    // DIRECT3D_GR_BACKEND = 3
    Direct3D = 3,

    // DAWN_GR_BACKEND = 4
    Dawn = 4,
}

// gr_surfaceorigin_t
public enum GRSurfaceOrigin
{
    // TOP_LEFT_GR_SURFACE_ORIGIN = 0
    TopLeft = 0,

    // BOTTOM_LEFT_GR_SURFACE_ORIGIN = 1
    BottomLeft = 1,
}

// sk_bitmap_allocflags_t
[Flags]
public enum SKBitmapAllocFlags
{
    // NONE_SK_BITMAP_ALLOC_FLAGS = 0
    None = 0,

    // ZERO_PIXELS_SK_BITMAP_ALLOC_FLAGS = 1 << 0
    ZeroPixels = 1,
}

// sk_codec_result_t
public enum SKCodecResult
{
    // SUCCESS_SK_CODEC_RESULT = 0
    Success = 0,

    // INCOMPLETE_INPUT_SK_CODEC_RESULT = 1
    IncompleteInput = 1,

    // ERROR_IN_INPUT_SK_CODEC_RESULT = 2
    ErrorInInput = 2,

    // INVALID_CONVERSION_SK_CODEC_RESULT = 3
    InvalidConversion = 3,

    // INVALID_SCALE_SK_CODEC_RESULT = 4
    InvalidScale = 4,

    // INVALID_PARAMETERS_SK_CODEC_RESULT = 5
    InvalidParameters = 5,

    // INVALID_INPUT_SK_CODEC_RESULT = 6
    InvalidInput = 6,

    // COULD_NOT_REWIND_SK_CODEC_RESULT = 7
    CouldNotRewind = 7,

    // INTERNAL_ERROR_SK_CODEC_RESULT = 8
    InternalError = 8,

    // UNIMPLEMENTED_SK_CODEC_RESULT = 9
    Unimplemented = 9,
}

// sk_codec_scanline_order_t
public enum SKCodecScanlineOrder
{
    // TOP_DOWN_SK_CODEC_SCANLINE_ORDER = 0
    TopDown = 0,

    // BOTTOM_UP_SK_CODEC_SCANLINE_ORDER = 1
    BottomUp = 1,
}

// sk_codec_zero_initialized_t
public enum SKZeroInitialized
{
    // YES_SK_CODEC_ZERO_INITIALIZED = 0
    Yes = 0,

    // NO_SK_CODEC_ZERO_INITIALIZED = 1
    No = 1,
}

// sk_codecanimation_disposalmethod_t
public enum SKCodecAnimationDisposalMethod
{
    // KEEP_SK_CODEC_ANIMATION_DISPOSAL_METHOD = 1
    Keep = 1,

    // RESTORE_BG_COLOR_SK_CODEC_ANIMATION_DISPOSAL_METHOD = 2
    RestoreBackgroundColor = 2,

    // RESTORE_PREVIOUS_SK_CODEC_ANIMATION_DISPOSAL_METHOD = 3
    RestorePrevious = 3,
}

// sk_colortype_t
internal enum SKColorTypeNative
{
    // UNKNOWN_SK_COLORTYPE = 0
    Unknown = 0,

    // ALPHA_8_SK_COLORTYPE = 1
    Alpha8 = 1,

    // RGB_565_SK_COLORTYPE = 2
    Rgb565 = 2,

    // ARGB_4444_SK_COLORTYPE = 3
    Argb4444 = 3,

    // RGBA_8888_SK_COLORTYPE = 4
    Rgba8888 = 4,

    // RGB_888X_SK_COLORTYPE = 5
    Rgb888x = 5,

    // BGRA_8888_SK_COLORTYPE = 6
    Bgra8888 = 6,

    // RGBA_1010102_SK_COLORTYPE = 7
    Rgba1010102 = 7,

    // BGRA_1010102_SK_COLORTYPE = 8
    Bgra1010102 = 8,

    // RGB_101010X_SK_COLORTYPE = 9
    Rgb101010x = 9,

    // BGR_101010X_SK_COLORTYPE = 10
    Bgr101010x = 10,

    // GRAY_8_SK_COLORTYPE = 11
    Gray8 = 11,

    // RGBA_F16_NORM_SK_COLORTYPE = 12
    RgbaF16Norm = 12,

    // RGBA_F16_SK_COLORTYPE = 13
    RgbaF16 = 13,

    // RGBA_F32_SK_COLORTYPE = 14
    RgbaF32 = 14,

    // R8G8_UNORM_SK_COLORTYPE = 15
    R8g8Unorm = 15,

    // A16_FLOAT_SK_COLORTYPE = 16
    A16Float = 16,

    // R16G16_FLOAT_SK_COLORTYPE = 17
    R16g16Float = 17,

    // A16_UNORM_SK_COLORTYPE = 18
    A16Unorm = 18,

    // R16G16_UNORM_SK_COLORTYPE = 19
    R16g16Unorm = 19,

    // R16G16B16A16_UNORM_SK_COLORTYPE = 20
    R16g16b16a16Unorm = 20,
}

// sk_crop_rect_flags_t
[Flags]
public enum SKCropRectFlags
{
    // HAS_NONE_SK_CROP_RECT_FLAG = 0x00
    HasNone = 0,

    // HAS_LEFT_SK_CROP_RECT_FLAG = 0x01
    HasLeft = 1,

    // HAS_TOP_SK_CROP_RECT_FLAG = 0x02
    HasTop = 2,

    // HAS_WIDTH_SK_CROP_RECT_FLAG = 0x04
    HasWidth = 4,

    // HAS_HEIGHT_SK_CROP_RECT_FLAG = 0x08
    HasHeight = 8,

    // HAS_ALL_SK_CROP_RECT_FLAG = 0x0F
    HasAll = 15,
}

// sk_encodedorigin_t
public enum SKEncodedOrigin
{
    // TOP_LEFT_SK_ENCODED_ORIGIN = 1
    TopLeft = 1,

    // TOP_RIGHT_SK_ENCODED_ORIGIN = 2
    TopRight = 2,

    // BOTTOM_RIGHT_SK_ENCODED_ORIGIN = 3
    BottomRight = 3,

    // BOTTOM_LEFT_SK_ENCODED_ORIGIN = 4
    BottomLeft = 4,

    // LEFT_TOP_SK_ENCODED_ORIGIN = 5
    LeftTop = 5,

    // RIGHT_TOP_SK_ENCODED_ORIGIN = 6
    RightTop = 6,

    // RIGHT_BOTTOM_SK_ENCODED_ORIGIN = 7
    RightBottom = 7,

    // LEFT_BOTTOM_SK_ENCODED_ORIGIN = 8
    LeftBottom = 8,

    // DEFAULT_SK_ENCODED_ORIGIN = TOP_LEFT_SK_ENCODED_ORIGIN
    Default = 1,
}

// sk_filter_quality_t
public enum SKFilterQuality
{
    // NONE_SK_FILTER_QUALITY = 0
    None = 0,

    // LOW_SK_FILTER_QUALITY = 1
    Low = 1,

    // MEDIUM_SK_FILTER_QUALITY = 2
    Medium = 2,

    // HIGH_SK_FILTER_QUALITY = 3
    High = 3,
}

// sk_font_edging_t
public enum SKFontEdging
{
    // ALIAS_SK_FONT_EDGING = 0
    Alias = 0,

    // ANTIALIAS_SK_FONT_EDGING = 1
    Antialias = 1,

    // SUBPIXEL_ANTIALIAS_SK_FONT_EDGING = 2
    SubpixelAntialias = 2,
}

// sk_font_hinting_t
public enum SKFontHinting
{
    // NONE_SK_FONT_HINTING = 0
    None = 0,

    // SLIGHT_SK_FONT_HINTING = 1
    Slight = 1,

    // NORMAL_SK_FONT_HINTING = 2
    Normal = 2,

    // FULL_SK_FONT_HINTING = 3
    Full = 3,
}

// sk_highcontrastconfig_invertstyle_t
public enum SKHighContrastConfigInvertStyle
{
    // NO_INVERT_SK_HIGH_CONTRAST_CONFIG_INVERT_STYLE = 0
    NoInvert = 0,

    // INVERT_BRIGHTNESS_SK_HIGH_CONTRAST_CONFIG_INVERT_STYLE = 1
    InvertBrightness = 1,

    // INVERT_LIGHTNESS_SK_HIGH_CONTRAST_CONFIG_INVERT_STYLE = 2
    InvertLightness = 2,
}

// sk_image_caching_hint_t
public enum SKImageCachingHint
{
    // ALLOW_SK_IMAGE_CACHING_HINT = 0
    Allow = 0,

    // DISALLOW_SK_IMAGE_CACHING_HINT = 1
    Disallow = 1,
}

// sk_jpegencoder_alphaoption_t
public enum SKJpegEncoderAlphaOption
{
    // IGNORE_SK_JPEGENCODER_ALPHA_OPTION = 0
    Ignore = 0,

    // BLEND_ON_BLACK_SK_JPEGENCODER_ALPHA_OPTION = 1
    BlendOnBlack = 1,
}

// sk_jpegencoder_downsample_t
public enum SKJpegEncoderDownsample
{
    // DOWNSAMPLE_420_SK_JPEGENCODER_DOWNSAMPLE = 0
    Downsample420 = 0,

    // DOWNSAMPLE_422_SK_JPEGENCODER_DOWNSAMPLE = 1
    Downsample422 = 1,

    // DOWNSAMPLE_444_SK_JPEGENCODER_DOWNSAMPLE = 2
    Downsample444 = 2,
}

// sk_lattice_recttype_t
public enum SKLatticeRectType
{
    // DEFAULT_SK_LATTICE_RECT_TYPE = 0
    Default = 0,

    // TRANSPARENT_SK_LATTICE_RECT_TYPE = 1
    Transparent = 1,

    // FIXED_COLOR_SK_LATTICE_RECT_TYPE = 2
    FixedColor = 2,
}

// sk_mask_format_t
public enum SKMaskFormat
{
    // BW_SK_MASK_FORMAT = 0
    BW = 0,

    // A8_SK_MASK_FORMAT = 1
    A8 = 1,

    // THREE_D_SK_MASK_FORMAT = 2
    ThreeD = 2,

    // ARGB32_SK_MASK_FORMAT = 3
    Argb32 = 3,

    // LCD16_SK_MASK_FORMAT = 4
    Lcd16 = 4,

    // SDF_SK_MASK_FORMAT = 5
    Sdf = 5,
}

// sk_path_add_mode_t
public enum SKPathAddMode
{
    // APPEND_SK_PATH_ADD_MODE = 0
    Append = 0,

    // EXTEND_SK_PATH_ADD_MODE = 1
    Extend = 1,
}

// sk_path_arc_size_t
public enum SKPathArcSize
{
    // SMALL_SK_PATH_ARC_SIZE = 0
    Small = 0,

    // LARGE_SK_PATH_ARC_SIZE = 1
    Large = 1,
}

// sk_path_direction_t
public enum SKPathDirection
{
    // CW_SK_PATH_DIRECTION = 0
    Clockwise = 0,
    
    // CCW_SK_PATH_DIRECTION = 1
    CounterClockwise = 1,
}

// sk_path_effect_trim_mode_t
public enum SKTrimPathEffectMode
{
    // NORMAL_SK_PATH_EFFECT_TRIM_MODE = 0
    Normal = 0,

    // INVERTED_SK_PATH_EFFECT_TRIM_MODE = 1
    Inverted = 1,
}

// sk_path_filltype_t
public enum SKPathFillType
{
    // WINDING_SK_PATH_FILLTYPE = 0
    Winding = 0,

    // EVENODD_SK_PATH_FILLTYPE = 1
    EvenOdd = 1,

    // INVERSE_WINDING_SK_PATH_FILLTYPE = 2
    InverseWinding = 2,

    // INVERSE_EVENODD_SK_PATH_FILLTYPE = 3
    InverseEvenOdd = 3,
}

// sk_path_segment_mask_t
[Flags]
public enum SKPathSegmentMask
{
    // LINE_SK_PATH_SEGMENT_MASK = 1 << 0
    Line = 1,

    // QUAD_SK_PATH_SEGMENT_MASK = 1 << 1
    Quad = 2,

    // CONIC_SK_PATH_SEGMENT_MASK = 1 << 2
    Conic = 4,

    // CUBIC_SK_PATH_SEGMENT_MASK = 1 << 3
    Cubic = 8,
}

// sk_path_verb_t
public enum SKPathVerb
{
    // MOVE_SK_PATH_VERB = 0
    Move = 0,

    // LINE_SK_PATH_VERB = 1
    Line = 1,

    // QUAD_SK_PATH_VERB = 2
    Quad = 2,

    // CONIC_SK_PATH_VERB = 3
    Conic = 3,

    // CUBIC_SK_PATH_VERB = 4
    Cubic = 4,

    // CLOSE_SK_PATH_VERB = 5
    Close = 5,

    // DONE_SK_PATH_VERB = 6
    Done = 6,
}

// sk_pathmeasure_matrixflags_t
[Flags]
public enum SKPathMeasureMatrixFlags
{
    // GET_POSITION_SK_PATHMEASURE_MATRIXFLAGS = 0x01
    GetPosition = 1,

    // GET_TANGENT_SK_PATHMEASURE_MATRIXFLAGS = 0x02
    GetTangent = 2,

    // GET_POS_AND_TAN_SK_PATHMEASURE_MATRIXFLAGS = GET_POSITION_SK_PATHMEASURE_MATRIXFLAGS | GET_TANGENT_SK_PATHMEASURE_MATRIXFLAGS
    GetPositionAndTangent = 3,
}

// sk_pixelgeometry_t
public enum SKPixelGeometry
{
    // UNKNOWN_SK_PIXELGEOMETRY = 0
    Unknown = 0,

    // RGB_H_SK_PIXELGEOMETRY = 1
    RgbHorizontal = 1,

    // BGR_H_SK_PIXELGEOMETRY = 2
    BgrHorizontal = 2,

    // RGB_V_SK_PIXELGEOMETRY = 3
    RgbVertical = 3,

    // BGR_V_SK_PIXELGEOMETRY = 4
    BgrVertical = 4,
}

// sk_pngencoder_filterflags_t
[Flags]
public enum SKPngEncoderFilterFlags
{
    // ZERO_SK_PNGENCODER_FILTER_FLAGS = 0x00
    NoFilters = 0,

    // NONE_SK_PNGENCODER_FILTER_FLAGS = 0x08
    None = 8,

    // SUB_SK_PNGENCODER_FILTER_FLAGS = 0x10
    Sub = 16,

    // UP_SK_PNGENCODER_FILTER_FLAGS = 0x20
    Up = 32,

    // AVG_SK_PNGENCODER_FILTER_FLAGS = 0x40
    Avg = 64,

    // PAETH_SK_PNGENCODER_FILTER_FLAGS = 0x80
    Paeth = 128,

    // ALL_SK_PNGENCODER_FILTER_FLAGS = NONE_SK_PNGENCODER_FILTER_FLAGS | SUB_SK_PNGENCODER_FILTER_FLAGS | UP_SK_PNGENCODER_FILTER_FLAGS | AVG_SK_PNGENCODER_FILTER_FLAGS | PAETH_SK_PNGENCODER_FILTER_FLAGS
    AllFilters = 248,
}

// sk_point_mode_t
public enum SKPointMode
{
    // POINTS_SK_POINT_MODE = 0
    Points = 0,

    // LINES_SK_POINT_MODE = 1
    Lines = 1,

    // POLYGON_SK_POINT_MODE = 2
    Polygon = 2,
}

// sk_region_op_t
public enum SKRegionOperation
{
    // DIFFERENCE_SK_REGION_OP = 0
    Difference = 0,

    // INTERSECT_SK_REGION_OP = 1
    Intersect = 1,

    // UNION_SK_REGION_OP = 2
    Union = 2,

    // XOR_SK_REGION_OP = 3
    XOR = 3,

    // REVERSE_DIFFERENCE_SK_REGION_OP = 4
    ReverseDifference = 4,

    // REPLACE_SK_REGION_OP = 5
    Replace = 5,
}

// sk_rrect_corner_t
public enum SKRoundRectCorner
{
    // UPPER_LEFT_SK_RRECT_CORNER = 0
    UpperLeft = 0,

    // UPPER_RIGHT_SK_RRECT_CORNER = 1
    UpperRight = 1,

    // LOWER_RIGHT_SK_RRECT_CORNER = 2
    LowerRight = 2,

    // LOWER_LEFT_SK_RRECT_CORNER = 3
    LowerLeft = 3,
}

// sk_rrect_type_t
public enum SKRoundRectType
{
    // EMPTY_SK_RRECT_TYPE = 0
    Empty = 0,

    // RECT_SK_RRECT_TYPE = 1
    Rect = 1,

    // OVAL_SK_RRECT_TYPE = 2
    Oval = 2,

    // SIMPLE_SK_RRECT_TYPE = 3
    Simple = 3,

    // NINE_PATCH_SK_RRECT_TYPE = 4
    NinePatch = 4,

    // COMPLEX_SK_RRECT_TYPE = 5
    Complex = 5,
}

// sk_surfaceprops_flags_t
[Flags]
public enum SKSurfacePropsFlags
{
    // NONE_SK_SURFACE_PROPS_FLAGS = 0
    None = 0,

    // USE_DEVICE_INDEPENDENT_FONTS_SK_SURFACE_PROPS_FLAGS = 1 << 0
    UseDeviceIndependentFonts = 1,
}
    
// sk_text_encoding_t
public enum SKTextEncoding
{
    // UTF8_SK_TEXT_ENCODING = 0
    Utf8 = 0,

    // UTF16_SK_TEXT_ENCODING = 1
    Utf16 = 1,

    // UTF32_SK_TEXT_ENCODING = 2
    Utf32 = 2,

    // GLYPH_ID_SK_TEXT_ENCODING = 3
    GlyphId = 3,
}

// sk_vertices_vertex_mode_t
public enum SKVertexMode
{
    // TRIANGLES_SK_VERTICES_VERTEX_MODE = 0
    Triangles = 0,

    // TRIANGLE_STRIP_SK_VERTICES_VERTEX_MODE = 1
    TriangleStrip = 1,

    // TRIANGLE_FAN_SK_VERTICES_VERTEX_MODE = 2
    TriangleFan = 2,
}

// sk_webpencoder_compression_t
public enum SKWebpEncoderCompression
{
    // LOSSY_SK_WEBPENCODER_COMPTRESSION = 0
    Lossy = 0,

    // LOSSLESS_SK_WEBPENCODER_COMPTRESSION = 1
    Lossless = 1,
}
#endif