#nullable disable

using System.Runtime.InteropServices;

namespace PixUI.Drawing.Skia;

/// <summary>Represents the method that resolves the address of a Vulkan function by name.</summary>
/// <param name="name">The name of the Vulkan function to resolve.</param>
/// <param name="instance">The Vulkan instance handle to resolve the function against, or <see langword="null" /> when resolving a global or device-level function.</param>
/// <param name="device">The Vulkan device handle to resolve the function against, or <see langword="null" /> when resolving a global or instance-level function.</param>
/// <returns>A pointer to the resolved Vulkan function, or <see cref="F:System.IntPtr.Zero" /> if the function could not be found.</returns>
public delegate IntPtr SKGraphiteVkGetProcedureAddressDelegate(string name, IntPtr instance, IntPtr device);

/// <summary>
/// Represents the method that is called when Skia is finished
/// using a wrapped Graphite backend texture and the caller may release the underlying resource.
/// </summary>
public delegate void SKGraphiteReleaseDelegate();

/// <summary>Represents a callback method that receives the path and transformation matrix for each glyph when enumerating glyph paths.</summary>
/// <param name="path">The path of the glyph, or <see langword="null" /> if the glyph has no path.</param>
/// <param name="matrix">The transformation matrix to position the glyph.</param>
/// <remarks />
public delegate void SKGlyphPathDelegate(SKPath path, SKMatrix matrix);

internal static unsafe partial class DelegateProxies
{
    private static partial void SKGlyphPathProxyImplementation(IntPtr pathOrNull, SKMatrix* matrix, void* context)
    {
        var del = Get<SKGlyphPathDelegate>((IntPtr)context, out _);
        var path = SKPath.GetObject(pathOrNull, false);
        del.Invoke(path, *matrix);
    }

    private static partial void SKImageAsyncReadPixelsProxyImplementation(void* context, IntPtr result)
    {
        // The captured Action<IntPtr> is the closure built by SKImage/SKSurface.RequestReadPixels.
        // `result` is non-owning and only valid for the duration of this invocation (it is IntPtr.Zero
        // on failure); the closure must read all data before returning. This fires at most once, so the
        // pinning handle is freed here.
        var del = Get<Action<IntPtr>>((IntPtr)context, out var gch);
        try
        {
            del.Invoke(result);
        }
        finally
        {
            gch.Free();
        }
    }

    private static partial IntPtr SKGraphiteImageProviderProxyImplementation(void* userData, IntPtr recorder,
        IntPtr image, byte mipmapped)
    {
        // userData is a GCHandle pinned by SKGraphiteContext.CreateRecorder; the
        // recorder keeps it alive for its own lifetime and frees it in DisposeNative.
        // Returning IntPtr.Zero drops the draw, same as if no callback were installed.
        var del = Get<SKGraphiteFindOrCreateImageProxy>((IntPtr)userData, out _);
        try
        {
            return del.Invoke(recorder, image, mipmapped);
        }
        catch
        {
            // Never throw across the FFI boundary. Drop the draw on any
            // managed exception inside FindOrCreate.
            return IntPtr.Zero;
        }
    }

    private static partial IntPtr SKGraphiteVkGetProxyImplementation(void* userData, void* name, IntPtr instance,
        IntPtr device)
    {
        var del = Get<SKGraphiteVkGetProcedureAddressDelegate>((IntPtr)userData, out _);

        return del.Invoke(Marshal.PtrToStringAnsi((IntPtr)name), instance, device);
    }

    private static partial void SKGraphiteReleaseProxyImplementation(void* releaseContext)
    {
        var del = Get<SKGraphiteReleaseDelegate>((IntPtr)releaseContext, out var gch);
        try
        {
            del.Invoke();
        }
        finally
        {
            gch.Free();
        }
    }
}