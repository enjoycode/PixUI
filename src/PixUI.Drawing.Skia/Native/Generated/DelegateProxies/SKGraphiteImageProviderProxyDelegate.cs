using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

unsafe partial class DelegateProxies
{
    /// Proxy for sk_graphite_image_provider_proc native function.
#if USE_LIBRARY_IMPORT
    public static readonly delegate* unmanaged[Cdecl] <void*, sk_graphite_recorder_t, sk_image_t, byte, sk_image_t>
        SKGraphiteImageProviderProxy = &SKGraphiteImageProviderProxyImplementation;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
#else
	public static readonly SKGraphiteImageProviderProxyDelegate SKGraphiteImageProviderProxy =
 SKGraphiteImageProviderProxyImplementation;
	[MonoPInvokeCallback (typeof (SKGraphiteImageProviderProxyDelegate))]
#endif
    private static partial sk_image_t SKGraphiteImageProviderProxyImplementation(void* userData,
        sk_graphite_recorder_t recorder, sk_image_t image, byte mipmapped);
}