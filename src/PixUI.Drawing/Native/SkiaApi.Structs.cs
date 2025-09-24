#if !__WEB__
using System;
using System.Runtime.InteropServices;

namespace PixUI;

// gr_context_options_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct GRContextOptionsNative : IEquatable<GRContextOptionsNative>
{
    // public bool fAvoidStencilBuffers
    public Byte fAvoidStencilBuffers;

    // public int fRuntimeProgramCacheSize
    public Int32 fRuntimeProgramCacheSize;

    // public size_t fGlyphCacheTextureMaximumBytes
    public /* size_t */ IntPtr fGlyphCacheTextureMaximumBytes;

    // public bool fAllowPathMaskCaching
    public Byte fAllowPathMaskCaching;

    // public bool fDoManualMipmapping
    public Byte fDoManualMipmapping;

    // public int fBufferMapThreshold
    public Int32 fBufferMapThreshold;

    public readonly bool Equals(GRContextOptionsNative obj) =>
        fAvoidStencilBuffers == obj.fAvoidStencilBuffers &&
        fRuntimeProgramCacheSize == obj.fRuntimeProgramCacheSize &&
        fGlyphCacheTextureMaximumBytes == obj.fGlyphCacheTextureMaximumBytes &&
        fAllowPathMaskCaching == obj.fAllowPathMaskCaching &&
        fDoManualMipmapping == obj.fDoManualMipmapping &&
        fBufferMapThreshold == obj.fBufferMapThreshold;

    public readonly override bool Equals(object obj) =>
        obj is GRContextOptionsNative f && Equals(f);

    public static bool operator ==(GRContextOptionsNative left,
        GRContextOptionsNative right) =>
        left.Equals(right);

    public static bool operator !=(GRContextOptionsNative left,
        GRContextOptionsNative right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fAvoidStencilBuffers);
        hash.Add(fRuntimeProgramCacheSize);
        hash.Add(fGlyphCacheTextureMaximumBytes);
        hash.Add(fAllowPathMaskCaching);
        hash.Add(fDoManualMipmapping);
        hash.Add(fBufferMapThreshold);
        return hash.ToHashCode();
    }
}

// gr_gl_framebufferinfo_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct GRGlFramebufferInfo : IEquatable<GRGlFramebufferInfo>
{
    // public unsigned int fFBOID
    private UInt32 fFBOID;

    public UInt32 FramebufferObjectId
    {
        readonly get => fFBOID;
        set => fFBOID = value;
    }

    // public unsigned int fFormat
    private UInt32 fFormat;

    public UInt32 Format
    {
        readonly get => fFormat;
        set => fFormat = value;
    }

    public readonly bool Equals(GRGlFramebufferInfo obj) =>
        fFBOID == obj.fFBOID && fFormat == obj.fFormat;

    public readonly override bool Equals(object obj) =>
        obj is GRGlFramebufferInfo f && Equals(f);

    public static bool operator ==(GRGlFramebufferInfo left, GRGlFramebufferInfo right) =>
        left.Equals(right);

    public static bool operator !=(GRGlFramebufferInfo left, GRGlFramebufferInfo right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fFBOID);
        hash.Add(fFormat);
        return hash.ToHashCode();
    }
}

// gr_gl_textureinfo_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct GRGlTextureInfo : IEquatable<GRGlTextureInfo>
{
    // public unsigned int fTarget
    private UInt32 fTarget;

    public UInt32 Target
    {
        readonly get => fTarget;
        set => fTarget = value;
    }

    // public unsigned int fID
    private UInt32 fID;

    public UInt32 Id
    {
        readonly get => fID;
        set => fID = value;
    }

    // public unsigned int fFormat
    private UInt32 fFormat;

    public UInt32 Format
    {
        readonly get => fFormat;
        set => fFormat = value;
    }

    public readonly bool Equals(GRGlTextureInfo obj) =>
        fTarget == obj.fTarget && fID == obj.fID && fFormat == obj.fFormat;

    public readonly override bool Equals(object obj) =>
        obj is GRGlTextureInfo f && Equals(f);

    public static bool operator ==(GRGlTextureInfo left, GRGlTextureInfo right) =>
        left.Equals(right);

    public static bool operator !=(GRGlTextureInfo left, GRGlTextureInfo right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fTarget);
        hash.Add(fID);
        hash.Add(fFormat);
        return hash.ToHashCode();
    }
}

// gr_mtl_textureinfo_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct GRMtlTextureInfoNative : IEquatable<GRMtlTextureInfoNative>
{
    // public const void* fTexture
    public void* fTexture;

    public readonly bool Equals(GRMtlTextureInfoNative obj) =>
        fTexture == obj.fTexture;

    public readonly override bool Equals(object obj) =>
        obj is GRMtlTextureInfoNative f && Equals(f);

    public static bool operator ==(GRMtlTextureInfoNative left,
        GRMtlTextureInfoNative right) =>
        left.Equals(right);

    public static bool operator !=(GRMtlTextureInfoNative left,
        GRMtlTextureInfoNative right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(new IntPtr(fTexture));
        return hash.ToHashCode();
    }
}

// gr_vk_alloc_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct GRVkAlloc : IEquatable<GRVkAlloc>
{
    // public uint64_t fMemory
    private UInt64 fMemory;

    public UInt64 Memory
    {
        readonly get => fMemory;
        set => fMemory = value;
    }

    // public uint64_t fOffset
    private UInt64 fOffset;

    public UInt64 Offset
    {
        readonly get => fOffset;
        set => fOffset = value;
    }

    // public uint64_t fSize
    private UInt64 fSize;

    public UInt64 Size
    {
        readonly get => fSize;
        set => fSize = value;
    }

    // public uint32_t fFlags
    private UInt32 fFlags;

    public UInt32 Flags
    {
        readonly get => fFlags;
        set => fFlags = value;
    }

    // public gr_vk_backendmemory_t fBackendMemory
    private IntPtr fBackendMemory;

    public IntPtr BackendMemory
    {
        readonly get => fBackendMemory;
        set => fBackendMemory = value;
    }

    // public bool _private_fUsesSystemHeap
    private Byte fUsesSystemHeap;

    public readonly bool Equals(GRVkAlloc obj) =>
        fMemory == obj.fMemory && fOffset == obj.fOffset && fSize == obj.fSize &&
        fFlags == obj.fFlags && fBackendMemory == obj.fBackendMemory &&
        fUsesSystemHeap == obj.fUsesSystemHeap;

    public readonly override bool Equals(object obj) =>
        obj is GRVkAlloc f && Equals(f);

    public static bool operator ==(GRVkAlloc left, GRVkAlloc right) =>
        left.Equals(right);

    public static bool operator !=(GRVkAlloc left, GRVkAlloc right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fMemory);
        hash.Add(fOffset);
        hash.Add(fSize);
        hash.Add(fFlags);
        hash.Add(fBackendMemory);
        hash.Add(fUsesSystemHeap);
        return hash.ToHashCode();
    }
}

// gr_vk_backendcontext_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct
    GRVkBackendContextNative : IEquatable<GRVkBackendContextNative>
{
    // public vk_instance_t* fInstance
    public IntPtr fInstance;

    // public vk_physical_device_t* fPhysicalDevice
    public IntPtr fPhysicalDevice;

    // public vk_device_t* fDevice
    public IntPtr fDevice;

    // public vk_queue_t* fQueue
    public IntPtr fQueue;

    // public uint32_t fGraphicsQueueIndex
    public UInt32 fGraphicsQueueIndex;

    // public uint32_t fMinAPIVersion
    public UInt32 fMinAPIVersion;

    // public uint32_t fInstanceVersion
    public UInt32 fInstanceVersion;

    // public uint32_t fMaxAPIVersion
    public UInt32 fMaxAPIVersion;

    // public uint32_t fExtensions
    public UInt32 fExtensions;

    // public const gr_vk_extensions_t* fVkExtensions
    public IntPtr fVkExtensions;

    // public uint32_t fFeatures
    public UInt32 fFeatures;

    // public const vk_physical_device_features_t* fDeviceFeatures
    public IntPtr fDeviceFeatures;

    // public const vk_physical_device_features_2_t* fDeviceFeatures2
    public IntPtr fDeviceFeatures2;

    // public gr_vk_memory_allocator_t* fMemoryAllocator
    public IntPtr fMemoryAllocator;

    // public gr_vk_get_proc fGetProc
    public GRVkGetProcProxyDelegate fGetProc;

    // public void* fGetProcUserData
    public void* fGetProcUserData;

    // public bool fOwnsInstanceAndDevice
    public Byte fOwnsInstanceAndDevice;

    // public bool fProtectedContext
    public Byte fProtectedContext;

    public readonly bool Equals(GRVkBackendContextNative obj) =>
        fInstance == obj.fInstance && fPhysicalDevice == obj.fPhysicalDevice &&
        fDevice == obj.fDevice && fQueue == obj.fQueue &&
        fGraphicsQueueIndex == obj.fGraphicsQueueIndex &&
        fMinAPIVersion == obj.fMinAPIVersion && fInstanceVersion == obj.fInstanceVersion &&
        fMaxAPIVersion == obj.fMaxAPIVersion && fExtensions == obj.fExtensions &&
        fVkExtensions == obj.fVkExtensions && fFeatures == obj.fFeatures &&
        fDeviceFeatures == obj.fDeviceFeatures &&
        fDeviceFeatures2 == obj.fDeviceFeatures2 &&
        fMemoryAllocator == obj.fMemoryAllocator && fGetProc == obj.fGetProc &&
        fGetProcUserData == obj.fGetProcUserData &&
        fOwnsInstanceAndDevice == obj.fOwnsInstanceAndDevice &&
        fProtectedContext == obj.fProtectedContext;

    public readonly override bool Equals(object obj) =>
        obj is GRVkBackendContextNative f && Equals(f);

    public static bool operator ==(GRVkBackendContextNative left,
        GRVkBackendContextNative right) =>
        left.Equals(right);

    public static bool operator !=(GRVkBackendContextNative left,
        GRVkBackendContextNative right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fInstance);
        hash.Add(fPhysicalDevice);
        hash.Add(fDevice);
        hash.Add(fQueue);
        hash.Add(fGraphicsQueueIndex);
        hash.Add(fMinAPIVersion);
        hash.Add(fInstanceVersion);
        hash.Add(fMaxAPIVersion);
        hash.Add(fExtensions);
        hash.Add(fVkExtensions);
        hash.Add(fFeatures);
        hash.Add(fDeviceFeatures);
        hash.Add(fDeviceFeatures2);
        hash.Add(fMemoryAllocator);
        hash.Add(fGetProc);
        hash.Add(new IntPtr(fGetProcUserData));
        hash.Add(fOwnsInstanceAndDevice);
        hash.Add(fProtectedContext);
        return hash.ToHashCode();
    }
}

// gr_vk_imageinfo_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct GRVkImageInfo : IEquatable<GRVkImageInfo>
{
    // public uint64_t fImage
    private UInt64 fImage;

    public UInt64 Image
    {
        readonly get => fImage;
        set => fImage = value;
    }

    // public gr_vk_alloc_t fAlloc
    private GRVkAlloc fAlloc;

    public GRVkAlloc Alloc
    {
        readonly get => fAlloc;
        set => fAlloc = value;
    }

    // public uint32_t fImageTiling
    private UInt32 fImageTiling;

    public UInt32 ImageTiling
    {
        readonly get => fImageTiling;
        set => fImageTiling = value;
    }

    // public uint32_t fImageLayout
    private UInt32 fImageLayout;

    public UInt32 ImageLayout
    {
        readonly get => fImageLayout;
        set => fImageLayout = value;
    }

    // public uint32_t fFormat
    private UInt32 fFormat;

    public UInt32 Format
    {
        readonly get => fFormat;
        set => fFormat = value;
    }

    // public uint32_t fImageUsageFlags
    private UInt32 fImageUsageFlags;

    public UInt32 ImageUsageFlags
    {
        readonly get => fImageUsageFlags;
        set => fImageUsageFlags = value;
    }

    // public uint32_t fSampleCount
    private UInt32 fSampleCount;

    public UInt32 SampleCount
    {
        readonly get => fSampleCount;
        set => fSampleCount = value;
    }

    // public uint32_t fLevelCount
    private UInt32 fLevelCount;

    public UInt32 LevelCount
    {
        readonly get => fLevelCount;
        set => fLevelCount = value;
    }

    // public uint32_t fCurrentQueueFamily
    private UInt32 fCurrentQueueFamily;

    public UInt32 CurrentQueueFamily
    {
        readonly get => fCurrentQueueFamily;
        set => fCurrentQueueFamily = value;
    }

    // public bool fProtected
    private Byte fProtected;

    public bool Protected
    {
        readonly get => fProtected > 0;
        set => fProtected = value ? (byte)1 : (byte)0;
    }

    // public gr_vk_ycbcrconversioninfo_t fYcbcrConversionInfo
    private GrVkYcbcrConversionInfo fYcbcrConversionInfo;

    public GrVkYcbcrConversionInfo YcbcrConversionInfo
    {
        readonly get => fYcbcrConversionInfo;
        set => fYcbcrConversionInfo = value;
    }

    // public uint32_t fSharingMode
    private UInt32 fSharingMode;

    public UInt32 SharingMode
    {
        readonly get => fSharingMode;
        set => fSharingMode = value;
    }

    public readonly bool Equals(GRVkImageInfo obj) =>
        fImage == obj.fImage && fAlloc == obj.fAlloc && fImageTiling == obj.fImageTiling &&
        fImageLayout == obj.fImageLayout && fFormat == obj.fFormat &&
        fImageUsageFlags == obj.fImageUsageFlags && fSampleCount == obj.fSampleCount &&
        fLevelCount == obj.fLevelCount && fCurrentQueueFamily == obj.fCurrentQueueFamily &&
        fProtected == obj.fProtected && fYcbcrConversionInfo == obj.fYcbcrConversionInfo &&
        fSharingMode == obj.fSharingMode;

    public readonly override bool Equals(object obj) =>
        obj is GRVkImageInfo f && Equals(f);

    public static bool operator ==(GRVkImageInfo left, GRVkImageInfo right) =>
        left.Equals(right);

    public static bool operator !=(GRVkImageInfo left, GRVkImageInfo right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fImage);
        hash.Add(fAlloc);
        hash.Add(fImageTiling);
        hash.Add(fImageLayout);
        hash.Add(fFormat);
        hash.Add(fImageUsageFlags);
        hash.Add(fSampleCount);
        hash.Add(fLevelCount);
        hash.Add(fCurrentQueueFamily);
        hash.Add(fProtected);
        hash.Add(fYcbcrConversionInfo);
        hash.Add(fSharingMode);
        return hash.ToHashCode();
    }
}

// gr_vk_ycbcrconversioninfo_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct GrVkYcbcrConversionInfo : IEquatable<GrVkYcbcrConversionInfo>
{
    // public uint32_t fFormat
    private UInt32 fFormat;

    public UInt32 Format
    {
        readonly get => fFormat;
        set => fFormat = value;
    }

    // public uint64_t fExternalFormat
    private UInt64 fExternalFormat;

    public UInt64 ExternalFormat
    {
        readonly get => fExternalFormat;
        set => fExternalFormat = value;
    }

    // public uint32_t fYcbcrModel
    private UInt32 fYcbcrModel;

    public UInt32 YcbcrModel
    {
        readonly get => fYcbcrModel;
        set => fYcbcrModel = value;
    }

    // public uint32_t fYcbcrRange
    private UInt32 fYcbcrRange;

    public UInt32 YcbcrRange
    {
        readonly get => fYcbcrRange;
        set => fYcbcrRange = value;
    }

    // public uint32_t fXChromaOffset
    private UInt32 fXChromaOffset;

    public UInt32 XChromaOffset
    {
        readonly get => fXChromaOffset;
        set => fXChromaOffset = value;
    }

    // public uint32_t fYChromaOffset
    private UInt32 fYChromaOffset;

    public UInt32 YChromaOffset
    {
        readonly get => fYChromaOffset;
        set => fYChromaOffset = value;
    }

    // public uint32_t fChromaFilter
    private UInt32 fChromaFilter;

    public UInt32 ChromaFilter
    {
        readonly get => fChromaFilter;
        set => fChromaFilter = value;
    }

    // public uint32_t fForceExplicitReconstruction
    private UInt32 fForceExplicitReconstruction;

    public UInt32 ForceExplicitReconstruction
    {
        readonly get => fForceExplicitReconstruction;
        set => fForceExplicitReconstruction = value;
    }

    // public uint32_t fFormatFeatures
    private UInt32 fFormatFeatures;

    public UInt32 FormatFeatures
    {
        readonly get => fFormatFeatures;
        set => fFormatFeatures = value;
    }

    public readonly bool Equals(GrVkYcbcrConversionInfo obj) =>
        fFormat == obj.fFormat && fExternalFormat == obj.fExternalFormat &&
        fYcbcrModel == obj.fYcbcrModel && fYcbcrRange == obj.fYcbcrRange &&
        fXChromaOffset == obj.fXChromaOffset && fYChromaOffset == obj.fYChromaOffset &&
        fChromaFilter == obj.fChromaFilter &&
        fForceExplicitReconstruction == obj.fForceExplicitReconstruction &&
        fFormatFeatures == obj.fFormatFeatures;

    public readonly override bool Equals(object obj) =>
        obj is GrVkYcbcrConversionInfo f && Equals(f);

    public static bool operator ==(GrVkYcbcrConversionInfo left,
        GrVkYcbcrConversionInfo right) =>
        left.Equals(right);

    public static bool operator !=(GrVkYcbcrConversionInfo left,
        GrVkYcbcrConversionInfo right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fFormat);
        hash.Add(fExternalFormat);
        hash.Add(fYcbcrModel);
        hash.Add(fYcbcrRange);
        hash.Add(fXChromaOffset);
        hash.Add(fYChromaOffset);
        hash.Add(fChromaFilter);
        hash.Add(fForceExplicitReconstruction);
        hash.Add(fFormatFeatures);
        return hash.ToHashCode();
    }
}

// sk_codec_frameinfo_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SKCodecFrameInfo : IEquatable<SKCodecFrameInfo>
{
    // public int fRequiredFrame
    private Int32 fRequiredFrame;

    public Int32 RequiredFrame
    {
        readonly get => fRequiredFrame;
        set => fRequiredFrame = value;
    }

    // public int fDuration
    private Int32 fDuration;

    public Int32 Duration
    {
        readonly get => fDuration;
        set => fDuration = value;
    }

    // public bool fFullyReceived
    private Byte fFullyReceived;

    public bool FullyRecieved
    {
        readonly get => fFullyReceived > 0;
        set => fFullyReceived = value ? (byte)1 : (byte)0;
    }

    // public sk_alphatype_t fAlphaType
    private AlphaType fAlphaType;

    public AlphaType AlphaType
    {
        readonly get => fAlphaType;
        set => fAlphaType = value;
    }

    // public sk_codecanimation_disposalmethod_t fDisposalMethod
    private SKCodecAnimationDisposalMethod fDisposalMethod;

    public SKCodecAnimationDisposalMethod DisposalMethod
    {
        readonly get => fDisposalMethod;
        set => fDisposalMethod = value;
    }

    public readonly bool Equals(SKCodecFrameInfo obj) =>
        fRequiredFrame == obj.fRequiredFrame && fDuration == obj.fDuration &&
        fFullyReceived == obj.fFullyReceived && fAlphaType == obj.fAlphaType &&
        fDisposalMethod == obj.fDisposalMethod;

    public readonly override bool Equals(object obj) =>
        obj is SKCodecFrameInfo f && Equals(f);

    public static bool operator ==(SKCodecFrameInfo left, SKCodecFrameInfo right) =>
        left.Equals(right);

    public static bool operator !=(SKCodecFrameInfo left, SKCodecFrameInfo right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fRequiredFrame);
        hash.Add(fDuration);
        hash.Add(fFullyReceived);
        hash.Add(fAlphaType);
        hash.Add(fDisposalMethod);
        return hash.ToHashCode();
    }
}

// sk_codec_options_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct SKCodecOptionsInternal : IEquatable<SKCodecOptionsInternal>
{
    // public sk_codec_zero_initialized_t fZeroInitialized
    public SKZeroInitialized fZeroInitialized;

    // public sk_irect_t* fSubset
    public PixUI.RectI* fSubset;

    // public int fFrameIndex
    public Int32 fFrameIndex;

    // public int fPriorFrame
    public Int32 fPriorFrame;

    public readonly bool Equals(SKCodecOptionsInternal obj) =>
        fZeroInitialized == obj.fZeroInitialized && fSubset == obj.fSubset &&
        fFrameIndex == obj.fFrameIndex && fPriorFrame == obj.fPriorFrame;

    public readonly override bool Equals(object obj) =>
        obj is SKCodecOptionsInternal f && Equals(f);

    public static bool operator ==(SKCodecOptionsInternal left,
        SKCodecOptionsInternal right) =>
        left.Equals(right);

    public static bool operator !=(SKCodecOptionsInternal left,
        SKCodecOptionsInternal right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fZeroInitialized);
        hash.Add(new IntPtr(fSubset));
        hash.Add(fFrameIndex);
        hash.Add(fPriorFrame);
        return hash.ToHashCode();
    }
}

// sk_color4f_t
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe partial struct SKColorF : IEquatable<SKColorF>
{
    // public float fR
    private readonly Single fR;
    public readonly Single Red => fR;

    // public float fG
    private readonly Single fG;
    public readonly Single Green => fG;

    // public float fB
    private readonly Single fB;
    public readonly Single Blue => fB;

    // public float fA
    private readonly Single fA;
    public readonly Single Alpha => fA;

    public readonly bool Equals(SKColorF obj) =>
        fR == obj.fR && fG == obj.fG && fB == obj.fB && fA == obj.fA;

    public readonly override bool Equals(object obj) =>
        obj is SKColorF f && Equals(f);

    public static bool operator ==(SKColorF left, SKColorF right) =>
        left.Equals(right);

    public static bool operator !=(SKColorF left, SKColorF right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fR);
        hash.Add(fG);
        hash.Add(fB);
        hash.Add(fA);
        return hash.ToHashCode();
    }
}

// sk_colorspace_primaries_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SKColorSpacePrimaries : IEquatable<PixUI.SKColorSpacePrimaries>
{
    // public float fRX
    private Single fRX;

    public Single RX
    {
        readonly get => fRX;
        set => fRX = value;
    }

    // public float fRY
    private Single fRY;

    public Single RY
    {
        readonly get => fRY;
        set => fRY = value;
    }

    // public float fGX
    private Single fGX;

    public Single GX
    {
        readonly get => fGX;
        set => fGX = value;
    }

    // public float fGY
    private Single fGY;

    public Single GY
    {
        readonly get => fGY;
        set => fGY = value;
    }

    // public float fBX
    private Single fBX;

    public Single BX
    {
        readonly get => fBX;
        set => fBX = value;
    }

    // public float fBY
    private Single fBY;

    public Single BY
    {
        readonly get => fBY;
        set => fBY = value;
    }

    // public float fWX
    private Single fWX;

    public Single WX
    {
        readonly get => fWX;
        set => fWX = value;
    }

    // public float fWY
    private Single fWY;

    public Single WY
    {
        readonly get => fWY;
        set => fWY = value;
    }

    public readonly bool Equals(PixUI.SKColorSpacePrimaries obj) =>
        fRX == obj.fRX && fRY == obj.fRY && fGX == obj.fGX && fGY == obj.fGY &&
        fBX == obj.fBX && fBY == obj.fBY && fWX == obj.fWX && fWY == obj.fWY;

    public readonly override bool Equals(object obj) =>
        obj is PixUI.SKColorSpacePrimaries f && Equals(f);

    public static bool operator ==(PixUI.SKColorSpacePrimaries left,
        PixUI.SKColorSpacePrimaries right) =>
        left.Equals(right);

    public static bool operator !=(PixUI.SKColorSpacePrimaries left,
        PixUI.SKColorSpacePrimaries right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fRX);
        hash.Add(fRY);
        hash.Add(fGX);
        hash.Add(fGY);
        hash.Add(fBX);
        hash.Add(fBY);
        hash.Add(fWX);
        hash.Add(fWY);
        return hash.ToHashCode();
    }
}

// sk_colorspace_transfer_fn_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct
    SKColorSpaceTransferFn : IEquatable<PixUI.SKColorSpaceTransferFn>
{
    // public float fG
    private Single fG;

    public Single G
    {
        readonly get => fG;
        set => fG = value;
    }

    // public float fA
    private Single fA;

    public Single A
    {
        readonly get => fA;
        set => fA = value;
    }

    // public float fB
    private Single fB;

    public Single B
    {
        readonly get => fB;
        set => fB = value;
    }

    // public float fC
    private Single fC;

    public Single C
    {
        readonly get => fC;
        set => fC = value;
    }

    // public float fD
    private Single fD;

    public Single D
    {
        readonly get => fD;
        set => fD = value;
    }

    // public float fE
    private Single fE;

    public Single E
    {
        readonly get => fE;
        set => fE = value;
    }

    // public float fF
    private Single fF;

    public Single F
    {
        readonly get => fF;
        set => fF = value;
    }

    public readonly bool Equals(PixUI.SKColorSpaceTransferFn obj) =>
        fG == obj.fG && fA == obj.fA && fB == obj.fB && fC == obj.fC && fD == obj.fD &&
        fE == obj.fE && fF == obj.fF;

    public readonly override bool Equals(object obj) =>
        obj is PixUI.SKColorSpaceTransferFn f && Equals(f);

    public static bool operator ==(PixUI.SKColorSpaceTransferFn left,
        PixUI.SKColorSpaceTransferFn right) =>
        left.Equals(right);

    public static bool operator !=(PixUI.SKColorSpaceTransferFn left,
        PixUI.SKColorSpaceTransferFn right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fG);
        hash.Add(fA);
        hash.Add(fB);
        hash.Add(fC);
        hash.Add(fD);
        hash.Add(fE);
        hash.Add(fF);
        return hash.ToHashCode();
    }
}

// sk_colorspace_xyz_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SKColorSpaceXyz : IEquatable<PixUI.SKColorSpaceXyz>
{
    // public float fM00
    private Single fM00;

    // public float fM01
    private Single fM01;

    // public float fM02
    private Single fM02;

    // public float fM10
    private Single fM10;

    // public float fM11
    private Single fM11;

    // public float fM12
    private Single fM12;

    // public float fM20
    private Single fM20;

    // public float fM21
    private Single fM21;

    // public float fM22
    private Single fM22;

    public readonly bool Equals(PixUI.SKColorSpaceXyz obj) =>
        fM00 == obj.fM00 && fM01 == obj.fM01 && fM02 == obj.fM02 && fM10 == obj.fM10 &&
        fM11 == obj.fM11 && fM12 == obj.fM12 && fM20 == obj.fM20 && fM21 == obj.fM21 &&
        fM22 == obj.fM22;

    public readonly override bool Equals(object obj) =>
        obj is PixUI.SKColorSpaceXyz f && Equals(f);

    public static bool operator ==(PixUI.SKColorSpaceXyz left,
        PixUI.SKColorSpaceXyz right) =>
        left.Equals(right);

    public static bool operator !=(PixUI.SKColorSpaceXyz left,
        PixUI.SKColorSpaceXyz right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fM00);
        hash.Add(fM01);
        hash.Add(fM02);
        hash.Add(fM10);
        hash.Add(fM11);
        hash.Add(fM12);
        hash.Add(fM20);
        hash.Add(fM21);
        hash.Add(fM22);
        return hash.ToHashCode();
    }
}

// sk_document_pdf_metadata_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct
    SKDocumentPdfMetadataInternal : IEquatable<SKDocumentPdfMetadataInternal>
{
    // public sk_string_t* fTitle
    public IntPtr fTitle;

    // public sk_string_t* fAuthor
    public IntPtr fAuthor;

    // public sk_string_t* fSubject
    public IntPtr fSubject;

    // public sk_string_t* fKeywords
    public IntPtr fKeywords;

    // public sk_string_t* fCreator
    public IntPtr fCreator;

    // public sk_string_t* fProducer
    public IntPtr fProducer;

    // public sk_time_datetime_t* fCreation
    public SKTimeDateTimeInternal* fCreation;

    // public sk_time_datetime_t* fModified
    public SKTimeDateTimeInternal* fModified;

    // public float fRasterDPI
    public Single fRasterDPI;

    // public bool fPDFA
    public Byte fPDFA;

    // public int fEncodingQuality
    public Int32 fEncodingQuality;

    public readonly bool Equals(SKDocumentPdfMetadataInternal obj) =>
        fTitle == obj.fTitle && fAuthor == obj.fAuthor && fSubject == obj.fSubject &&
        fKeywords == obj.fKeywords && fCreator == obj.fCreator &&
        fProducer == obj.fProducer && fCreation == obj.fCreation &&
        fModified == obj.fModified && fRasterDPI == obj.fRasterDPI && fPDFA == obj.fPDFA &&
        fEncodingQuality == obj.fEncodingQuality;

    public readonly override bool Equals(object obj) =>
        obj is SKDocumentPdfMetadataInternal f && Equals(f);

    public static bool operator ==(SKDocumentPdfMetadataInternal left,
        SKDocumentPdfMetadataInternal right) =>
        left.Equals(right);

    public static bool operator !=(SKDocumentPdfMetadataInternal left,
        SKDocumentPdfMetadataInternal right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fTitle);
        hash.Add(fAuthor);
        hash.Add(fSubject);
        hash.Add(fKeywords);
        hash.Add(fCreator);
        hash.Add(fProducer);
        hash.Add(new IntPtr(fCreation));
        hash.Add(new IntPtr(fModified));
        hash.Add(fRasterDPI);
        hash.Add(fPDFA);
        hash.Add(fEncodingQuality);
        return hash.ToHashCode();
    }
}

// sk_fontmetrics_t
[StructLayout(LayoutKind.Sequential)]
public struct FontMetrics : IEquatable<FontMetrics>
{
    public uint Flags;
    public float Top;
    public float Ascent;
    public float Descent;
    public float Bottom;
    public float Leading;
    public float AvgCharWidth;
    public float MaxCharWidth;
    public float XMin;
    public float XMax;
    public float XHeight;
    public float CapHeight;
    public float UnderlineThickness;
    public float UnderlinePosition;
    public float StrikeoutThickness;
    public float StrikeoutPosition;

    public bool HasUnderlineThickness => (Flags & 1) == 1;
    public bool HasUnderlinePosition => (Flags & 2) == 2;
    public bool HasStrikeoutThickness => (Flags & 4) == 4;
    public bool HasStrikeoutPosition => (Flags & 8) == 8;

    public readonly bool Equals(FontMetrics obj) =>
        Flags == obj.Flags && Top == obj.Top && Ascent == obj.Ascent &&
        Descent == obj.Descent && Bottom == obj.Bottom && Leading == obj.Leading &&
        AvgCharWidth == obj.AvgCharWidth && MaxCharWidth == obj.MaxCharWidth &&
        XMin == obj.XMin && XMax == obj.XMax && XHeight == obj.XHeight &&
        CapHeight == obj.CapHeight && UnderlineThickness == obj.UnderlineThickness &&
        UnderlinePosition == obj.UnderlinePosition &&
        StrikeoutThickness == obj.StrikeoutThickness &&
        StrikeoutPosition == obj.StrikeoutPosition;

    public readonly override bool Equals(object obj) =>
        obj is FontMetrics f && Equals(f);

    public static bool operator ==(FontMetrics left, FontMetrics right) =>
        left.Equals(right);

    public static bool operator !=(FontMetrics left, FontMetrics right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Flags);
        hash.Add(Top);
        hash.Add(Ascent);
        hash.Add(Descent);
        hash.Add(Bottom);
        hash.Add(Leading);
        hash.Add(AvgCharWidth);
        hash.Add(MaxCharWidth);
        hash.Add(XMin);
        hash.Add(XMax);
        hash.Add(XHeight);
        hash.Add(CapHeight);
        hash.Add(UnderlineThickness);
        hash.Add(UnderlinePosition);
        hash.Add(StrikeoutThickness);
        hash.Add(StrikeoutPosition);
        return hash.ToHashCode();
    }
}

// sk_highcontrastconfig_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SKHighContrastConfig : IEquatable<SKHighContrastConfig>
{
    // public bool fGrayscale
    private Byte fGrayscale;

    public bool Grayscale
    {
        readonly get => fGrayscale > 0;
        set => fGrayscale = value ? (byte)1 : (byte)0;
    }

    // public sk_highcontrastconfig_invertstyle_t fInvertStyle
    private SKHighContrastConfigInvertStyle fInvertStyle;

    public SKHighContrastConfigInvertStyle InvertStyle
    {
        readonly get => fInvertStyle;
        set => fInvertStyle = value;
    }

    // public float fContrast
    private Single fContrast;

    public Single Contrast
    {
        readonly get => fContrast;
        set => fContrast = value;
    }

    public readonly bool Equals(SKHighContrastConfig obj) =>
        fGrayscale == obj.fGrayscale && fInvertStyle == obj.fInvertStyle &&
        fContrast == obj.fContrast;

    public readonly override bool Equals(object obj) =>
        obj is SKHighContrastConfig f && Equals(f);

    public static bool operator ==(SKHighContrastConfig left, SKHighContrastConfig right) =>
        left.Equals(right);

    public static bool operator !=(SKHighContrastConfig left, SKHighContrastConfig right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fGrayscale);
        hash.Add(fInvertStyle);
        hash.Add(fContrast);
        return hash.ToHashCode();
    }
}

// sk_imageinfo_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct SKImageInfoNative : IEquatable<SKImageInfoNative>
{
    // public sk_colorspace_t* colorspace
    public IntPtr colorspace;

    // public int32_t width
    public Int32 width;

    // public int32_t height
    public Int32 height;

    // public sk_colortype_t colorType
    public SKColorTypeNative colorType;

    // public sk_alphatype_t alphaType
    public AlphaType alphaType;

    public readonly bool Equals(SKImageInfoNative obj) =>
        colorspace == obj.colorspace && width == obj.width && height == obj.height &&
        colorType == obj.colorType && alphaType == obj.alphaType;

    public readonly override bool Equals(object? obj) => obj is SKImageInfoNative f && Equals(f);

    public static bool operator ==(SKImageInfoNative left, SKImageInfoNative right) => left.Equals(right);

    public static bool operator !=(SKImageInfoNative left, SKImageInfoNative right) => !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(colorspace);
        hash.Add(width);
        hash.Add(height);
        hash.Add(colorType);
        hash.Add(alphaType);
        return hash.ToHashCode();
    }
}

// sk_ipoint_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct PointI : IEquatable<PixUI.PointI>
{
    // public int32_t x
    private Int32 x;

    public Int32 X
    {
        readonly get => x;
        set => x = value;
    }

    // public int32_t y
    private Int32 y;

    public Int32 Y
    {
        readonly get => y;
        set => y = value;
    }

    public readonly bool Equals(PixUI.PointI obj) =>
        x == obj.x && y == obj.y;

    public readonly override bool Equals(object obj) =>
        obj is PixUI.PointI f && Equals(f);

    public static bool operator ==(PixUI.PointI left, PixUI.PointI right) =>
        left.Equals(right);

    public static bool operator !=(PixUI.PointI left, PixUI.PointI right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(x);
        hash.Add(y);
        return hash.ToHashCode();
    }
}

// sk_irect_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct RectI : IEquatable<PixUI.RectI>
{
    // public int32_t left
    private Int32 left;

    public Int32 Left
    {
        readonly get => left;
        set => left = value;
    }

    public int X
    {
        get => left;
        set
        {
            var diff = value - left;
            left = value;
            right += diff;
        }
    }

    public int Y
    {
        get => top;
        set
        {
            var diff = value - top;
            top = value;
            bottom += diff;
        }
    }

    // public int32_t top
    private Int32 top;

    public Int32 Top
    {
        readonly get => top;
        set => top = value;
    }

    // public int32_t right
    private Int32 right;

    public Int32 Right
    {
        readonly get => right;
        set => right = value;
    }

    // public int32_t bottom
    private Int32 bottom;

    public Int32 Bottom
    {
        readonly get => bottom;
        set => bottom = value;
    }

    public readonly bool Equals(PixUI.RectI obj) =>
        left == obj.left && top == obj.top && right == obj.right && bottom == obj.bottom;

    public readonly override bool Equals(object obj) =>
        obj is PixUI.RectI f && Equals(f);

    public static bool operator ==(PixUI.RectI left, PixUI.RectI right) =>
        left.Equals(right);

    public static bool operator !=(PixUI.RectI left, PixUI.RectI right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(left);
        hash.Add(top);
        hash.Add(right);
        hash.Add(bottom);
        return hash.ToHashCode();
    }
}

// sk_isize_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SizeI : IEquatable<PixUI.SizeI>
{
    // public int32_t w
    private Int32 w;

    public Int32 Width
    {
        readonly get => w;
        set => w = value;
    }

    // public int32_t h
    private Int32 h;

    public Int32 Height
    {
        readonly get => h;
        set => h = value;
    }

    public readonly bool Equals(PixUI.SizeI obj) =>
        w == obj.w && h == obj.h;

    public readonly override bool Equals(object obj) =>
        obj is PixUI.SizeI f && Equals(f);

    public static bool operator ==(PixUI.SizeI left, PixUI.SizeI right) =>
        left.Equals(right);

    public static bool operator !=(PixUI.SizeI left, PixUI.SizeI right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(w);
        hash.Add(h);
        return hash.ToHashCode();
    }
}

// sk_jpegencoder_options_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SKJpegEncoderOptions : IEquatable<SKJpegEncoderOptions>
{
    // public int fQuality
    private Int32 fQuality;

    public Int32 Quality
    {
        readonly get => fQuality;
        set => fQuality = value;
    }

    // public sk_jpegencoder_downsample_t fDownsample
    private SKJpegEncoderDownsample fDownsample;

    public SKJpegEncoderDownsample Downsample
    {
        readonly get => fDownsample;
        set => fDownsample = value;
    }

    // public sk_jpegencoder_alphaoption_t fAlphaOption
    private SKJpegEncoderAlphaOption fAlphaOption;

    public SKJpegEncoderAlphaOption AlphaOption
    {
        readonly get => fAlphaOption;
        set => fAlphaOption = value;
    }

    public readonly bool Equals(SKJpegEncoderOptions obj) =>
        fQuality == obj.fQuality && fDownsample == obj.fDownsample &&
        fAlphaOption == obj.fAlphaOption;

    public readonly override bool Equals(object obj) =>
        obj is SKJpegEncoderOptions f && Equals(f);

    public static bool operator ==(SKJpegEncoderOptions left, SKJpegEncoderOptions right) =>
        left.Equals(right);

    public static bool operator !=(SKJpegEncoderOptions left, SKJpegEncoderOptions right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fQuality);
        hash.Add(fDownsample);
        hash.Add(fAlphaOption);
        return hash.ToHashCode();
    }
}

// sk_lattice_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct SKLatticeInternal : IEquatable<SKLatticeInternal>
{
    // public const int* fXDivs
    public Int32* fXDivs;

    // public const int* fYDivs
    public Int32* fYDivs;

    // public const sk_lattice_recttype_t* fRectTypes
    public SKLatticeRectType* fRectTypes;

    // public int fXCount
    public Int32 fXCount;

    // public int fYCount
    public Int32 fYCount;

    // public const sk_irect_t* fBounds
    public PixUI.RectI* fBounds;

    // public const sk_color_t* fColors
    public UInt32* fColors;

    public readonly bool Equals(SKLatticeInternal obj) =>
        fXDivs == obj.fXDivs && fYDivs == obj.fYDivs && fRectTypes == obj.fRectTypes &&
        fXCount == obj.fXCount && fYCount == obj.fYCount && fBounds == obj.fBounds &&
        fColors == obj.fColors;

    public readonly override bool Equals(object obj) =>
        obj is SKLatticeInternal f && Equals(f);

    public static bool operator ==(SKLatticeInternal left, SKLatticeInternal right) =>
        left.Equals(right);

    public static bool operator !=(SKLatticeInternal left, SKLatticeInternal right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(new IntPtr(fXDivs));
        hash.Add(new IntPtr(fYDivs));
        hash.Add(new IntPtr(fRectTypes));
        hash.Add(fXCount);
        hash.Add(fYCount);
        hash.Add(new IntPtr(fBounds));
        hash.Add(new IntPtr(fColors));
        return hash.ToHashCode();
    }
}

// sk_manageddrawable_procs_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct
    SKManagedDrawableDelegates : IEquatable<SKManagedDrawableDelegates>
{
    // public sk_manageddrawable_draw_proc fDraw
    public SKManagedDrawableDrawProxyDelegate fDraw;

    // public sk_manageddrawable_getBounds_proc fGetBounds
    public SKManagedDrawableGetBoundsProxyDelegate fGetBounds;

    // public sk_manageddrawable_newPictureSnapshot_proc fNewPictureSnapshot
    public SKManagedDrawableNewPictureSnapshotProxyDelegate fNewPictureSnapshot;

    // public sk_manageddrawable_destroy_proc fDestroy
    public SKManagedDrawableDestroyProxyDelegate fDestroy;

    public readonly bool Equals(SKManagedDrawableDelegates obj) =>
        fDraw == obj.fDraw && fGetBounds == obj.fGetBounds &&
        fNewPictureSnapshot == obj.fNewPictureSnapshot && fDestroy == obj.fDestroy;

    public readonly override bool Equals(object obj) =>
        obj is SKManagedDrawableDelegates f && Equals(f);

    public static bool operator ==(SKManagedDrawableDelegates left,
        SKManagedDrawableDelegates right) =>
        left.Equals(right);

    public static bool operator !=(SKManagedDrawableDelegates left,
        SKManagedDrawableDelegates right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fDraw);
        hash.Add(fGetBounds);
        hash.Add(fNewPictureSnapshot);
        hash.Add(fDestroy);
        return hash.ToHashCode();
    }
}

// sk_managedstream_procs_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct
    SKManagedStreamDelegates : IEquatable<SKManagedStreamDelegates>
{
    // public sk_managedstream_read_proc fRead
    public SKManagedStreamReadProxyDelegate fRead;

    // public sk_managedstream_peek_proc fPeek
    public SKManagedStreamPeekProxyDelegate fPeek;

    // public sk_managedstream_isAtEnd_proc fIsAtEnd
    public SKManagedStreamIsAtEndProxyDelegate fIsAtEnd;

    // public sk_managedstream_hasPosition_proc fHasPosition
    public SKManagedStreamHasPositionProxyDelegate fHasPosition;

    // public sk_managedstream_hasLength_proc fHasLength
    public SKManagedStreamHasLengthProxyDelegate fHasLength;

    // public sk_managedstream_rewind_proc fRewind
    public SKManagedStreamRewindProxyDelegate fRewind;

    // public sk_managedstream_getPosition_proc fGetPosition
    public SKManagedStreamGetPositionProxyDelegate fGetPosition;

    // public sk_managedstream_seek_proc fSeek
    public SKManagedStreamSeekProxyDelegate fSeek;

    // public sk_managedstream_move_proc fMove
    public SKManagedStreamMoveProxyDelegate fMove;

    // public sk_managedstream_getLength_proc fGetLength
    public SKManagedStreamGetLengthProxyDelegate fGetLength;

    // public sk_managedstream_duplicate_proc fDuplicate
    public SKManagedStreamDuplicateProxyDelegate fDuplicate;

    // public sk_managedstream_fork_proc fFork
    public SKManagedStreamForkProxyDelegate fFork;

    // public sk_managedstream_destroy_proc fDestroy
    public SKManagedStreamDestroyProxyDelegate fDestroy;

    public readonly bool Equals(SKManagedStreamDelegates obj) =>
        fRead == obj.fRead && fPeek == obj.fPeek && fIsAtEnd == obj.fIsAtEnd &&
        fHasPosition == obj.fHasPosition && fHasLength == obj.fHasLength &&
        fRewind == obj.fRewind && fGetPosition == obj.fGetPosition && fSeek == obj.fSeek &&
        fMove == obj.fMove && fGetLength == obj.fGetLength &&
        fDuplicate == obj.fDuplicate && fFork == obj.fFork && fDestroy == obj.fDestroy;

    public readonly override bool Equals(object obj) =>
        obj is SKManagedStreamDelegates f && Equals(f);

    public static bool operator ==(SKManagedStreamDelegates left,
        SKManagedStreamDelegates right) =>
        left.Equals(right);

    public static bool operator !=(SKManagedStreamDelegates left,
        SKManagedStreamDelegates right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fRead);
        hash.Add(fPeek);
        hash.Add(fIsAtEnd);
        hash.Add(fHasPosition);
        hash.Add(fHasLength);
        hash.Add(fRewind);
        hash.Add(fGetPosition);
        hash.Add(fSeek);
        hash.Add(fMove);
        hash.Add(fGetLength);
        hash.Add(fDuplicate);
        hash.Add(fFork);
        hash.Add(fDestroy);
        return hash.ToHashCode();
    }
}

// sk_managedtracememorydump_procs_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct
    SKManagedTraceMemoryDumpDelegates : IEquatable<SKManagedTraceMemoryDumpDelegates>
{
    // public sk_managedtraceMemoryDump_dumpNumericValue_proc fDumpNumericValue
    public SKManagedTraceMemoryDumpDumpNumericValueProxyDelegate fDumpNumericValue;

    // public sk_managedtraceMemoryDump_dumpStringValue_proc fDumpStringValue
    public SKManagedTraceMemoryDumpDumpStringValueProxyDelegate fDumpStringValue;

    public readonly bool Equals(SKManagedTraceMemoryDumpDelegates obj) =>
        fDumpNumericValue == obj.fDumpNumericValue &&
        fDumpStringValue == obj.fDumpStringValue;

    public readonly override bool Equals(object obj) =>
        obj is SKManagedTraceMemoryDumpDelegates f && Equals(f);

    public static bool operator ==(SKManagedTraceMemoryDumpDelegates left,
        SKManagedTraceMemoryDumpDelegates right) =>
        left.Equals(right);

    public static bool operator !=(SKManagedTraceMemoryDumpDelegates left,
        SKManagedTraceMemoryDumpDelegates right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fDumpNumericValue);
        hash.Add(fDumpStringValue);
        return hash.ToHashCode();
    }
}

// sk_managedwstream_procs_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct
    SKManagedWStreamDelegates : IEquatable<SKManagedWStreamDelegates>
{
    // public sk_managedwstream_write_proc fWrite
    public SKManagedWStreamWriteProxyDelegate fWrite;

    // public sk_managedwstream_flush_proc fFlush
    public SKManagedWStreamFlushProxyDelegate fFlush;

    // public sk_managedwstream_bytesWritten_proc fBytesWritten
    public SKManagedWStreamBytesWrittenProxyDelegate fBytesWritten;

    // public sk_managedwstream_destroy_proc fDestroy
    public SKManagedWStreamDestroyProxyDelegate fDestroy;

    public readonly bool Equals(SKManagedWStreamDelegates obj) =>
        fWrite == obj.fWrite && fFlush == obj.fFlush &&
        fBytesWritten == obj.fBytesWritten && fDestroy == obj.fDestroy;

    public readonly override bool Equals(object obj) =>
        obj is SKManagedWStreamDelegates f && Equals(f);

    public static bool operator ==(SKManagedWStreamDelegates left,
        SKManagedWStreamDelegates right) =>
        left.Equals(right);

    public static bool operator !=(SKManagedWStreamDelegates left,
        SKManagedWStreamDelegates right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fWrite);
        hash.Add(fFlush);
        hash.Add(fBytesWritten);
        hash.Add(fDestroy);
        return hash.ToHashCode();
    }
}

// sk_mask_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SKMask : IEquatable<SKMask>
{
    // public uint8_t* fImage
    private Byte* fImage;

    // public sk_irect_t fBounds
    private PixUI.RectI fBounds;

    // public uint32_t fRowBytes
    private UInt32 fRowBytes;

    // public sk_mask_format_t fFormat
    private SKMaskFormat fFormat;

    public readonly bool Equals(SKMask obj) =>
        fImage == obj.fImage && fBounds == obj.fBounds && fRowBytes == obj.fRowBytes &&
        fFormat == obj.fFormat;

    public readonly override bool Equals(object obj) =>
        obj is SKMask f && Equals(f);

    public static bool operator ==(SKMask left, SKMask right) =>
        left.Equals(right);

    public static bool operator !=(SKMask left, SKMask right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(new IntPtr(fImage));
        hash.Add(fBounds);
        hash.Add(fRowBytes);
        hash.Add(fFormat);
        return hash.ToHashCode();
    }
}

// sk_pngencoder_options_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SKPngEncoderOptions : IEquatable<SKPngEncoderOptions>
{
    // public sk_pngencoder_filterflags_t fFilterFlags
    private SKPngEncoderFilterFlags fFilterFlags;

    // public int fZLibLevel
    private Int32 fZLibLevel;

    // public void* fComments
    private void* fComments;

    public readonly bool Equals(SKPngEncoderOptions obj) =>
        fFilterFlags == obj.fFilterFlags && fZLibLevel == obj.fZLibLevel &&
        fComments == obj.fComments;

    public readonly override bool Equals(object obj) =>
        obj is SKPngEncoderOptions f && Equals(f);

    public static bool operator ==(SKPngEncoderOptions left, SKPngEncoderOptions right) =>
        left.Equals(right);

    public static bool operator !=(SKPngEncoderOptions left, SKPngEncoderOptions right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fFilterFlags);
        hash.Add(fZLibLevel);
        hash.Add(new IntPtr(fComments));
        return hash.ToHashCode();
    }
}

// sk_point_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Point : IEquatable<PixUI.Point>
{
    // public float x
    private Single x;

    public Single X
    {
        readonly get => x;
        set => x = value;
    }

    // public float y
    private Single y;

    public Single Y
    {
        readonly get => y;
        set => y = value;
    }

    public readonly bool Equals(Point obj) => x == obj.x && y == obj.y;

    public readonly override bool Equals(object obj) => obj is Point f && Equals(f);

    public static bool operator ==(Point left, Point right) => left.Equals(right);

    public static bool operator !=(Point left, Point right) => !left.Equals(right);

    public readonly override int GetHashCode() => HashCode.Combine(x, y);
}

// sk_point3_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Point3 : IEquatable<PixUI.Point3>
{
    // public float x
    private Single x;

    public Single X
    {
        readonly get => x;
        set => x = value;
    }

    // public float y
    private Single y;

    public Single Y
    {
        readonly get => y;
        set => y = value;
    }

    // public float z
    private Single z;

    public Single Z
    {
        readonly get => z;
        set => z = value;
    }

    public readonly bool Equals(PixUI.Point3 obj) =>
        x == obj.x && y == obj.y && z == obj.z;

    public readonly override bool Equals(object obj) =>
        obj is PixUI.Point3 f && Equals(f);

    public static bool operator ==(PixUI.Point3 left, PixUI.Point3 right) =>
        left.Equals(right);

    public static bool operator !=(PixUI.Point3 left, PixUI.Point3 right) =>
        !left.Equals(right);

    public readonly override int GetHashCode() => HashCode.Combine(x, y, z);
}

// sk_rect_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Rect : IEquatable<PixUI.Rect>
{
    // public float left
    private Single left;

    public Single Left
    {
        readonly get => left;
        set => left = value;
    }

    public float X
    {
        get => left;
        set
        {
            var diff = value - left;
            left = value;
            right += diff;
        }
    }

    public float Y
    {
        get => top;
        set
        {
            var diff = value - top;
            top = value;
            bottom += diff;
        }
    }

    // public float top
    private Single top;

    public Single Top
    {
        readonly get => top;
        set => top = value;
    }

    // public float right
    private Single right;

    public Single Right
    {
        readonly get => right;
        set => right = value;
    }

    // public float bottom
    private Single bottom;

    public Single Bottom
    {
        readonly get => bottom;
        set => bottom = value;
    }

    public readonly bool Equals(PixUI.Rect obj) =>
        left == obj.left && top == obj.top && right == obj.right && bottom == obj.bottom;

    public readonly override bool Equals(object obj) =>
        obj is PixUI.Rect f && Equals(f);

    public static bool operator ==(PixUI.Rect left, PixUI.Rect right) =>
        left.Equals(right);

    public static bool operator !=(PixUI.Rect left, PixUI.Rect right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(left);
        hash.Add(top);
        hash.Add(right);
        hash.Add(bottom);
        return hash.ToHashCode();
    }
}

// sk_rsxform_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SKRotationScaleMatrix : IEquatable<SKRotationScaleMatrix>
{
    // public float fSCos
    private Single fSCos;

    public Single SCos
    {
        readonly get => fSCos;
        set => fSCos = value;
    }

    // public float fSSin
    private Single fSSin;

    public Single SSin
    {
        readonly get => fSSin;
        set => fSSin = value;
    }

    // public float fTX
    private Single fTX;

    public Single TX
    {
        readonly get => fTX;
        set => fTX = value;
    }

    // public float fTY
    private Single fTY;

    public Single TY
    {
        readonly get => fTY;
        set => fTY = value;
    }

    public readonly bool Equals(SKRotationScaleMatrix obj) =>
        fSCos == obj.fSCos && fSSin == obj.fSSin && fTX == obj.fTX && fTY == obj.fTY;

    public readonly override bool Equals(object obj) =>
        obj is SKRotationScaleMatrix f && Equals(f);

    public static bool operator ==(SKRotationScaleMatrix left,
        SKRotationScaleMatrix right) =>
        left.Equals(right);

    public static bool operator !=(SKRotationScaleMatrix left,
        SKRotationScaleMatrix right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fSCos);
        hash.Add(fSSin);
        hash.Add(fTX);
        hash.Add(fTY);
        return hash.ToHashCode();
    }
}

// sk_size_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Size : IEquatable<PixUI.Size>
{
    // public float w
    private float w;

    public float Width
    {
        readonly get => w;
        set => w = value;
    }

    // public float h
    private float h;

    public float Height
    {
        readonly get => h;
        set => h = value;
    }

    public readonly bool Equals(PixUI.Size obj) =>
        w == obj.w && h == obj.h;

    public readonly override bool Equals(object obj) =>
        obj is PixUI.Size f && Equals(f);

    public static bool operator ==(PixUI.Size left, PixUI.Size right) =>
        left.Equals(right);

    public static bool operator !=(PixUI.Size left, PixUI.Size right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(w);
        hash.Add(h);
        return hash.ToHashCode();
    }
}

// sk_textblob_builder_runbuffer_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct SKRunBufferInternal : IEquatable<SKRunBufferInternal>
{
    // public void* glyphs
    public void* glyphs;

    // public void* pos
    public void* pos;

    // public void* utf8text
    public void* utf8text;

    // public void* clusters
    public void* clusters;

    public readonly bool Equals(SKRunBufferInternal obj) =>
        glyphs == obj.glyphs && pos == obj.pos && utf8text == obj.utf8text &&
        clusters == obj.clusters;

    public readonly override bool Equals(object obj) =>
        obj is SKRunBufferInternal f && Equals(f);

    public static bool operator ==(SKRunBufferInternal left, SKRunBufferInternal right) =>
        left.Equals(right);

    public static bool operator !=(SKRunBufferInternal left, SKRunBufferInternal right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(new IntPtr(glyphs));
        hash.Add(new IntPtr(pos));
        hash.Add(new IntPtr(utf8text));
        hash.Add(new IntPtr(clusters));
        return hash.ToHashCode();
    }
}

// sk_time_datetime_t
[StructLayout(LayoutKind.Sequential)]
internal unsafe partial struct SKTimeDateTimeInternal : IEquatable<SKTimeDateTimeInternal>
{
    // public int16_t fTimeZoneMinutes
    public Int16 fTimeZoneMinutes;

    // public uint16_t fYear
    public UInt16 fYear;

    // public uint8_t fMonth
    public Byte fMonth;

    // public uint8_t fDayOfWeek
    public Byte fDayOfWeek;

    // public uint8_t fDay
    public Byte fDay;

    // public uint8_t fHour
    public Byte fHour;

    // public uint8_t fMinute
    public Byte fMinute;

    // public uint8_t fSecond
    public Byte fSecond;

    public readonly bool Equals(SKTimeDateTimeInternal obj) =>
        fTimeZoneMinutes == obj.fTimeZoneMinutes && fYear == obj.fYear &&
        fMonth == obj.fMonth && fDayOfWeek == obj.fDayOfWeek && fDay == obj.fDay &&
        fHour == obj.fHour && fMinute == obj.fMinute && fSecond == obj.fSecond;

    public readonly override bool Equals(object obj) =>
        obj is SKTimeDateTimeInternal f && Equals(f);

    public static bool operator ==(SKTimeDateTimeInternal left,
        SKTimeDateTimeInternal right) =>
        left.Equals(right);

    public static bool operator !=(SKTimeDateTimeInternal left,
        SKTimeDateTimeInternal right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fTimeZoneMinutes);
        hash.Add(fYear);
        hash.Add(fMonth);
        hash.Add(fDayOfWeek);
        hash.Add(fDay);
        hash.Add(fHour);
        hash.Add(fMinute);
        hash.Add(fSecond);
        return hash.ToHashCode();
    }
}

// sk_webpencoder_options_t
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct SKWebpEncoderOptions : IEquatable<SKWebpEncoderOptions>
{
    // public sk_webpencoder_compression_t fCompression
    private SKWebpEncoderCompression fCompression;

    public SKWebpEncoderCompression Compression
    {
        readonly get => fCompression;
        set => fCompression = value;
    }

    // public float fQuality
    private Single fQuality;

    public Single Quality
    {
        readonly get => fQuality;
        set => fQuality = value;
    }

    public readonly bool Equals(SKWebpEncoderOptions obj) =>
        fCompression == obj.fCompression && fQuality == obj.fQuality;

    public readonly override bool Equals(object obj) =>
        obj is SKWebpEncoderOptions f && Equals(f);

    public static bool operator ==(SKWebpEncoderOptions left, SKWebpEncoderOptions right) =>
        left.Equals(right);

    public static bool operator !=(SKWebpEncoderOptions left, SKWebpEncoderOptions right) =>
        !left.Equals(right);

    public readonly override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(fCompression);
        hash.Add(fQuality);
        return hash.ToHashCode();
    }
}

#endif