using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

internal static unsafe partial class DelegateProxies
{
    /// Proxy for sk_managedstream_hasLength_proc native function.
#if USE_LIBRARY_IMPORT
    public static readonly delegate* unmanaged[Cdecl] <sk_stream_managedstream_t, void*, byte> SKManagedStreamHasLengthProxy = &SKManagedStreamHasLengthProxyImplementation;
    [UnmanagedCallersOnly(CallConvs = new [] {typeof(CallConvCdecl)})]
#else
	public static readonly SKManagedStreamHasLengthProxyDelegate SKManagedStreamHasLengthProxy = SKManagedStreamHasLengthProxyImplementation;
	[MonoPInvokeCallback (typeof (SKManagedStreamHasLengthProxyDelegate))]
#endif
    private static partial byte SKManagedStreamHasLengthProxyImplementation(sk_stream_managedstream_t s,void* context);

}