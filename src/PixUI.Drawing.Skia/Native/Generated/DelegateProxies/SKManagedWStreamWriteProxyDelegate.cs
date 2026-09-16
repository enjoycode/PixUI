using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

internal static unsafe partial class DelegateProxies
{
    /// Proxy for sk_managedwstream_write_proc native function.
#if USE_LIBRARY_IMPORT
    public static readonly delegate* unmanaged[Cdecl] <sk_wstream_managedstream_t, void*, void*, /* size_t */ IntPtr, byte> SKManagedWStreamWriteProxy = &SKManagedWStreamWriteProxyImplementation;
    [UnmanagedCallersOnly(CallConvs = new [] {typeof(CallConvCdecl)})]
#else
	public static readonly SKManagedWStreamWriteProxyDelegate SKManagedWStreamWriteProxy = SKManagedWStreamWriteProxyImplementation;
	[MonoPInvokeCallback (typeof (SKManagedWStreamWriteProxyDelegate))]
#endif
    private static partial byte SKManagedWStreamWriteProxyImplementation(sk_wstream_managedstream_t s,void* context,void* buffer,/* size_t */ IntPtr size);

}