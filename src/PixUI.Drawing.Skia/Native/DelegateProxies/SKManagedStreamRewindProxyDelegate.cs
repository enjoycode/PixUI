using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

internal static unsafe partial class DelegateProxies
{
    /// Proxy for sk_managedstream_rewind_proc native function.
#if USE_LIBRARY_IMPORT
    public static readonly delegate* unmanaged[Cdecl] <sk_stream_managedstream_t, void*, byte> SKManagedStreamRewindProxy = &SKManagedStreamRewindProxyImplementation;
    [UnmanagedCallersOnly(CallConvs = new [] {typeof(CallConvCdecl)})]
#else
	public static readonly SKManagedStreamRewindProxyDelegate SKManagedStreamRewindProxy = SKManagedStreamRewindProxyImplementation;
	[MonoPInvokeCallback (typeof (SKManagedStreamRewindProxyDelegate))]
#endif
    private static partial byte SKManagedStreamRewindProxyImplementation(sk_stream_managedstream_t s,void* context);

}